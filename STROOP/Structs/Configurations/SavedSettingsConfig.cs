using STROOP.Controls;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Structs.Configurations
{
    public static class SavedSettingsConfig
    {
        public static bool IsLoaded = false;

        private static bool _displayYawAnglesAsUnsigned;
        public static bool DisplayYawAnglesAsUnsigned
        {
            get => _displayYawAnglesAsUnsigned;
            set
            {
                if (_displayYawAnglesAsUnsigned == value) return;
                _displayYawAnglesAsUnsigned = value;
                WatchVariableControlSettingsManager.AddSettings(
                    new WatchVariableControlSettings(
                        changeYawSigned: true, newYawSigned: value));
                if (IsLoaded) Save();
            }
        }

        private static bool _startSlotIndexsFromOne;
        public static bool StartSlotIndexsFromOne
        {
            get => _startSlotIndexsFromOne;
            set
            {
                if (_startSlotIndexsFromOne == value) return;
                _startSlotIndexsFromOne = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _moveCameraWithPu;
        public static bool MoveCameraWithPu
        {
            get => _moveCameraWithPu;
            set
            {
                if (_moveCameraWithPu == value) return;
                _moveCameraWithPu = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _scaleDiagonalPositionControllerButtons;
        public static bool ScaleDiagonalPositionControllerButtons
        {
            get => _scaleDiagonalPositionControllerButtons;
            set
            {
                if (_scaleDiagonalPositionControllerButtons == value) return;
                _scaleDiagonalPositionControllerButtons = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _excludeDustForClosestObject;
        public static bool ExcludeDustForClosestObject
        {
            get => _excludeDustForClosestObject;
            set
            {
                if (_excludeDustForClosestObject == value) return;
                _excludeDustForClosestObject = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _useMisalignmentOffsetForDistanceToLine;
        public static bool UseMisalignmentOffsetForDistanceToLine
        {
            get => _useMisalignmentOffsetForDistanceToLine;
            set
            {
                if (_useMisalignmentOffsetForDistanceToLine == value) return;
                _useMisalignmentOffsetForDistanceToLine = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _dontRoundValuesToZero;
        public static bool DontRoundValuesToZero
        {
            get => _dontRoundValuesToZero;
            set
            {
                if (_dontRoundValuesToZero == value) return;
                _dontRoundValuesToZero = value;
                if (IsLoaded) Save();
            }
        }

        private static bool _neutralizeTrianglesWith21;
        public static bool NeutralizeTrianglesWith21
        {
            get => _neutralizeTrianglesWith21;
            set
            {
                if (_neutralizeTrianglesWith21 == value) return;
                _neutralizeTrianglesWith21 = value;
                if (IsLoaded) Save();
            }
        }

        public static short NeutralizeTriangleValue(bool? use21Nullable = null)
        {
            bool use21 = use21Nullable ?? NeutralizeTrianglesWith21;
            return (short)(use21 ? 21 : 0);
        }

        private static bool _useInGameTrigForAngleLogic;
        public static bool UseInGameTrigForAngleLogic
        {
            get => _useInGameTrigForAngleLogic;
            set
            {
                if (_useInGameTrigForAngleLogic == value) return;
                _useInGameTrigForAngleLogic = value;
                if (IsLoaded) Save();
            }
        }

        public static List<TabPage> _recommendedTabOrder;

        public static void StoreRecommendedTabOrder()
        {
            _recommendedTabOrder = ControlUtilities.GetTabPages(Config.TabControlMain);
        }

        public static void InvokeRecommendedTabOrder()
        {
            InvokeTabOrderCleanly(_recommendedTabOrder);
            Save();
        }

        public static List<string> InitiallySavedTabOrder;

        public static void InvokeInitiallySavedTabOrder()
        {
            List<TabPage> allTabPages = ControlUtilities.GetTabPages(Config.TabControlMain);
            List<TabPage> initiallySavedTabPages = new List<TabPage>();
            foreach (string tabName in InitiallySavedTabOrder)
            {
                TabPage tabPage = allTabPages.FirstOrDefault(t => t.Text == tabName);
                if (tabPage == null) continue;
                initiallySavedTabPages.Add(tabPage);
            }
            InvokeTabOrder(initiallySavedTabPages);
        }

        private static void InvokeTabOrder(List<TabPage> tabPages)
        {
            for (int i = 0; i < tabPages.Count; i++)
            {
                TabPage tabPage = tabPages[i];
                Config.TabControlMain.TabPages.Remove(tabPage);
                Config.TabControlMain.TabPages.Insert(i, tabPage);
            }
        }

        /** Doesn't remove the currently selected tab. */
        private static void InvokeTabOrderCleanly(List<TabPage> orderedTabPages)
        {
            // Get the selected tab/index
            TabPage selectedTab = Config.TabControlMain.SelectedTab;
            int selectedIndex = Config.TabControlMain.SelectedIndex;

            // Get the final combined ordering of tab pages
            List<TabPage> allTabPages = ControlUtilities.GetTabPages(Config.TabControlMain);
            List<TabPage> nonOrderedTabPages = allTabPages.FindAll(
                tabPage => !orderedTabPages.Contains(tabPage));
            List<TabPage> combinedTabPages = orderedTabPages.Concat(nonOrderedTabPages).ToList();

            // Remove all but the selected tab
            foreach (TabPage tabPage in allTabPages)
            {
                if (tabPage != selectedTab)
                    Config.TabControlMain.TabPages.Remove(tabPage);
            }

            // Add back all of the non-selected tabs
            for (int i = 0; i < combinedTabPages.Count; i++)
            {
                TabPage tabPage = combinedTabPages[i];
                if (tabPage == selectedTab) continue;
                Config.TabControlMain.TabPages.Insert(i, tabPage);
            }
        }

        public static List<XElement> ToXML()
        {
            XElement tabOrderXElement = new XElement("TabOrder");
            foreach (TabPage tabPage in Config.TabControlMain.TabPages)
            {
                XElement tabXElement = new XElement("Tab", tabPage.Text);
                tabOrderXElement.Add(tabXElement);
            }

            return new List<XElement>
            {
                new XElement("DisplayYawAnglesAsUnsigned", _displayYawAnglesAsUnsigned),
                new XElement("StartSlotIndexsFromOne", _startSlotIndexsFromOne),
                new XElement("MoveCameraWithPu", _moveCameraWithPu),
                new XElement("ScaleDiagonalPositionControllerButtons", _scaleDiagonalPositionControllerButtons),
                new XElement("ExcludeDustForClosestObject", _excludeDustForClosestObject),
                new XElement("UseMisalignmentOffsetForDistanceToLine", _useMisalignmentOffsetForDistanceToLine),
                new XElement("DontRoundValuesToZero", _dontRoundValuesToZero),
                new XElement("NeutralizeTrianglesWith21", _neutralizeTrianglesWith21),
                new XElement("UseInGameTrigForAngleLogic", _useInGameTrigForAngleLogic),
                tabOrderXElement,
            };
        }

        public static void Save()
        {
            DialogUtilities.SaveXmlElements(
                FileType.Xml, "SavedSettings", ToXML(), @"Config/SavedSettings.xml");
        }

        public static void ResetSavedSettings()
        {
            _displayYawAnglesAsUnsigned = true;
            _startSlotIndexsFromOne = true;
            _moveCameraWithPu = true;
            _scaleDiagonalPositionControllerButtons = true;
            _excludeDustForClosestObject = true;
            _useMisalignmentOffsetForDistanceToLine = true;
            _dontRoundValuesToZero = true;
            _neutralizeTrianglesWith21 = true;
            _useInGameTrigForAngleLogic = false;
            Save();
        }
    }
}
