using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class SelectionForm : Form
    {
        public SelectionForm()
        {
            InitializeComponent();
        }

        public void Initialize<T>(
            string selectionText,
            string buttonText,
            List<T> items,
            Action<T> selectionAction)
        {
            textBoxSelect.Text = selectionText;
            buttonSet.Text = buttonText;
            listBoxSelections.DataSource = items;
                
            Action enterAction = () =>
            {
                T selection = (T)listBoxSelections.SelectedItem;
                selectionAction(selection);
                Close();
            };
            buttonSet.Click += (sender, e) => enterAction();
            listBoxSelections.DoubleClick += (sender, e) => enterAction();
        }

        public static void ShowActionSelectionForm()
        {
            SelectionForm selectionForm = new SelectionForm();
            selectionForm.Initialize(
                "Select an Action",
                "Set Action",
                TableConfig.MarioActions.GetActionNameList(),
                actionName =>
                {
                    uint? action = TableConfig.MarioActions.GetActionFromName(actionName);
                    if (action.HasValue)
                        Config.Stream.SetValue(action.Value, MarioConfig.StructAddress + MarioConfig.ActionOffset);
                });
            selectionForm.Show();
        }

        public static void ShowAnimationSelectionForm()
        {
            SelectionForm selectionForm = new SelectionForm();
            selectionForm.Initialize(
                "Select an Animation",
                "Set Animation",
                TableConfig.MarioAnimations.GetAnimationNameList(),
                animationName =>
                {
                    int? animation = TableConfig.MarioAnimations.GetAnimationFromName(animationName);
                    if (animation.HasValue)
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        Config.Stream.SetValue((short)animation.Value, marioObjRef + MarioObjectConfig.AnimationOffset);
                    }
                });
            selectionForm.Show();
        }

        public static void ShowDataManagerSelectionForm(WatchVariableControl control)
        {
            SelectionForm selectionForm = new SelectionForm();
            selectionForm.Initialize(
                "Select a Tab",
                "Add Variable to Tab",
                Config.GetDataManagers(),
                dataManager => control.AddToTab(dataManager, false, false));
            selectionForm.Show();
        }
    }
}
