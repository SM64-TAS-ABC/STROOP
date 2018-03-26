using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class MemoryManager
    {
        public MemoryManager(TabPage tabControl)
        {
            /*
            SplitContainer splitContainerTas = tabControl.Controls["splitContainerTas"] as SplitContainer;
            SplitContainer splitContainerTasTable = splitContainerTas.Panel1.Controls["splitContainerTasTable"] as SplitContainer;
            _dataGridViewTas = splitContainerTasTable.Panel2.Controls["dataGridViewTas"] as DataGridView;
            _checkBoxTasRecordData = splitContainerTasTable.Panel1.Controls["checkBoxTasRecordData"] as CheckBox;
            _buttonTasClearData = splitContainerTasTable.Panel1.Controls["buttonTasClearData"] as Button;
            _buttonTasClearData.Click += (sender, e) => ClearData();
            _richTextBoxTasInstructions = splitContainerTasTable.Panel1.Controls["richTextBoxTasInstructions"] as RichTextBox;

            Button buttonTasStoreMarioPosition = splitContainerTasTable.Panel1.Controls["buttonTasStoreMarioPosition"] as Button;
            buttonTasStoreMarioPosition.Click += (sender, e) => StoreMarioInfo(x: true, y: true, z: true);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTasStoreMarioPosition,
                new List<string>() { "Store Position", "Store Lateral Position", "Store X", "Store Y", "Store Z" },
                new List<Action>() {
                    () => StoreMarioInfo(x: true, y: true, z: true),
                    () => StoreMarioInfo(x: true, z: true),
                    () => StoreMarioInfo(x: true),
                    () => StoreMarioInfo(y: true),
                    () => StoreMarioInfo(z: true),
                });

            Button buttonTasStoreMarioAngle = splitContainerTasTable.Panel1.Controls["buttonTasStoreMarioAngle"] as Button;
            buttonTasStoreMarioAngle.Click += (sender, e) => StoreMarioInfo(angle: true);

            _waitingGlobalTimer = 0;
            _waitingDateTime = DateTime.Now;
            _lastUpdatedGlobalTimer = 0;

            _dataDictionary = new Dictionary<uint, TasDataStruct>();
            _rowDictionary = new Dictionary<uint, DataGridViewRow>();
            */
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
           
        }
    }
}
