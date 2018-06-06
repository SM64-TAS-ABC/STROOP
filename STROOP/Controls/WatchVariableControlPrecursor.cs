using STROOP.Extensions;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Controls
{
    public class WatchVariableControlPrecursor
    {
        public readonly string Name;
        public readonly WatchVariable WatchVar;
        public readonly WatchVariableSubclass Subclass;
        public readonly Color? BackgroundColor;
        public readonly Type DisplayType;
        public readonly int? RoundingLimit;
        public readonly bool? UseHex;
        public readonly bool? InvertBool;
        public readonly bool? IsYaw;
        public readonly WatchVariableCoordinate? Coordinate;
        public readonly List<VariableGroup> GroupList;
        public readonly List<uint> FixedAddresses;

        public WatchVariableControlPrecursor(
            string name,
            WatchVariable watchVar,
            WatchVariableSubclass subclass,
            Color? backgroundColor,
            Type displayType,
            int? roundingLimit,
            bool? useHex,
            bool? invertBool,
            bool? isYaw,
            WatchVariableCoordinate? coordinate,
            List<VariableGroup> groupList,
            List<uint> fixedAddresses = null)
        {
            Name = name;
            WatchVar = watchVar;
            Subclass = subclass;
            BackgroundColor = backgroundColor;
            DisplayType = displayType;
            RoundingLimit = roundingLimit;
            UseHex = useHex;
            InvertBool = invertBool;
            IsYaw = isYaw;
            Coordinate = coordinate;
            GroupList = groupList;
            FixedAddresses = fixedAddresses;

            VerifyState();
        }

        public WatchVariableControlPrecursor(XElement element)
        {
            /// Watchvariable params
            string typeName = (element.Attribute(XName.Get("type"))?.Value);
            string specialType = element.Attribute(XName.Get("specialType"))?.Value;
            BaseAddressTypeEnum baseAddressType = WatchVariableUtilities.GetBaseAddressType(element.Attribute(XName.Get("base")).Value);
            uint? offsetUS = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetUS"))?.Value);
            uint? offsetJP = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetJP"))?.Value);
            uint? offsetPAL = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offsetPAL"))?.Value);
            uint? offsetDefault = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("offset"))?.Value);
            uint? mask = element.Attribute(XName.Get("mask")) != null ?
                (uint?)ParsingUtilities.ParseHex(element.Attribute(XName.Get("mask")).Value) : null;

            WatchVar = 
                new WatchVariable(
                    typeName,
                    specialType,
                    baseAddressType,
                    offsetUS,
                    offsetJP,
                    offsetPAL,
                    offsetDefault,
                    mask);

            Name = element.Value;
            Subclass = WatchVariableUtilities.GetSubclass(element.Attribute(XName.Get("subclass"))?.Value);
            GroupList = WatchVariableUtilities.ParseVariableGroupList(element.Attribute(XName.Get("groupList"))?.Value);
            BackgroundColor = (element.Attribute(XName.Get("color")) != null) ?
                ColorUtilities.GetColorFromString(element.Attribute(XName.Get("color")).Value) : (Color?)null;
            string displayTypeName = (element.Attribute(XName.Get("display"))?.Value);
            DisplayType = displayTypeName != null ? TypeUtilities.StringToType[displayTypeName] : null;
            RoundingLimit = (element.Attribute(XName.Get("round")) != null) ?
                ParsingUtilities.ParseInt(element.Attribute(XName.Get("round")).Value) : (int?)null;
            UseHex = (element.Attribute(XName.Get("useHex")) != null) ?
                bool.Parse(element.Attribute(XName.Get("useHex")).Value) : (bool?)null;
            InvertBool = element.Attribute(XName.Get("invertBool")) != null ?
                bool.Parse(element.Attribute(XName.Get("invertBool")).Value) : (bool?)null;
            IsYaw = (element.Attribute(XName.Get("yaw")) != null) ?
                bool.Parse(element.Attribute(XName.Get("yaw")).Value) : (bool?)null;
            Coordinate = element.Attribute(XName.Get("coord")) != null ?
                WatchVariableUtilities.GetCoordinate(element.Attribute(XName.Get("coord")).Value) : (WatchVariableCoordinate?)null;
            FixedAddresses = element.Attribute(XName.Get("fixed")) != null ?
                ParsingUtilities.ParseHexList(element.Attribute(XName.Get("fixed")).Value) : null;

            VerifyState();
        }

        private void VerifyState()
        {
            if (WatchVar.IsSpecial != (WatchVar.MemoryType == null))
            {
                throw new ArgumentOutOfRangeException("Watch vars have a memory type iff they're special");
            }

            if (Subclass == WatchVariableSubclass.Angle && WatchVar.IsSpecial)
            {
                if (DisplayType != typeof(ushort) &&
                    DisplayType != typeof(short) &&
                    DisplayType != typeof(uint) &&
                    DisplayType != typeof(int))
                {
                    throw new ArgumentOutOfRangeException("Special angle vars must have a display type");
                }
            }

            if (Subclass == WatchVariableSubclass.Object && WatchVar.MemoryType != typeof(uint))
            {
                throw new ArgumentOutOfRangeException("Object vars must have type uint");
            }

            if (Subclass == WatchVariableSubclass.Triangle && WatchVar.MemoryType != typeof(uint) && !WatchVar.IsSpecial)
            {
                throw new ArgumentOutOfRangeException("Triangle vars must have type uint");
            }

            if (DisplayType != null)
            {
                if (Subclass != WatchVariableSubclass.Angle)
                {
                    throw new ArgumentOutOfRangeException("DisplayType is only valid for Angle");
                }
                if (DisplayType != typeof(ushort) &&
                    DisplayType != typeof(short) &&
                    DisplayType != typeof(uint) &&
                    DisplayType != typeof(int))
                {
                    throw new ArgumentOutOfRangeException("DisplayType for Angle must be either ushort/short/uint/int");
                }
            }

            if (UseHex.HasValue && (Subclass == WatchVariableSubclass.String))
            {
                throw new ArgumentOutOfRangeException("useHex cannot be used with var subclass String");
            }

            if (UseHex.HasValue && (Subclass == WatchVariableSubclass.Object))
            {
                throw new ArgumentOutOfRangeException("useHex is redundant with var subclass Object");
            }

            if (UseHex.HasValue && (Subclass == WatchVariableSubclass.Triangle))
            {
                throw new ArgumentOutOfRangeException("useHex is redundant with var subclass Triangle");
            }

            if (InvertBool.HasValue && (Subclass != WatchVariableSubclass.Boolean))
            {
                throw new ArgumentOutOfRangeException("invertBool must be used with var subclass Boolean");
            }

            if (IsYaw.HasValue)
            {
                if (IsYaw.Value == false)
                {
                    throw new ArgumentOutOfRangeException("setting yaw to false is redundant");
                }

                if (Subclass != WatchVariableSubclass.Angle)
                {
                    throw new ArgumentOutOfRangeException("yaw must be used with var subclass Angle");
                }
            }

            if (Coordinate.HasValue && (Subclass == WatchVariableSubclass.String))
            {
                throw new ArgumentOutOfRangeException("coordinate cannot be used with var subclass String");
            }

            if (UseHex.HasValue && UseHex.Value == false)
            {
                throw new ArgumentOutOfRangeException("setting useHex to false is redundant");
            }

            if (InvertBool.HasValue && InvertBool.Value == false)
            {
                throw new ArgumentOutOfRangeException("setting invertBool to false is redundant");
            }
        }

        public WatchVariableControl CreateWatchVariableControl(
            Color? newColor = null,
            string newName = null,
            List<uint> newFixedAddresses = null,
            List<VariableGroup> newVariableGroupList = null)
        {
            return new WatchVariableControl(
                this,
                newName ?? Name,
                WatchVar,
                Subclass,
                newColor ?? BackgroundColor,
                DisplayType,
                RoundingLimit,
                UseHex,
                InvertBool,
                IsYaw,
                Coordinate,
                newVariableGroupList ?? GroupList,
                newFixedAddresses ?? FixedAddresses);
        }

        public XElement ToXML(Color? newColor = null, string newName = null, List<uint> newFixedAddresses = null)
        {
            string name = newName ?? Name;
            XElement xElement = new XElement("Data", name);

            if (GroupList.Count > 0)
                xElement.Add(new XAttribute("groupList", String.Join(",", GroupList)));

            xElement.Add(new XAttribute("base", WatchVar.BaseAddressType.ToString()));

            if (WatchVar.OffsetDefault != null)
                xElement.Add(new XAttribute(
                    "offset",
                    String.Format("0x{0:X}", WatchVar.OffsetDefault.Value)));

            if (WatchVar.OffsetUS != null)
                xElement.Add(new XAttribute(
                    "offsetUS",
                    String.Format("0x{0:X}", WatchVar.OffsetUS.Value)));

            if (WatchVar.OffsetJP != null)
                xElement.Add(new XAttribute(
                    "offsetJP",
                    String.Format("0x{0:X}", WatchVar.OffsetJP.Value)));

            if (WatchVar.OffsetPAL != null)
                xElement.Add(new XAttribute(
                    "offsetPAL",
                    String.Format("0x{0:X}", WatchVar.OffsetPAL.Value)));

            if (WatchVar.MemoryTypeName != null)
                xElement.Add(new XAttribute("type", WatchVar.MemoryTypeName));

            if (WatchVar.SpecialType != null)
                xElement.Add(new XAttribute("specialType", WatchVar.SpecialType));

            if (WatchVar.Mask != null)
                xElement.Add(new XAttribute(
                    "mask",
                    String.Format("0x{0:X" + WatchVar.NibbleCount + "}", WatchVar.Mask.Value)));

            if (Subclass != WatchVariableSubclass.Number)
                xElement.Add(new XAttribute("subclass", Subclass.ToString()));

            if (RoundingLimit.HasValue)
                xElement.Add(new XAttribute("round", RoundingLimit.Value.ToString()));

            if (InvertBool.HasValue)
                xElement.Add(new XAttribute("invertBool", InvertBool.Value.ToString().ToLower()));

            if (UseHex.HasValue)
                xElement.Add(new XAttribute("useHex", UseHex.Value.ToString().ToLower()));

            if (Coordinate.HasValue)
                xElement.Add(new XAttribute("coord", Coordinate.Value.ToString()));

            Color? color = newColor ?? BackgroundColor;
            if (color.HasValue)
                xElement.Add(new XAttribute(
                    "color",
                    ColorUtilities.ConvertColorToString(color.Value)));

            List<uint> fixedAddresses = newFixedAddresses ?? FixedAddresses;
            if (fixedAddresses != null)
                xElement.Add(new XAttribute("fixed", String.Join(
                    ",", fixedAddresses.ConvertAll(
                        address => String.Format("0x{0:X}", address)))));

            return xElement;
        }

        public override string ToString()
        {
            return ToXML().ToString();
        }
    }
}
