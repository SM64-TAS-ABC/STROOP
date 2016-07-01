using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.Structs
{
    public class ObjectAssociations
    {
        Dictionary<uint, Image> _objectTransparentImageAssoc = new Dictionary<uint, Image>();
        Dictionary<uint, Image> _objectImageAssoc = new Dictionary<uint, Image>();
        Dictionary<uint, string> _objectNameAssoc = new Dictionary<uint, string>();

        Image _defaultImage;
        Image _transparentDefaultImage;

        public Image EmptyImage;
        public Image MarioImage;
        public Image MarioMapImage;
        public Image HolpImage;
        public Image CameraImage;
        public Color MarioColor;
        public uint MarioBehavior;
        public uint RamOffset;

        public Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                _defaultImage = value;
                _transparentDefaultImage = value.GetOpaqueImage(0.5f);
            }
        }

        public void AddAssociation(uint behaviorAddress, Image image, string name)
        {
            _objectImageAssoc.Add(behaviorAddress, image);
            var transparentImage = image.GetOpaqueImage(0.5f);
            _objectTransparentImageAssoc.Add(behaviorAddress, transparentImage);
            _objectNameAssoc.Add(behaviorAddress, name);
        }

        public Image GetObjectImage(uint behaviorAddress, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            if (!_objectImageAssoc.ContainsKey(behaviorAddress))
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? _objectTransparentImageAssoc[behaviorAddress] : _objectImageAssoc[behaviorAddress];
        }

        public string GetObjectName(uint behaviorAddress)
        {
            if (!_objectNameAssoc.ContainsKey(behaviorAddress))
                return "Unknown Object";

            return _objectNameAssoc[behaviorAddress];
        }

        ~ObjectAssociations()
        {
            foreach (Image image in _objectImageAssoc.Values)
                image.Dispose();

            foreach (Image image in _objectTransparentImageAssoc.Values)
                image.Dispose();

            _transparentDefaultImage?.Dispose();
            _defaultImage?.Dispose();
            EmptyImage?.Dispose();
            MarioImage?.Dispose();
            HolpImage?.Dispose();
        }
    }
}
