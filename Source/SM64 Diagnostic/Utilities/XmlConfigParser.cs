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
using System.Xml;
using System.Net;

namespace SM64_Diagnostic.Utilities
{
    public static class XmlConfigParser
    {
        public class ResourceXmlResolver : XmlResolver
        {
            /// <summary>
            /// When overridden in a derived class, maps a URI to an object containing the actual resource.
            /// </summary>
            /// <returns>
            /// A System.IO.Stream object or null if a type other than stream is specified.
            /// </returns>
            /// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)"/>. </param><param name="role">The current version does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios. </param><param name="ofObjectToReturn">The type of object to return. The current version only returns System.IO.Stream objects. </param><exception cref="T:System.Xml.XmlException"><paramref name="ofObjectToReturn"/> is not a Stream type. </exception><exception cref="T:System.UriFormatException">The specified URI is not an absolute URI. </exception><exception cref="T:System.ArgumentNullException"><paramref name="absoluteUri"/> is null. </exception><exception cref="T:System.Exception">There is a runtime error (for example, an interrupted server connection). </exception>
            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                // If ofObjectToReturn is null, then any of the following types can be returned for correct processing:
                // Stream, TextReader, XmlReader or descendants of XmlSchema
                var result = this.GetType().Assembly.GetManifestResourceStream(
                    string.Format("SM64_Diagnostic.Schemas.{0}", Path.GetFileName(absoluteUri.ToString())));

                // set a conditional breakpoint "result==null" here
                return result;
            }
        }

        public static Config OpenConfig(string path)
        {
            Config config = new Config();
            config.ObjectSlots = new ObjectSlotsConfig();
            config.ObjectGroups = new ObjectGroupsConfig();
            config.ObjectGroups.ProcessingGroups = new List<byte>();
            config.ObjectGroups.ProcessingGroupsColor = new Dictionary<byte, Color>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ConfigSchema.xsd", "ConfigSchema.xsd");
            schemaSet.Compile();

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
                                case "RotationOffset":
                                    config.ObjectSlots.ObjectRotationOffset = ParsingUtilities.ParseHex(subElement.Value);
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
                                case "CeilingYOffset":
                                    config.Mario.CeilingYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GroundYOffset":
                                    config.Mario.GroundYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HSpeedOffset":
                                    config.Mario.HSpeedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FloorTriangleOffset":
                                    config.Mario.FloorTriangleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Debug":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ToggleAddress":
                                    config.Debug.Toggle = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SettingAddress":
                                    config.Debug.Setting = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "TriangleOffsets":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "x1":
                                    config.TriangleOffsets.X1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "y1":
                                    config.TriangleOffsets.Y1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "z1":
                                    config.TriangleOffsets.Z1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "x2":
                                    config.TriangleOffsets.X2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "y2":
                                    config.TriangleOffsets.Y2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "z2":
                                    config.TriangleOffsets.Z2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "x3":
                                    config.TriangleOffsets.X3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "y3":
                                    config.TriangleOffsets.Y3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "z3":
                                    config.TriangleOffsets.Z3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "normX":
                                    config.TriangleOffsets.NormX = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "normY":
                                    config.TriangleOffsets.NormY = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "normZ":
                                    config.TriangleOffsets.NormZ = ParsingUtilities.ParseHex(subElement.Value);
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

                    case "MissionLayoutAddress":
                        config.MissionAddress = ParsingUtilities.ParseHex(element.Value);
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
                    case "CameraX":
                        config.CameraX = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "CameraY":
                        config.CameraY = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "CameraZ":
                        config.CameraZ = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "CameraRot":
                        config.CameraRot = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "RngRecordingAreaAddress":
                        config.RngRecordingAreaAddress = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "RngAddress":
                        config.RngAddress = ParsingUtilities.ParseHex(element.Value);
                        break;
                }
            }

            return config;
        }

        public static List<WatchVariable> OpenMiscData(string path)
        {
            var miscData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/MiscDataSchema.xsd", "MiscDataSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            var mainElements = doc.Root.Elements().ToArray();
            for(int i = 0; i < mainElements.Count(); i++)
            {
                var element = mainElements[i];
                var watchVar = GetWatchVariableFromElement(element);
                miscData.Add(watchVar);
            }

            return miscData;
        }

        public static List<WatchVariable> OpenObjectData(string path)
        {
            var objectData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ObjectDataSchema.xsd", "ObjectDataSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

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
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/MarioDataSchema.xsd", "MarioDataSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

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

        public static List<WatchVariable> OpenHudData(Config config, string path)
        {
            var objectData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/HudDataSchema.xsd", "HudDataSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                if (element.Name.ToString() != "Data")
                    continue;

                var watchVar = GetWatchVariableFromElement(element);
                objectData.Add(watchVar);
            }

            return objectData;
        }

        public static List<WatchVariable> OpenCameraData(Config config, string path)
        {
            var objectData = new List<WatchVariable>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/CameraDataSchema.xsd", "CameraDataSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                if (element.Name.ToString() != "Data")
                    continue;

                var watchVar = GetWatchVariableFromElement(element);

                objectData.Add(watchVar);
            }

            return objectData;
        }

        public static ObjectAssociations OpenObjectAssoc(string path)
        {
            var assoc = new ObjectAssociations();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ObjectAssociationsSchema.xsd", "ObjectAssociationsSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            // Create Behavior-ImagePath list
            string defaultImagePath = "", emptyImagePath = "", imageDir = "", mapImageDir = "",
                marioImagePath = "", holpMapImagePath = "", hudImagePath = "", debugImagePath = "", 
                miscImagePath = "", cameraImagePath = "", marioMapImagePath = "", cameraMapImagePath = "";
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
                        marioMapImagePath = element.Element(XName.Get("MapImage")) != null ?
                            element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value : null;
                        assoc.MarioColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        marioBehavior = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        break;

                    case "Hud":
                        hudImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.HudColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                            break;

                    case "Debug":
                        debugImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.DebugColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        break;

                    case "Misc":
                        miscImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.MiscColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        break;

                    case "Camera":
                        cameraImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.CameraColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        cameraMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Holp":
                        holpMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Object":
                        uint behaviorAddress = (ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value)
                            - ramToBehaviorOffset) & 0x00FFFFFF;
                        string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        string mapImagePath = null;
                        bool rotates = false;
                        if (element.Element(XName.Get("MapImage")) != null)
                        {
                            mapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                            rotates = bool.Parse(element.Element(XName.Get("MapImage")).Attribute(XName.Get("rotates")).Value);
                        }
                        string name = element.Attribute(XName.Get("name")).Value;
                        var watchVars = new List<WatchVariable>();
                        foreach (var subElement in element.Elements().Where(x => x.Name == "Data"))
                        {
                            var watchVar = GetWatchVariableFromElement(subElement);
                            watchVar.OtherOffset = (subElement.Attribute(XName.Get("objectOffset")) != null) ?
                                bool.Parse(subElement.Attribute(XName.Get("objectOffset")).Value) : false;

                            watchVars.Add(watchVar);
                        }

                        if (assoc.BehaviorAssociations.ContainsKey(behaviorAddress))
                            throw new Exception("More than one behavior address was defined.");

                        var newBehavior = new ObjectBehaviorAssociation()
                        {
                            Behavior = behaviorAddress,
                            ImagePath = imagePath,
                            MapImagePath = mapImagePath,
                            Name = name,
                            RotatesOnMap = rotates,
                            WatchVariables  = watchVars
                        };

                        assoc.AddAssociation(newBehavior);

                        break;
                }
            }

            // Load Images
            // TODO: Exceptions
            assoc.DefaultImage = Bitmap.FromFile(imageDir + defaultImagePath);
            assoc.EmptyImage = Bitmap.FromFile(imageDir + emptyImagePath);
            assoc.MarioImage = Bitmap.FromFile(imageDir + marioImagePath);
            assoc.CameraImage = Bitmap.FromFile(imageDir + cameraImagePath);
            assoc.MarioMapImage = marioMapImagePath == "" ? assoc.MarioImage : Bitmap.FromFile(mapImageDir + marioMapImagePath);
            assoc.HudImage = Bitmap.FromFile(imageDir + hudImagePath);
            assoc.DebugImage = Bitmap.FromFile(imageDir + debugImagePath);
            assoc.MiscImage = Bitmap.FromFile(imageDir + miscImagePath);
            assoc.HolpImage = Bitmap.FromFile(mapImageDir + holpMapImagePath);
            assoc.CameraMapImage = Bitmap.FromFile(mapImageDir + cameraMapImagePath);
            assoc.MarioBehavior = marioBehavior - ramToBehaviorOffset;
            foreach (var obj in assoc.BehaviorAssociations.Values)
            {
                using (var preLoad = Bitmap.FromFile(imageDir + obj.ImagePath))
                {
                    float scale = Math.Max(preLoad.Height / 128f, preLoad.Width / 128f);
                    obj.Image = new Bitmap(preLoad, new Size((int)(preLoad.Width / scale), (int)(preLoad.Height / scale)));
                }
                if (obj.MapImagePath == "" || obj.MapImagePath == null)
                {
                    obj.MapImage = obj.Image;
                }
                else
                {
                    using (var preLoad = Bitmap.FromFile(mapImageDir + obj.MapImagePath))
                    {
                        float scale = Math.Max(preLoad.Height / 128f, preLoad.Width / 128f);
                        obj.MapImage = new Bitmap(preLoad, new Size((int)(preLoad.Width / scale), (int)(preLoad.Height / scale)));
                    }
                }
                obj.TransparentImage = obj.Image.GetOpaqueImage(0.5f);
                obj.TransparentMapImage = obj.Image.GetOpaqueImage(0.5f);
            }

            return assoc;
        }

        public static MapAssociations OpenMapAssoc(string path)
        {
            var assoc = new MapAssociations();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/MapAssociationsSchema.xsd", "MapAssociationsSchema.xsd");
            schemaSet.Compile();

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
                        ushort? missionLayout = element.Attribute(XName.Get("missionLayout")) != null ?
                            (ushort?)ushort.Parse(element.Attribute(XName.Get("missionLayout")).Value) : null;
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

                        Map map = new Map() { Level = level, Area = area, LoadingPoint = loadingPoint, MissionLayout = missionLayout,
                            Coordinates = coordinates, ImagePath = imagePath, Y = y, Name = name, SubName = subName, BackgroundPath = bgImagePath};

                        assoc.AddAssociation(map);
                        break;
                }
            }

            return assoc;
        }

        public static ScriptParser OpenScripts(string path)
        {
            var parser = new ScriptParser();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ScriptsSchema.xsd", "ScriptsSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            string scriptDir = "";
            List<Tuple<string, uint>> scriptLocations = new List<Tuple<string, uint>>();

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ScriptDirectory":
                                    scriptDir = subElement.Value;
                                    break;
                                case "FreeMemoryArea":
                                    parser.FreeMemoryArea = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Script":
                        string scriptPath = element.Attribute(XName.Get("path")).Value;
                        uint insertAddress = ParsingUtilities.ParseHex(element.Attribute(XName.Get("insertAddress")).Value);
                        parser.AddScript(scriptDir + scriptPath, insertAddress, 0, 0);
                        break;
                }
            }

            return parser;
        }

        public static List<RomHack> OpenHacks(string path)
        {
            var hacks = new List<RomHack>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ScriptsSchema.xsd", "ScriptsSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            string hackDir = "";

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "HackDirectory":
                                    hackDir = subElement.Value;
                                    break;
                            }
                        }
                        break;

                    case "Hack":
                        string hackPath = hackDir + element.Attribute(XName.Get("path")).Value;
                        string name = element.Attribute(XName.Get("name")).Value;
                        hacks.Add(new RomHack(hackPath, name));
                        break;
                }
            }

            return hacks;
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
            watchVar.IsAngle = element.Attribute(XName.Get("isAngle")) != null ?
                bool.Parse(element.Attribute(XName.Get("isAngle")).Value) : false;
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
