using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class SelectionForm : Form
    {
        public SelectionForm(string selectionText, List<string> items, Action<string> action)
        {
            InitializeComponent();
            textBoxSelect.Text = selectionText;
            listBoxSelections.DataSource = items;
            buttonSet.Click += (sender, e) =>
            {
                string selection = listBoxSelections.SelectedItem.ToString();
                action(selection);
                Close();
            };
        }
        
        public static void ShowActionSelectionForm()
        {
            SelectionForm selectionForm = new SelectionForm(
                "Select an Action",
                TableConfig.MarioActions.GetActionNameList(),
                actionName =>
                {
                    uint? action = TableConfig.MarioActions.GetActionFromName(actionName);
                    if (action.HasValue)
                        Config.Stream.SetValue(action.Value, MarioConfig.StructAddress + MarioConfig.ActionOffset);
                });
            selectionForm.Show();
        }
    }
}
