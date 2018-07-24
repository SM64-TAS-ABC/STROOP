﻿using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Forms
{
    public partial class VariablePopOutForm : Form
    {
        public static int? WIDTH = null;
        public static int? HEIGHT = null;

        private bool _borderless = false;
        private bool _isDragging = false;
        private int _dragX = 0;
        private int _dragY = 0;

        private bool _alwaysOnTop = false;

        public VariablePopOutForm()
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

        public void Initialize(List<WatchVariableControl> controls)
        {
            // initialize panel
            _watchVariablePanel.Initialize();
            _watchVariablePanel.AddVariables(controls);

            // add borderless item to panel
            ToolStripMenuItem itemBorderless = new ToolStripMenuItem("Borderless");
            itemBorderless.Click += (sender, e) =>
            {
                _borderless = !_borderless;
                itemBorderless.Checked = _borderless;
                FormBorderStyle = _borderless ? FormBorderStyle.None : FormBorderStyle.Sizable;
            };
            itemBorderless.Checked = _borderless;
            _watchVariablePanel.ContextMenuStrip.Items.Insert(0, itemBorderless);

            // add always on top item to panel
            ToolStripMenuItem itemAlwaysOnTop = new ToolStripMenuItem("Always On Top");
            itemAlwaysOnTop.Click += (sender, e) =>
            {
                _alwaysOnTop = !_alwaysOnTop;
                itemAlwaysOnTop.Checked = _alwaysOnTop;
                TopMost = _alwaysOnTop;
            };
            itemBorderless.Checked = _alwaysOnTop;
            _watchVariablePanel.ContextMenuStrip.Items.Insert(1, itemAlwaysOnTop);

            // add close item to panel
            ToolStripMenuItem itemClose = new ToolStripMenuItem("Close");
            itemClose.Click += (sender, e) => Close();
            _watchVariablePanel.ContextMenuStrip.Items.Insert(2, itemClose);

            // make panel draggable when borderless
            _watchVariablePanel.MouseDown += (sender, e) =>
            {
                if (!_borderless) return;
                _isDragging = true;
                _dragX = e.X;
                _dragY = e.Y;
            };
            _watchVariablePanel.MouseUp += (sender, e) =>
            {
                if (!_borderless) return;
                _isDragging = false;
            };
            _watchVariablePanel.MouseMove += (sender, e) =>
            {
                if (!_borderless) return;
                if (_isDragging)
                {
                    SetDesktopLocation(MousePosition.X - _dragX, MousePosition.Y - _dragY);
                }
            };

            // set up timer
            Timer timer = new Timer { Interval = 10 };
            timer.Tick += (s, e) => UpdateForm();
            timer.Start();

            FormClosed += (sender, e) =>
            {
                timer.Stop();
            };
        }

        private void UpdateForm()
        {
            _watchVariablePanel.UpdatePanel();
        }

        public void ShowForm()
        {
            Show();
            _watchVariablePanel.UnselectText();
        }

    }
}
