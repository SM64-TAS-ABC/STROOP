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

namespace STROOP.Utilities
{
    public static class ObjectOrderingUtilities
    {

        private static List<List<uint>> GetProcessGroups()
        {
            List<List<uint>> processGroups = new List<List<uint>>();
            int slotIndex = 0;

            // processed slots
            foreach (byte processGroupByte in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt32(processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset);
                while ((objAddress != processGroupStructAddress && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            // vacant slots
            {
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt32(ObjectSlotsConfig.VacantPointerAddress);
                while ((objAddress != 0 && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            return processGroups;
        }

        private static void Apply(List<List<uint>> processGroups)
        {
            // processed slots
            for (int i = 0; i < ObjectSlotsConfig.ProcessingGroups.Count; i++)
            {
                byte processGroupByte = ObjectSlotsConfig.ProcessingGroups[i];
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
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
                expandedProcessGroup.Insert(0, ObjectSlotsConfig.VacantPointerAddress);
                expandedProcessGroup.Add(0);

                for (int j = 0; j < expandedProcessGroup.Count - 1; j++)
                {
                    uint address1 = expandedProcessGroup[j];
                    uint address2 = expandedProcessGroup[j + 1];
                    uint nextLinkOffset = j == 0 ? 0 : ObjectConfig.ProcessedNextLinkOffset;
                    Config.Stream.SetValue(address2, address1 + nextLinkOffset);
                }
            }
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
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                uint nextAddress = processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset;
                uint prevAddress = processGroupStructAddress + ObjectConfig.ProcessedPreviousLinkOffset;

                string nextString = processGroupByte + "\t" + "next" + "\t" + HexUtilities.FormatValue(nextAddress);
                string prevString = processGroupByte + "\t" + "prev" + "\t" + HexUtilities.FormatValue(prevAddress);

                outputList.Add(nextString);
                outputList.Add(prevString);
            }
            outputList.Add("vacant\t\t" + HexUtilities.FormatValue(ObjectSlotsConfig.VacantPointerAddress));
            InfoForm.ShowValue(String.Join("\r\n", outputList));
        }

        public static void Debug3()
        {
            List<List<uint>> processGroups = GetProcessGroups();
            Apply(processGroups);
        }

    }
}
