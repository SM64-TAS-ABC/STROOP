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

        Dictionary<int, float> _marioYDictionary;
        int? _previousGlobalTimer;
        int _currentGlobalTimer;
        int _globalTimerDiff;
        int _collisions;
        int _badCollisions;
        int _gaps;


        public TestingManager(TabPage tabControl)
        {
            _checkBoxTestingRecord = tabControl.Controls["checkBoxTestingRecord"] as CheckBox;
            _buttonTestingClear = tabControl.Controls["buttonTestingClear"] as Button;
            _buttonTestingClear.Click += (sender, e) => ClearData();
            _buttonTestingShow = tabControl.Controls["buttonTestingShow"] as Button;
            _buttonTestingShow.Click += (sender, e) => ShowData();

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

            _labelMetric1Name.Text = "Data Count:";
            _labelMetric2Name.Text = "Collisions:";
            _labelMetric3Name.Text = "Bad Collisions:";
            _labelMetric4Name.Text = "Gaps:";
            _labelMetric5Name.Text = "Global Timer Diff:";

            _marioYDictionary = new Dictionary<int, float>();
            ClearData();
        }

        private void ClearData()
        {
            _marioYDictionary.Clear();
            _previousGlobalTimer = null;
            _currentGlobalTimer = 0;
            _globalTimerDiff = 0;
            _collisions = 0;
            _badCollisions = 0;
            _gaps = 0;
        }

        private void ShowData()
        {
            System.Diagnostics.Trace.WriteLine("------------------------------------");
            foreach (KeyValuePair<int, float> entry in _marioYDictionary)
            {
                System.Diagnostics.Trace.WriteLine(entry.Key + " => " + entry.Value);
            }
            System.Diagnostics.Trace.WriteLine("------------------------------------");
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            if (_checkBoxTestingRecord.Checked)
            {
                // get current stream values
                _currentGlobalTimer = Config.Stream.GetInt32(Config.GlobalTimerAddress);
                float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);

                // check for key collisions
                bool keyCollision = _marioYDictionary.ContainsKey(_currentGlobalTimer);
                if (keyCollision) _collisions++;

                // check for value collisions
                bool valueCollision = keyCollision && _marioYDictionary[_currentGlobalTimer] == marioY;
                if (keyCollision && !valueCollision) _badCollisions++;

                // check the global timer difference
                if (_previousGlobalTimer != null)
                {
                    _globalTimerDiff = _currentGlobalTimer - _previousGlobalTimer.Value;
                    if (_globalTimerDiff > 1) _gaps += _globalTimerDiff - 1;
                }

                // update dictionary if need be
                if (!keyCollision) _marioYDictionary[_currentGlobalTimer] = marioY;

                // update previous global timer value
                _previousGlobalTimer = _currentGlobalTimer;
            }

            _labelMetric1Value.Text = _marioYDictionary.Count.ToString();
            _labelMetric2Value.Text = _collisions.ToString();
            _labelMetric3Value.Text = _badCollisions.ToString();
            _labelMetric4Value.Text = _gaps.ToString();
            _labelMetric5Value.Text = _globalTimerDiff.ToString();
        }
    }
}
