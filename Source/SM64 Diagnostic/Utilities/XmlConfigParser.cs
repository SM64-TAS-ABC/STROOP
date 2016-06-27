using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using SM64_Diagnostic.Structs;
using System.Drawing;
using System.Windows.Forms;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.Utilities
{
    public static class XmlConfigParser
    {
        public static Config OpenConfig(string path)
        {
            Config config = new Config();
            config.ObjectSlots = new ObjectSlotsConfig();
            config.ObjectGroups = new ObjectGroupsConfig();
            config.ObjectGroups.ProcessingGroups = new List<byte>();
            config.ObjectGroups.ProcessingGroupsColor = new Dictionary<byte, Color>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ConfigSchema.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach(XElement element in doc.Root.Elements())
            {
                switch(element.Name.ToString())
                {
                    case "RefreshRateFreq":
                        config.RefreshRateFreq = int.Parse(element.Value);
                        break;

                    case "ProcessDefaultName":
                        config.ProcessName = element.Value;
                        break;

                    case "RAMStartAddress":
                        config.RamStartAddress = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "RAMSize":
                        config.RamSize = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "ObjectSlots":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "FirstObjectAddress":
                                    config.ObjectSlots.LinkStartAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectStructSize":
                                    config.ObjectSlots.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HeaderOffset":
                                    config.ObjectSlots.HeaderOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ListNextLinkOffset":
                                    config.ObjectSlots.NextLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ListPreviousLinkOffset":
                                    config.ObjectSlots.PreviousLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "BehaviorScriptOffset":
                                    config.ObjectSlots.BehaviorScriptOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectActiveOffset":
                                    config.ObjectSlots.ObjectActiveOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetX":
                                    config.ObjectSlots.ObjectXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetY":
                                    config.ObjectSlots.ObjectYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetZ":
                                    config.ObjectSlots.ObjectZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MoveToMarioYOffset":
                                    config.ObjectSlots.MoveToMarioYOffset = float.Parse(subElement.Value);
                                    break;
                                case "MaxObjectSlots":
                                    config.ObjectSlots.MaxSlots = int.Parse(subElement.Value);
                                    break;
                            }
                        }
                        break;
               
                    case "ObjectGroups":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ProcessNextLinkOffset":
                                    config.ObjectGroups.ProcessNextLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ProcessPreviousLinkOffset":
                                    config.ObjectGroups.ProcessPreviousLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ParentObjectOffset":
                                    config.ObjectGroups.ParentObjectOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FirstObjectGroupingAddress":
                                    config.ObjectGroups.FirstGroupingAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "VacantPointerAddress":
                                    config.ObjectGroups.VactantPointerAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    config.ObjectGroups.VacantSlotColor = ColorTranslator.FromHtml(subElement.Attribute(XName.Get("color")).Value);
                                    break;
                                case "ProcessGroupStructSize":
                                    config.ObjectGroups.ProcessGroupStructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ProcessGroupsOrdering":
                                    foreach(var subSubElement in subElement.Elements())
                                    {
                                        var group = (byte)ParsingUtilities.ParseHex(
                                            subSubElement.Attribute(XName.Get("index")).Value);
                                        var color = ColorTranslator.FromHtml(
                                            subSubElement.Attribute(XName.Get("color")).Value);

                                        config.ObjectGroups.ProcessingGroups.Add(group);
                                        config.ObjectGroups.ProcessingGroupsColor.Add(group,color);
                                    }
                                    break;
                            }
                        }
                        break;

                    case "Mario":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "MarioStructAddress":
                                    config.Mario.MarioStructAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetX":
                                    config.Mario.XOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetY":
                                    config.Mario.YOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetZ":
                                    config.Mario.ZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FacingAngleOffset":
                                    config.Mario.RotationOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioStructSize":
                                    config.Mario.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ActionOffset":
                                    config.Mario.ActionOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MoveToObjectYOffset":
                                    config.Mario.MoveToObjectYOffset = float.Parse(subElement.Value);
                                    break;
                                case "HoldingObjectPointerOffset":
                                    config.Mario.HoldingObjectPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "LevelAddress":
                        config.LevelAddress = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "AreaAddress":
                        config.AreaAddress = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "LoadingPointAddress":
                        config.LoadingPointAddress = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "HolpX":
                        config.HolpX = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "HolpY":
                        config.HolpY = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "HolpZ":
                        config.HolpZ = ParsingUtilities.ParseHex(element.Value);
                        break;
                }
            }

            return config;
        }

        public static Dictionary<int, WatchVariable> OpenOtherData(string path)
        {
            var otherData = new Dictionary<int, WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.OtherDataSchema.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            var mainElements = doc.Root.Elements().ToArray();
            for(int i = 0; i < mainElements.Count(); i++)
            {
                var element = mainElements[i];
                var watchVar = GetWatchVariableFromElement(element);
                otherData.Add(i, watchVar);
            }

            return otherData;
        }

        public static List<WatchVariable> OpenObjectData(string path)
        {
            var objectData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ObjectDataSchema.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            //doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                if (element.Name.ToString() != "Data")
                    continue;

                var watchVar = GetWatchVariableFromElement(element);
                watchVar.OtherOffset = (element.Attribute(XName.Get("objectOffset")) != null) ?
                    bool.Parse(element.Attribute(XName.Get("objectOffset")).Value) : false;

                objectData.Add(watchVar);
            }

            return objectData;
        }

        public static List<WatchVariable> OpenMarioData(Config config, string path)
        {
            var objectData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.MarioDataSchema.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            //doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                if (element.Name.ToString() != "Data")
                    continue;

                var watchVar = GetWatchVariableFromElement(element);
                watchVar.OtherOffset = (element.Attribute(XName.Get("marioOffset")) != null) ?
                    bool.Parse(element.Attribute(XName.Get("marioOffset")).Value) : false;

                objectData.Add(watchVar);
            }

            return objectData;
        }

        public static ObjectAssociations OpenObjectAssoc(string path)
        {
            var assoc = new ObjectAssociations();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ObjectAssociationsSchema.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            // Create Behavior-ImagePath list
            var behaviorImageAssoc = new Dictionary<uint, Tuple<string, string>>();
            string defaultImagePath = "", emptyImagePath = "", imageDir = "", mapImageDir = "", marioImagePath = "", holpMapImagePath = "";
            var usedBehaviors = new List<uint>();
            uint ramToBehaviorOffset = 0;
            uint marioBehavior = 0;

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch(subElement.Name.ToString())
                            {
                                case "ImageDirectory":
                                    imageDir = subElement.Value;
                                    break;
                                case "DefaultImage":
                                    defaultImagePath = subElement.Value;
                                    break;
                                case "MapImageDirectory":
                                    mapImageDir = subElement.Value;
                                    break;
                                case "EmptyImage":
                                    emptyImagePath = subElement.Value;
                                    break;
                                case "RamToBehaviorOffset":
                                    ramToBehaviorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    assoc.RamOffset = ramToBehaviorOffset;
                                    break;
                            }
                        }
                        break;

                    case "Mario":
                        marioImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.MarioColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        marioBehavior = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        break;

                    case "Holp":
                        holpMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Object":
                        uint behaviorAddress = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        string name = element.Attribute(XName.Get("name")).Value;
                        if (usedBehaviors.Contains(behaviorAddress))
                            throw new Exception("More than one behavior address was defined.");
                        usedBehaviors.Add(behaviorAddress);
                        behaviorImageAssoc.Add(behaviorAddress, Tuple.Create<string,string>(imagePath, name));
                        break;
                }
            }

            // Load Images
            // TODO: Exceptions
            assoc.DefaultImage = Bitmap.FromFile(imageDir + defaultImagePath);
            assoc.EmptyImage = Bitmap.FromFile(imageDir + emptyImagePath);
            assoc.MarioImage = Bitmap.FromFile(imageDir + marioImagePath);
            assoc.HolpImage = Bitmap.FromFile(mapImageDir + holpMapImagePath);
            assoc.MarioBehavior = marioBehavior - ramToBehaviorOffset;
            foreach (var v in behaviorImageAssoc)
            {
                var preLoad = Bitmap.FromFile(imageDir + v.Value.Item1);
                var image = new Bitmap(preLoad, new Size(32, 32));
               
                preLoad.Dispose();
                assoc.AddAssociation(v.Key - ramToBehaviorOffset, image, v.Value.Item2);
            }

            return assoc;
        }

        public static MapAssociations OpenMapAssoc(string path)
        {
            var assoc = new MapAssociations();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.MapAssociationsSchema.xsd"), null));

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ImageDirectory":
                                    assoc.FolderPath = subElement.Value;
                                    break;
                                case "DefaultImage":
                                    var defaultMap = new Map() { ImagePath = subElement.Value };
                                    assoc.DefaultMap = defaultMap;
                                    break;
                                case "DefaultCoordinates":
                                    float dx1 = float.Parse(subElement.Attribute(XName.Get("x1")).Value);
                                    float dx2 = float.Parse(subElement.Attribute(XName.Get("x2")).Value);
                                    float dz1 = float.Parse(subElement.Attribute(XName.Get("z1")).Value);
                                    float dz2 = float.Parse(subElement.Attribute(XName.Get("z2")).Value);
                                    var dCoordinates = new RectangleF(dx1, dz1, dx2 - dx1, dz2 - dz1);
                                    assoc.DefaultMap.Coordinates = dCoordinates;
                                    break;
                            }
                        }
                        break;

                    case "Map":
                        byte level = byte.Parse(element.Attribute(XName.Get("level")).Value);
                        byte area = byte.Parse(element.Attribute(XName.Get("area")).Value);
                        ushort? loadingPoint = element.Attribute(XName.Get("loadingPoint")) != null ?
                            (ushort?)ushort.Parse(element.Attribute(XName.Get("loadingPoint")).Value) : null;
                        string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        string bgImagePath = (element.Element(XName.Get("BackgroundImage")) != null) ?
                          element.Element(XName.Get("BackgroundImage")).Attribute(XName.Get("path")).Value : null;

                        var coordinatesElement = element.Element(XName.Get("Coordinates"));
                        float x1 = float.Parse(coordinatesElement.Attribute(XName.Get("x1")).Value);
                        float x2 = float.Parse(coordinatesElement.Attribute(XName.Get("x2")).Value);
                        float z1 = float.Parse(coordinatesElement.Attribute(XName.Get("z1")).Value);
                        float z2 = float.Parse(coordinatesElement.Attribute(XName.Get("z2")).Value);
                        float y = (coordinatesElement.Attribute(XName.Get("y")) != null) ?
                            float.Parse(coordinatesElement.Attribute(XName.Get("y")).Value) : float.MinValue;

                        string name = element.Attribute(XName.Get("name")).Value;
                        string subName = (element.Attribute(XName.Get("subName")) != null) ?
                            element.Attribute(XName.Get("subName")).Value : null;

                        var coordinates = new RectangleF(x1, z1, x2 - x1, z2 - z1);

                        Map map = new Map() { Level = level, Area = area, LoadingPoint = loadingPoint, Coordinates = coordinates,
                            ImagePath = imagePath, Y = y, Name = name, SubName = subName, BackgroundPath = bgImagePath};

                        assoc.AddAssociation(map);
                        break;
                }
            }

            return assoc;
        }

        public static WatchVariable GetWatchVariableFromElement(XElement element)
        {
            var watchVar = new WatchVariable();
            watchVar.UseHex = (element.Attribute(XName.Get("useHex")) != null) ?
                bool.Parse(element.Attribute(XName.Get("useHex")).Value) : false;
            watchVar.AbsoluteAddressing = element.Attribute(XName.Get("absoluteAddress")) != null ?
                 bool.Parse(element.Attribute(XName.Get("absoluteAddress")).Value) : false;
            watchVar.Mask = element.Attribute(XName.Get("mask")) != null ?
                (UInt64?) ParsingUtilities.ParseExtHex(element.Attribute(XName.Get("mask")).Value) : null;
            watchVar.Name = element.Value;
            watchVar.IsBool = element.Attribute(XName.Get("isBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("isBool")).Value) : false;
            watchVar.Type = WatchVariableExtensions.GetStringType(element.Attribute(XName.Get("type")).Value);
            watchVar.Address = ParsingUtilities.ParseHex(element.Attribute(XName.Get("address")).Value);
            watchVar.InvertBool = element.Attribute(XName.Get("invertBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("invertBool")).Value) : false;
            return watchVar;
        }

        public static void AddWatchVariableOtherData(WatchVariable watchVar)
        {
            
        }

        public static void ModifyWatchVariableOtherData(int index, WatchVariable modifiedVar)
        {

        }

        public static void DeleteWatchVariablesOtherData(List<int> indexes)
        {

        }

        private static void Validation(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }
    }
}
