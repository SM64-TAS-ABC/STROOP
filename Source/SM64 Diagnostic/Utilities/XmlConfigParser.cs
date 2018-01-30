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
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Controls;

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

        public static void OpenConfig(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            PositionControllerRelativityConfig.Relativity = PositionControllerRelativity.Recommended;
            PositionControllerRelativityConfig.CustomAngle = 32768;

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ConfigSchema.xsd", "ConfigSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach(var element in doc.Root.Elements())
            {
                switch(element.Name.ToString())
                {
                    case "Emulators":
                        foreach (var subElement in element.Elements())
                        {
                            Config.Emulators.Add(new Emulator()
                            {
                                Name = subElement.Attribute(XName.Get("name")).Value,
                                ProcessName = subElement.Attribute(XName.Get("processName")).Value,
                                RamStart = ParsingUtilities.ParseHex(subElement.Attribute(XName.Get("ramStart")).Value),
                                Dll = subElement.Attribute(XName.Get("offsetDll")) != null
                                    ? subElement.Attribute(XName.Get("offsetDll")).Value : null
                            });
                        }
                        break;
                    case "RomVersion":
                        Config.Version = (RomVersion)Enum.Parse(typeof(RomVersion), element.Value);
                        break;
                    case "RefreshRateFreq":
                        RefreshRateConfig.RefreshRateFreq = uint.Parse(element.Value);
                        break;
                    case "RamSize":
                        Config.RamSize = ParsingUtilities.ParseHex(element.Value);
                        break;
                }
            }
        }

        public static List<WatchVariableControl> OpenWatchVariableControls(string path, string schemaFile)
        {
            return OpenWatchVariableControlPrecursors(path, schemaFile)
                .ConvertAll(precursor => precursor.CreateWatchVariableControl());
        }

        public static List<WatchVariableControlPrecursor> OpenWatchVariableControlPrecursors(string path, string schemaFile)
        {
            var objectData = new List<WatchVariableControlPrecursor>();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/CameraDataSchema.xsd", schemaFile);
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                if (element.Name.ToString() != "Data")
                    continue;

                WatchVariableControlPrecursor watchVarControl = GetWatchVariablePrecursorFromElement(element);
                objectData.Add(watchVarControl);
            }

            return objectData;
        }

        public static ObjectAssociations OpenObjectAssoc(string path, ObjectSlotManagerGui objectSlotManagerGui)
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
            string defaultImagePath = "", emptyImagePath = "", imageDir = "", mapImageDir = "", overlayImageDir = "",
                marioImagePath = "", holpMapImagePath = "", intendedNextPositionImagePath = "", hudImagePath = "", debugImagePath = "",
                miscImagePath = "", cameraImagePath = "", marioMapImagePath = "", cameraMapImagePath = "",
                selectedOverlayImagePath = "", trackedAndShownOverlayImagePath = "", trackedNotShownOverlayImagePath = "",
                stoodOnOverlayImagePath = "", heldOverlayImagePath = "", interactionOverlayImagePath = "",
                usedOverlayImagePath = "", closestOverlayImagePath = "", cameraOverlayImagePath = "", cameraHackOverlayImagePath = "",
                modelOverlayImagePath = "", floorOverlayImagePath = "", wallOverlayImagePath = "", ceilingOverlayImagePath = "",
                parentOverlayImagePath = "", parentUnusedOverlayImagePath = "", parentNoneOverlayImagePath = "", markedOverlayImagePath = "";
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
                                case "OverlayImageDirectory":
                                    overlayImageDir = subElement.Value;
                                    break;
                                case "EmptyImage":
                                    emptyImagePath = subElement.Value;
                                    break;
                                case "SegmentTableUS":
                                    assoc.SegmentTableUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SegmentTableJP":
                                    assoc.SegmentTableJP = ParsingUtilities.ParseHex(subElement.Value);
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

                    case "IntendedNextPosition":
                        intendedNextPositionImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;
                    
                    case "Overlays":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "Selected":
                                    selectedOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "TrackedAndShown":
                                    trackedAndShownOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "TrackedNotShown":
                                    trackedNotShownOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "StoodOn":
                                    stoodOnOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Held":
                                    heldOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Interaction":
                                    interactionOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Used":
                                    usedOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;
                                    
                                case "Closest":
                                    closestOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Camera":
                                    cameraOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CameraHack":
                                    cameraHackOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Model":
                                    modelOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Floor":
                                    floorOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Wall":
                                    wallOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Ceiling":
                                    ceilingOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Parent":
                                    parentOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ParentUnused":
                                    parentUnusedOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ParentNone":
                                    parentNoneOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Marked":
                                    markedOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;
                            }
                        }
                        break;

                    case "Object":
                        string name = element.Attribute(XName.Get("name")).Value;
                        uint behaviorSegmented = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        uint? gfxId = null;
                        int? subType = null, appearance = null;
                        if (element.Attribute(XName.Get("gfxId")) != null)
                            gfxId = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxId")).Value) | 0x80000000U;
                        if (element.Attribute(XName.Get("subType")) != null)
                            subType = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("subType")).Value);
                        if (element.Attribute(XName.Get("appearance")) != null)
                            appearance = ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("appearance")).Value);

                        var spawnElement = element.Element(XName.Get("SpawnCode"));
                        if (spawnElement != null)
                        {
                            byte spawnGfxId = (byte)(spawnElement.Attribute(XName.Get("gfxId")) != null ? 
                                ParsingUtilities.ParseHex(spawnElement.Attribute(XName.Get("gfxId")).Value) : 0);
                            byte spawnExtra = (byte)(spawnElement.Attribute(XName.Get("extra")) != null ? 
                                ParsingUtilities.ParseHex(spawnElement.Attribute(XName.Get("extra")).Value) : (byte)(subType.HasValue ? subType : 0));
                            assoc.AddSpawnHack(new SpawnHack()
                            {
                                Name = name,
                                Behavior = behaviorSegmented,
                                GfxId = spawnGfxId,
                                Extra = spawnExtra
                            });
                        }

                        string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        string mapImagePath = null;
                        bool rotates = false;
                        if (element.Element(XName.Get("MapImage")) != null)
                        {
                            mapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                            rotates = bool.Parse(element.Element(XName.Get("MapImage")).Attribute(XName.Get("rotates")).Value);
                        }

                        List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
                        foreach (var subElement in element.Elements().Where(x => x.Name == "Data"))
                        {
                            WatchVariableControlPrecursor precursor = GetWatchVariablePrecursorFromElement(subElement);
                            precursors.Add(precursor);
                        }

                        var newBehavior = new ObjectBehaviorAssociation()
                        {
                            BehaviorCriteria = new BehaviorCriteria()
                            {
                                BehaviorAddress = behaviorSegmented,
                                GfxId = gfxId,
                                SubType = subType,
                                Appearance = appearance
                            },
                            ImagePath = imagePath,
                            MapImagePath = mapImagePath,
                            Name = name,
                            RotatesOnMap = rotates,
                            Precursors = precursors,
                        };

                        if (!assoc.AddAssociation(newBehavior))
                            throw new Exception("More than one behavior address was defined.");

                        break;
                }
            }

            // Load Images
            // TODO: Exceptions
            assoc.DefaultImage = Image.FromFile(imageDir + defaultImagePath);
            assoc.EmptyImage = Image.FromFile(imageDir + emptyImagePath);
            assoc.MarioImage = Image.FromFile(imageDir + marioImagePath);
            assoc.CameraImage = Image.FromFile(imageDir + cameraImagePath);
            assoc.MarioMapImage = marioMapImagePath == "" ? assoc.MarioImage : Image.FromFile(mapImageDir + marioMapImagePath);
            assoc.HudImage = Image.FromFile(imageDir + hudImagePath);
            assoc.DebugImage = Image.FromFile(imageDir + debugImagePath);
            assoc.MiscImage = Image.FromFile(imageDir + miscImagePath);
            assoc.HolpImage = Image.FromFile(mapImageDir + holpMapImagePath);
            assoc.IntendedNextPositionImage = Image.FromFile(mapImageDir + intendedNextPositionImagePath);
            assoc.CameraMapImage = Image.FromFile(mapImageDir + cameraMapImagePath);
            assoc.MarioBehavior = marioBehavior;
            objectSlotManagerGui.SelectedObjectOverlayImage = Image.FromFile(overlayImageDir + selectedOverlayImagePath);
            objectSlotManagerGui.TrackedAndShownObjectOverlayImage = Image.FromFile(overlayImageDir + trackedAndShownOverlayImagePath);
            objectSlotManagerGui.TrackedNotShownObjectOverlayImage = Image.FromFile(overlayImageDir + trackedNotShownOverlayImagePath);
            objectSlotManagerGui.StoodOnObjectOverlayImage = Image.FromFile(overlayImageDir + stoodOnOverlayImagePath);
            objectSlotManagerGui.HeldObjectOverlayImage = Image.FromFile(overlayImageDir + heldOverlayImagePath);
            objectSlotManagerGui.InteractionObjectOverlayImage = Image.FromFile(overlayImageDir + interactionOverlayImagePath);
            objectSlotManagerGui.UsedObjectOverlayImage = Image.FromFile(overlayImageDir + usedOverlayImagePath);
            objectSlotManagerGui.ClosestObjectOverlayImage = Image.FromFile(overlayImageDir + closestOverlayImagePath);
            objectSlotManagerGui.CameraObjectOverlayImage = Image.FromFile(overlayImageDir + cameraOverlayImagePath);
            objectSlotManagerGui.CameraHackObjectOverlayImage = Image.FromFile(overlayImageDir + cameraHackOverlayImagePath);
            objectSlotManagerGui.ModelObjectOverlayImage = Image.FromFile(overlayImageDir + modelOverlayImagePath);
            objectSlotManagerGui.FloorObjectOverlayImage = Image.FromFile(overlayImageDir + floorOverlayImagePath);
            objectSlotManagerGui.WallObjectOverlayImage = Image.FromFile(overlayImageDir + wallOverlayImagePath);
            objectSlotManagerGui.CeilingObjectOverlayImage = Image.FromFile(overlayImageDir + ceilingOverlayImagePath);
            objectSlotManagerGui.ParentObjectOverlayImage = Image.FromFile(overlayImageDir + parentOverlayImagePath);
            objectSlotManagerGui.ParentUnusedObjectOverlayImage = Image.FromFile(overlayImageDir + parentUnusedOverlayImagePath);
            objectSlotManagerGui.ParentNoneObjectOverlayImage = Image.FromFile(overlayImageDir + parentNoneOverlayImagePath);
            objectSlotManagerGui.MarkedObjectOverlayImage = Image.FromFile(overlayImageDir + markedOverlayImagePath);

            foreach (var obj in assoc.BehaviorAssociations)
            {
                if (obj.ImagePath == null || obj.ImagePath == "")
                    continue;

                using (var preLoad = Image.FromFile(imageDir + obj.ImagePath))
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
                    using (var preLoad = Image.FromFile(mapImageDir + obj.MapImagePath))
                    {
                        float scale = Math.Max(preLoad.Height / 128f, preLoad.Width / 128f);
                        obj.MapImage = new Bitmap(preLoad, new Size((int)(preLoad.Width / scale), (int)(preLoad.Height / scale)));
                    }
                }
                obj.TransparentImage = obj.Image.GetOpaqueImage(0.5f);
            }

            return assoc;
        }

        public static void OpenInputImageAssoc(string path, InputImageGui inputImageGui)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/InputImageAssociationsSchema.xsd", "InputImageAssociationsSchema.xsd");
            schemaSet.Compile();
            
            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            // Create path list
            string inputImageDir = "",
                   buttonAPath = "",
                   buttonBPath = "",
                   buttonZPath = "",
                   buttonStartPath = "",
                   buttonRPath = "",
                   buttonLPath = "",
                   buttonCUpPath = "",
                   buttonCDownPath = "",
                   buttonCLeftPath = "",
                   buttonCRightPath = "",
                   buttonDUpPath = "",
                   buttonDDownPath = "",
                   buttonDLeftPath = "",
                   buttonDRightPath = "",
                   controlStickPath = "",
                   controllerPath = "";

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "InputImageDirectory":
                                    inputImageDir = subElement.Value;
                                    break;
                            }
                        }
                        break;

                    case "InputImages":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ButtonA":
                                    buttonAPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonB":
                                    buttonBPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonZ":
                                    buttonZPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonStart":
                                    buttonStartPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonR":
                                    buttonRPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonL":
                                    buttonLPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonCUp":
                                    buttonCUpPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonCDown":
                                    buttonCDownPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonCLeft":
                                    buttonCLeftPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonCRight":
                                    buttonCRightPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonDUp":
                                    buttonDUpPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonDDown":
                                    buttonDDownPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonDLeft":
                                    buttonDLeftPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonDRight":
                                    buttonDRightPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ControlStick":
                                    controlStickPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Controller":
                                    controllerPath = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;
                            }
                        }
                        break;
                }
            }

            // Load Images
            // TODO: Exceptions
            inputImageGui.ButtonAImage = Image.FromFile(inputImageDir + buttonAPath);
            inputImageGui.ButtonBImage = Image.FromFile(inputImageDir + buttonBPath);
            inputImageGui.ButtonZImage = Image.FromFile(inputImageDir + buttonZPath);
            inputImageGui.ButtonStartImage = Image.FromFile(inputImageDir + buttonStartPath);

            inputImageGui.ButtonRImage = Image.FromFile(inputImageDir + buttonRPath);
            inputImageGui.ButtonLImage = Image.FromFile(inputImageDir + buttonLPath);

            inputImageGui.ButtonCUpImage = Image.FromFile(inputImageDir + buttonCUpPath);
            inputImageGui.ButtonCDownImage = Image.FromFile(inputImageDir + buttonCDownPath);
            inputImageGui.ButtonCLeftImage = Image.FromFile(inputImageDir + buttonCLeftPath);
            inputImageGui.ButtonCRightImage = Image.FromFile(inputImageDir + buttonCRightPath);

            inputImageGui.ButtonDUpImage = Image.FromFile(inputImageDir + buttonDUpPath);
            inputImageGui.ButtonDDownImage = Image.FromFile(inputImageDir + buttonDDownPath);
            inputImageGui.ButtonDLeftImage = Image.FromFile(inputImageDir + buttonDLeftPath);
            inputImageGui.ButtonDRightImage = Image.FromFile(inputImageDir + buttonDRightPath);

            inputImageGui.ControlStickImage = Image.FromFile(inputImageDir + controlStickPath);
            inputImageGui.ControllerImage = Image.FromFile(inputImageDir + controllerPath);
        }

        public static void OpenFileImageAssoc(string path, FileImageGui fileImageGui)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/FileImageAssociationsSchema.xsd", "FileImageAssociationsSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            // Create path list
            string fileImageDir = "",
                   powerStarPath = "",
                   powerStarBlackPath = "",
                   cannonPath = "",
                   cannonLidPath = "",
                   door1StarPath = "",
                   door3StarPath = "",
                   doorBlackPath = "",
                   starDoorOpenPath = "",
                   starDoorClosedPath = "",
                   capSwitchRedPressedPath = "",
                   capSwitchRedUnpressedPath = "",
                   capSwitchGreenPressedPath = "",
                   capSwitchGreenUnpressedPath = "",
                   capSwitchBluePressedPath = "",
                   capSwitchBlueUnpressedPath = "",
                   fileStartedPath = "",
                   fileNotStartedPath = "",
                   dddPaintingMovedBackPath = "",
                   dddPaintingNotMovedBackPath = "",
                   moatDrainedPath = "",
                   moatNotDrainedPath = "",
                   keyDoorClosedPath = "",
                   keyDoorClosedKeyPath = "",
                   keyDoorOpenPath = "",
                   keyDoorOpenKeyPath = "",
                   hatOnMarioPath = "",
                   hatOnMarioGreyPath = "",
                   hatOnKleptoPath = "",
                   hatOnKleptoGreyPath = "",
                   hatOnSnowmanPath = "",
                   hatOnSnowmanGreyPath = "",
                   hatOnUkikiPath = "",
                   hatOnUkikiGreyPath = "",
                   hatOnGroundInSSLPath = "",
                   hatOnGroundInSSLGreyPath = "",
                   hatOnGroundInSLPath = "",
                   hatOnGroundInSLGreyPath = "",
                   hatOnGroundInTTMPath = "",
                   hatOnGroundInTTMGrey = "";

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "FileImageDirectory":
                                    fileImageDir = subElement.Value;
                                    break;
                            }
                        }
                        break;

                    case "FileImages":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "PowerStar":
                                    powerStarPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "PowerStarBlack":
                                    powerStarBlackPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Cannon":
                                    cannonPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CannonLid":
                                    cannonLidPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Door1Star":
                                    door1StarPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Door3Star":
                                    door3StarPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "DoorBlack":
                                    doorBlackPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "StarDoorOpen":
                                    starDoorOpenPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "StarDoorClosed":
                                    starDoorClosedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchRedPressed":
                                    capSwitchRedPressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchRedUnpressed":
                                    capSwitchRedUnpressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchGreenPressed":
                                    capSwitchGreenPressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchGreenUnpressed":
                                    capSwitchGreenUnpressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchBluePressed":
                                    capSwitchBluePressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "CapSwitchBlueUnpressed":
                                    capSwitchBlueUnpressedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "FileStarted":
                                    fileStartedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "FileNotStarted":
                                    fileNotStartedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "DDDPaintingMovedBack":
                                    dddPaintingMovedBackPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "DDDPaintingNotMovedBack":
                                    dddPaintingNotMovedBackPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MoatDrained":
                                    moatDrainedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MoatNotDrained":
                                    moatNotDrainedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "KeyDoorClosed":
                                    keyDoorClosedPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "KeyDoorClosedKey":
                                    keyDoorClosedKeyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "KeyDoorOpen":
                                    keyDoorOpenPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "KeyDoorOpenKey":
                                    keyDoorOpenKeyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnMario":
                                    hatOnMarioPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnMarioGrey":
                                    hatOnMarioGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnKlepto":
                                    hatOnKleptoPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnKleptoGrey":
                                    hatOnKleptoGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnSnowman":
                                    hatOnSnowmanPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnSnowmanGrey":
                                    hatOnSnowmanGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnUkiki":
                                    hatOnUkikiPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnUkikiGrey":
                                    hatOnUkikiGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInSSL":
                                    hatOnGroundInSSLPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInSSLGrey":
                                    hatOnGroundInSSLGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInSL":
                                    hatOnGroundInSLPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInSLGrey":
                                    hatOnGroundInSLGreyPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInTTM":
                                    hatOnGroundInTTMPath = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HatOnGroundInTTMGrey":
                                    hatOnGroundInTTMGrey = subElement.Element(XName.Get("FileImage")).Attribute(XName.Get("path")).Value;
                                    break;
                            }
                        }
                        break;
                }
            }

            // Load Images
            // TODO: Exceptions
            fileImageGui.PowerStarImage = Image.FromFile(fileImageDir + powerStarPath);
            fileImageGui.PowerStarBlackImage = Image.FromFile(fileImageDir + powerStarBlackPath);
            fileImageGui.CannonImage = Image.FromFile(fileImageDir + cannonPath);
            fileImageGui.CannonLidImage = Image.FromFile(fileImageDir + cannonLidPath);
            fileImageGui.Door1StarImage = Image.FromFile(fileImageDir + door1StarPath);
            fileImageGui.Door3StarImage = Image.FromFile(fileImageDir + door3StarPath);
            fileImageGui.DoorBlackImage = Image.FromFile(fileImageDir + doorBlackPath);
            fileImageGui.StarDoorOpenImage = Image.FromFile(fileImageDir + starDoorOpenPath);
            fileImageGui.StarDoorClosedImage = Image.FromFile(fileImageDir + starDoorClosedPath);
            fileImageGui.CapSwitchRedPressedImage = Image.FromFile(fileImageDir + capSwitchRedPressedPath);
            fileImageGui.CapSwitchRedUnpressedImage = Image.FromFile(fileImageDir + capSwitchRedUnpressedPath);
            fileImageGui.CapSwitchGreenPressedImage = Image.FromFile(fileImageDir + capSwitchGreenPressedPath);
            fileImageGui.CapSwitchGreenUnpressedImage = Image.FromFile(fileImageDir + capSwitchGreenUnpressedPath);
            fileImageGui.CapSwitchBluePressedImage = Image.FromFile(fileImageDir + capSwitchBluePressedPath);
            fileImageGui.CapSwitchBlueUnpressedImage = Image.FromFile(fileImageDir + capSwitchBlueUnpressedPath);
            fileImageGui.FileStartedImage = Image.FromFile(fileImageDir + fileStartedPath);
            fileImageGui.FileNotStartedImage = Image.FromFile(fileImageDir + fileNotStartedPath);
            fileImageGui.DDDPaintingMovedBackImage = Image.FromFile(fileImageDir + dddPaintingMovedBackPath);
            fileImageGui.DDDPaintingNotMovedBackImage = Image.FromFile(fileImageDir + dddPaintingNotMovedBackPath);
            fileImageGui.MoatDrainedImage = Image.FromFile(fileImageDir + moatDrainedPath);
            fileImageGui.MoatNotDrainedImage = Image.FromFile(fileImageDir + moatNotDrainedPath);
            fileImageGui.KeyDoorClosedImage = Image.FromFile(fileImageDir + keyDoorClosedPath);
            fileImageGui.KeyDoorClosedKeyImage = Image.FromFile(fileImageDir + keyDoorClosedKeyPath);
            fileImageGui.KeyDoorOpenImage = Image.FromFile(fileImageDir + keyDoorOpenPath);
            fileImageGui.KeyDoorOpenKeyImage = Image.FromFile(fileImageDir + keyDoorOpenKeyPath);
            fileImageGui.HatOnMarioImage = Image.FromFile(fileImageDir + hatOnMarioPath);
            fileImageGui.HatOnMarioGreyImage = Image.FromFile(fileImageDir + hatOnMarioGreyPath);
            fileImageGui.HatOnKleptoImage = Image.FromFile(fileImageDir + hatOnKleptoPath);
            fileImageGui.HatOnKleptoGreyImage = Image.FromFile(fileImageDir + hatOnKleptoGreyPath);
            fileImageGui.HatOnSnowmanImage = Image.FromFile(fileImageDir + hatOnSnowmanPath);
            fileImageGui.HatOnSnowmanGreyImage = Image.FromFile(fileImageDir + hatOnSnowmanGreyPath);
            fileImageGui.HatOnUkikiImage = Image.FromFile(fileImageDir + hatOnUkikiPath);
            fileImageGui.HatOnUkikiGreyImage = Image.FromFile(fileImageDir + hatOnUkikiGreyPath);
            fileImageGui.HatOnGroundInSSLImage = Image.FromFile(fileImageDir + hatOnGroundInSSLPath);
            fileImageGui.HatOnGroundInSSLGreyImage = Image.FromFile(fileImageDir + hatOnGroundInSSLGreyPath);
            fileImageGui.HatOnGroundInSLImage = Image.FromFile(fileImageDir + hatOnGroundInSLPath);
            fileImageGui.HatOnGroundInSLGreyImage = Image.FromFile(fileImageDir + hatOnGroundInSLGreyPath);
            fileImageGui.HatOnGroundInTTMImage = Image.FromFile(fileImageDir + hatOnGroundInTTMPath);
            fileImageGui.HatOnGroundInTTMGreyImage = Image.FromFile(fileImageDir + hatOnGroundInTTMGrey);
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

                    case "SpawnHack":
                        string spawnHackPath = hackDir + element.Attribute(XName.Get("path")).Value;
                        HackConfig.SpawnHack = new RomHack(spawnHackPath, "Spawn Hack");
                        break;

                    case "Hack":
                        string hackPath = hackDir + element.Attribute(XName.Get("path")).Value;
                        string name = element.Attribute(XName.Get("name")).Value;
                        RomHack romHack = new RomHack(hackPath, name);
                        hacks.Add(romHack);
                        if (name == "Display Variable") VarHackConfig.ShowVarRomHack = romHack;
                        break;
                }
            }

            return hacks;
        }

        public static WatchVariableControl GetWatchVariableControlFromElement(XElement element)
        {
            return GetWatchVariablePrecursorFromElement(element).CreateWatchVariableControl();
        }

        public static WatchVariableControlPrecursor GetWatchVariablePrecursorFromElement(XElement element)
        {
            string name = element.Value;

            BaseAddressTypeEnum baseAddressType = WatchVariableUtilities.GetBaseAddressType(element.Attribute(XName.Get("base")).Value);

            WatchVariableSubclass subclass = WatchVariableUtilities.GetSubclass(element.Attribute(XName.Get("subclass"))?.Value);

            List<VariableGroup> groupList = WatchVariableUtilities.ParseVariableGroupList(element.Attribute(XName.Get("groupList"))?.Value);

            string specialType = element.Attribute(XName.Get("specialType"))?.Value;

            Color? backgroundColor = (element.Attribute(XName.Get("color")) != null) ?
                ColorTranslator.FromHtml(element.Attribute(XName.Get("color")).Value) : (Color?)null;

            string typeName = (element.Attribute(XName.Get("type"))?.Value);

            uint? mask = element.Attribute(XName.Get("mask")) != null ?
                (uint?)ParsingUtilities.ParseHex(element.Attribute(XName.Get("mask")).Value) : null;

            uint? offsetUS = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetUS"))?.Value);
            uint? offsetJP = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetJP"))?.Value);
            uint? offsetPAL = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetPAL"))?.Value);
            uint? offsetDefault = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offset"))?.Value);

            if (offsetDefault.HasValue && (offsetUS.HasValue || offsetJP.HasValue || offsetPAL.HasValue))
            {
                throw new ArgumentOutOfRangeException("Can't have both a default offset value and a rom-specific offset value");
            }

            if (specialType != null)
            {
                if (baseAddressType != BaseAddressTypeEnum.None &&
                    baseAddressType != BaseAddressTypeEnum.Object &&
                    baseAddressType != BaseAddressTypeEnum.Triangle)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have base address type " + baseAddressType);
                }

                if (offsetDefault.HasValue || offsetUS.HasValue || offsetJP.HasValue || offsetPAL.HasValue)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have any type of offset");
                }

                if (mask != null)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have mask");
                }
            }

            WatchVariable watchVar =
                new WatchVariable(
                    typeName,
                    specialType,
                    baseAddressType,
                    offsetUS,
                    offsetJP,
                    offsetPAL,
                    offsetDefault,
                    mask);

            bool? useHex = (element.Attribute(XName.Get("useHex")) != null) ?
                bool.Parse(element.Attribute(XName.Get("useHex")).Value) : (bool?)null;

            bool? invertBool = element.Attribute(XName.Get("invertBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("invertBool")).Value) : (bool?)null;

            WatchVariableCoordinate? coordinate = element.Attribute(XName.Get("coord")) != null ?
                WatchVariableUtilities.GetCoordinate(element.Attribute(XName.Get("coord")).Value) : (WatchVariableCoordinate?)null;

            if (subclass == WatchVariableSubclass.Angle && specialType != null)
            {
                if (typeName != "ushort" && typeName != "short" && typeName != "uint" && typeName != "int")
                {
                    throw new ArgumentOutOfRangeException("Special angle vars must have a good type");
                }
            }

            if (useHex.HasValue && (subclass == WatchVariableSubclass.String))
            {
                throw new ArgumentOutOfRangeException("useHex cannot be used with var subclass String");
            }

            if ((useHex == true) && (subclass == WatchVariableSubclass.Object))
            {
                throw new ArgumentOutOfRangeException("useHex as true is redundant with var subclass Object");
            }

            if (invertBool.HasValue && (subclass != WatchVariableSubclass.Boolean))
            {
                throw new ArgumentOutOfRangeException("invertBool must be used with var subclass Boolean");
            }

            if (coordinate.HasValue && (subclass == WatchVariableSubclass.String))
            {
                throw new ArgumentOutOfRangeException("coordinate cannot be used with var subclass String");
            }

            return new WatchVariableControlPrecursor(
                name,
                watchVar,
                subclass,
                backgroundColor,
                useHex,
                invertBool,
                coordinate,
                groupList);
        }

        public static ActionTable OpenActionTable(string path)
        {
            ActionTable actionTable = null;
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ActionTableSchema.xsd", "ActionTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                switch(element.Name.ToString())
                {
                    case "Default":
                        uint defaultAfterCloneValue = ParsingUtilities.ParseHex(
                            element.Attribute(XName.Get("afterCloneValue")).Value);
                        uint defaultAfterUncloneValue = ParsingUtilities.ParseHex(
                            element.Attribute(XName.Get("afterUncloneValue")).Value);
                        uint defaultHandsfreeValue = ParsingUtilities.ParseHex(
                            element.Attribute(XName.Get("handsfreeValue")).Value);
                        actionTable = new ActionTable(defaultAfterCloneValue, defaultAfterUncloneValue, defaultHandsfreeValue);
                        break;

                    case "Action":
                        uint actionValue = ParsingUtilities.ParseHex(
                            element.Attribute(XName.Get("value")).Value);
                        string actionName = element.Attribute(XName.Get("name")).Value;
                        uint? afterCloneValue = element.Attribute(XName.Get("afterCloneValue")) != null ?
                            ParsingUtilities.ParseHex(element.Attribute(XName.Get("afterCloneValue")).Value) : (uint?) null;
                        uint? afterUncloneValue = element.Attribute(XName.Get("afterUncloneValue")) != null ?
                            ParsingUtilities.ParseHex(element.Attribute(XName.Get("afterUncloneValue")).Value) : (uint?) null;
                        uint? handsfreeValue = element.Attribute(XName.Get("handsfreeValue")) != null ?
                            ParsingUtilities.ParseHex(element.Attribute(XName.Get("handsfreeValue")).Value) : (uint?)null;
                        actionTable?.Add(new ActionTable.ActionReference()
                        {
                            Action = actionValue,
                            ActionName = actionName,
                            AfterClone = afterCloneValue,
                            AfterUnclone = afterUncloneValue,
                            Handsfree = handsfreeValue
                        });
                        break;
                }
            }

            return actionTable;
        }

        public static AnimationTable OpenAnimationTable(string path)
        {
            AnimationTable animationTable = new AnimationTable();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/AnimationTableSchema.xsd", "AnimationTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                int animationValue = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("value")).Value);
                string animationName = element.Attribute(XName.Get("name")).Value;
                animationTable.Add(new AnimationTable.AnimationReference()
                {
                    AnimationValue = animationValue,
                    AnimationName = animationName
                });
            }

            return animationTable;
        }

        public static CourseDataTable OpenCourseDataTable(string path)
        {
            CourseDataTable courseDataTable = new CourseDataTable();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/CourseDataTableSchema.xsd", "CourseDataTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                int index = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("index")).Value);
                string fullName = element.Attribute(XName.Get("fullName")).Value;
                string shortName = element.Attribute(XName.Get("shortName")).Value;
                byte maxCoinsWithoutGlitches = (byte)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("maxCoinsWithoutGlitches")).Value);
                byte maxCoinsWithGlitches = (byte)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("maxCoinsWithGlitches")).Value);
                courseDataTable.Add(new CourseDataTable.CourseDataReference()
                {
                    Index = index,
                    FullName = fullName,
                    ShortName = shortName,
                    MaxCoinsWithoutGlitches = maxCoinsWithoutGlitches,
                    MaxCoinsWithGlitches = maxCoinsWithGlitches
                });
            }

            return courseDataTable;
        }

        public static PendulumSwingTable OpenPendulumSwingTable(string path)
        {
            PendulumSwingTable pendulumSwingTable = new PendulumSwingTable();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/PendulumSwingTableSchema.xsd", "PendulumSwingTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                int index = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("index")).Value);
                int amplitude = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("amplitude")).Value);
                pendulumSwingTable.Add(new PendulumSwingTable.PendulumSwingReference()
                {
                    Index = index,
                    Amplitude = amplitude
                });
            }

            return pendulumSwingTable;
        }

        public static WaypointTable OpenWaypointTable(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/WaypointTableSchema.xsd", "WaypointTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            List<WaypointTable.WaypointReference> waypoints = new List<WaypointTable.WaypointReference>();
            foreach (XElement element in doc.Root.Elements())
            {
                short index = (short)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("index")).Value);
                short x = (short)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("x")).Value);
                short y = (short)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("y")).Value);
                short z = (short)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("z")).Value);
                waypoints.Add(new WaypointTable.WaypointReference()
                {
                    Index = index,
                    X = x,
                    Y = y,
                    Z = z,
                });
            }

            return new WaypointTable(waypoints);
        }

        public static MissionTable OpenMissionTable(string path)
        {
            MissionTable missionTable = new MissionTable();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/MissionTableSchema.xsd", "MissionTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                int courseIndex = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("courseIndex")).Value);
                int missionIndex = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("missionIndex")).Value);
                string missionName = element.Attribute(XName.Get("missionName")).Value;
                missionTable.Add(new MissionTable.MissionReference()
                {
                    CourseIndex = courseIndex,
                    MissionIndex = missionIndex,
                    MissionName = missionName,
                });
            }

            return missionTable;
        }
        
        private static void Validation(object sender, ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }
    }
}
