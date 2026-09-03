using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public class TrackPlatformTable
    {
        // Number of updates needed to reach the last frame on which the platform is still at
        // rest. The next update transitions it to PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK.
        private const int WarmupFrames = 22;

        // How far forward GetFrame is willing to simulate while searching for the live
        // object's state. SetFrame is not bounded by this.
        private const int MaxCachedFrames = 100000;

        private const int ExtendChunkSize = 1024;

        // _states[i] is the platform's state on frame i, where frame 0 is the last resting
        // frame. _reverseDictionary maps oPosX to every frame that has that oPosX, which
        // narrows the search in Lookup down to a handful of candidates.
        private readonly List<TrackPlatform> _states;
        private readonly Dictionary<float, List<int>> _reverseDictionary;
        private TrackPlatform _cursor;

        public TrackPlatformTable()
        {
            _states = new List<TrackPlatform>();
            _reverseDictionary = new Dictionary<float, List<int>>();
        }

        public int GetFrame(uint objAddress)
        {
            EnsureInitialized();

            TrackPlatform live = new TrackPlatform(objAddress);
            if (!CouldBeThisPlatform(live)) return -1;

            int frame = Lookup(live);
            if (frame == -1 && _states.Count <= MaxCachedFrames)
            {
                // Extend by one chunk per call rather than all at once, so that a state which
                // never matches costs a little work on many refreshes instead of stalling one.
                CacheThrough(Math.Min(_states.Count + ExtendChunkSize - 1, MaxCachedFrames));
                frame = Lookup(live);
            }
            return frame;
        }

        public bool SetFrame(int frame, uint objAddress)
        {
            TrackPlatform state = GetState(frame);
            if (state == null) return false;

            // Frame 0 puts the platform back in the resting action but deliberately leaves
            // oTimer alone, so the caller keeps control of when it starts moving again.
            state.ApplyToObject(objAddress, applyTimer: frame != 0);
            return true;
        }

        public TrackPlatform GetState(int index)
        {
            if (index < 0) return null;

            if (index <= MaxCachedFrames)
            {
                CacheThrough(index);
                return _states[index];
            }

            // Past the cache, simulate from scratch rather than storing a state per frame.
            TrackPlatform platform = CreateRestingState();
            Advance(platform, 0, index);
            return platform;
        }

        private static TrackPlatform CreateRestingState()
        {
            TrackPlatform platform = new TrackPlatform();
            for (int i = 0; i < WarmupFrames; i++)
            {
                platform.Update(true);
            }
            return platform;
        }

        // Mario only has to stand on the platform long enough to start it moving. After that
        // he affects nothing but oTimer, so he is simulated as stepping off immediately.
        private static void Advance(TrackPlatform platform, int fromIndex, int toIndex)
        {
            for (int index = fromIndex; index < toIndex; index++)
            {
                platform.Update(index == 0);
            }
        }

        private void EnsureInitialized()
        {
            if (_cursor != null) return;

            _cursor = CreateRestingState();
            Append(_cursor);
        }

        private void CacheThrough(int index)
        {
            EnsureInitialized();

            while (_states.Count <= index)
            {
                Advance(_cursor, _states.Count - 1, _states.Count);
                Append(_cursor);
            }
        }

        private void Append(TrackPlatform platform)
        {
            TrackPlatform state = platform.Clone();

            List<int> indexes;
            if (!_reverseDictionary.TryGetValue(state.oPosX, out indexes))
            {
                indexes = new List<int>();
                _reverseDictionary[state.oPosX] = indexes;
            }
            indexes.Add(_states.Count);

            _states.Add(state);
        }

        private int Lookup(TrackPlatform live)
        {
            List<int> indexes;
            if (!_reverseDictionary.TryGetValue(live.oPosX, out indexes)) return -1;

            foreach (int index in indexes)
            {
                if (live.Equals(_states[index])) return index;
            }
            return -1;
        }

        // Cheap rejection of objects the simulation doesn't model, so that a state which can
        // never match doesn't cause the cache to be extended all the way to MaxCachedFrames.
        private bool CouldBeThisPlatform(TrackPlatform live)
        {
            TrackPlatform resting = _states[0];

            return live.oPlatformOnTrackStartWaypoint == resting.oPlatformOnTrackStartWaypoint
                && live.oBehParams == resting.oBehParams
                && (live.oAction == TrackPlatform.PLATFORM_ON_TRACK_ACT_WAIT_FOR_MARIO
                    || live.oAction == TrackPlatform.PLATFORM_ON_TRACK_ACT_MOVE_ALONG_TRACK);
        }
    }
}
