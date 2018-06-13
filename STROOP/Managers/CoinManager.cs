using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class CoinManager
    {
        private readonly DataGridView _dataGridViewCoin;

        private readonly ListBox _listBoxCoinObjects;

        private readonly BetterTextbox _textBoxCoinHSpeedScale;
        private readonly BetterTextbox _textBoxCoinVSpeedScale;
        private readonly BetterTextbox _textBoxCoinVSpeedOffset;

        private readonly Label _labelCoinHSpeedRange;
        private readonly Label _labelCoinVSpeedRange;

        private readonly BetterTextbox _textBoxCoinFilterHSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterHSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterAngleMin;
        private readonly BetterTextbox _textBoxCoinFilterAngleMax;

        private readonly BetterTextbox _textBoxCoinStartingRng;

        private readonly Button _buttonCoinClear;
        private readonly Button _buttonCoinCalculate;

        public CoinManager(TabPage tabControl)
        {
            SplitContainer splitContainerCoin = tabControl.Controls["splitContainerCoin"] as SplitContainer;

            _dataGridViewCoin = splitContainerCoin.Panel2.Controls["dataGridViewCoin"] as DataGridView;

            _listBoxCoinObjects = splitContainerCoin.Panel1.Controls["listBoxCoinObjects"] as ListBox;

            _textBoxCoinHSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinHSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedOffset = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedOffset"] as BetterTextbox;

            _labelCoinHSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinHSpeedRange"] as Label;
            _labelCoinVSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinVSpeedRange"] as Label;

            GroupBox groupBoxCoinFilter = splitContainerCoin.Panel1.Controls["groupBoxCoinFilter"] as GroupBox;
            _textBoxCoinFilterHSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterHSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterAngleMin = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMin"] as BetterTextbox;
            _textBoxCoinFilterAngleMax = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMax"] as BetterTextbox;

            _textBoxCoinStartingRng = splitContainerCoin.Panel1.Controls["textBoxCoinStartingRng"] as BetterTextbox;

            _buttonCoinClear = splitContainerCoin.Panel1.Controls["buttonCoinClear"] as Button;
            _buttonCoinCalculate = splitContainerCoin.Panel1.Controls["buttonCoinCalculate"] as Button;
        }
        
        public void Update(bool updateView)
        {
            if (!updateView) return;
            
        }
    }
}
