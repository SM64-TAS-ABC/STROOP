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
        Dictionary<uint, Image> _objectMapImageAssoc = new Dictionary<uint, Image>();
        Dictionary<uint, Image> _objectTransparentMapImageAssoc = new Dictionary<uint, Image>();
        Dictionary<uint, bool> _objectMapRotates = new Dictionary<uint, bool>();
        Dictionary<uint, string> _objectNameAssoc = new Dictionary<uint, string>();
        
        Image _defaultImage;
        Image _transparentDefaultImage;

        public Image EmptyImage;
        public Image MarioImage;
        public Image HudImage;
        public Image CameraImage;
        public Image HolpImage;
        public Image MarioMapImage;
        public Color MarioColor;
        public Color HudColor;
        public Color CameraColor;
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

        public void AddAssociation(uint behaviorAddress, Image image, Image mapImage, string name, bool rotates)
        {
            _objectImageAssoc.Add(behaviorAddress, image);
            var transparentImage = image.GetOpaqueImage(0.5f);
            _objectTransparentImageAssoc.Add(behaviorAddress, transparentImage);

            _objectNameAssoc.Add(behaviorAddress, name);

            var transparentMapImage = mapImage.GetOpaqueImage(0.5f);
            _objectMapImageAssoc.Add(behaviorAddress, mapImage);
            _objectTransparentMapImageAssoc.Add(behaviorAddress, transparentMapImage);

            _objectMapRotates.Add(behaviorAddress, rotates);
        }

        public Image GetObjectImage(uint behaviorAddress, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            if (!_objectImageAssoc.ContainsKey(behaviorAddress))
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? _objectTransparentImageAssoc[behaviorAddress] : _objectImageAssoc[behaviorAddress];
        }

        public Image GetObjectMapImage(uint behaviorAddress, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            if (!_objectMapImageAssoc.ContainsKey(behaviorAddress))
                return _defaultImage;

            return transparent ? _objectTransparentMapImageAssoc[behaviorAddress] : _objectMapImageAssoc[behaviorAddress];
        }

        public bool GetObjectMapRotates(uint behaviorAddress)
        {
            if (!_objectMapRotates.ContainsKey(behaviorAddress))
                return false;

            return _objectMapRotates[behaviorAddress];
        }

        public string GetObjectName(uint behaviorAddress)
        {
            if (!_objectNameAssoc.ContainsKey(behaviorAddress))
                return "Unknown Object";

            return _objectNameAssoc[behaviorAddress];
        }

        ~ObjectAssociations()
        {
            // Unload and dipose of all images
            foreach (Image image in _objectImageAssoc.Values)
                image.Dispose();

            foreach (Image image in _objectMapImageAssoc.Values)
                image.Dispose();

            foreach (Image image in _objectTransparentImageAssoc.Values)
                image.Dispose();

            foreach (Image image in _objectTransparentMapImageAssoc.Values)
                image.Dispose();

            _transparentDefaultImage?.Dispose();
            _defaultImage?.Dispose();
            EmptyImage?.Dispose();
            MarioImage?.Dispose();
            MarioMapImage?.Dispose();
            HolpImage?.Dispose();
            HudImage?.Dispose();
            CameraImage?.Dispose();
        }
    }
}
