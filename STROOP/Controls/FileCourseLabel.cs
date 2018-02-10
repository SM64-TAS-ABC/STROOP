using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;
using STROOP.Structs.Configurations;

namespace STROOP
{
    public class FileCourseLabel : Label
    {
        protected uint _addressOffset;
        protected byte _mask;

        static ToolTip _toolTip;
        public static ToolTip AddressToolTip
        {
            get
            {
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip();
                    _toolTip.IsBalloon = false;
                    _toolTip.ShowAlways = true;
                }
                return _toolTip;
            }
            set
            {
                _toolTip = value;
            }
        }

        public FileCourseLabel()
        {
        }

        public void Initialize(uint addressOffset, byte mask, int courseIndex)
        {
            _addressOffset = addressOffset;
            _mask = mask;

            this.Click += ClickAction;
            this.MouseEnter += (s, e) => this.Cursor = Cursors.Hand;
            this.MouseLeave += (s, e) => this.Cursor = Cursors.Arrow;

            string fullCourseName = TableConfig.CourseData.GetFullName(courseIndex);
            AddressToolTip.SetToolTip(this, fullCourseName);
        }

        private void SetValue(byte value)
        {
            byte maskedValue = (byte)(value & _mask);
            byte oldByte = Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + _addressOffset);
            byte unmaskedOldByte = (byte)(oldByte & ~_mask);
            byte newByte = (byte)(unmaskedOldByte | maskedValue);
            Config.Stream.SetValue(newByte, Config.FileManager.CurrentFileAddress + _addressOffset);
        }

        private byte GetValue()
        {
            byte currentByte = Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + _addressOffset);
            byte maskedCurrentByte = (byte)(currentByte & _mask);
            return maskedCurrentByte;
        }

        private byte GetNewValueForValue(byte oldValue)
        {
            return oldValue != _mask ? _mask : (byte)0;
        }

        private void ClickAction(object sender, EventArgs e)
        {
            byte oldValue = GetValue();
            byte newValue = GetNewValueForValue(oldValue);
            SetValue(newValue);
        }
    }
}
