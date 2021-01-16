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
    public class LazyImage
    {
        private string _filePath;
        private LazyImage _preLazyImage;
        private float? _opacity;

        private Image _image;

        public Image Image
        {
            get
            {
                if (_image == null)
                {
                    if (_filePath != null)
                    {
                        _image = Image.FromFile(_filePath);
                    }
                    else
                    {
                        _image = _preLazyImage.Image.GetOpaqueImage(_opacity.Value);
                    }
                }
                return _image;
            }
        }

        public LazyImage(string filePath)
        {
            _filePath = filePath;
        }

        public LazyImage(LazyImage preLazyImage, float opacity)
        {
            _preLazyImage = preLazyImage;
            _opacity = opacity;
        }

        public LazyImage(Image image)
        {
            _image = image;
        }
    }
}
