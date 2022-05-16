using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Forms;
using STROOP.Models;
using STROOP.Managers;

namespace STROOP.Utilities
{
    public static class ObjectOrderingUtilities
    {
        public static void Move(bool rightwards, ObjectSlotsManager.SortMethodType sortMethodType)
        {
            switch (sortMethodType)
            {
                case ObjectSlotsManager.SortMethodType.ProcessingOrder:
                    Move_ProcessGroups(rightwards);
                    break;
                case ObjectSlotsManager.SortMethodType.MemoryOrder:
                    Move_Memory(rightwards);
                    break;
                case ObjectSlotsManager.SortMethodType.DistanceToMario:
                case ObjectSlotsManager.SortMethodType.LockedLabels:
                    // do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Move_Memory(bool rightwards)
        {
            Config.Stream.Suspend();
            int multiplicity = KeyboardUtilities.GetCurrentlyInputtedNumber() ?? 1;
            List<List<uint>> processGroups = GetProcessGroups();
            Dictionary<uint, ObjectSnapshot> objectSnapshots = new Dictionary<uint, ObjectSnapshot>();
            List<uint> selectedAddresses = Config.ObjectSlotsManager.SelectedObjects.ConvertAll(obj => obj.Address);
            for (int i = 0; i < multiplicity; i++)
            {
                List<uint> newSelectedAddresses = new List<uint>();
                selectedAddresses.Sort((uint objAddress1, uint objAddress2) =>
                {
                    int multiplier = rightwards ? -1 : +1;
                    int diff = objAddress1.CompareTo(objAddress2);
                    return multiplier * diff;
                });
                foreach (uint address in selectedAddresses)
                {
                    Move_Memory(address, rightwards, processGroups, objectSnapshots, newSelectedAddresses);
                }
                selectedAddresses.Clear();
                selectedAddresses.AddRange(newSelectedAddresses);
            }
            ApplyProcessGroups(processGroups);
            foreach (uint address in objectSnapshots.Keys)
            {
                ObjectSnapshot objectSnapshot = objectSnapshots[address];
                objectSnapshot.Apply(address, false);
            }
            Config.ObjectSlotsManager.SelectAddresses(selectedAddresses);
            Config.Stream.Resume();
        }

        public static void Move_Memory(
            uint objAddressToMove,
            bool rightwards,
            List<List<uint>> processGroups,
            Dictionary<uint, ObjectSnapshot> objectSnapshots,
            List<uint> newSelectedAddresses)
        {
            uint objAddress1 = objAddressToMove;
            int objIndex1 = ObjectUtilities.GetObjectIndex(objAddress1).Value;
            if ((objIndex1 == 0 && !rightwards) || (objIndex1 == ObjectSlotsConfig.MaxSlots - 1 && rightwards))
            {
                newSelectedAddresses.Add(objAddress1);
                return;
            }
            int objIndex2 = objIndex1 + (rightwards ? +1 : -1);
            uint objAddress2 = ObjectUtilities.GetObjectAddress(objIndex2);

            SwapAddresses(objAddress1, objAddress2, processGroups);
            SwapObjects(objAddress1, objAddress2, objectSnapshots);
            newSelectedAddresses.Add(objAddress2);
        }

        private static void SwapObjects(uint objAddress1, uint objAddress2, Dictionary<uint, ObjectSnapshot> objectSnapshots)
        {
            ObjectSnapshot obj1 = objectSnapshots.ContainsKey(objAddress1) ? objectSnapshots[objAddress1] : new ObjectSnapshot(objAddress1);
            ObjectSnapshot obj2 = objectSnapshots.ContainsKey(objAddress2) ? objectSnapshots[objAddress2] : new ObjectSnapshot(objAddress2);
            objectSnapshots[objAddress1] = obj2;
            objectSnapshots[objAddress2] = obj1;
        }

        private static void SwapAddresses(uint objAddress1, uint objAddress2, List<List<uint>> processGroups)
        {
            uint temp = uint.MaxValue;
            SetAddressTo(objAddress1, temp, processGroups);
            SetAddressTo(objAddress2, objAddress1, processGroups);
            SetAddressTo(temp, objAddress2, processGroups);
        }

        private static void SetAddressTo(uint objAddress, uint replacement, List<List<uint>> processGroups)
        {
            for (int i = 0; i < processGroups.Count; i++)
            {
                List<uint> processGroup = processGroups[i];
                for (int j = 0; j < processGroup.Count; j++)
                {
                    if (processGroup[j] == objAddress)
                    {
                        processGroup[j] = replacement;
                    }
                }
            }
        }

        private static List<List<uint>> GetProcessGroups()
        {
            List<List<uint>> processGroups = new List<List<uint>>();
            int slotIndex = 0;

            // processed slots
            foreach (byte processGroupByte in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.ProcessGroupsStartAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt(processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset);
                while ((objAddress != processGroupStructAddress && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            // vacant slots
            {
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt(ObjectSlotsConfig.VacantSlotsNodeAddress + ObjectConfig.ProcessedNextLinkOffset);
                while ((objAddress != 0 && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            return processGroups;
        }

        public static List<uint> GetObjectAddressesInProcessingOrder()
        {
            return GetProcessGroups().SelectMany(list => list).ToList();
        }

        private static void ApplyProcessGroups(List<List<uint>> processGroups)
        {
            // processed slots
            for (int i = 0; i < ObjectSlotsConfig.ProcessingGroups.Count; i++)
            {
                byte processGroupByte = ObjectSlotsConfig.ProcessingGroups[i];
                uint processGroupStructAddress = ObjectSlotsConfig.ProcessGroupsStartAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                List<uint> expandedProcessGroup = new List<uint>(processGroups[i]);
                expandedProcessGroup.Insert(0, processGroupStructAddress);
                expandedProcessGroup.Add(processGroupStructAddress);

                for (int j = 0; j < expandedProcessGroup.Count - 1; j++)
                {
                    uint address1 = expandedProcessGroup[j];
                    uint address2 = expandedProcessGroup[j + 1];
                    Config.Stream.SetValue(address2, address1 + ObjectConfig.ProcessedNextLinkOffset);
                    Config.Stream.SetValue(address1, address2 + ObjectConfig.ProcessedPreviousLinkOffset);
                }
            }

            // vacant slots
            {
                List<uint> expandedProcessGroup = new List<uint>(processGroups[processGroups.Count - 1]);
                expandedProcessGroup.Insert(0, ObjectSlotsConfig.VacantSlotsNodeAddress);
                expandedProcessGroup.Add(0);

                for (int j = 0; j < expandedProcessGroup.Count - 1; j++)
                {
                    uint address1 = expandedProcessGroup[j];
                    uint address2 = expandedProcessGroup[j + 1];
                    Config.Stream.SetValue(address2, address1 + ObjectConfig.ProcessedNextLinkOffset);
                }
            }
        }

        private static int GetProcessedIndex(uint objAddressToFind)
        {
            List<List<uint>> processGroups = GetProcessGroups();
            int index = 0;
            for (int i = 0; i < processGroups.Count; i++)
            {
                for (int j = 0; j < processGroups[i].Count; j++)
                {
                    uint objAddress = processGroups[i][j];
                    if (objAddress == objAddressToFind) return index;
                    index++;
                }
            }
            return -1;
        }

        public static void Move_ProcessGroups(bool rightwards)
        {
            Config.Stream.Suspend();
            List<ObjectDataModel> selectedObjects = Config.ObjectSlotsManager.SelectedObjects;
            List<uint> selectedAddresses = selectedObjects.ConvertAll(obj => obj.Address);
            selectedAddresses.Sort((uint objAddress1, uint objAddress2) =>
            {
                int multiplier = rightwards ? -1 : +1;
                int diff = GetProcessedIndex(objAddress1) - GetProcessedIndex(objAddress2);
                return multiplier * diff;
            });
            int multiplicity = KeyboardUtilities.GetCurrentlyInputtedNumber() ?? 1;
            List<List<uint>> processGroups = GetProcessGroups();
            for (int i = 0; i < multiplicity; i++)
            {
                foreach (uint address in selectedAddresses)
                {
                    processGroups = Move_ProcessGroups(address, rightwards, processGroups);
                }
            }
            ApplyProcessGroups(processGroups);
            Config.Stream.Resume();
        }

        public static List<List<uint>> Move_ProcessGroups(uint objAddressToMove, bool rightwards, List<List<uint>> processGroups)
        {
            int i = 0;
            int j = 0;
            bool foundAddress = false;
            for (i = 0; i < processGroups.Count; i++)
            {
                for (j = 0; j < processGroups[i].Count; j++)
                {
                    uint objAddress = processGroups[i][j];
                    if (objAddress == objAddressToMove)
                    {
                        foundAddress = true;
                        break;
                    }
                }
                if (foundAddress) break;
            }
            if (!foundAddress) return processGroups;

            // if moving before start or after end, then return
            if (i == 0 && j == 0 && !rightwards) return processGroups;
            if (i == processGroups.Count - 1 && j == processGroups[i].Count - 1 && rightwards) return processGroups;

            // moving to previous list
            if (j == 0 && !rightwards)
            {
                processGroups[i].Remove(objAddressToMove);
                processGroups[i - 1].Add(objAddressToMove);
            }

            // moving to next list
            else if (j == processGroups[i].Count - 1 && rightwards)
            {
                processGroups[i].Remove(objAddressToMove);
                processGroups[i + 1].Insert(0, objAddressToMove);
            }

            // moving within list
            else
            {
                int newJ = j + (rightwards ? +1 : -1);
                processGroups[i].Remove(objAddressToMove);
                processGroups[i].Insert(newJ, objAddressToMove);
            }

            return processGroups;
        }

        public static void Debug()
        {
            List<List<string>> labelLists = GetProcessGroups().ConvertAll(
                processGroup => processGroup.ConvertAll(
                    objAddress => Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(objAddress, true)));
            string output = String.Join("\r\n", labelLists.ConvertAll(labelList => String.Join(", ", labelList)));
            InfoForm.ShowValue(output);
        }

        public static void Debug2()
        {
            List<string> outputList = new List<string>();
            foreach (byte processGroupByte in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.ProcessGroupsStartAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                uint nextAddress = processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset;
                uint prevAddress = processGroupStructAddress + ObjectConfig.ProcessedPreviousLinkOffset;

                string nextString = processGroupByte + "\t" + "next" + "\t" + HexUtilities.FormatValue(nextAddress);
                string prevString = processGroupByte + "\t" + "prev" + "\t" + HexUtilities.FormatValue(prevAddress);

                outputList.Add(nextString);
                outputList.Add(prevString);
            }
            outputList.Add("vacant\t\t" + HexUtilities.FormatValue(ObjectSlotsConfig.VacantSlotsNodeAddress + ObjectConfig.ProcessedNextLinkOffset));
            InfoForm.ShowValue(String.Join("\r\n", outputList));
        }

        public static void Debug3()
        {
            List<List<uint>> processGroups = GetProcessGroups();
            ApplyProcessGroups(processGroups);
        }
    }
}
