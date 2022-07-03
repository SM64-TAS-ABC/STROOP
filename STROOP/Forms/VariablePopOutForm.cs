using STROOP.Controls;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
using STROOP.Utilities;

namespace STROOP.Forms
{
    public partial class VariablePopOutForm : Form, IUpdatableForm
    {
        public static int? WIDTH = null;
        public static int? HEIGHT = null;

        private bool _borderless = false;
        private bool _isDragging = false;
        private int _dragX = 0;
        private int _dragY = 0;

        private bool _alwaysOnTop = false;

        private static int _instanceCouner = 0;

        private ToolStripMenuItem _itemBorderless;
        private ToolStripMenuItem _itemAlwaysOnTop;

        public VariablePopOutForm()
        {
            InitializeComponent();
            FormManager.AddForm(this);
            FormClosing += (sender, e) => FormManager.RemoveForm(this);

            _instanceCouner++;
            Text = "Pop Out " + _instanceCouner;

            if (WIDTH.HasValue) Width = WIDTH.Value;
            if (HEIGHT.HasValue) Height = HEIGHT.Value;
            Resize += (sender, e) =>
            {
                WIDTH = Width;
                HEIGHT = Height;
            };

            _itemBorderless = new ToolStripMenuItem("Borderless");
            _itemAlwaysOnTop = new ToolStripMenuItem("Always On Top");
        }

        public void Initialize(List<WatchVariableControl> controls)
        {
            // initialize panel
            _watchVariablePanel.Initialize();
            _watchVariablePanel.AddVariables(controls);

            // add borderless item to panel
            _itemBorderless.Click += (sender, e) => SetBorderless(!_borderless);
            _itemBorderless.Checked = _borderless;
            _watchVariablePanel.ContextMenuStrip.Items.Insert(0, _itemBorderless);

            // add always on top item to panel
            _itemAlwaysOnTop.Click += (sender, e) => SetAlwaysOnTop(!_alwaysOnTop);
            _itemBorderless.Checked = _alwaysOnTop;
            _watchVariablePanel.ContextMenuStrip.Items.Insert(1, _itemAlwaysOnTop);

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
        }

        public void SetBorderless(bool borderless)
        {
            _borderless = borderless;
            _itemBorderless.Checked = borderless;
            FormBorderStyle = borderless ? FormBorderStyle.None : FormBorderStyle.Sizable;
        }

        public void SetAlwaysOnTop(bool alwaysOnTop)
        {
            _alwaysOnTop = alwaysOnTop;
            _itemAlwaysOnTop.Checked = alwaysOnTop;
            TopMost = alwaysOnTop;
        }

        public static void OpenPopOutForm(XElement element)
        {
            VariablePopOutForm form = new VariablePopOutForm();

            List<XElement> subElements = element.Elements().ToList();
            List<WatchVariableControl> controls = subElements
                .ConvertAll(subElement => new WatchVariableControlPrecursor(subElement))
                .ConvertAll(precursor => precursor.CreateWatchVariableControl());
            form.ShowForm();

            bool? borderless = ParsingUtilities.ParseBoolNullable(element.Attribute(XName.Get("borderless"))?.Value);
            if (borderless.HasValue && borderless.Value)
            {
                form.SetBorderless(borderless.Value);
            }

            bool? alwaysOnTop = ParsingUtilities.ParseBoolNullable(element.Attribute(XName.Get("alwaysOnTop"))?.Value);
            if (alwaysOnTop.HasValue && alwaysOnTop.Value)
            {
                form.SetAlwaysOnTop(alwaysOnTop.Value);
            }

            int? locationX = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("locationX"))?.Value);
            int? locationY = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("locationY"))?.Value);
            if (locationX.HasValue && locationY.HasValue)
            {
                form.Location = new System.Drawing.Point(locationX.Value, locationY.Value);
            }

            int? width = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("width"))?.Value);
            if (width.HasValue)
            {
                form.Width = width.Value;
            }

            int? height = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("height"))?.Value);
            if (height.HasValue)
            {
                form.Height = height.Value;
            }

            form.Initialize(controls);
        }

        public void UpdateForm()
        {
            _watchVariablePanel.UpdatePanel();
        }

        public void ShowForm()
        {
            Show();
            _watchVariablePanel.UnselectText();
        }

        public VariablePopOutFormHelper GetHelper()
        {
            return new VariablePopOutFormHelper(_watchVariablePanel, Text);
        }

        public class VariablePopOutFormHelper : IVariableAdder
        {
            private WatchVariableFlowLayoutPanel _watchVariablePanel;
            private string _text;

            public VariablePopOutFormHelper(WatchVariableFlowLayoutPanel watchVariablePanel, string text)
            {
                _watchVariablePanel = watchVariablePanel;
                _text = text;
            }

            public void AddVariable(WatchVariableControl watchVarControl)
            {
                _watchVariablePanel.AddVariable(watchVarControl);
            }

            public void AddVariables(List<WatchVariableControl> watchVarControls)
            {
                _watchVariablePanel.AddVariables(watchVarControls);
            }

            public override string ToString()
            {
                return _text;
            }
        }

        public XElement GetData()
        {
            XElement xElement = new XElement("PopOut");
            xElement.Add(new XAttribute("locationX", Location.X));
            xElement.Add(new XAttribute("locationY", Location.Y));
            xElement.Add(new XAttribute("width", Width));
            xElement.Add(new XAttribute("height", Height));
            xElement.Add(new XAttribute("borderless", _borderless));
            xElement.Add(new XAttribute("alwaysOnTop", _alwaysOnTop));
            foreach (WatchVariableControl control in _watchVariablePanel.GetCurrentVariableControls())
            {
                xElement.Add(control.ToXml());
            }
            return xElement;
        }
    }
}
