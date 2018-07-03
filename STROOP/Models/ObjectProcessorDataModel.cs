﻿using System;
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
            // Update behavior bank
            Config.ObjectAssociations.BehaviorBankStart = Config.Stream.GetUInt32(Config.ObjectAssociations.SegmentTable + 0x13 * 4);

            int? vacantIndexStart = UpdateGetProcessedObjects();
            if (vacantIndexStart.HasValue)
                UpdateGetVacantObjects(vacantIndexStart.Value);

            // Sort objects by address
            _objects.Sort((a, b) =>
            {
                if (a == null || b == null)
                    return 0;

                if (a.Address > b.Address)
                    return 1;

                if (a.Address < b.Address)
                    return -1;

                return 0;
            });

            ActiveObjectCount = DataModels.Objects.Count(o => o?.IsActive ?? false);
        }

        public void Update2()
        {
            _objects.ForEach(o => o?.Update2());
        }

        int successiveFails = 0;
        const int successiveFailsThreshold = 5;

        private int? UpdateGetProcessedObjects()
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
                    {
                        if (successiveFails++ > successiveFailsThreshold)
                            ClearAllObjectSlots();
                        return null;
                    }

                    ObjectDataModel obj = GetOrCreateObjectSlot(slotIndex, objAddress);

                    // Get data
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
                {
                    if (successiveFails++ > successiveFailsThreshold)
                        ClearAllObjectSlots();
                    return;
                }

                ObjectDataModel obj = GetOrCreateObjectSlot(slotIndex, objAddress);

                obj.CurrentProcessGroup = null;
                obj.ProcessIndex = slotIndex;
                obj.VacantSlotIndex = vacantSlotIndex;
                obj.Update();

                objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
            }

            successiveFails = 0;
        }

        private void ClearAllObjectSlots()
        {
            // Clear a slots
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                _objects[i] = null;
            }
        }

        private ObjectDataModel GetOrCreateObjectSlot(int slotIndex, uint address)
        {
            ObjectDataModel obj = _objects[slotIndex];
            if (obj == null)
            {
                obj = new ObjectDataModel(address, false);
                _objects[slotIndex] = obj;
            }
            obj.Address = address;

            return obj;
        }
    }
}
