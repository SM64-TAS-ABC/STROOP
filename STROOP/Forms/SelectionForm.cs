﻿using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class SelectionForm : Form
    {
        public static int? WIDTH = null;
        public static int? HEIGHT = null;

        public SelectionForm()
        {
            InitializeComponent();
            if (WIDTH.HasValue) Width = WIDTH.Value;
            if (HEIGHT.HasValue) Height = HEIGHT.Value;
            Resize += (sender, e) =>
            {
                WIDTH = Width;
                HEIGHT = Height;
            };
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

        public static void ShowDataManagerSelectionForm(List<WatchVariableControl> controls)
        {
            SelectionForm selectionForm = new SelectionForm();
            selectionForm.Initialize(
                "Select a Tab",
                "Add Variable(s) to Tab",
                Config.GetDataManagers(),
                dataManager => WatchVariableControl.AddVarsToTab(controls, dataManager));
            selectionForm.Show();
        }
    }
}
