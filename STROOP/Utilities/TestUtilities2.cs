using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class TestUtilities2
    {
        public static void Test()
        {
            uint pendulumAddress = 0x8033EEA8;
            List<bool> solution = PendulumMain.Solution;
            solution = solution.Take(10).ToList(); // REMOVE THIS
            List<List<int>> dustFrameLists = TtcMain.FindIdealPendulumManipulation2(pendulumAddress, solution);
            List<int> dustFrames = dustFrameLists.SelectMany(list => list).ToList();
            int mupenFrame = MupenUtilities.GetFrameCount();
            int maxDustFrame = dustFrames[dustFrames.Count - 1];
            List<int> frameActions = new List<int>();
            for (int i = mupenFrame; i <= maxDustFrame; i++)
            {
                frameActions.Add(0);
            }
            foreach (int frame in dustFrames)
            {
                frameActions[frame - mupenFrame] = 1;
            }

            for (int i = 0; i < dustFrames.Count - 1; i++)
            {
                int dustFrame1 = dustFrames[i];
                int dustFrame2 = dustFrames[i + 1];
                int diff = dustFrame2 - dustFrame1;
                if (diff > 800)
                {
                    int num400s = diff / 400;
                    for (int j = 1; j < num400s; j++)
                    {
                        int zPressFrame = dustFrame1 + 400 * j;
                        frameActions[zPressFrame - mupenFrame] = 2;
                    }
                }
            }

            uint emptyFrameValue = 0x00001000;
            uint walkFrameValue = 0x7F001000;
            uint zFrameValue = 0x00001020;

            List<byte> emptyFrameBytes = BitConverter.GetBytes(emptyFrameValue).ToList();
            List<byte> walkFrameBytes = BitConverter.GetBytes(walkFrameValue).ToList();
            List<byte> zFrameBytes = BitConverter.GetBytes(zFrameValue).ToList();

            byte[] newBytes = frameActions.ConvertAll(frameAction =>
            {
                if (frameAction == 0) return emptyFrameBytes;
                if (frameAction == 1) return walkFrameBytes;
                if (frameAction == 2) return zFrameBytes;
                throw new ArgumentOutOfRangeException();
            }).SelectMany(list => list).ToArray();
        }
    }
}
