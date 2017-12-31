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
using static SM64_Diagnostic.Structs.Configurations.PositionControllerRelativeAngleConfig;
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
            Config.ObjectSlots = new ObjectSlotsConfig();
            Config.ObjectGroups = new ObjectGroupsConfig();
            Config.ObjectGroups.ProcessingGroups = new List<byte>();
            Config.ObjectGroups.ProcessingGroupsColor = new Dictionary<byte, Color>();
            var assembly = Assembly.GetExecutingAssembly();

            Config.PositionControllerRelativeAngle.Relativity = RelativityType.Recommended;
            Config.PositionControllerRelativeAngle.CustomAngle = 32768;

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
                        Config.Version = (Config.RomVersion)Enum.Parse(typeof(Config.RomVersion), element.Value);
                        break;
                    case "RefreshRateFreq":
                        Config.RefreshRateFreq = uint.Parse(element.Value);
                        break;
                    case "RamSize":
                        Config.RamSize = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "ObjectSlots":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "FirstObjectAddressUS":
                                    Config.ObjectSlots.LinkStartAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FirstObjectAddressJP":
                                    Config.ObjectSlots.LinkStartAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectStructSize":
                                    Config.ObjectSlots.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HeaderOffset":
                                    Config.ObjectSlots.HeaderOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ListNextLinkOffset":
                                    Config.ObjectSlots.NextLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ListPreviousLinkOffset":
                                    Config.ObjectSlots.PreviousLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ParentOffset":
                                    Config.ObjectSlots.ParentOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "UnusedSlotAddressUS":
                                    Config.ObjectSlots.UnusedSlotAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "UnusedSlotAddressJP":
                                    Config.ObjectSlots.UnusedSlotAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "BehaviorScriptOffset":
                                    Config.ObjectSlots.BehaviorScriptOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "BehaviorGfxOffset":
                                    Config.ObjectSlots.BehaviorGfxOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "BehaviorSubtypeOffset":
                                    Config.ObjectSlots.BehaviorSubtypeOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "BehaviorAppearance":
                                    Config.ObjectSlots.BehaviorAppearance = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ModelPointerOffset":
                                    Config.ObjectSlots.ModelPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectActiveOffset":
                                    Config.ObjectSlots.ObjectActiveOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TimerOffset":
                                    Config.ObjectSlots.TimerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "AnimationOffset":
                                    Config.ObjectSlots.AnimationOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioGraphic":
                                    Config.ObjectSlots.MarioGraphic = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "MarioBehavior":
                                    Config.ObjectSlots.MarioBehavior = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "DustSpawnerBehavior":
                                    Config.ObjectSlots.DustSpawnerBehavior = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "DustBallBehavior":
                                    Config.ObjectSlots.DustBallBehavior = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "DustBehavior":
                                    Config.ObjectSlots.DustBehavior = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CoordinateOffsetX":
                                    Config.ObjectSlots.ObjectXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetY":
                                    Config.ObjectSlots.ObjectYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetZ":
                                    Config.ObjectSlots.ObjectZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "YSpeedOffset":
                                    Config.ObjectSlots.YSpeedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HSpeedOffset":
                                    Config.ObjectSlots.HSpeedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HomeOffsetX":
                                    Config.ObjectSlots.HomeXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HomeOffsetY":
                                    Config.ObjectSlots.HomeYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HomeOffsetZ":
                                    Config.ObjectSlots.HomeZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "RotationOffset":
                                    Config.ObjectSlots.ObjectRotationOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "GraphicsOffsetX":
                                    Config.ObjectSlots.GraphicsXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GraphicsOffsetY":
                                    Config.ObjectSlots.GraphicsYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GraphicsOffsetZ":
                                    Config.ObjectSlots.GraphicsZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GraphicsOffsetYaw":
                                    Config.ObjectSlots.GraphicsYawOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GraphicsOffsetPitch":
                                    Config.ObjectSlots.GraphicsPitchOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "GraphicsOffsetRoll":
                                    Config.ObjectSlots.GraphicsRollOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "MaxObjectSlots":
                                    Config.ObjectSlots.MaxSlots = int.Parse(subElement.Value);
                                    break;
                                case "HitboxRadius":
                                    Config.ObjectSlots.HitboxRadius = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HitboxHeight":
                                    Config.ObjectSlots.HitboxHeight = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HitboxDownOffset":
                                    Config.ObjectSlots.HitboxDownOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YawFacingOffset":
                                    Config.ObjectSlots.YawFacingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PitchFacingOffset":
                                    Config.ObjectSlots.PitchFacingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "RollFacingOffset":
                                    Config.ObjectSlots.RollFacingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YawMovingOffset":
                                    Config.ObjectSlots.YawMovingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PitchMovingOffset":
                                    Config.ObjectSlots.PitchMovingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "RollMovingOffset":
                                    Config.ObjectSlots.RollMovingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ScaleWidthOffset":
                                    Config.ObjectSlots.ScaleWidthOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ScaleHeightOffset":
                                    Config.ObjectSlots.ScaleHeightOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ScaleDepthOffset":
                                    Config.ObjectSlots.ScaleDepthOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ReleaseStatusOffset":
                                    Config.ObjectSlots.ReleaseStatusOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StackIndexOffset":
                                    Config.ObjectSlots.StackIndexOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StackIndexReleasedValue":
                                    Config.ObjectSlots.StackIndexReleasedValue = uint.Parse(subElement.Value);
                                    break;
                                case "StackIndexUnReleasedValue":
                                    Config.ObjectSlots.StackIndexUnReleasedValue = uint.Parse(subElement.Value);
                                    break;
                                case "InitialReleaseStatusOffset":
                                    Config.ObjectSlots.InitialReleaseStatusOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ReleaseStatusThrownValue":
                                    Config.ObjectSlots.ReleaseStatusThrownValue = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ReleaseStatusDroppedValue":
                                    Config.ObjectSlots.ReleaseStatusDroppedValue = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "InteractionStatusOffset":
                                    Config.ObjectSlots.InteractionStatusOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PendulumAccelerationDirection":
                                    Config.ObjectSlots.PendulumAccelerationDirection = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PendulumAccelerationMagnitude":
                                    Config.ObjectSlots.PendulumAccelerationMagnitude = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PendulumAngularVelocity":
                                    Config.ObjectSlots.PendulumAngularVelocity = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PendulumAngle":
                                    Config.ObjectSlots.PendulumAngle = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "WaypointOffset":
                                    Config.ObjectSlots.WaypointOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PitchToWaypointOffset":
                                    Config.ObjectSlots.PitchToWaypointOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "RacingPenguinEffortOffset":
                                    Config.ObjectSlots.RacingPenguinEffortOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "KoopaTheQuickHSpeedMultiplierOffset":
                                    Config.ObjectSlots.KoopaTheQuickHSpeedMultiplierOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FlyGuyOscillationTimerOffset":
                                    Config.ObjectSlots.FlyGuyOscillationTimerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ScuttlebugTargetAngleOffset":
                                    Config.ObjectSlots.ScuttlebugTargetAngleOffset = ParsingUtilities.ParseHex(subElement.Value);
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
                                    Config.ObjectGroups.ProcessNextLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ProcessPreviousLinkOffset":
                                    Config.ObjectGroups.ProcessPreviousLinkOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ParentObjectOffset":
                                    Config.ObjectGroups.ParentObjectOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FirstObjectGroupingAddressUS":
                                    Config.ObjectGroups.FirstGroupingAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FirstObjectGroupingAddressJP":
                                    Config.ObjectGroups.FirstGroupingAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "VacantPointerAddressUS":
                                    Config.ObjectGroups.VactantPointerAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    Config.ObjectGroups.VacantSlotColor = ColorTranslator.FromHtml(subElement.Attribute(XName.Get("color")).Value);
                                    break;
                                case "VacantPointerAddressJP":
                                    Config.ObjectGroups.VactantPointerAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    Config.ObjectGroups.VacantSlotColor = ColorTranslator.FromHtml(subElement.Attribute(XName.Get("color")).Value);
                                    break;
                                case "ProcessGroupStructSize":
                                    Config.ObjectGroups.ProcessGroupStructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ProcessGroupsOrdering":
                                    foreach(var subSubElement in subElement.Elements())
                                    {
                                        var group = (byte)ParsingUtilities.ParseHex(
                                            subSubElement.Attribute(XName.Get("index")).Value);
                                        var color = ColorTranslator.FromHtml(
                                            subSubElement.Attribute(XName.Get("color")).Value);

                                        Config.ObjectGroups.ProcessingGroups.Add(group);
                                        Config.ObjectGroups.ProcessingGroupsColor.Add(group,color);
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
                                case "MarioStructAddressUS":
                                    Config.Mario.StructAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioStructAddressJP":
                                    Config.Mario.StructAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetX":
                                    Config.Mario.XOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetY":
                                    Config.Mario.YOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoordinateOffsetZ":
                                    Config.Mario.ZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FacingAngleOffset":
                                    Config.Mario.RotationOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YawFacingOffset":
                                    Config.Mario.YawFacingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YawIntendedOffset":
                                    Config.Mario.YawIntendedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioStructSize":
                                    Config.Mario.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ActionOffset":
                                    Config.Mario.ActionOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PrevActionOffset":
                                    Config.Mario.PrevActionOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StoodOnObjectPointerUS":
                                    Config.Mario.StoodOnObjectPointerUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StoodOnObjectPointerJP":
                                    Config.Mario.StoodOnObjectPointerJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "InteractionObjectPointerOffset":
                                    Config.Mario.InteractionObjectPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break; 
                                case "HeldObjectPointerOffset":
                                    Config.Mario.HeldObjectPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "UsedObjectPointerOffset":
                                    Config.Mario.UsedObjectPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CeilingYOffset":
                                    Config.Mario.CeilingYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FloorYOffset":
                                    Config.Mario.FloorYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HSpeedOffset":
                                    Config.Mario.HSpeedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "VSpeedOffset":
                                    Config.Mario.VSpeedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FloorTriangleOffset":
                                    Config.Mario.FloorTriangleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "WallTriangleOffset":
                                    Config.Mario.WallTriangleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CeilingTriangleOffset":
                                    Config.Mario.CeilingTriangleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SlidingSpeedXOffset":
                                    Config.Mario.SlidingSpeedXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SlidingSpeedZOffset":
                                    Config.Mario.SlidingSpeedZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SlidingYawOffset":
                                    Config.Mario.SlidingYawOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TwirlYawOffset":
                                    Config.Mario.TwirlYawOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "PeakHeightOffset":
                                    Config.Mario.PeakHeightOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectReferenceAddressUS":
                                    Config.Mario.ObjectReferenceAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectReferenceAddressJP":
                                    Config.Mario.ObjectReferenceAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectAnimationOffset":
                                    Config.Mario.ObjectAnimationOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ObjectAnimationTimerOffset":
                                    Config.Mario.ObjectAnimationTimerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HOLPXOffset":
                                    Config.Mario.HOLPXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HOLPYOffset":
                                    Config.Mario.HOLPYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HOLPZOffset":
                                    Config.Mario.HOLPZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "WaterLevelOffset":
                                    Config.Mario.WaterLevelOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "AreaPointerOffset":
                                    Config.Mario.AreaPointerOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Hud":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "LifeCountOffset":
                                    Config.Hud.LifeCountOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HpCountOffset":
                                    Config.Hud.HpCountOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoinCountOffset":
                                    Config.Hud.CoinCountOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StarCountOffset":
                                    Config.Hud.StarCountOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "LifeDisplayOffset":
                                    Config.Hud.LifeDisplayOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "HpDisplayOffset":
                                    Config.Hud.HpDisplayOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CoinDisplayOffset":
                                    Config.Hud.CoinDisplayOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StarDisplayOffset":
                                    Config.Hud.StarDisplayOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "VisibilityOffset":
                                    Config.Hud.VisibilityOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "VisibilityMask":
                                    Config.Hud.VisibilityMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FullHpInt":
                                    Config.Hud.FullHpInt = short.Parse(subElement.Value);
                                    break;
                                case "FullHp":
                                    Config.Hud.FullHp = short.Parse(subElement.Value);
                                    break;
                                case "StandardLives":
                                    Config.Hud.StandardLives = sbyte.Parse(subElement.Value);
                                    break;
                                case "StandardCoins":
                                    Config.Hud.StandardCoins = short.Parse(subElement.Value);
                                    break;
                                case "StandardStars":
                                    Config.Hud.StandardStars = short.Parse(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Debug":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "AdvancedModeAddressUS":
                                    Config.Debug.AdvancedModeAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "AdvancedModeAddressJP":
                                    Config.Debug.AdvancedModeAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "AdvancedModeSettingAddressUS":
                                    Config.Debug.AdvancedModeSettingAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "AdvancedModeSettingAddressJP":
                                    Config.Debug.AdvancedModeSettingAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ResourceMeterAddressUS":
                                    Config.Debug.ResourceMeterAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ResourceMeterAddressJP":
                                    Config.Debug.ResourceMeterAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ResourceMeterSettingAddressUS":
                                    Config.Debug.ResourceMeterSettingAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ResourceMeterSettingAddressJP":
                                    Config.Debug.ResourceMeterSettingAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ClassicModeAddressUS":
                                    Config.Debug.ClassicModeAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ClassicModeAddressJP":
                                    Config.Debug.ClassicModeAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "SpawnModeAddressUS":
                                    Config.Debug.SpawnModeAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SpawnModeAddressJP":
                                    Config.Debug.SpawnModeAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "StageSelectAddressUS":
                                    Config.Debug.StageSelectAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "StageSelectAddressJP":
                                    Config.Debug.StageSelectAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FreeMovementAddressUS":
                                    Config.Debug.FreeMovementAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FreeMovementAddressJP":
                                    Config.Debug.FreeMovementAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FreeMovementOnValueUS":
                                    Config.Debug.FreeMovementOnValueUS = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FreeMovementOnValueJP":
                                    Config.Debug.FreeMovementOnValueJP = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FreeMovementOffValueUS":
                                    Config.Debug.FreeMovementOffValueUS = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FreeMovementOffValueJP":
                                    Config.Debug.FreeMovementOffValueJP = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "TriangleOffsets":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "SurfaceType":
                                    Config.TriangleOffsets.SurfaceType = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ExertionForceIndex":
                                    Config.TriangleOffsets.ExertionForceIndex = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ExertionAngle":
                                    Config.TriangleOffsets.ExertionAngle = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Flags":
                                    Config.TriangleOffsets.Flags = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Room":
                                    Config.TriangleOffsets.Room = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YMin":
                                    Config.TriangleOffsets.YMin = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YMax":
                                    Config.TriangleOffsets.YMax = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "X1":
                                    Config.TriangleOffsets.X1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Y1":
                                    Config.TriangleOffsets.Y1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Z1":
                                    Config.TriangleOffsets.Z1 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "X2":
                                    Config.TriangleOffsets.X2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Y2":
                                    Config.TriangleOffsets.Y2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Z2":
                                    Config.TriangleOffsets.Z2 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "X3":
                                    Config.TriangleOffsets.X3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Y3":
                                    Config.TriangleOffsets.Y3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "Z3":
                                    Config.TriangleOffsets.Z3 = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "NormX":
                                    Config.TriangleOffsets.NormX = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "NormY":
                                    Config.TriangleOffsets.NormY = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "NormZ":
                                    Config.TriangleOffsets.NormZ = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "NormOffset":
                                    Config.TriangleOffsets.NormOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "AssociatedObject":
                                    Config.TriangleOffsets.AssociatedObject = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BelongsToObjectMask":
                                    Config.TriangleOffsets.BelongsToObjectMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "NoCamCollisionMask":
                                    Config.TriangleOffsets.NoCamCollisionMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ProjectionMask":
                                    Config.TriangleOffsets.ProjectionMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        } 
                        break;

                    case "Triangle":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "TriangleStructSize":
                                    Config.Triangle.TriangleStructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "TriangleListPointerAddress":
                                    Config.Triangle.TriangleListPointerAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "LevelTriangleCountAddressUS":
                                    Config.Triangle.LevelTriangleCountAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "LevelTriangleCountAddressJP":
                                    Config.Triangle.LevelTriangleCountAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TotalTriangleCountAddressUS":
                                    Config.Triangle.TotalTriangleCountAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TotalTriangleCountAddressJP":
                                    Config.Triangle.TotalTriangleCountAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "NodeListPointerAddress":
                                    Config.Triangle.NodeListPointerAddress = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "LevelNodeCountAddressUS":
                                    Config.Triangle.LevelNodeCountAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "LevelNodeCountAddressJP":
                                    Config.Triangle.LevelNodeCountAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TotalNodeCountAddressUS":
                                    Config.Triangle.TotalNodeCountAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "TotalNodeCountAddressJP":
                                    Config.Triangle.TotalNodeCountAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ExertionForceTableAddressUS":
                                    Config.Triangle.ExertionForceTableAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ExertionForceTableAddressJP":
                                    Config.Triangle.ExertionForceTableAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Camera":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "CameraStructAddressUS":
                                    Config.Camera.CameraStructAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CameraStructAddressJP":
                                    Config.Camera.CameraStructAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "XOffset":
                                    Config.Camera.XOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YOffset":
                                    Config.Camera.YOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "ZOffset":
                                    Config.Camera.ZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FocusXOffset":
                                    Config.Camera.FocusXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FocusYOffset":
                                    Config.Camera.FocusYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "FocusZOffset":
                                    Config.Camera.FocusZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "YawFacingOffset":
                                    Config.Camera.YawFacingOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioCamPossibleOffset":
                                    Config.Camera.MarioCamPossibleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "MarioCamPossibleMask":
                                    Config.Camera.MarioCamPossibleMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SecondaryObjectAddressUS":
                                    Config.Camera.SecondaryObjectAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "SecondaryObjectAddressJP":
                                    Config.Camera.SecondaryObjectAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Input":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "CurrentInputAddressUS":
                                    Config.Input.CurrentInputAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CurrentInputAddressJP":
                                    Config.Input.CurrentInputAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "JustPressedInputAddressUS":
                                    Config.Input.JustPressedInputAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "JustPressedInputAddressJP":
                                    Config.Input.JustPressedInputAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BufferedInputAddressUS":
                                    Config.Input.BufferedInputAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BufferedInputAddressJP":
                                    Config.Input.BufferedInputAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonAOffset":
                                    Config.Input.ButtonAOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonBOffset":
                                    Config.Input.ButtonBOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonZOffset":
                                    Config.Input.ButtonZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonStartOffset":
                                    Config.Input.ButtonStartOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonROffset":
                                    Config.Input.ButtonROffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonLOffset":
                                    Config.Input.ButtonLOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCUpOffset":
                                    Config.Input.ButtonCUpOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCDownOffset":
                                    Config.Input.ButtonCDownOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCLeftOffset":
                                    Config.Input.ButtonCLeftOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCRightOffset":
                                    Config.Input.ButtonCRightOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDUpOffset":
                                    Config.Input.ButtonDUpOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDDownOffset":
                                    Config.Input.ButtonDDownOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDLeftOffset":
                                    Config.Input.ButtonDLeftOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDRightOffset":
                                    Config.Input.ButtonDRightOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ControlStickXOffset":
                                    Config.Input.ControlStickXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ControlStickYOffset":
                                    Config.Input.ControlStickYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonAMask":
                                    Config.Input.ButtonAMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonBMask":
                                    Config.Input.ButtonBMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonZMask":
                                    Config.Input.ButtonZMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonStartMask":
                                    Config.Input.ButtonStartMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonRMask":
                                    Config.Input.ButtonRMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonLMask":
                                    Config.Input.ButtonLMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCUpMask":
                                    Config.Input.ButtonCUpMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCDownMask":
                                    Config.Input.ButtonCDownMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCLeftMask":
                                    Config.Input.ButtonCLeftMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonCRightMask":
                                    Config.Input.ButtonCRightMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDUpMask":
                                    Config.Input.ButtonDUpMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDDownMask":
                                    Config.Input.ButtonDDownMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDLeftMask":
                                    Config.Input.ButtonDLeftMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ButtonDRightMask":
                                    Config.Input.ButtonDRightMask = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "File":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "FileStructAddressUS":
                                    Config.File.FileStructAddressUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FileStructAddressJP":
                                    Config.File.FileStructAddressJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FileStructSize":
                                    Config.File.FileStructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ChecksumConstantOffset":
                                    Config.File.ChecksumConstantOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ChecksumConstantValue":
                                    Config.File.ChecksumConstantValue = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ChecksumOffset":
                                    Config.File.ChecksumOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CourseStarsOffsetStart":
                                    Config.File.CourseStarsOffsetStart = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "TotWCStarOffset":
                                    Config.File.TotWCStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CotMCStarOffset":
                                    Config.File.CotMCStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "VCutMStarOffset":
                                    Config.File.VCutMStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "PSSStarsOffset":
                                    Config.File.PSSStarsOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "SAStarOffset":
                                    Config.File.SAStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "WMotRStarOffset":
                                    Config.File.WMotRStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitDWStarOffset":
                                    Config.File.BitDWStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitFSStarOffset":
                                    Config.File.BitFSStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitSStarOffset":
                                    Config.File.BitSStarOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ToadMIPSStarsOffset":
                                    Config.File.ToadMIPSStarsOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "MainCourseCannonsOffsetStart":
                                    Config.File.MainCourseCannonsOffsetStart = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "WMotRCannonOffset":
                                    Config.File.WMotRCannonOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CannonMask":
                                    Config.File.CannonMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "WFDoorOffset":
                                    Config.File.WFDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "JRBDoorOffset":
                                    Config.File.JRBDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CCMDoorOffset":
                                    Config.File.CCMDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "PSSDoorOffset":
                                    Config.File.PSSDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitDWDoorOffset":
                                    Config.File.BitDWDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitFSDoorOffset":
                                    Config.File.BitFSDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitSDoorOffset":
                                    Config.File.BitSDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "WFDoorMask":
                                    Config.File.WFDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "JRBDoorMask":
                                    Config.File.JRBDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CCMDoorMask":
                                    Config.File.CCMDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "PSSDoorMask":
                                    Config.File.PSSDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitDWDoorMask":
                                    Config.File.BitDWDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitFSDoorMask":
                                    Config.File.BitFSDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BitSDoorMask":
                                    Config.File.BitSDoorMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CoinScoreOffsetStart":
                                    Config.File.CoinScoreOffsetStart = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FileStartedOffset":
                                    Config.File.FileStartedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FileStartedMask":
                                    Config.File.FileStartedMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CapSwitchPressedOffset":
                                    Config.File.CapSwitchPressedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "RedCapSwitchMask":
                                    Config.File.RedCapSwitchMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "GreenCapSwitchMask":
                                    Config.File.GreenCapSwitchMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "BlueCapSwitchMask":
                                    Config.File.BlueCapSwitchMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "KeyDoorOffset":
                                    Config.File.KeyDoorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "KeyDoor1KeyMask":
                                    Config.File.KeyDoor1KeyMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "KeyDoor1OpenedMask":
                                    Config.File.KeyDoor1OpenedMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "KeyDoor2KeyMask":
                                    Config.File.KeyDoor2KeyMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "KeyDoor2OpenedMask":
                                    Config.File.KeyDoor2OpenedMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "MoatDrainedOffset":
                                    Config.File.MoatDrainedOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "MoatDrainedMask":
                                    Config.File.MoatDrainedMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "DDDMovedBackOffset":
                                    Config.File.DDDMovedBackOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "DDDMovedBackMask":
                                    Config.File.DDDMovedBackMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationModeOffset":
                                    Config.File.HatLocationModeOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationModeMask":
                                    Config.File.HatLocationModeMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationMarioMask":
                                    Config.File.HatLocationMarioMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationGroundMask":
                                    Config.File.HatLocationGroundMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationKleptoMask":
                                    Config.File.HatLocationKleptoMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationSnowmanMask":
                                    Config.File.HatLocationSnowmanMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationUkikiMask":
                                    Config.File.HatLocationUkikiMask = (byte)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationCourseOffset":
                                    Config.File.HatLocationCourseOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationCourseSSLValue":
                                    Config.File.HatLocationCourseSSLValue = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationCourseSLValue":
                                    Config.File.HatLocationCourseSLValue = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatLocationCourseTTMValue":
                                    Config.File.HatLocationCourseTTMValue = (ushort)ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatPositionXOffset":
                                    Config.File.HatPositionXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatPositionYOffset":
                                    Config.File.HatPositionYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "HatPositionZOffset":
                                    Config.File.HatPositionZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Waypoint":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "StructSize":
                                    Config.Waypoint.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "IndexOffset":
                                    Config.Waypoint.IndexOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "XOffset":
                                    Config.Waypoint.XOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "YOffset":
                                    Config.Waypoint.YOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ZOffset":
                                    Config.Waypoint.ZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "CameraHack":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "CameraHackStructUS":
                                    Config.CameraHack.CameraHackStructUS = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                                case "CameraHackStructJP":
                                    Config.CameraHack.CameraHackStructJP = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CameraModeOffset":
                                    Config.CameraHack.CameraModeOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CameraXOffset":
                                    Config.CameraHack.CameraXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CameraYOffset":
                                    Config.CameraHack.CameraYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "CameraZOffset":
                                    Config.CameraHack.CameraZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FocusXOffset":
                                    Config.CameraHack.FocusXOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FocusYOffset":
                                    Config.CameraHack.FocusYOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "FocusZOffset":
                                    Config.CameraHack.FocusZOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "AbsoluteAngleOffset":
                                    Config.CameraHack.AbsoluteAngleOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ThetaOffset":
                                    Config.CameraHack.ThetaOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "RadiusOffset":
                                    Config.CameraHack.RadiusOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "RelativeHeightOffset":
                                    Config.CameraHack.RelativeHeightOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;

                                case "ObjectOffset":
                                    Config.CameraHack.ObjectOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "GotoRetrieve":
                        foreach (XElement subElement in element.Elements())
                        {
                            switch (subElement.Name.ToString())
                            {
                                case "GotoAboveDefault":
                                    Config.GotoRetrieve.GotoAboveDefault = int.Parse(subElement.Value);
                                    Config.GotoRetrieve.GotoAboveOffset = int.Parse(subElement.Value);
                                    break;
                                case "GotoInfrontDefault":
                                    Config.GotoRetrieve.GotoInfrontDefault = int.Parse(subElement.Value);
                                    Config.GotoRetrieve.GotoInfrontOffset = int.Parse(subElement.Value);
                                    break;
                                case "RetrieveAboveDefault":
                                    Config.GotoRetrieve.RetrieveAboveDefault = int.Parse(subElement.Value);
                                    Config.GotoRetrieve.RetrieveAboveOffset = int.Parse(subElement.Value);
                                    break;
                                case "RetrieveInfrontDefault":
                                    Config.GotoRetrieve.RetrieveInfrontDefault = int.Parse(subElement.Value);
                                    Config.GotoRetrieve.RetrieveInfrontOffset = int.Parse(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "LevelAddressUS":
                        Config.LevelAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "LevelAddressJP":
                        Config.LevelAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "AreaAddressUS":
                        Config.AreaAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "AreaAddressJP":
                        Config.AreaAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "LoadingPointAddressUS":
                        Config.LoadingPointAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "LoadingPointAddressJP":
                        Config.LoadingPointAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "MissionLayoutAddressUS":
                        Config.MissionAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "MissionLayoutAddressJP":
                        Config.MissionAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "LevelIndexAddressUS":
                        Config.LevelIndexAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "LevelIndexAddressJP":
                        Config.LevelIndexAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "WaterLevelMedianAddressUS":
                        Config.WaterLevelMedianAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "WaterLevelMedianAddressJP":
                        Config.WaterLevelMedianAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "WaterPointerAddressUS":
                        Config.WaterPointerAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "WaterPointerAddressJP":
                        Config.WaterPointerAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "CurrentFileAddressUS":
                        Config.CurrentFileAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "CurrentFileAddressJP":
                        Config.CurrentFileAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "SpecialTripleJumpAddressUS":
                        Config.SpecialTripleJumpAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "SpecialTripleJumpAddressJP":
                        Config.SpecialTripleJumpAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "HackedAreaAddressUS":
                        Config.HackedAreaAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "HackedAreaAddressJP":
                        Config.HackedAreaAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "RngAddressUS":
                        Config.RngAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "RngAddressJP":
                        Config.RngAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "GlobalTimerAddressUS":
                        Config.GlobalTimerAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "GlobalTimerAddressJP":
                        Config.GlobalTimerAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "AnimationTimerAddressUS":
                        Config.AnimationTimerAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "AnimationTimerAddressJP":
                        Config.AnimationTimerAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "MusicOnAddressUS":
                        Config.MusicOnAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "MusicOnAddressJP":
                        Config.MusicOnAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "MusicOnMask":
                        Config.MusicOnMask = (byte)ParsingUtilities.ParseHex(element.Value);
                        break;

                    case "MusicVolumeAddressUS":
                        Config.MusicVolumeAddressUS = ParsingUtilities.ParseHex(element.Value);
                        break;
                    case "MusicVolumeAddressJP":
                        Config.MusicVolumeAddressJP = ParsingUtilities.ParseHex(element.Value);
                        break;
                }
            }
        }

        public static List<WatchVariable> OpenWatchVarData(string path, string schemaFile)
        {
            var objectData = new List<WatchVariable>();
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

                var watchVar = GetWatchVariableFromElement(element);
                objectData.Add(watchVar);
            }

            return objectData;
        }

        public static List<VarX> OpenWatchVarX(string path, string schemaFile)
        {
            var objectData = new List<VarX>();
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

                VarX varX = GetVarXFromElement(element);
                objectData.Add(varX);
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

                        var watchVars = new List<WatchVariable>();
                        foreach (var subElement in element.Elements().Where(x => x.Name == "Data"))
                        {
                            var watchVar = GetWatchVariableFromElement(subElement);
                            watchVars.Add(watchVar);
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
                            WatchVariables = watchVars,
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

        public static Tuple<HackConfig, List<RomHack>> OpenHacks(string path)
        {
            var hacks = new List<RomHack>();
            var hackConfig = new HackConfig();
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
                        hackConfig.SpawnHack = new RomHack(spawnHackPath, "Spawn Hack");
                        hackConfig.BehaviorAddress = ParsingUtilities.ParseHex(element.Attribute(XName.Get("behavior")).Value);
                        hackConfig.GfxIdAddress = ParsingUtilities.ParseHex(element.Attribute(XName.Get("gfxId")).Value);
                        hackConfig.ExtraAddress = ParsingUtilities.ParseHex(element.Attribute(XName.Get("extra")).Value);
                        break;

                    case "Hack":
                        string hackPath = hackDir + element.Attribute(XName.Get("path")).Value;
                        string name = element.Attribute(XName.Get("name")).Value;
                        hacks.Add(new RomHack(hackPath, name));
                        break;
                }
            }

            return new Tuple<HackConfig, List<RomHack>>(hackConfig, hacks);
        }

        public static WatchVariable GetWatchVariableFromElement(XElement element)
        {
            string name = element.Value;

            BaseAddressTypeEnum baseAddressType = VarXUtilities.GetBaseAddressType(element.Attribute(XName.Get("baseAddressType")).Value);

            List<VariableGroup> groupList = VarXUtilities.ParseVariableGroupList(element.Attribute(XName.Get("groups"))?.Value);

            string specialType = (element.Attribute(XName.Get("specialType")) != null) ?
                element.Attribute(XName.Get("specialType")).Value : null;

            Color? backgroundColor = (element.Attribute(XName.Get("color")) != null) ?
                ColorTranslator.FromHtml(element.Attribute(XName.Get("color")).Value) : (Color?)null;

            string typeName = (element.Attribute(XName.Get("type"))?.Value);
            typeName = baseAddressType == BaseAddressTypeEnum.Special ? "byte" : typeName; // TODO fix this hacky solution

            AddressHolder addressHolder =
                new AddressHolder(
                    typeName,
                    baseAddressType,
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetUS"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetJP"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetPAL"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offset"))?.Value),
                    true);

            bool useHex = (element.Attribute(XName.Get("useHex")) != null) ?
                bool.Parse(element.Attribute(XName.Get("useHex")).Value) : false;

            ulong? mask = element.Attribute(XName.Get("mask")) != null ?
                (ulong?)ParsingUtilities.ParseExtHex(element.Attribute(XName.Get("mask")).Value) : null;

            bool isBool = element.Attribute(XName.Get("isBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("isBool")).Value) : false;

            bool isObject = element.Attribute(XName.Get("isObject")) != null ?
                bool.Parse(element.Attribute(XName.Get("isObject")).Value) : false;

            bool invertBool = element.Attribute(XName.Get("invertBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("invertBool")).Value) : false;

            bool isAngle = element.Attribute(XName.Get("isAngle")) != null ?
                bool.Parse(element.Attribute(XName.Get("isAngle")).Value) : false;

            return new WatchVariable(
                name,
                baseAddressType,
                groupList,
                specialType,
                backgroundColor,
                addressHolder,
                useHex,
                mask,
                isBool,
                isObject,
                typeName,
                invertBool,
                isAngle);
        }

        public static VarX GetVarXFromElement(XElement element)
        {
            string name = element.Value;

            BaseAddressTypeEnum baseAddressType = VarXUtilities.GetBaseAddressType(element.Attribute(XName.Get("baseAddressType")).Value);

            List<VariableGroup> groupList = VarXUtilities.ParseVariableGroupList(element.Attribute(XName.Get("groups"))?.Value);

            string specialType = (element.Attribute(XName.Get("specialType")) != null) ?
                element.Attribute(XName.Get("specialType")).Value : null;

            Color? backgroundColor = (element.Attribute(XName.Get("color")) != null) ?
                ColorTranslator.FromHtml(element.Attribute(XName.Get("color")).Value) : (Color?)null;

            string typeName = (element.Attribute(XName.Get("type"))?.Value);

            AddressHolder addressHolder =
                new AddressHolder(
                    typeName,
                    baseAddressType,
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetUS"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetJP"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetPAL"))?.Value),
                    ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offset"))?.Value),
                    false);

            bool useHex = (element.Attribute(XName.Get("useHex")) != null) ?
                bool.Parse(element.Attribute(XName.Get("useHex")).Value) : false;

            ulong? mask = element.Attribute(XName.Get("mask")) != null ?
                (ulong?)ParsingUtilities.ParseExtHex(element.Attribute(XName.Get("mask")).Value) : null;

            bool isBool = element.Attribute(XName.Get("isBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("isBool")).Value) : false;

            bool isObject = element.Attribute(XName.Get("isObject")) != null ?
                bool.Parse(element.Attribute(XName.Get("isObject")).Value) : false;

            bool invertBool = element.Attribute(XName.Get("invertBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("invertBool")).Value) : false;

            bool isAngle = element.Attribute(XName.Get("isAngle")) != null ?
                bool.Parse(element.Attribute(XName.Get("isAngle")).Value) : false;

            return new VarX(
                name,
                groupList,
                specialType,
                backgroundColor,
                addressHolder,
                useHex,
                mask,
                isBool,
                isObject,
                invertBool,
                isAngle);
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
