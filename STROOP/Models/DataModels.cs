using STROOP.Structs;
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
        private static List<IUpdatableDataModel> _models = new List<IUpdatableDataModel>();

        public static MarioDataModel Mario { get; private set; }
        public static CameraDataModel Camera { get; private set; }
        public static LevelDataModel Level { get; private set; }
        public static ObjectProcessorDataModel ObjectProcessor { get; private set; }
        public static IReadOnlyList<ObjectDataModel> Objects { get => ObjectProcessor.Objects; }

        static DataModels()
        {
            _models.Add(ObjectProcessor = new ObjectProcessorDataModel());
            _models.Add(Mario = new MarioDataModel());
            _models.Add(Level = new LevelDataModel());
            _models.Add(Camera = new CameraDataModel());
            // Object models are dynamically created
        }

        public static void Update()
        {
            // Update all models
            _models.ForEach(m => m.Update());
            _models.ForEach(m => m.Update2());
        }
    }
}
