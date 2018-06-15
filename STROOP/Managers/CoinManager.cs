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
        private readonly BetterTextbox _textBoxCoinParamOrder;
        private readonly BetterTextbox _textBoxCoinNumCoins;

        private readonly Label _labelCoinHSpeedRange;
        private readonly Label _labelCoinVSpeedRange;
        private readonly Label _labelCoinTableEntries;

        private readonly BetterTextbox _textBoxCoinFilterHSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterHSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterAngleMin;
        private readonly BetterTextbox _textBoxCoinFilterAngleMax;
        private readonly BetterTextbox _textBoxCoinFilterRequiredNumOfQualifiedCoins;

        private readonly CheckBox _checkBoxCoinCustomizatonDisplayNonQualifiedCoinsOfAQualifiedCoinGroup;
        private readonly BetterTextbox _textBoxCoinCustomizatonNumDecimalDigits;
        private readonly BetterTextbox _textBoxCoinCustomizatonStartingRngIndex;

        private readonly Button _buttonCoinClear;
        private readonly Button _buttonCoinCalculate;

        public CoinManager(TabPage tabControl)
        {
            // set controls

            SplitContainer splitContainerCoin = tabControl.Controls["splitContainerCoin"] as SplitContainer;

            _dataGridViewCoin = splitContainerCoin.Panel2.Controls["dataGridViewCoin"] as DataGridView;

            _listBoxCoinObjects = splitContainerCoin.Panel1.Controls["listBoxCoinObjects"] as ListBox;

            _textBoxCoinHSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinHSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedOffset = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedOffset"] as BetterTextbox;
            _textBoxCoinParamOrder = splitContainerCoin.Panel1.Controls["textBoxCoinParamOrder"] as BetterTextbox;
            _textBoxCoinNumCoins = splitContainerCoin.Panel1.Controls["textBoxCoinNumCoins"] as BetterTextbox;

            _labelCoinHSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinHSpeedRange"] as Label;
            _labelCoinVSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinVSpeedRange"] as Label;
            _labelCoinTableEntries = splitContainerCoin.Panel1.Controls["labelCoinTableEntries"] as Label;
            
            GroupBox groupBoxCoinFilter = splitContainerCoin.Panel1.Controls["groupBoxCoinFilter"] as GroupBox;
            _textBoxCoinFilterHSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterHSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterAngleMin = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMin"] as BetterTextbox;
            _textBoxCoinFilterAngleMax = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMax"] as BetterTextbox;
            _textBoxCoinFilterRequiredNumOfQualifiedCoins =
                groupBoxCoinFilter.Controls["textBoxCoinFilterRequiredNumOfQualifiedCoins"] as BetterTextbox;

            GroupBox groupBoxCoinCustomization =
                splitContainerCoin.Panel1.Controls["groupBoxCoinCustomization"] as GroupBox;
            _checkBoxCoinCustomizatonDisplayNonQualifiedCoinsOfAQualifiedCoinGroup =
                groupBoxCoinCustomization.Controls[
                    "checkBoxCoinCustomizatonDisplayNonQualifiedCoinsOfAQualifiedCoinGroup"] as CheckBox;
            _textBoxCoinCustomizatonNumDecimalDigits =
                groupBoxCoinCustomization.Controls["textBoxCoinCustomizatonNumDecimalDigits"] as BetterTextbox;
            _textBoxCoinCustomizatonStartingRngIndex =
                groupBoxCoinCustomization.Controls["textBoxCoinCustomizatonStartingRngIndex"] as BetterTextbox;

            _buttonCoinClear = splitContainerCoin.Panel1.Controls["buttonCoinClear"] as Button;
            _buttonCoinCalculate = splitContainerCoin.Panel1.Controls["buttonCoinCalculate"] as Button;

            // initialize controls

            _listBoxCoinObjects.DataSource = CoinObject.GetCoinObjects();
            _listBoxCoinObjects.ClearSelected();
            _listBoxCoinObjects.SelectedValueChanged += (sender, e) => ListBoxSelectionChange();

            _buttonCoinCalculate.Click += (sender, e) => CalculateCoinTrajectories();
            _buttonCoinClear.Click += (sender, e) => ClearCoinTrajectories();

            Color lightBlue = Color.FromArgb(235, 255, 255);
            Color lightPink = Color.FromArgb(255, 240, 255);
            Color lightYellow = Color.FromArgb(255, 255, 220);

            _dataGridViewCoin.Columns[0].DefaultCellStyle.BackColor = lightBlue;
            _dataGridViewCoin.Columns[1].DefaultCellStyle.BackColor = lightBlue;
            _dataGridViewCoin.Columns[2].DefaultCellStyle.BackColor = lightPink;
            _dataGridViewCoin.Columns[3].DefaultCellStyle.BackColor = lightYellow;
            _dataGridViewCoin.Columns[4].DefaultCellStyle.BackColor = lightYellow;
            _dataGridViewCoin.Columns[5].DefaultCellStyle.BackColor = lightYellow;
        }

        private void ListBoxSelectionChange()
        {
            CoinObject coinObject = _listBoxCoinObjects.SelectedItem as CoinObject;
            _textBoxCoinHSpeedScale.Text = coinObject.HSpeedScale.ToString();
            _textBoxCoinVSpeedScale.Text = coinObject.VSpeedScale.ToString();
            _textBoxCoinVSpeedOffset.Text = coinObject.VSpeedOffset.ToString();
            _textBoxCoinParamOrder.Text = coinObject.CoinParamOrder.ToString();
            _textBoxCoinNumCoins.Text = coinObject.NumCoins.ToString();
        }

        public void ClearCoinTrajectories()
        {
            _dataGridViewCoin.Rows.Clear();
        }

        private void CalculateCoinTrajectories()
        {
            ClearCoinTrajectories();

            double? hSpeedScale = ParsingUtilities.ParseIntNullable(_textBoxCoinHSpeedScale.Text);
            double? vSpeedScale = ParsingUtilities.ParseIntNullable(_textBoxCoinVSpeedScale.Text);
            double? vSpeedOffset = ParsingUtilities.ParseIntNullable(_textBoxCoinVSpeedOffset.Text);
            bool coinParamOrderParsed = Enum.TryParse(_textBoxCoinParamOrder.Text, out CoinParamOrder coinParamOrder);
            int? numCoins = ParsingUtilities.ParseIntNullable(_textBoxCoinNumCoins.Text);

            if (!hSpeedScale.HasValue ||
                !vSpeedScale.HasValue ||
                !vSpeedOffset.HasValue ||
                !coinParamOrderParsed ||
                !numCoins.HasValue)
            {
                DialogUtilities.DisplayMessage(
                    "Could not parse coin param fields.",
                    "Parsing Error");
                return;
            }

            CoinObject coinObject = new CoinObject(
                hSpeedScale: hSpeedScale.Value,
                vSpeedScale: vSpeedScale.Value,
                vSpeedOffset: vSpeedOffset.Value,
                coinParamOrder: coinParamOrder,
                numCoins: numCoins.Value,
                name: "Dummy");

            int? startingRngIndexNullable = ParsingUtilities.ParseIntNullable(
                _textBoxCoinCustomizatonStartingRngIndex.Text);
            int startingRngIndex = startingRngIndexNullable ?? RngIndexer.GetRngIndex();

            int? numDecimalDigitsNullable = ParsingUtilities.ParseIntNullable(
                _textBoxCoinCustomizatonNumDecimalDigits.Text);
            int numDecimalDigits = numDecimalDigitsNullable ?? 3;

            List<int> rngIndexes = Enumerable.Range(0, 65114).ToList();

            foreach (int rngIndex in rngIndexes)
            {
                // rng based values
                ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                int rngToGo = MoreMath.NonNegativeModulus(rngIndex - startingRngIndex, 65114);

                // coin trajectory
                List<CoinTrajectory> coinTrajectories = coinObject.CalculateCoinTrajectories(rngIndex);

                // filter the values
                CoinTrajectoryFilter filter = new CoinTrajectoryFilter(
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterHSpeedMin.Text),
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterHSpeedMax.Text),
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterVSpeedMin.Text),
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterVSpeedMax.Text),
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterAngleMin.Text),
                    ParsingUtilities.ParseDoubleNullable(_textBoxCoinFilterAngleMax.Text),
                    ParsingUtilities.ParseIntNullable(_textBoxCoinFilterRequiredNumOfQualifiedCoins.Text));
                if (!filter.Qualifies(coinTrajectories)) continue;

                if (!_checkBoxCoinCustomizatonDisplayNonQualifiedCoinsOfAQualifiedCoinGroup.Checked)
                {
                    coinTrajectories = coinTrajectories.FindAll(
                        coinTrajectory => filter.Qualifies(coinTrajectory));
                }

                List<double> hSpeedList = coinTrajectories.ConvertAll(
                    coinTrajectory => Math.Round(coinTrajectory.HSpeed, numDecimalDigits));
                List<double> vSpeedList = coinTrajectories.ConvertAll(
                    coinTrajectory => Math.Round(coinTrajectory.VSpeed, numDecimalDigits));
                List<ushort> angleList = coinTrajectories.ConvertAll(
                    coinTrajectory => coinTrajectory.Angle);

                string hSpeedJoined = String.Join(", ", hSpeedList);
                string vSpeedJoined = String.Join(", ", vSpeedList);
                string angleJoined = String.Join(", ", angleList);

                // add a new row to the table
                _dataGridViewCoin.Rows.Add(
                    rngIndex, rngValue, rngToGo, hSpeedJoined, vSpeedJoined, angleJoined);
            }
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            double? hSpeedScaleNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinHSpeedScale.Text);
            if (hSpeedScaleNullable.HasValue)
            {
                double hSpeedScale = hSpeedScaleNullable.Value;
                double hSpeedMin = 0;
                double hSpeedMax = hSpeedScale;
                _labelCoinHSpeedRange.Text = String.Format("HSpeed Range: [{0}, {1})", hSpeedMin, hSpeedMax);
            }
            else
            {
                _labelCoinHSpeedRange.Text = "HSpeed Range:";
            }

            double? vSpeedScaleNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinVSpeedScale.Text);
            double? vSpeedOffsetNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinVSpeedOffset.Text);
            if (vSpeedScaleNullable.HasValue && vSpeedOffsetNullable.HasValue)
            {
                double vSpeedScale = vSpeedScaleNullable.Value;
                double vSpeedOffset = vSpeedOffsetNullable.Value;
                double vSpeedMin = vSpeedOffset;
                double vSpeedMax = vSpeedScale + vSpeedOffset;
                _labelCoinVSpeedRange.Text = String.Format("VSpeed Range: [{0}, {1})", vSpeedMin, vSpeedMax);
            }
            else
            {
                _labelCoinVSpeedRange.Text = "VSpeed Range:";
            }

            _labelCoinTableEntries.Text = "Table Entries: " + _dataGridViewCoin.Rows.Count;
        }
    }
}
