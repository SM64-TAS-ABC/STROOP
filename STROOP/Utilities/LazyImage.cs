using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using STROOP.Structs;
using System.Drawing;
using System.Windows.Forms;
using STROOP.Extensions;
using System.Xml;
using System.Net;
using STROOP.Structs.Configurations;
using STROOP.Controls;
using STROOP.Models;

namespace STROOP.Utilities
{
    public static class LazyImage
    {
        private string _filePath;
        private Image _image;

        public Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = Image.FromFile(_filePath);
                }
                return _image;
            }
        }

        public LazyImage(string filePath)
        {
            _filePath = filePath;
            _image = null;
        }
    }
}
