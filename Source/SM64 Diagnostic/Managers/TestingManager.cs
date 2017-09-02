using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System.Windows.Forms;
using static SM64_Diagnostic.Structs.Configurations.Config;
using static SM64_Diagnostic.Structs.Configurations.PositionControllerRelativeAngleConfig;

namespace SM64_Diagnostic.Managers
{
    public class TestingManager
    {
        CheckBox _checkBoxTestingRecord;
        Button _buttonTestingClear;
        Button _buttonTestingShow;

        Label _labelMetric1Name;
        Label _labelMetric2Name;
        Label _labelMetric3Name;
        Label _labelMetric4Name;
        Label _labelMetric5Name;
        Label _labelMetric6Name;

        Label _labelMetric1Value;
        Label _labelMetric2Value;
        Label _labelMetric3Value;
        Label _labelMetric4Value;
        Label _labelMetric5Value;
        Label _labelMetric6Value;

        public TestingManager(TabPage tabControl)
        {
            _checkBoxTestingRecord = tabControl.Controls["checkBoxTestingRecord"] as CheckBox;
            _buttonTestingClear = tabControl.Controls["buttonTestingClear"] as Button;
            _buttonTestingShow = tabControl.Controls["buttonTestingShow"] as Button;

            _labelMetric1Name = tabControl.Controls["labelMetric1Name"] as Label;
            _labelMetric2Name = tabControl.Controls["labelMetric2Name"] as Label;
            _labelMetric3Name = tabControl.Controls["labelMetric3Name"] as Label;
            _labelMetric4Name = tabControl.Controls["labelMetric4Name"] as Label;
            _labelMetric5Name = tabControl.Controls["labelMetric5Name"] as Label;
            _labelMetric6Name = tabControl.Controls["labelMetric6Name"] as Label;

            _labelMetric1Value = tabControl.Controls["labelMetric1Value"] as Label;
            _labelMetric2Value = tabControl.Controls["labelMetric2Value"] as Label;
            _labelMetric3Value = tabControl.Controls["labelMetric3Value"] as Label;
            _labelMetric4Value = tabControl.Controls["labelMetric4Value"] as Label;
            _labelMetric5Value = tabControl.Controls["labelMetric5Value"] as Label;
            _labelMetric6Value = tabControl.Controls["labelMetric6Value"] as Label;
        }

        internal void Update(bool updateView)
        {
            if (!updateView) return;


        }
    }
}
