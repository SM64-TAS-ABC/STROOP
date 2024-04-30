﻿using System;
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
using STROOP.Map;

namespace STROOP.Utilities
{
    public static class XmlConfigParser
    {
        private static string FixPathSep(string s)
        {
            return Path.DirectorySeparatorChar == '\\' ? s : s.Replace('\\', Path.DirectorySeparatorChar);
        }

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
                    string.Format("STROOP.Schemas.{0}", Path.GetFileName(absoluteUri.ToString())));

                // set a conditional breakpoint "result==null" here
                return result;
            }
        }

        public static void OpenConfig(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

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
                            string special = subElement.Attribute(XName.Get("special")) != null ?
                                subElement.Attribute(XName.Get("special")).Value : null;
                            bool autoDetect = subElement.Attribute(XName.Get("autoDetect")) != null ?
                                bool.Parse(subElement.Attribute(XName.Get("autoDetect")).Value) : false;
                            uint ramStart = subElement.Attribute(XName.Get("ramStart")) != null ?
                                ParsingUtilities.ParseHex(subElement.Attribute(XName.Get("ramStart")).Value) : 0;
                            Config.Emulators.Add(new Emulator()
                            {
                                Name = subElement.Attribute(XName.Get("name")).Value,
                                ProcessName = subElement.Attribute(XName.Get("processName")).Value,
                                RamStart = ramStart,
                                Dll = subElement.Attribute(XName.Get("offsetDll")) != null
                                    ? subElement.Attribute(XName.Get("offsetDll")).Value : null,
                                Endianness = subElement.Attribute(XName.Get("endianness")).Value == "big" 
                                    ? EndiannessType.Big : EndiannessType.Little,
                                IOType = special == "dolphin" ? typeof(DolphinProcessIO) : typeof(WindowsProcessRamIO),
                                AutoDetect = autoDetect,
                            });
                        }
                        break;
                    case "RomVersion":
                        RomVersionConfig.Version = (RomVersion)Enum.Parse(typeof(RomVersion), element.Value);
                        break;
                    case "RefreshRateFreq":
                        RefreshRateConfig.RefreshRateFreq = uint.Parse(element.Value);
                        break;
                }
            }
        }

        public static void OpenSavedSettings(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/ReusableTypes.xsd", "ReusableTypes.xsd");
            schemaSet.Add("http://tempuri.org/ConfigSchema.xsd", "ConfigSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (var element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "UseNightMode":
                        SavedSettingsConfig.UseNightMode = bool.Parse(element.Value);
                        break;
                    case "DisplayYawAnglesAsUnsigned":
                        SavedSettingsConfig.DisplayYawAnglesAsUnsigned = bool.Parse(element.Value);
                        break;
                    case "VariableValuesFlushRight":
                        SavedSettingsConfig.VariableValuesFlushRight = bool.Parse(element.Value);
                        break;
                    case "StartSlotIndexsFromOne":
                        SavedSettingsConfig.StartSlotIndexsFromOne = bool.Parse(element.Value);
                        break;
                    case "OffsetGotoRetrieveFunctions":
                        SavedSettingsConfig.OffsetGotoRetrieveFunctions = bool.Parse(element.Value);
                        break;
                    case "MoveCameraWithPu":
                        SavedSettingsConfig.MoveCameraWithPu = bool.Parse(element.Value);
                        break;
                    case "ScaleDiagonalPositionControllerButtons":
                        SavedSettingsConfig.ScaleDiagonalPositionControllerButtons = bool.Parse(element.Value);
                        break;
                    case "ExcludeDustForClosestObject":
                        SavedSettingsConfig.ExcludeDustForClosestObject = bool.Parse(element.Value);
                        break;
                    case "UseMisalignmentOffsetForDistanceToLine":
                        SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine = bool.Parse(element.Value);
                        break;
                    case "DontRoundValuesToZero":
                        SavedSettingsConfig.DontRoundValuesToZero = bool.Parse(element.Value);
                        break;
                    case "DisplayAsHexUsesMemory":
                        SavedSettingsConfig.DisplayAsHexUsesMemory = bool.Parse(element.Value);
                        break;
                    case "NeutralizeTrianglesWith0x15":
                        SavedSettingsConfig.NeutralizeTrianglesWith0x15 = bool.Parse(element.Value);
                        break;
                    case "CloningUpdatesHolpType":
                        SavedSettingsConfig.CloningUpdatesHolpType = bool.Parse(element.Value);
                        break;
                    case "UseInGameTrigForAngleLogic":
                        SavedSettingsConfig.UseInGameTrigForAngleLogic = bool.Parse(element.Value);
                        break;
                    case "UseExtendedLevelBoundaries":
                        SavedSettingsConfig.UseExtendedLevelBoundaries = bool.Parse(element.Value);
                        break;
                    case "UseExpandedRamSize":
                        SavedSettingsConfig.UseExpandedRamSize = bool.Parse(element.Value);
                        break;
                    case "DoQuickStartup":
                        SavedSettingsConfig.DoQuickStartup = bool.Parse(element.Value);
                        break;

                    case "TabOrder":
                        {
                            List<string> tabNames = new List<string>();
                            foreach (var tabName in element.Elements())
                            {
                                tabNames.Add(tabName.Value);
                            }
                            SavedSettingsConfig.InitiallySavedTabOrder = tabNames;
                        }
                        break;

                    case "RemovedTabs":
                        {
                            List<string> tabNames = new List<string>();
                            foreach (var tabName in element.Elements())
                            {
                                tabNames.Add(tabName.Value);
                            }
                            SavedSettingsConfig.InitiallySavedRemovedTabs = tabNames;
                        }
                        break;
                }
            }
            SavedSettingsConfig.IsLoaded = true;
        }

        public static List<WatchVariableControl> OpenWatchVariableControls(string path)
        {
            return OpenWatchVariableControlPrecursors(path)
                .ConvertAll(precursor => precursor.CreateWatchVariableControl());
        }

        public static List<WatchVariableControlPrecursor> OpenWatchVariableControlPrecursors(string path)
        {
            string schemaFile = "MiscDataSchema.xsd";
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

                WatchVariableControlPrecursor watchVarControl = new WatchVariableControlPrecursor(element);
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
                marioImagePath = "", holpMapImagePath = "", greenHolpMapImagePath = "",
                homeMapImagePath = "", intendedNextPositionImagePath = "", hudImagePath = "", debugImagePath = "",
                miscImagePath = "", cameraImagePath = "", marioMapImagePath = "", cameraMapImagePath = "",
                blueMarioMapImagePath = "", greenMarioMapImagePath = "", orangeMarioMapImagePath = "", purpleMarioMapImagePath = "", turquoiseMarioMapImagePath = "",
                yellowMarioMapImagePath = "", pinkMarioMapImagePath = "", brownMarioMapImagePath = "", whiteMarioMapImagePath = "", greyMarioMapImagePath = "",
                redCircleImagePath = "", blueCircleImagePath = "", yellowCircleImagePath = "",
                cameraFocusImagePath = "", triangleFloorImagePath = "", triangleWallImagePath = "", triangleCeilingImagePath = "", triangleOtherImagePath = "", hitboxTrisImagePath = "",
                cellGridlinesImagePath = "", currentCellImagePath = "", unitGridlinesImagePath = "", currentUnitImagePath = "",
                nextPositionsImagePath = "", previousPositionsImagePath = "", arrowImagePath = "", iwerlipsesImagePath = "", cylinderImagePath = "", sphereImagePath = "",
                pathImagePath = "", customPointsImagePath = "", customGridlinesImagePath = "",
                selectedOverlayImagePath = "", trackedAndShownOverlayImagePath = "", trackedNotShownOverlayImagePath = "",
                stoodOnOverlayImagePath = "", riddenOverlayImagePath = "", heldOverlayImagePath = "", interactionOverlayImagePath = "",
                usedOverlayImagePath = "", closestOverlayImagePath = "", cameraOverlayImagePath = "", cameraHackOverlayImagePath = "",
                modelOverlayImagePath = "", floorOverlayImagePath = "", wallOverlayImagePath = "", ceilingOverlayImagePath = "",
                parentOverlayImagePath = "", parentUnusedOverlayImagePath = "", parentNoneOverlayImagePath = "", childOverlayImagePath = "",
                collision1OverlayImagePath = "", collision2OverlayImagePath = "", collision3OverlayImagePath = "", collision4OverlayImagePath = "", hitboxOverlapImagePath = "",
                markedRedOverlayImagePath = "", markedOrangeOverlayImagePath = "", markedYellowOverlayImagePath = "", markedGreenOverlayImagePath = "",
                markedLightBlueOverlayImagePath = "", markedBlueOverlayImagePath = "", markedPurpleOverlayImagePath = "", markedPinkOverlayImagePath = "",
                markedGreyOverlayImagePath = "", markedWhiteOverlayImagePath = "", markedBlackOverlayImagePath = "",
                lockedImagePath = "", lockDisabledImagePath = "", lockReadOnlyImagePath = "",
                aggregatedPathImagePath = "", angleRangeImagePath = "", branchPathImagePath = "", coffinBoxImagePath = "",
                compassImagePath = "", coordinateLabelsImagePath = "", facingDividerImagePath = "", homeLineImagePath = "",
                ledgeGrabCheckerImagePath = "", lineSegmentImagePath = "", sectorImagePath = "", cameraViewImagePath = "", watersImagePath = "";
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
                                    imageDir = FixPathSep(subElement.Value);
                                    break;
                                case "DefaultImage":
                                    defaultImagePath = subElement.Value;
                                    break;
                                case "MapImageDirectory":
                                    mapImageDir = FixPathSep(subElement.Value);
                                    break;
                                case "OverlayImageDirectory":
                                    overlayImageDir = FixPathSep(subElement.Value);
                                    break;
                                case "EmptyImage":
                                    emptyImagePath = subElement.Value;
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

                    case "GreenHolp":
                        greenHolpMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "BlueMario":
                        blueMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "GreenMario":
                        greenMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "OrangeMario":
                        orangeMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "PurpleMario":
                        purpleMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "TurquoiseMario":
                        turquoiseMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "YellowMario":
                        yellowMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "PinkMario":
                        pinkMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "BrownMario":
                        brownMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "WhiteMario":
                        whiteMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "GreyMario":
                        greyMarioMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "RedCircle":
                        redCircleImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "BlueCircle":
                        blueCircleImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "YellowCircle":
                        yellowCircleImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CameraFocus":
                        cameraFocusImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Home":
                        homeMapImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "IntendedNextPosition":
                        intendedNextPositionImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "TriangleFloor":
                        triangleFloorImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "TriangleWall":
                        triangleWallImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "TriangleCeiling":
                        triangleCeilingImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "TriangleOther":
                        triangleOtherImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "HitboxTris":
                        hitboxTrisImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CellGridlines":
                        cellGridlinesImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CurrentCell":
                        currentCellImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "UnitGridlines":
                        unitGridlinesImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CurrentUnit":
                        currentUnitImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "NextPositions":
                        nextPositionsImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "PreviousPositions":
                        previousPositionsImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Arrow":
                        arrowImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Iwerlipses":
                        iwerlipsesImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Cylinder":
                        cylinderImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Sphere":
                        sphereImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Path":
                        pathImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CustomPoints":
                        customPointsImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CustomGridlines":
                        customGridlinesImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "AggregatedPath":
                        aggregatedPathImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "AngleRange":
                        angleRangeImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "BranchPath":
                        branchPathImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CoffinBox":
                        coffinBoxImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Compass":
                        compassImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CoordinateLabels":
                        coordinateLabelsImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "FacingDivider":
                        facingDividerImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "HomeLine":
                        homeLineImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "LedgeGrabChecker":
                        ledgeGrabCheckerImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "LineSegment":
                        lineSegmentImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Sector":
                        sectorImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "CameraView":
                        cameraViewImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
                        break;

                    case "Waters":
                        watersImagePath = element.Element(XName.Get("MapImage")).Attribute(XName.Get("path")).Value;
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

                                case "Ridden":
                                    riddenOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
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

                                case "Child":
                                    childOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Collision1":
                                    collision1OverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Collision2":
                                    collision2OverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Collision3":
                                    collision3OverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Collision4":
                                    collision4OverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "HitboxOverlap":
                                    hitboxOverlapImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedRed":
                                    markedRedOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedOrange":
                                    markedOrangeOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedYellow":
                                    markedYellowOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedGreen":
                                    markedGreenOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedLightBlue":
                                    markedLightBlueOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedBlue":
                                    markedBlueOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedPurple":
                                    markedPurpleOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedPink":
                                    markedPinkOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedGrey":
                                    markedGreyOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedWhite":
                                    markedWhiteOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "MarkedBlack":
                                    markedBlackOverlayImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "Locked":
                                    lockedImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "LockDisabled":
                                    lockDisabledImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "LockReadOnly":
                                    lockReadOnlyImagePath = subElement.Element(XName.Get("OverlayImage")).Attribute(XName.Get("path")).Value;
                                    break;
                            }
                        }
                        break;

                    case "Object":
                        string name = element.Attribute(XName.Get("name")).Value;
                        uint behaviorSegmented = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        uint? gfxIdUS = null;
                        uint? gfxIdJP = null;
                        uint? gfxIdSH = null;
                        uint? gfxIdEU = null;
                        uint? subType = null;
                        uint? appearance = null;
                        uint? spawnObjUS = null;
                        uint? spawnObjJP = null;
                        uint? spawnObjSH = null;
                        uint? spawnObjEU = null;

                        if (element.Attribute(XName.Get("gfxIdUS")) != null)
                            gfxIdUS = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxIdUS")).Value) | 0x80000000U;
                        if (element.Attribute(XName.Get("gfxIdJP")) != null)
                            gfxIdJP = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxIdJP")).Value) | 0x80000000U;
                        if (element.Attribute(XName.Get("gfxIdSH")) != null)
                            gfxIdSH = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxIdSH")).Value) | 0x80000000U;
                        if (element.Attribute(XName.Get("gfxIdEU")) != null)
                            gfxIdEU = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxIdEU")).Value) | 0x80000000U;

                        if (element.Attribute(XName.Get("subType")) != null)
                            subType = ParsingUtilities.ParseUIntNullable(element.Attribute(XName.Get("subType")).Value);
                        if (element.Attribute(XName.Get("appearance")) != null)
                            appearance = ParsingUtilities.ParseUIntNullable(element.Attribute(XName.Get("appearance")).Value);

                        if (element.Attribute(XName.Get("spawnObjUS")) != null)
                            spawnObjUS = ParsingUtilities.ParseHex(element.Attribute(XName.Get("spawnObjUS")).Value);
                        if (element.Attribute(XName.Get("spawnObjJP")) != null)
                            spawnObjJP = ParsingUtilities.ParseHex(element.Attribute(XName.Get("spawnObjJP")).Value);
                        if (element.Attribute(XName.Get("spawnObjSH")) != null)
                            spawnObjSH = ParsingUtilities.ParseHex(element.Attribute(XName.Get("spawnObjSH")).Value);
                        if (element.Attribute(XName.Get("spawnObjEU")) != null)
                            spawnObjEU = ParsingUtilities.ParseHex(element.Attribute(XName.Get("spawnObjEU")).Value);

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

                        PushHitbox pushHitbox = null;
                        var pushHitboxElement = element.Element(XName.Get("PushHitbox"));
                        if (pushHitboxElement != null)
                        {
                            int? padding = ParsingUtilities.ParseIntNullable(pushHitboxElement.Attribute(XName.Get("padding"))?.Value);
                            int? radius = ParsingUtilities.ParseIntNullable(pushHitboxElement.Attribute(XName.Get("radius"))?.Value);
                            int? extentY = ParsingUtilities.ParseIntNullable(pushHitboxElement.Attribute(XName.Get("extentY"))?.Value);
                            bool isKoopaTheQuick = ParsingUtilities.ParseBoolNullable(pushHitboxElement.Attribute(XName.Get("isKoopaTheQuick"))?.Value) ?? false;
                            bool isRacingPenguin = ParsingUtilities.ParseBoolNullable(pushHitboxElement.Attribute(XName.Get("isRacingPenguin"))?.Value) ?? false;
                            pushHitbox = new PushHitbox(padding, radius, extentY, isKoopaTheQuick, isRacingPenguin);
                        }

                        List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
                        foreach (var subElement in element.Elements().Where(x => x.Name == "Data"))
                        {
                            WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(subElement);
                            precursors.Add(precursor);
                        }

                        var newBehavior = new ObjectBehaviorAssociation()
                        {
                            Criteria = new BehaviorCriteria()
                            {
                                BehaviorAddress = behaviorSegmented,
                                GfxIdUS = gfxIdUS,
                                GfxIdJP = gfxIdJP,
                                GfxIdSH = gfxIdSH,
                                GfxIdEU = gfxIdEU,
                                SubType = subType,
                                Appearance = appearance,
                                SpawnObjUS = spawnObjUS,
                                SpawnObjJP = spawnObjJP,
                                SpawnObjSH = spawnObjSH,
                                SpawnObjEU = spawnObjEU,
                            },
                            ImagePath = imagePath,
                            MapImagePath = mapImagePath,
                            Name = name,
                            RotatesOnMap = rotates,
                            PushHitbox = pushHitbox,
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
            assoc.GreenHolpImage = Image.FromFile(mapImageDir + greenHolpMapImagePath);
            assoc.HomeImage = Image.FromFile(mapImageDir + homeMapImagePath);
            assoc.IntendedNextPositionImage = Image.FromFile(mapImageDir + intendedNextPositionImagePath);
            assoc.CameraMapImage = Image.FromFile(mapImageDir + cameraMapImagePath);
            assoc.BlueMarioMapImage = Image.FromFile(mapImageDir + blueMarioMapImagePath);
            assoc.GreenMarioMapImage = Image.FromFile(mapImageDir + greenMarioMapImagePath);
            assoc.OrangeMarioMapImage = Image.FromFile(mapImageDir + orangeMarioMapImagePath);
            assoc.PurpleMarioMapImage = Image.FromFile(mapImageDir + purpleMarioMapImagePath);
            assoc.TurqoiseMarioMapImage = Image.FromFile(mapImageDir + turquoiseMarioMapImagePath);
            assoc.YellowMarioMapImage = Image.FromFile(mapImageDir + yellowMarioMapImagePath);
            assoc.PinkMarioMapImage = Image.FromFile(mapImageDir + pinkMarioMapImagePath);
            assoc.BrownMarioMapImage = Image.FromFile(mapImageDir + brownMarioMapImagePath);
            assoc.WhiteMarioMapImage = Image.FromFile(mapImageDir + whiteMarioMapImagePath);
            assoc.GreyMarioMapImage = Image.FromFile(mapImageDir + greyMarioMapImagePath);
            assoc.CameraFocusMapImage = Image.FromFile(mapImageDir + cameraFocusImagePath);

            assoc.RedCircleMapImage = Image.FromFile(mapImageDir + redCircleImagePath);
            assoc.BlueCircleMapImage = Image.FromFile(mapImageDir + blueCircleImagePath);
            assoc.YellowCircleMapImage = Image.FromFile(mapImageDir + yellowCircleImagePath);

            assoc.AddEmptyAssociation(); // Need to do this after Empty Image is set

            assoc.TriangleFloorImage = Image.FromFile(mapImageDir + triangleFloorImagePath);
            assoc.TriangleWallImage = Image.FromFile(mapImageDir + triangleWallImagePath);
            assoc.TriangleCeilingImage = Image.FromFile(mapImageDir + triangleCeilingImagePath);
            assoc.TriangleOtherImage = Image.FromFile(mapImageDir + triangleOtherImagePath);
            assoc.HitboxTrisImage = Image.FromFile(mapImageDir + hitboxTrisImagePath);

            assoc.CellGridlinesImage = Image.FromFile(mapImageDir + cellGridlinesImagePath);
            assoc.CurrentCellImage = Image.FromFile(mapImageDir + currentCellImagePath);
            assoc.UnitGridlinesImage = Image.FromFile(mapImageDir + unitGridlinesImagePath);
            assoc.CurrentUnitImage = Image.FromFile(mapImageDir + currentUnitImagePath);
            assoc.NextPositionsImage = Image.FromFile(mapImageDir + nextPositionsImagePath);
            assoc.PreviousPositionsImage = Image.FromFile(mapImageDir + previousPositionsImagePath);
            assoc.ArrowImage = Image.FromFile(mapImageDir + arrowImagePath);
            assoc.IwerlipsesImage = Image.FromFile(mapImageDir + iwerlipsesImagePath);
            assoc.CylinderImage = Image.FromFile(mapImageDir + cylinderImagePath);
            assoc.SphereImage = Image.FromFile(mapImageDir + sphereImagePath);
            assoc.PathImage = Image.FromFile(mapImageDir + pathImagePath);
            assoc.CustomPointsImage = Image.FromFile(mapImageDir + customPointsImagePath);
            assoc.CustomGridlinesImage = Image.FromFile(mapImageDir + customGridlinesImagePath);

            assoc.AggregatedPathImage = Image.FromFile(mapImageDir + aggregatedPathImagePath);
            assoc.AngleRangeImage = Image.FromFile(mapImageDir + angleRangeImagePath);
            assoc.BranchPathImage = Image.FromFile(mapImageDir + branchPathImagePath);
            assoc.CoffinBoxImage = Image.FromFile(mapImageDir + coffinBoxImagePath);
            assoc.CompassImage = Image.FromFile(mapImageDir + compassImagePath);
            assoc.CoordinateLabelsImage = Image.FromFile(mapImageDir + coordinateLabelsImagePath);
            assoc.FacingDividerImage = Image.FromFile(mapImageDir + facingDividerImagePath);
            assoc.HomeLineImage = Image.FromFile(mapImageDir + homeLineImagePath);
            assoc.LedgeGrabCheckerImage = Image.FromFile(mapImageDir + ledgeGrabCheckerImagePath);
            assoc.LineSegmentImage = Image.FromFile(mapImageDir + lineSegmentImagePath);
            assoc.SectorImage = Image.FromFile(mapImageDir + sectorImagePath);
            assoc.CameraViewImage = Image.FromFile(mapImageDir + cameraViewImagePath);
            assoc.WatersImage = Image.FromFile(mapImageDir + watersImagePath);


            assoc.MarioBehavior = marioBehavior;

            objectSlotManagerGui.SelectedObjectOverlayImage = Image.FromFile(overlayImageDir + selectedOverlayImagePath);
            objectSlotManagerGui.TrackedAndShownObjectOverlayImage = Image.FromFile(overlayImageDir + trackedAndShownOverlayImagePath);
            objectSlotManagerGui.TrackedNotShownObjectOverlayImage = Image.FromFile(overlayImageDir + trackedNotShownOverlayImagePath);
            objectSlotManagerGui.StoodOnObjectOverlayImage = Image.FromFile(overlayImageDir + stoodOnOverlayImagePath);
            objectSlotManagerGui.RiddenObjectOverlayImage = Image.FromFile(overlayImageDir + riddenOverlayImagePath);
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
            objectSlotManagerGui.ChildObjectOverlayImage = Image.FromFile(overlayImageDir + childOverlayImagePath);
            objectSlotManagerGui.Collision1OverlayImage = Image.FromFile(overlayImageDir + collision1OverlayImagePath);
            objectSlotManagerGui.Collision2OverlayImage = Image.FromFile(overlayImageDir + collision2OverlayImagePath);
            objectSlotManagerGui.Collision3OverlayImage = Image.FromFile(overlayImageDir + collision3OverlayImagePath);
            objectSlotManagerGui.Collision4OverlayImage = Image.FromFile(overlayImageDir + collision4OverlayImagePath);
            objectSlotManagerGui.HitboxOverlapImage = Image.FromFile(overlayImageDir + hitboxOverlapImagePath);
            objectSlotManagerGui.MarkedRedObjectOverlayImage = Image.FromFile(overlayImageDir + markedRedOverlayImagePath);
            objectSlotManagerGui.MarkedOrangeObjectOverlayImage = Image.FromFile(overlayImageDir + markedOrangeOverlayImagePath);
            objectSlotManagerGui.MarkedYellowObjectOverlayImage = Image.FromFile(overlayImageDir + markedYellowOverlayImagePath);
            objectSlotManagerGui.MarkedGreenObjectOverlayImage = Image.FromFile(overlayImageDir + markedGreenOverlayImagePath);
            objectSlotManagerGui.MarkedLightBlueObjectOverlayImage = Image.FromFile(overlayImageDir + markedLightBlueOverlayImagePath);
            objectSlotManagerGui.MarkedBlueObjectOverlayImage = Image.FromFile(overlayImageDir + markedBlueOverlayImagePath);
            objectSlotManagerGui.MarkedPurpleObjectOverlayImage = Image.FromFile(overlayImageDir + markedPurpleOverlayImagePath);
            objectSlotManagerGui.MarkedPinkObjectOverlayImage = Image.FromFile(overlayImageDir + markedPinkOverlayImagePath);
            objectSlotManagerGui.MarkedGreyObjectOverlayImage = Image.FromFile(overlayImageDir + markedGreyOverlayImagePath);
            objectSlotManagerGui.MarkedWhiteObjectOverlayImage = Image.FromFile(overlayImageDir + markedWhiteOverlayImagePath);
            objectSlotManagerGui.MarkedBlackObjectOverlayImage = Image.FromFile(overlayImageDir + markedBlackOverlayImagePath);
            objectSlotManagerGui.LockedOverlayImage = Image.FromFile(overlayImageDir + lockedImagePath);
            objectSlotManagerGui.LockDisabledOverlayImage = Image.FromFile(overlayImageDir + lockDisabledImagePath);
            objectSlotManagerGui.LockReadOnlyOverlayImage = Image.FromFile(overlayImageDir + lockReadOnlyImagePath);

            objectSlotManagerGui.InitializeMarkedColorDictionary();

            foreach (var obj in assoc.BehaviorAssociations)
            {
                if (obj.ImagePath == null || obj.ImagePath == "")
                    continue;

                obj.Image = new LazyImage(imageDir + obj.ImagePath);
                if (obj.MapImagePath == "" || obj.MapImagePath == null)
                {
                    obj.MapImage = obj.Image;
                }
                else
                {
                    obj.MapImage = new LazyImage(mapImageDir + obj.MapImagePath);
                }
                obj.TransparentImage = new LazyImage(obj.Image, 0.5f);
            }

            return assoc;
        }

        public static List<InputImageGui> CreateInputImageAssocList(string path)
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

            List<InputImageGui> guiList = new List<InputImageGui>();
            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "Config":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "ClassicInputImageDirectory":
                                    guiList.Add(CreateInputImageAssoc(
                                        path, FixPathSep(subElement.Value), InputDisplayTypeEnum.Classic));
                                    break;
                                case "SleekInputImageDirectory":
                                    guiList.Add(CreateInputImageAssoc(
                                        path, FixPathSep(subElement.Value), InputDisplayTypeEnum.Sleek));
                                    break;
                                case "VerticalInputImageDirectory":
                                    guiList.Add(CreateInputImageAssoc(
                                        path, FixPathSep(subElement.Value), InputDisplayTypeEnum.Vertical));
                                    break;
                            }
                        }
                        break;
                }
            }
            return guiList;
        }

        public static InputImageGui CreateInputImageAssoc(
            string path, string inputImageDir, InputDisplayTypeEnum inputDisplayType)
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
            string buttonAPath = "",
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
                   buttonU1Path = "",
                   buttonU2Path = "",
                   controlStickPath = "",
                   controllerPath = "";

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
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

                                case "ButtonU1":
                                    buttonU1Path = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
                                    break;

                                case "ButtonU2":
                                    buttonU2Path = subElement.Element(XName.Get("InputImage")).Attribute(XName.Get("path")).Value;
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
            return new InputImageGui()
            {
                InputDisplayType = inputDisplayType,

                ButtonAImage = Image.FromFile(inputImageDir + buttonAPath),
                ButtonBImage = Image.FromFile(inputImageDir + buttonBPath),
                ButtonZImage = Image.FromFile(inputImageDir + buttonZPath),
                ButtonStartImage = Image.FromFile(inputImageDir + buttonStartPath),

                ButtonRImage = Image.FromFile(inputImageDir + buttonRPath),
                ButtonLImage = Image.FromFile(inputImageDir + buttonLPath),

                ButtonCUpImage = Image.FromFile(inputImageDir + buttonCUpPath),
                ButtonCDownImage = Image.FromFile(inputImageDir + buttonCDownPath),
                ButtonCLeftImage = Image.FromFile(inputImageDir + buttonCLeftPath),
                ButtonCRightImage = Image.FromFile(inputImageDir + buttonCRightPath),

                ButtonDUpImage = Image.FromFile(inputImageDir + buttonDUpPath),
                ButtonDDownImage = Image.FromFile(inputImageDir + buttonDDownPath),
                ButtonDLeftImage = Image.FromFile(inputImageDir + buttonDLeftPath),
                ButtonDRightImage = Image.FromFile(inputImageDir + buttonDRightPath),

                ButtonU1Image = Image.FromFile(inputImageDir + buttonU1Path),
                ButtonU2Image = Image.FromFile(inputImageDir + buttonU2Path),

                ControlStickImage = Image.FromFile(inputImageDir + controlStickPath),
                ControllerImage = Image.FromFile(inputImageDir + controllerPath)
            };
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
                                    fileImageDir = FixPathSep(subElement.Value);
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
                                case "MapImageDirectory":
                                    assoc.MapImageFolderPath = FixPathSep(subElement.Value);
                                    break;
                                case "BackgroundImageDirectory":
                                    assoc.BackgroundImageFolderPath = FixPathSep(subElement.Value);
                                    break;
                                case "DefaultImage":
                                    var defaultMap = new MapLayout() { ImagePath = subElement.Value };
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

                    case "Background":
                        {
                            string name = element.Attribute(XName.Get("name")).Value;
                            string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                            Bitmap image = Image.FromFile(assoc.BackgroundImageFolderPath + imagePath) as Bitmap;
                            BackgroundImage backgroundImage = new BackgroundImage()
                            {
                                Name = name,
                                Image = image,
                            };
                            assoc.AddBackgroundImage(backgroundImage);
                        }
                        break;

                    case "Map":
                        {
                            string id = element.Attribute(XName.Get("id")).Value;
                            byte level = byte.Parse(element.Attribute(XName.Get("level")).Value);
                            byte area = byte.Parse(element.Attribute(XName.Get("area")).Value);
                            ushort? loadingPoint = element.Attribute(XName.Get("loadingPoint")) != null ?
                                (ushort?)ushort.Parse(element.Attribute(XName.Get("loadingPoint")).Value) : null;
                            ushort? missionLayout = element.Attribute(XName.Get("missionLayout")) != null ?
                                (ushort?)ushort.Parse(element.Attribute(XName.Get("missionLayout")).Value) : null;
                            string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;

                            string backgroundImageName = (element.Element(XName.Get("BackgroundImage")) != null) ?
                              element.Element(XName.Get("BackgroundImage")).Attribute(XName.Get("name")).Value : null;
                            BackgroundImage? backgroundImage = assoc.GetBackgroundImage(backgroundImageName);

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

                            MapLayout map = new MapLayout()
                            {
                                Id = id,
                                Level = level,
                                Area = area,
                                LoadingPoint = loadingPoint,
                                MissionLayout = missionLayout,
                                Coordinates = coordinates,
                                ImagePath = imagePath,
                                Y = y,
                                Name = name,
                                SubName = subName,
                                Background = backgroundImage,
                            };

                            assoc.AddAssociation(map);
                        }
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
                                    hackDir = FixPathSep(subElement.Value);
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
                        if (name == "Display Variable 2") VarHackConfig.ShowVarRomHack2 = romHack;
                        if (name == "Previous Positions (U)") MapObjectPreviousPositions._romHackUS = romHack;
                        if (name == "Previous Positions (J)") MapObjectPreviousPositions._romHackJP = romHack;
                        break;
                }
            }

            return hacks;
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

        public static TriangleInfoTable OpenTriangleInfoTable(string path)
        {
            TriangleInfoTable table = new TriangleInfoTable();
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/TriangleInfoTableSchema.xsd", "TriangleInfoTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            foreach (XElement element in doc.Root.Elements())
            {
                short type = short.Parse(element.Attribute(XName.Get("type")).Value);
                string description = element.Attribute(XName.Get("description")).Value;
                short slipperiness = (short)ParsingUtilities.ParseHex(
                    element.Attribute(XName.Get("slipperiness")).Value);
                bool exertion = bool.Parse(element.Attribute(XName.Get("exertion")).Value);

                table?.Add(new TriangleInfoTable.TriangleInfoReference()
                {
                    Type = type,
                    Description = description,
                    Slipperiness = slipperiness,
                    Exertion = exertion,
                });
            }

            return table;
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

            pendulumSwingTable.FillInExtended();

            return pendulumSwingTable;
        }

        public static PendulumVertexTable OpenPendulumVertexTable(string path)
        {
            return null;
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

        public static PointTable OpenPointTable(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/WaypointTableSchema.xsd", "WaypointTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            List<PointTable.PointReference> points = new List<PointTable.PointReference>();
            foreach (XElement element in doc.Root.Elements())
            {
                int index = ParsingUtilities.ParseInt (element.Attribute(XName.Get("index")).Value);
                double x = ParsingUtilities.ParseDouble(element.Attribute(XName.Get("x")).Value);
                double y = ParsingUtilities.ParseDouble(element.Attribute(XName.Get("y")).Value);
                double z = ParsingUtilities.ParseDouble(element.Attribute(XName.Get("z")).Value);
                points.Add(new PointTable.PointReference()
                {
                    Index = index,
                    X = x,
                    Y = y,
                    Z = z,
                });
            }

            return new PointTable(points);
        }

        public static MusicTable OpenMusicTable(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Create schema set
            var schemaSet = new XmlSchemaSet() { XmlResolver = new ResourceXmlResolver() };
            schemaSet.Add("http://tempuri.org/WaypointTableSchema.xsd", "WaypointTableSchema.xsd");
            schemaSet.Compile();

            // Load and validate document
            var doc = XDocument.Load(path);
            doc.Validate(schemaSet, Validation);

            List<MusicEntry> musicEntries = new List<MusicEntry>();
            foreach (XElement element in doc.Root.Elements())
            {
                int index = ParsingUtilities.ParseInt(element.Attribute(XName.Get("index")).Value);
                string name = element.Attribute(XName.Get("name")).Value;
                musicEntries.Add(new MusicEntry(index, name));
            }

            return new MusicTable(musicEntries);
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
                int inGameCourseIndex = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("inGameCourseIndex")).Value);
                int inGameMissionIndex = (int)ParsingUtilities.ParseIntNullable(element.Attribute(XName.Get("inGameMissionIndex")).Value);
                string missionName = element.Attribute(XName.Get("missionName")).Value;
                missionTable.Add(new MissionTable.MissionReference()
                {
                    CourseIndex = courseIndex,
                    MissionIndex = missionIndex,
                    InGameCourseIndex = inGameCourseIndex,
                    InGameMissionIndex = inGameMissionIndex,
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
