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
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ConfigSchema.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));

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
                                case "MarioPointerAddress":
                                    config.Mario.MarioPointerAddress = ParsingUtilities.ParseHex(subElement.Value);
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
                                case "MarioStructSize":
                                    config.Mario.StructSize = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
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
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.OtherDataSchema.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));

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
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ObjectDataSchema.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));

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
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.MarioDataSchema.xsd"), null));
            schemaSet.Add(XmlSchema.Read(assembly.GetManifestResourceStream("SM64_Diagnostic.Schemas.ReusableTypes.xsd"), null));

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
            string defaultImagePath = "", emptyImagePath = "", imageDir = "", marioImagePath = "";
            var usedBehaviors = new List<uint>();
            uint ramToBehaviorOffset = 0;

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
                                case "EmptyImage":
                                    emptyImagePath = subElement.Value;
                                    break;
                                case "RamToBehaviorOffset":
                                    ramToBehaviorOffset = ParsingUtilities.ParseHex(subElement.Value);
                                    break;
                            }
                        }
                        break;

                    case "Mario":
                        marioImagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        assoc.MarioColor = ColorTranslator.FromHtml(element.Element(XName.Get("Color")).Value);
                        break;

                    case "Object":
                        uint behaviorAddress = ParsingUtilities.ParseHex(
                            element.Attribute(XName.Get("behaviorScriptAddress")).Value);
                        string imagePath = element.Element(XName.Get("Image")).Attribute(XName.Get("path")).Value;
                        string name = element.Attribute(XName.Get("name")).Value;
                        if (usedBehaviors.Contains(behaviorAddress))
                            throw new Exception();
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
            foreach(var v in behaviorImageAssoc)
            {
                var preLoad = Bitmap.FromFile(imageDir + v.Value.Item1);
                var image = new Bitmap(preLoad, new Size(32, 32));
               
                preLoad.Dispose();
                assoc.AddAssociation(v.Key - ramToBehaviorOffset, image, v.Value.Item2);
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
            watchVar.Type = WatchVariableParsingExtensions.GetStringType(element.Attribute(XName.Get("type")).Value);
            watchVar.Address = ParsingUtilities.ParseHex(element.Attribute(XName.Get("address")).Value);
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
