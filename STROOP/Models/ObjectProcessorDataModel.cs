using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;

namespace STROOP.Models
{
    public class ObjectProcessorDataModel : IUpdatableDataModel
    {
        private List<ObjectDataModel> _objects;
        public IReadOnlyList<ObjectDataModel> Objects { get => _objects.AsReadOnly(); }

        public int ActiveObjectCount { get; private set; }

        public ObjectProcessorDataModel()
        {
            _objects = Enumerable.Repeat<ObjectDataModel>(null, ObjectSlotsConfig.MaxSlots).ToList();
        }

        public void Update()
        {
            // Check behavior bank
            Config.ObjectAssociations.BehaviorBankStart = Config.Stream.GetUInt32(Config.ObjectAssociations.SegmentTable + 0x13 * 4);

            int vacantIndexStart = UpdateGetProcessedObjects();
            UpdateGetVacantObjects(vacantIndexStart);

            ActiveObjectCount = DataModels.Objects.Count(o => o.IsActive);
        }

        public void Update2()
        {
            _objects.ForEach(o => o?.Update2());
        }

        private int UpdateGetProcessedObjects()
        {
            int slotIndex = 0;
            foreach (var processGroup in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + processGroup * ObjectSlotsConfig.ProcessGroupStructSize;

                // Calculate start object
                uint objAddress = Config.Stream.GetUInt32(processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset);

                // Loop through every object within the group
                while ((objAddress != processGroupStructAddress && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    // Validate current object
                    if (objAddress == 0 ||
                        Config.Stream.GetUInt16(objAddress + ObjectConfig.HeaderOffset) != 0x18)
                        ClearRemainingObjectSlots(slotIndex);

                    ObjectDataModel obj = GetOrCreateObjectSlot(slotIndex);

                    // Get data
                    obj.Address = objAddress;
                    obj.CurrentProcessGroup = processGroup;
                    obj.ProcessIndex = slotIndex;
                    obj.VacantSlotIndex = null;
                    obj.Update();

                    // Move to next object
                    objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);

                    // Mark next slot
                    slotIndex++;
                }
            }

            return slotIndex;
        }

        private void UpdateGetVacantObjects(int slotIndex)
        {
            // Now calculate vacant addresses
            uint objAddress = Config.Stream.GetUInt32(ObjectSlotsConfig.VactantPointerAddress);
            for (int vacantSlotIndex = 0; slotIndex < ObjectSlotsConfig.MaxSlots; slotIndex++, vacantSlotIndex++)
            {
                // Validate current object
                if (objAddress == 0 || 
                    Config.Stream.GetUInt16(objAddress + ObjectConfig.HeaderOffset) != 0x18)
                    ClearRemainingObjectSlots(slotIndex);

                ObjectDataModel obj = GetOrCreateObjectSlot(slotIndex);

                obj.Address = objAddress;
                obj.CurrentProcessGroup = null;
                obj.ProcessIndex = slotIndex;
                obj.VacantSlotIndex = vacantSlotIndex;
                obj.Update();

                objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
            }
        }

        private void ClearRemainingObjectSlots(int startIndex = 0)
        {
            // Clear remaining slots
            for (; startIndex < ObjectSlotsConfig.MaxSlots; startIndex++)
            {
                _objects[startIndex] = null;
            }
        }

        private ObjectDataModel GetOrCreateObjectSlot(int slotIndex)
        {
            ObjectDataModel obj = _objects[slotIndex];
            if (obj == null)
            {
                obj = new ObjectDataModel();
                _objects[slotIndex] = obj;
            }

            return obj;
        }
    }
}
