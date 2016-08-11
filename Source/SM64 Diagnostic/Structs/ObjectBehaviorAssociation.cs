﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class ObjectBehaviorAssociation
    {
        public uint Behavior;
        public string Name;
        public bool RotatesOnMap;
        public string ImagePath = "";
        public string MapImagePath = "";
        public Image Image;
        public Image TransparentImage;
        public Image MapImage;
        public Image TransparentMapImage;
        public List<WatchVariable> WatchVariables = new List<WatchVariable>();
    }
}
