using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public static class DataModels
    {
        private const int MaxDependencyLevel = 0;

        static List<UpdatableDataModel> _models;

        static public MarioDataModel Mario;
        static public CameraDataModel Camera;
        static public LevelDataModel Level;

        static DataModels()
        {
            Mario = new MarioDataModel();
            Level = new LevelDataModel();
            Camera = new CameraDataModel();

            // Add to models list (for updating)
            _models = new List<UpdatableDataModel>()
            {
                Mario,
                Camera,
                Level,
            };
        }

        static public void Update()
        {
            // Update all models
            foreach (int level in Enumerable.Range(0, MaxDependencyLevel + 1))
                _models.ForEach(m => m.Update(level));
        }
    }
}
