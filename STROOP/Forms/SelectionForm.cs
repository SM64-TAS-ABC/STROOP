using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class SelectionForm<E> : Form
    {
        public SelectionForm(string selectionText, List<E> items, Action<E> action)
        {
            InitializeComponent();
            textBoxSelect.Text = selectionText;
            listBoxSelections.DataSource = items;
            buttonSet.Click += (sender, e) =>
            {
                E selection = (E) listBoxSelections.SelectedItem;
                action(selection);
                Close();
            };
        }
        
        public static void ShowActionSelectionForm()
        {
            SelectionForm<string> selectionForm = new SelectionForm<string>(
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
