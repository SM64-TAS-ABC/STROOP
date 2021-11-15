using STROOP.Controls;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace STROOP.Structs
{
    public static class WatchVariableSpecialUtilities
    {
        private readonly static Func<uint, object> DEFAULT_GETTER = (uint address) => Double.NaN;
        private readonly static Func<object, bool, uint, bool> DEFAULT_SETTER = (object value, bool allowToggle, uint address) => false;
        private readonly static WatchVariableSpecialDictionary _dictionary;

        static WatchVariableSpecialUtilities()
        {
            _dictionary = new WatchVariableSpecialDictionary();
            AddLiteralEntriesToDictionary();
            AddGeneratedEntriesToDictionary();
            AddPanEntriesToDictionary();
            AddMap3DEntriesToDictionary();
        }

        public static (Func<uint, object> getter, Func<object, bool, uint, bool> setter)
            CreateGetterSetterFunctions(string specialType)
        {
            if (_dictionary.ContainsKey(specialType))
                return _dictionary.Get(specialType);
            else
                throw new ArgumentOutOfRangeException();
        }

        private static int _numBinaryMathOperationEntries = 0;

        public static string AddBinaryMathOperationEntry(WatchVariableControl control1, WatchVariableControl control2, BinaryMathOperation operation)
        {
            string specialType = "BinaryMathOperation" + _numBinaryMathOperationEntries;
            switch (operation)
            {
                case BinaryMathOperation.Add:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return value1 + value2;
                        },
                        (double sum, bool allowToggle, uint dummy) =>
                        {
                            if (!KeyboardUtilities.GetToggle(allowToggle))
                            {
                                double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                                double newValue2 = sum - value1;
                                return control2.SetValue(newValue2, allowToggle);
                            }
                            else
                            {
                                double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                                double newValue1 = sum - value2;
                                return control1.SetValue(newValue1, allowToggle);
                            }
                        }));
                    break;

                case BinaryMathOperation.Subtract:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return value1 - value2;
                        },
                        (double diff, bool allowToggle, uint dummy) =>
                        {
                            if (!KeyboardUtilities.GetToggle(allowToggle))
                            {
                                double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                                double newValue2 = value1 - diff;
                                return control2.SetValue(newValue2, allowToggle);
                            }
                            else
                            {
                                double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                                double newValue1 = value2 + diff;
                                return control1.SetValue(newValue1, allowToggle);
                            }
                        }));
                    break;

                case BinaryMathOperation.Multiply:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return value1 * value2;
                        },
                        (double product, bool allowToggle, uint dummy) =>
                        {
                            if (!KeyboardUtilities.GetToggle(allowToggle))
                            {
                                double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                                double newValue2 = product / value1;
                                return control2.SetValue(newValue2, allowToggle);
                            }
                            else
                            {
                                double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                                double newValue1 = product / value2;
                                return control1.SetValue(newValue1, allowToggle);
                            }
                        }));
                    break;

                case BinaryMathOperation.Divide:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return value1 / value2;
                        },
                        (double quotient, bool allowToggle, uint dummy) =>
                        {
                            if (!KeyboardUtilities.GetToggle(allowToggle))
                            {
                                double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                                double newValue2 = value1 / quotient;
                                return control2.SetValue(newValue2, allowToggle);
                            }
                            else
                            {
                                double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                                double newValue1 = value2 * quotient;
                                return control1.SetValue(newValue1, allowToggle);
                            }
                        }));
                    break;

                case BinaryMathOperation.Modulo:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return value1 % value2;
                        },
                        DEFAULT_SETTER));
                    break;

                case BinaryMathOperation.NonNegativeModulo:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return MoreMath.NonNegativeModulus(value1, value2);
                        },
                        DEFAULT_SETTER));
                    break;

                case BinaryMathOperation.Exponent:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            double value1 = ParsingUtilities.ParseDouble(control1.GetValue(handleFormatting: false));
                            double value2 = ParsingUtilities.ParseDouble(control2.GetValue(handleFormatting: false));
                            return Math.Pow(value1, value2);
                        },
                        DEFAULT_SETTER));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            _numBinaryMathOperationEntries++;
            return specialType;
        }

        private static int _numAggregateMathOperationEntries = 0;

        public static string AddAggregateMathOperationEntry(List<WatchVariableControl> controls, AggregateMathOperation operation)
        {
            string specialType = "AggregateMathOperation" + _numAggregateMathOperationEntries;
            switch (operation)
            {
                case AggregateMathOperation.Mean:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            return controls
                                .ConvertAll(control => control.GetValue(handleFormatting: false))
                                .ConvertAll(value => ParsingUtilities.ParseDouble(value))
                                .Average();
                        },
                        DEFAULT_SETTER));
                    break;
                case AggregateMathOperation.Median:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            List<double> doubleValues = controls
                                .ConvertAll(control => control.GetValue(handleFormatting: false))
                                .ConvertAll(value => ParsingUtilities.ParseDouble(value));
                            doubleValues.Sort();
                            if (doubleValues.Count % 2 == 1)
                            {
                                return doubleValues[doubleValues.Count / 2];
                            }
                            else
                            {
                                return (doubleValues[doubleValues.Count / 2 - 1] + doubleValues[doubleValues.Count / 2]) / 2;
                            }
                        },
                        DEFAULT_SETTER));
                    break;
                case AggregateMathOperation.Min:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            return controls
                                .ConvertAll(control => control.GetValue(handleFormatting: false))
                                .ConvertAll(value => ParsingUtilities.ParseDouble(value))
                                .Min();
                        },
                        DEFAULT_SETTER));
                    break;
                case AggregateMathOperation.Max:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            return controls
                                .ConvertAll(control => control.GetValue(handleFormatting: false))
                                .ConvertAll(value => ParsingUtilities.ParseDouble(value))
                                .Max();
                        },
                        DEFAULT_SETTER));
                    break;
                case AggregateMathOperation.Sum:
                    _dictionary.Add(specialType,
                        ((uint dummy) =>
                        {
                            return controls
                                .ConvertAll(control => control.GetValue(handleFormatting: false))
                                .ConvertAll(value => ParsingUtilities.ParseDouble(value))
                                .Sum();
                        },
                        DEFAULT_SETTER));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _numAggregateMathOperationEntries++;
            return specialType;
        }

        private static int _numDistanceMathOperationEntries = 0;

        public static string AddDistanceMathOperationEntry(List<WatchVariableControl> controls, bool use3D)
        {
            string specialType = "DistanceMathOperation" + _numDistanceMathOperationEntries;
            if (use3D)
            {
                PositionAngle p1 = PositionAngle.Functions(
                    new List<Func<double>>()
                    {
                        () => ParsingUtilities.ParseDouble(controls[0].GetValue(handleFormatting: false)),
                        () => ParsingUtilities.ParseDouble(controls[1].GetValue(handleFormatting: false)),
                        () => ParsingUtilities.ParseDouble(controls[2].GetValue(handleFormatting: false)),
                    },
                    new List<Func<double, bool>>()
                    {
                        (double value) => controls[0].SetValue(value, false),
                        (double value) => controls[1].SetValue(value, false),
                        (double value) => controls[2].SetValue(value, false),
                    });
                PositionAngle p2 = PositionAngle.Functions(
                    new List<Func<double>>()
                    {
                        () => ParsingUtilities.ParseDouble(controls[3].GetValue(handleFormatting: false)),
                        () => ParsingUtilities.ParseDouble(controls[4].GetValue(handleFormatting: false)),
                        () => ParsingUtilities.ParseDouble(controls[5].GetValue(handleFormatting: false)),
                    },
                    new List<Func<double, bool>>()
                    {
                        (double value) => controls[3].SetValue(value, false),
                        (double value) => controls[4].SetValue(value, false),
                        (double value) => controls[5].SetValue(value, false),
                    });
                _dictionary.Add(specialType,
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetDistance(p1, p2);
                    },
                    (double dist, bool allowToggle, uint dummy) =>
                    {
                        return PositionAngle.SetDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle));
                    }));
            }
            else
            {
                PositionAngle p1 = PositionAngle.Functions(
                    new List<Func<double>>()
                    {
                        () => ParsingUtilities.ParseDouble(controls[0].GetValue(handleFormatting: false)),
                        () => 0,
                        () => ParsingUtilities.ParseDouble(controls[1].GetValue(handleFormatting: false)),
                    },
                    new List<Func<double, bool>>()
                    {
                        (double value) => controls[0].SetValue(value, false),
                        (double value) => true,
                        (double value) => controls[1].SetValue(value, false),
                    });
                PositionAngle p2 = PositionAngle.Functions(
                    new List<Func<double>>()
                    {
                        () => ParsingUtilities.ParseDouble(controls[2].GetValue(handleFormatting: false)),
                        () => 0,
                        () => ParsingUtilities.ParseDouble(controls[3].GetValue(handleFormatting: false)),
                    },
                    new List<Func<double, bool>>()
                    {
                        (double value) => controls[2].SetValue(value, false),
                        (double value) => true,
                        (double value) => controls[3].SetValue(value, false),
                    });
                _dictionary.Add(specialType,
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetHDistance(p1, p2);
                    },
                    (double dist, bool allowToggle, uint dummy) =>
                    {
                        return PositionAngle.SetHDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle));
                    }));
            }
            _numDistanceMathOperationEntries++;
            return specialType;
        }

        private static int _numRealTimeEntries = 0;

        public static string AddRealTimeEntry(WatchVariableControl control)
        {
            string specialType = "RealTime" + _numRealTimeEntries;
            _dictionary.Add(specialType,
                ((uint dummy) =>
                {
                    uint totalFrames = ParsingUtilities.ParseUIntRoundingWrapping(
                        control.GetValue(useRounding: false, handleFormatting: false)) ?? 0;
                    return GetRealTime(totalFrames);
                },
                DEFAULT_SETTER));
            _numRealTimeEntries++;
            return specialType;
        }

        private static int _numActionDescriptionEntries = 0;

        public static string AddActionDescriptionEntry(WatchVariableControl control)
        {
            string specialType = "ActionDescription" + _numActionDescriptionEntries;
            _dictionary.Add(specialType,
                ((uint dummy) =>
                {
                    uint action = ParsingUtilities.ParseUInt(
                        control.GetValue(useRounding: false, handleFormatting: false));
                    return TableConfig.MarioActions.GetActionName(action);
                },
                DEFAULT_SETTER));
            _numActionDescriptionEntries++;
            return specialType;
        }

        private static int _numDereferencedEntries = 0;

        public static string AddDereferencedEntry(WatchVariableControl control, string typeString, uint? offset)
        {
            string specialType = "Dereferenced" + _numDereferencedEntries;
            _dictionary.Add(specialType,
                ((uint dummy) =>
                {
                    uint address = ParsingUtilities.ParseUInt(control.GetValue(useRounding: false, handleFormatting: false)) + (offset ?? 0);
                    switch (typeString)
                    {
                        case "byte":
                            return Config.Stream.GetByte(address);
                        case "sbyte":
                            return Config.Stream.GetSByte(address);
                        case "short":
                            return Config.Stream.GetShort(address);
                        case "ushort":
                            return Config.Stream.GetUShort(address);
                        case "int":
                            return Config.Stream.GetInt(address);
                        case "uint":
                            return Config.Stream.GetUInt(address);
                        case "float":
                            return Config.Stream.GetFloat(address);
                        case "double":
                            return Config.Stream.GetDouble(address);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                },
                (object value, bool allowToggle, uint dummy) =>
                {
                    uint address = ParsingUtilities.ParseUInt(control.GetValue(useRounding: false, handleFormatting: false)) + (offset ?? 0);
                    switch (typeString)
                    {
                        case "byte":
                            return Config.Stream.SetValue(ParsingUtilities.ParseByte(value), address);
                        case "sbyte":
                            return Config.Stream.SetValue(ParsingUtilities.ParseSByte(value), address);
                        case "short":
                            return Config.Stream.SetValue(ParsingUtilities.ParseShort(value), address);
                        case "ushort":
                            return Config.Stream.SetValue(ParsingUtilities.ParseUShort(value), address);
                        case "int":
                            return Config.Stream.SetValue(ParsingUtilities.ParseInt(value), address);
                        case "uint":
                            return Config.Stream.SetValue(ParsingUtilities.ParseUInt(value), address);
                        case "float":
                            return Config.Stream.SetValue(ParsingUtilities.ParseFloat(value), address);
                        case "double":
                            return Config.Stream.SetValue(ParsingUtilities.ParseDouble(value), address);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            ));
            _numDereferencedEntries++;
            return specialType;
        }

        public static string AddDummyEntry(string typeString)
        {
            if (typeString == "string")
            {
                int index = SpecialConfig.DummyValues.Count;
                SpecialConfig.DummyValues.Add("value");
                string specialType = "Dummy" + index + StringUtilities.Capitalize(typeString);
                _dictionary.Add(specialType,
                    ((uint dummy) =>
                    {
                        return SpecialConfig.DummyValues[index] as string;
                    },
                    (string value, bool allowToggle, uint dummy) =>
                    {
                        SpecialConfig.DummyValues[index] = value;
                        return true;
                    }
                ));
                return specialType;
            }
            else
            {
                int index = SpecialConfig.DummyValues.Count;
                Type type = TypeUtilities.StringToType[typeString];
                SpecialConfig.DummyValues.Add(ParsingUtilities.ParseValueRoundingWrapping(0, type));
                string specialType = "Dummy" + index + StringUtilities.Capitalize(typeString);
                _dictionary.Add(specialType,
                    ((uint dummy) =>
                    {
                        return SpecialConfig.DummyValues[index];
                    },
                    (double value, bool allowToggle, uint dummy) =>
                    {
                        object o = ParsingUtilities.ParseValueRoundingWrapping(value, type);
                        if (o == null) return false;
                        SpecialConfig.DummyValues[index] = o;
                        return true;
                    }
                ));
                return specialType;
            }
        }

        public static string AddSchedulerEntry(int index)
        {
            string specialType = "Scheduler" + index;
            _dictionary.Add(specialType,
                ((uint dummy) =>
                {
                    return PositionAngle.Scheduler.GetAdditionalValue(index);
                },
                DEFAULT_SETTER));
            return specialType;
        }

        public static void AddPanEntriesToDictionary()
        {
            List<(string, Func<double>, Action<double>)> entries =
                new List<(string, Func<double>, Action<double>)>()
                {
                    ("NumPans", () => SpecialConfig.NumPans, (double value) => SpecialConfig.NumPans = value),
                    ("CurrentPan", () => SpecialConfig.CurrentPan, (double value) => {}),
                    ("PanCamPos", () => SpecialConfig.PanCamPos, (double value) => SpecialConfig.PanCamPos = value),
                    ("PanCamAngle", () => SpecialConfig.PanCamAngle, (double value) => SpecialConfig.PanCamAngle = value),
                    ("PanCamRotation", () => SpecialConfig.PanCamRotation, (double value) => SpecialConfig.PanCamRotation = value),
                    ("PanFOV", () => SpecialConfig.PanFOV, (double value) => SpecialConfig.PanFOV = value),
                };

            foreach ((string key, Func<double> getter, Action<double> setter) in entries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (double doubleValue, bool allowToggle, uint dummy) =>
                    {
                        setter(doubleValue);
                        return true;
                    }));
            }
        }

        public static void AddPanEntriesToDictionary(int index)
        {
            List<(string, Func<double>, Action<double>)> entries =
                new List<(string, Func<double>, Action<double>)>()
                {
                    (String.Format("Pan{0}GlobalTimer", index), () => SpecialConfig.PanModels[index].PanGlobalTimer, (double value) => SpecialConfig.PanModels[index].PanGlobalTimer = value),
                    (String.Format("Pan{0}StartTime", index), () => SpecialConfig.PanModels[index].PanStartTime, (double value) => SpecialConfig.PanModels[index].PanStartTime = value),
                    (String.Format("Pan{0}EndTime", index), () => SpecialConfig.PanModels[index].PanEndTime, (double value) => SpecialConfig.PanModels[index].PanEndTime = value),
                    (String.Format("Pan{0}Duration", index), () => SpecialConfig.PanModels[index].PanDuration, (double value) => SpecialConfig.PanModels[index].PanDuration = value),

                    (String.Format("Pan{0}EaseStart", index), () => SpecialConfig.PanModels[index].PanEaseStart, (double value) => SpecialConfig.PanModels[index].PanEaseStart = value),
                    (String.Format("Pan{0}EaseEnd", index), () => SpecialConfig.PanModels[index].PanEaseEnd, (double value) => SpecialConfig.PanModels[index].PanEaseEnd = value),
                    (String.Format("Pan{0}EaseDegree", index), () => SpecialConfig.PanModels[index].PanEaseDegree, (double value) => SpecialConfig.PanModels[index].PanEaseDegree = value),

                    (String.Format("Pan{0}RotateCW", index), () => SpecialConfig.PanModels[index].PanRotateCW, (double value) => SpecialConfig.PanModels[index].PanRotateCW = value),

                    (String.Format("Pan{0}CamStartX", index), () => SpecialConfig.PanModels[index].PanCamStartX, (double value) => SpecialConfig.PanModels[index].PanCamStartX = value),
                    (String.Format("Pan{0}CamStartY", index), () => SpecialConfig.PanModels[index].PanCamStartY, (double value) => SpecialConfig.PanModels[index].PanCamStartY = value),
                    (String.Format("Pan{0}CamStartZ", index), () => SpecialConfig.PanModels[index].PanCamStartZ, (double value) => SpecialConfig.PanModels[index].PanCamStartZ = value),
                    (String.Format("Pan{0}CamStartYaw", index), () => SpecialConfig.PanModels[index].PanCamStartYaw, (double value) => SpecialConfig.PanModels[index].PanCamStartYaw = value),
                    (String.Format("Pan{0}CamStartPitch", index), () => SpecialConfig.PanModels[index].PanCamStartPitch, (double value) => SpecialConfig.PanModels[index].PanCamStartPitch = value),

                    (String.Format("Pan{0}CamEndX", index), () => SpecialConfig.PanModels[index].PanCamEndX, (double value) => SpecialConfig.PanModels[index].PanCamEndX = value),
                    (String.Format("Pan{0}CamEndY", index), () => SpecialConfig.PanModels[index].PanCamEndY, (double value) => SpecialConfig.PanModels[index].PanCamEndY = value),
                    (String.Format("Pan{0}CamEndZ", index), () => SpecialConfig.PanModels[index].PanCamEndZ, (double value) => SpecialConfig.PanModels[index].PanCamEndZ = value),
                    (String.Format("Pan{0}CamEndYaw", index), () => SpecialConfig.PanModels[index].PanCamEndYaw, (double value) => SpecialConfig.PanModels[index].PanCamEndYaw = value),
                    (String.Format("Pan{0}CamEndPitch", index), () => SpecialConfig.PanModels[index].PanCamEndPitch, (double value) => SpecialConfig.PanModels[index].PanCamEndPitch = value),

                    (String.Format("Pan{0}RadiusStart", index), () => SpecialConfig.PanModels[index].PanRadiusStart, (double value) => SpecialConfig.PanModels[index].PanRadiusStart = value),
                    (String.Format("Pan{0}RadiusEnd", index), () => SpecialConfig.PanModels[index].PanRadiusEnd, (double value) => SpecialConfig.PanModels[index].PanRadiusEnd = value),

                    (String.Format("Pan{0}FOVStart", index), () => SpecialConfig.PanModels[index].PanFOVStart, (double value) => SpecialConfig.PanModels[index].PanFOVStart = value),
                    (String.Format("Pan{0}FOVEnd", index), () => SpecialConfig.PanModels[index].PanFOVEnd, (double value) => SpecialConfig.PanModels[index].PanFOVEnd = value),
                };

            foreach ((string key, Func<double> getter, Action<double> setter) in entries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (double doubleValue, bool allowToggle, uint dummy) =>
                    {
                        setter(doubleValue);
                        return true;
                    }));
            }
        }

        public static void AddMap3DEntriesToDictionary()
        {
            List<(string, Func<float>, Action<float>)> floatEntries =
                new List<(string, Func<float>, Action<float>)>()
                {
                    ("Map3DCameraX", () => MapConfig.Map3DCameraX, (float value) => MapConfig.Map3DCameraX = value),
                    ("Map3DCameraY", () => MapConfig.Map3DCameraY, (float value) => MapConfig.Map3DCameraY = value),
                    ("Map3DCameraZ", () => MapConfig.Map3DCameraZ, (float value) => MapConfig.Map3DCameraZ = value),
                    ("Map3DCameraYaw", () => MapConfig.Map3DCameraYaw, (float value) => MapConfig.Map3DCameraYaw = value),
                    ("Map3DCameraPitch", () => MapConfig.Map3DCameraPitch, (float value) => MapConfig.Map3DCameraPitch = value),
                    ("Map3DCameraRoll", () => MapConfig.Map3DCameraRoll, (float value) => MapConfig.Map3DCameraRoll = value),
                    ("Map3DFocusX", () => MapConfig.Map3DFocusX, (float value) => MapConfig.Map3DFocusX = value),
                    ("Map3DFocusY", () => MapConfig.Map3DFocusY, (float value) => MapConfig.Map3DFocusY = value),
                    ("Map3DFocusZ", () => MapConfig.Map3DFocusZ, (float value) => MapConfig.Map3DFocusZ = value),
                    ("Map3DFollowingRadius", () => MapConfig.Map3DFollowingRadius, (float value) => MapConfig.Map3DFollowingRadius = value),
                    ("Map3DFollowingYOffset", () => MapConfig.Map3DFollowingYOffset, (float value) => MapConfig.Map3DFollowingYOffset = value),
                    ("Map3DFollowingYaw", () => MapConfig.Map3DFollowingYaw, (float value) => MapConfig.Map3DFollowingYaw = value),
                    ("Map3DFOV", () => MapConfig.Map3DFOV, (float value) => MapConfig.Map3DFOV = value),

                    ("CompassLineHeight", () => MapConfig.CompassLineHeight, (float value) => MapConfig.CompassLineHeight = value),
                    ("CompassLineWidth", () => MapConfig.CompassLineWidth, (float value) => MapConfig.CompassLineWidth = value),
                    ("CompassArrowHeight", () => MapConfig.CompassArrowHeight, (float value) => MapConfig.CompassArrowHeight = value),
                    ("CompassArrowWidth", () => MapConfig.CompassArrowWidth, (float value) => MapConfig.CompassArrowWidth = value),
                    ("CompassHorizontalMargin", () => MapConfig.CompassHorizontalMargin, (float value) => MapConfig.CompassHorizontalMargin = value),
                    ("CompassVerticalMargin", () => MapConfig.CompassVerticalMargin, (float value) => MapConfig.CompassVerticalMargin = value),
                    ("CompassDirectionTextSize", () => MapConfig.CompassDirectionTextSize, (float value) => MapConfig.CompassDirectionTextSize = value),
                    ("CompassAngleTextSize", () => MapConfig.CompassAngleTextSize, (float value) => MapConfig.CompassAngleTextSize = value),
                    ("CompassDirectionTextPosition", () => MapConfig.CompassDirectionTextPosition, (float value) => MapConfig.CompassDirectionTextPosition = value),
                    ("CompassAngleTextPosition", () => MapConfig.CompassAngleTextPosition, (float value) => MapConfig.CompassAngleTextPosition = value),
                    ("CompassShowDirectionText", () => MapConfig.CompassShowDirectionText, (float value) => MapConfig.CompassShowDirectionText = value),
                    ("CompassShowAngleText", () => MapConfig.CompassShowAngleText, (float value) => MapConfig.CompassShowAngleText = value),
                    ("CompassAngleTextSigned", () => MapConfig.CompassAngleTextSigned, (float value) => MapConfig.CompassAngleTextSigned = value),
                };

            foreach ((string key, Func<float> getter, Action<float> setter) in floatEntries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (float floatValue, bool allowToggle, uint dummy) =>
                    {
                        setter(floatValue);
                        return true;
                    }
                ));
            }

            List<(string, Func<double>, Action<double>)> doubleEntries =
                new List<(string, Func<double>, Action<double>)>()
                {
                    ("Map2DScrollSpeed", () => MapConfig.Map2DScrollSpeed, (double value) => MapConfig.Map2DScrollSpeed = value),
                    ("Map2DOrthographicHorizontalRotateSpeed", () => MapConfig.Map2DOrthographicHorizontalRotateSpeed, (double value) => MapConfig.Map2DOrthographicHorizontalRotateSpeed = value),
                    ("Map2DOrthographicVerticalRotateSpeed", () => MapConfig.Map2DOrthographicVerticalRotateSpeed, (double value) => MapConfig.Map2DOrthographicVerticalRotateSpeed = value),
                    ("Map3DScrollSpeed", () => MapConfig.Map3DScrollSpeed, (double value) => MapConfig.Map3DScrollSpeed = value),
                    ("Map3DTranslateSpeed", () => MapConfig.Map3DTranslateSpeed, (double value) => MapConfig.Map3DTranslateSpeed = value),
                    ("Map3DRotateSpeed", () => MapConfig.Map3DRotateSpeed, (double value) => MapConfig.Map3DRotateSpeed = value),

                    ("MapUnitPrecisionThreshold", () => MapConfig.MapUnitPrecisionThreshold, (double value) => MapConfig.MapUnitPrecisionThreshold = value),
                    ("MapSortOrthographicTris", () => MapConfig.MapSortOrthographicTris, (double value) => MapConfig.MapSortOrthographicTris = value),
                    ("MapUseNotForCeilings", () => MapConfig.MapUseNotForCeilings, (double value) => MapConfig.MapUseNotForCeilings = value),
                    ("MapUseXForCeilings", () => MapConfig.MapUseXForCeilings, (double value) => MapConfig.MapUseXForCeilings = value),

                    ("MapScaleValue", () => Config.MapGraphics.MapViewScaleValue, (double value) => Config.MapGraphics.SetCustomScale(value)),
                    ("MapCenterXValue", () => Config.MapGraphics.MapViewCenterXValue, (double value) => Config.MapGraphics.SetCustomCenter(xValue: value)),
                    ("MapCenterYValue", () => Config.MapGraphics.MapViewCenterYValue, (double value) => Config.MapGraphics.SetCustomCenter(yValue: value)),
                    ("MapCenterZValue", () => Config.MapGraphics.MapViewCenterZValue, (double value) => Config.MapGraphics.SetCustomCenter(zValue: value)),
                    ("MapYawValue", () => Config.MapGraphics.MapViewYawValue, (double value) => Config.MapGraphics.SetCustomYaw(value)),
                    ("MapPitchValue", () => Config.MapGraphics.MapViewPitchValue, (double value) => Config.MapGraphics.SetCustomPitch(value)),

                    ("CoordinateLabelsCustomSpacing", () => MapConfig.CoordinateLabelsCustomSpacing, (double value) => MapConfig.CoordinateLabelsCustomSpacing = value),
                    ("CoordinateLabelsMargin", () => MapConfig.CoordinateLabelsMargin, (double value) => MapConfig.CoordinateLabelsMargin = value),
                    ("CoordinateLabelsLabelDensity", () => MapConfig.CoordinateLabelsLabelDensity, (double value) => MapConfig.CoordinateLabelsLabelDensity = value),
                    ("CoordinateLabelsShowCursorPos", () => MapConfig.CoordinateLabelsShowCursorPos, (double value) => MapConfig.CoordinateLabelsShowCursorPos = value),
                    ("CoordinateLabelsShowXLabels", () => MapConfig.CoordinateLabelsShowXLabels, (double value) => MapConfig.CoordinateLabelsShowXLabels = value),
                    ("CoordinateLabelsShowZLabels", () => MapConfig.CoordinateLabelsShowZLabels, (double value) => MapConfig.CoordinateLabelsShowZLabels = value),
                    ("CoordinateLabelsUseHighX", () => MapConfig.CoordinateLabelsUseHighX, (double value) => MapConfig.CoordinateLabelsUseHighX = value),
                    ("CoordinateLabelsUseHighZ", () => MapConfig.CoordinateLabelsUseHighZ, (double value) => MapConfig.CoordinateLabelsUseHighZ = value),
                    ("CoordinateLabelsBoldText", () => MapConfig.CoordinateLabelsBoldText, (double value) => MapConfig.CoordinateLabelsBoldText = value),
                };

            foreach ((string key, Func<double> getter, Action<double> setter) in doubleEntries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (double doubleValue, bool allowToggle, uint dummy) =>
                    {
                        setter(doubleValue);
                        return true;
                    }
                ));
            }

            List<(string, Func<int>, Action<int>)> intEntries =
                new List<(string, Func<int>, Action<int>)>()
                {
                    ("MapCircleNumPoints2D", () => MapConfig.MapCircleNumPoints2D, (int value) => MapConfig.MapCircleNumPoints2D = value),
                    ("MapCircleNumPoints3D", () => MapConfig.MapCircleNumPoints3D, (int value) => MapConfig.MapCircleNumPoints3D = value),
                };

            foreach ((string key, Func<int> getter, Action<int> setter) in intEntries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (int intValue, bool allowToggle, uint dummy) =>
                    {
                        setter(intValue);
                        return true;
                    }
                ));
            }

            List<(string, Func<string>, Action<PositionAngle>)> posAngleEntries =
                new List<(string, Func<string>, Action<PositionAngle>)>()
                {
                    ("Map3DCameraPosPA", () => MapConfig.Map3DCameraPosPA.ToString(), (PositionAngle value) => MapConfig.Map3DCameraPosPA = value),
                    ("Map3DCameraAnglePA", () => MapConfig.Map3DCameraAnglePA.ToString(), (PositionAngle value) => MapConfig.Map3DCameraAnglePA = value),
                    ("Map3DFocusPosPA", () => MapConfig.Map3DFocusPosPA.ToString(), (PositionAngle value) => MapConfig.Map3DFocusPosPA = value),
                    ("Map3DFocusAnglePA", () => MapConfig.Map3DFocusAnglePA.ToString(), (PositionAngle value) => MapConfig.Map3DFocusAnglePA = value),
                };

            foreach ((string key, Func<string> getter, Action<PositionAngle> setter) in posAngleEntries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                    {
                        setter(posAngle);
                        return true;
                    }
                ));
            }

            List<(string, Func<string>, Action<string>)> stringEntries =
                new List<(string, Func<string>, Action<string>)>()
                {
                    ("Map3DMode", () => MapConfig.Map3DMode.ToString(), (string value) => MapConfig.Map3DMode = (Map3DCameraMode)Enum.Parse(typeof(Map3DCameraMode), value, true)),
                    ("CompassPosition", () => MapConfig.CompassPosition.ToString(), (string value) => MapConfig.CompassPosition = (CompassPosition)Enum.Parse(typeof(CompassPosition), value, true)),
                };

            foreach ((string key, Func<string> getter, Action<string> setter) in stringEntries)
            {
                _dictionary.Add(key,
                    ((uint dummy) =>
                    {
                        return getter();
                    },
                    (string value, bool allowToggle, uint dummy) =>
                    {
                        try
                        {
                            setter(value);
                            return true;
                        }
                        catch (Exception) { }
                        return false;
                    }
                ));
            }
        }

        public static void AddGeneratedEntriesToDictionary()
        {
            List<Func<uint, PositionAngle>> posAngleFuncs =
                new List<Func<uint, PositionAngle>>()
                {
                    (uint address) => PositionAngle.Custom,
                    (uint address) => PositionAngle.Mario,
                    (uint address) => PositionAngle.Holp,
                    (uint address) => PositionAngle.Camera,
                    (uint address) => PositionAngle.Obj(address),
                    (uint address) => PositionAngle.ObjHome(address),
                    (uint address) => PositionAngle.Ghost,
                    (uint address) => PositionAngle.Tri(address, 1),
                    (uint address) => PositionAngle.Tri(address, 2),
                    (uint address) => PositionAngle.Tri(address, 3),
                    (uint address) => SpecialConfig.PointPA,
                    (uint address) => SpecialConfig.SelfPA,
                    (uint address) => SpecialConfig.Point2PA,
                    (uint address) => SpecialConfig.Self2PA,
                };

            List<string> posAngleStrings =
                new List<string>()
                {
                    "Custom",
                    "Mario",
                    "Holp",
                    "Camera",
                    "Obj",
                    "ObjHome",
                    "Ghost",
                    "TriV1",
                    "TriV2",
                    "TriV3",
                    "Point",
                    "Self",
                    "Point2",
                    "Self2",
                };

            for (int i = 0; i < posAngleFuncs.Count; i++)
            {
                Func<uint, PositionAngle> func1 = posAngleFuncs[i];
                string string1 = posAngleStrings[i];

                for (int j = 0; j < posAngleFuncs.Count; j++)
                {
                    if (j == i) continue;
                    Func<uint, PositionAngle> func2 = posAngleFuncs[j];
                    string string2 = posAngleStrings[j];

                    List<string> distTypes = new List<string>() { "X", "Y", "Z", "H", "", "F", "S" };
                    List<Func<PositionAngle, PositionAngle, double>> distGetters =
                        new List<Func<PositionAngle, PositionAngle, double>>()
                        {
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetXDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetYDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetZDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetHDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetFDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetSDistance(p1, p2),
                        };
                    List<Func<PositionAngle, PositionAngle, double, bool, bool>> distSetters =
                        new List<Func<PositionAngle, PositionAngle, double, bool, bool>>()
                        {
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetXDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetYDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetZDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetHDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetFDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                            (PositionAngle p1, PositionAngle p2, double dist, bool allowToggle) => PositionAngle.SetSDistance(p1, p2, dist, KeyboardUtilities.GetToggle(allowToggle)),
                        };

                    for (int k = 0; k < distTypes.Count; k++)
                    {
                        string distType = distTypes[k];
                        Func<PositionAngle, PositionAngle, double> getter = distGetters[k];
                        Func<PositionAngle, PositionAngle, double, bool, bool> setter = distSetters[k];

                        _dictionary.Add(String.Format("{0}Dist{1}To{2}", distType, string1, string2),
                            ((uint address) =>
                            {
                                return getter(func1(address), func2(address));
                            },
                            (double dist, bool allowToggle, uint address) =>
                            {
                                return setter(func1(address), func2(address), dist, KeyboardUtilities.GetToggle(allowToggle));
                            }));
                    }

                    _dictionary.Add(String.Format("Angle{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleTo(func1(address), func2(address), null, false);
                        },
                        (double angle, bool allowToggle, uint address) =>
                        {
                            return PositionAngle.SetAngleTo(func1(address), func2(address), angle, KeyboardUtilities.GetToggle(allowToggle));
                        }));

                    _dictionary.Add(String.Format("DAngle{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetDAngleTo(func1(address), func2(address), null, false);
                        },
                        (double angleDiff, bool allowToggle, uint address) =>
                        {
                            return PositionAngle.SetDAngleTo(func1(address), func2(address), angleDiff, KeyboardUtilities.GetToggle(allowToggle));
                        }));

                    _dictionary.Add(String.Format("AngleDiff{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleDifference(func1(address), func2(address), false);
                        },
                        (double angleDiff, bool allowToggle, uint address) =>
                        {
                            return PositionAngle.SetAngleDifference(func1(address), func2(address), angleDiff, KeyboardUtilities.GetToggle(allowToggle));
                        }));
                }
            }
        }

        public static void AddLiteralEntriesToDictionary()
        {
            // Buffer

            _dictionary.Add("Buffer",
                ((uint objAddress) => 0,
                DEFAULT_SETTER));

            // Object vars

            _dictionary.Add("DAngleMarioToObjMod512",
                ((uint objAddress) =>
                {
                    double dAngle = PositionAngle.GetDAngleTo(PositionAngle.Mario, PositionAngle.Obj(objAddress), false, false);
                    return MoreMath.MaybeNegativeModulus(dAngle, 512);
                },
                DEFAULT_SETTER));

            _dictionary.Add("PitchMarioToObj",
                ((uint objAddress) =>
                {
                    PositionAngle mario = PositionAngle.Mario;
                    PositionAngle obj = PositionAngle.Obj(objAddress);
                    return MoreMath.GetPitch(mario.X, mario.Y, mario.Z, obj.X, obj.Y, obj.Z);
                },
                DEFAULT_SETTER));

            _dictionary.Add("DPitchMarioToObj",
                ((uint objAddress) =>
                {
                    PositionAngle mario = PositionAngle.Mario;
                    PositionAngle obj = PositionAngle.Obj(objAddress);
                    double pitch = MoreMath.GetPitch(mario.X, mario.Y, mario.Z, obj.X, obj.Y, obj.Z);
                    ushort marioPitch = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
                    return marioPitch - pitch;
                },
                (double diff, bool allowToggle, uint objAddress) =>
                {
                    PositionAngle mario = PositionAngle.Mario;
                    PositionAngle obj = PositionAngle.Obj(objAddress);
                    double pitch = MoreMath.GetPitch(mario.X, mario.Y, mario.Z, obj.X, obj.Y, obj.Z);
                    short newMarioPitch = MoreMath.NormalizeAngleShort(pitch + diff);
                    return Config.Stream.SetValue(newMarioPitch, MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
                }));

            _dictionary.Add("ObjectInGameDeltaYaw",
                ((uint objAddress) =>
                {
                    ushort objectAngle = Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    return GetDeltaInGameAngle(objectAngle);
                },
                DEFAULT_SETTER));

            _dictionary.Add("EffectiveHitboxRadius",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);
                    float objHitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
                    return mObjHitboxRadius + objHitboxRadius;
                },
                DEFAULT_SETTER));

            _dictionary.Add("EffectiveHurtboxRadius",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);
                    float objHurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);
                    return mObjHurtboxRadius + objHurtboxRadius;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioHitboxAwayFromObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
                    float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
                    float mObjHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);

                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float objHitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);

                    double marioHitboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
                    return marioHitboxAwayFromObject;
                },
                (double hitboxDistAway, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
                    float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
                    float mObjHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);

                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float objHitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = hitboxDistAway + mObjHitboxRadius + objHitboxRadius;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetHDistance(objPA, marioPA, distAway, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetHDistance(objPA, marioObjPA, distAway, KeyboardUtilities.GetToggle(allowToggle)));
                }
            ));

            _dictionary.Add("MarioHitboxAboveObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHitboxBottom = mObjY - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                    double marioHitboxAboveObject = mObjHitboxBottom - objHitboxTop;
                    return marioHitboxAboveObject;
                },
                (double hitboxDistAbove, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                    double newMarioY = objHitboxTop + mObjHitboxDownOffset + hitboxDistAbove;
                    double deltaY = newMarioY - objY;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetYDistance(objPA, marioPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetYDistance(objPA, marioObjPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)));
                }));

            _dictionary.Add("MarioHitboxBelowObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHitboxBottom = objY - objHitboxDownOffset;

                    double marioHitboxBelowObject = objHitboxBottom - mObjHitboxTop;
                    return marioHitboxBelowObject;
                }, 
                (double hitboxDistBelow, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHitboxBottom = objY - objHitboxDownOffset;

                    double newMarioY = objHitboxBottom - (mObjHitboxTop - mObjY) - hitboxDistBelow;
                    double deltaY = newMarioY - objY;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetYDistance(objPA, marioPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetYDistance(objPA, marioObjPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)));
                }
            ));

            _dictionary.Add("MarioHitboxOverlapsObject",
                ((uint objAddress) =>
                {
                    return IsMarioHitboxOverlapping(objAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioHurtboxAwayFromObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
                    float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
                    float mObjHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);

                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float objHurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);

                    double marioHurtboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHurtboxRadius - objHurtboxRadius;
                    return marioHurtboxAwayFromObject;
                },
                (double hurtboxDistAway, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
                    float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
                    float mObjHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);

                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float objHurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = hurtboxDistAway + mObjHurtboxRadius + objHurtboxRadius;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetHDistance(objPA, marioPA, distAway, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetHDistance(objPA, marioObjPA, distAway, KeyboardUtilities.GetToggle(allowToggle)));
                }
            ));

            _dictionary.Add("MarioHurtboxAboveObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHurtboxBottom = mObjY - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHurtboxTop = objY + objHurtboxHeight - objHitboxDownOffset;

                    double marioHurtboxAboveObject = mObjHurtboxBottom - objHurtboxTop;
                    return marioHurtboxAboveObject;
                },
                (double hurtboxDistAbove, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHurtboxTop = objY + objHurtboxHeight - objHitboxDownOffset;

                    double newMarioY = objHurtboxTop + mObjHitboxDownOffset + hurtboxDistAbove;
                    double deltaY = newMarioY - objY;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetYDistance(objPA, marioPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetYDistance(objPA, marioObjPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)));
                }
            ));

            _dictionary.Add("MarioHurtboxBelowObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHurtboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHurtboxBottom = objY - objHitboxDownOffset;

                    double marioHurtboxBelowObject = objHurtboxBottom - mObjHurtboxTop;
                    return marioHurtboxBelowObject;
                },
                (double hurtboxDistBelow, bool allowToggle, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHurtboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objHurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHurtboxBottom = objY - objHitboxDownOffset;

                    double newMarioY = objHurtboxBottom - (mObjHurtboxTop - mObjY) - hurtboxDistBelow;
                    double deltaY = newMarioY - objY;

                    PositionAngle marioPA = PositionAngle.Mario;
                    PositionAngle marioObjPA = PositionAngle.MarioObj();
                    PositionAngle objPA = PositionAngle.Obj(objAddress);

                    return BoolUtilities.Combine(
                        PositionAngle.SetYDistance(objPA, marioPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)),
                        PositionAngle.SetYDistance(objPA, marioObjPA, deltaY, KeyboardUtilities.GetToggle(allowToggle)));
                }
            ));

            _dictionary.Add("MarioHurtboxOverlapsObject",
                ((uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
                    float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
                    float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
                    float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
                    float mObjHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);
                    float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
                    float mObjHurtboxBottom = mObjY - mObjHitboxDownOffset;
                    float mObjHurtboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float objHurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);
                    float objHurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
                    float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
                    float objHurtboxBottom = objY - objHitboxDownOffset;
                    float objHurtboxTop = objY + objHurtboxHeight - objHitboxDownOffset;

                    double marioHurtboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHurtboxRadius - objHurtboxRadius;
                    double marioHurtboxAboveObject = mObjHurtboxBottom - objHurtboxTop;
                    double marioHurtboxBelowObject = objHurtboxBottom - mObjHurtboxTop;

                    bool overlap = marioHurtboxAwayFromObject < 0 && marioHurtboxAboveObject <= 0 && marioHurtboxBelowObject <= 0;
                    return overlap ? 1 : 0;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioPunchAngleAway",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                        marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                    double angleDiff = marioPos.Angle - angleToObj;
                    int angleDiffShort = MoreMath.NormalizeAngleShort(angleDiff);
                    int angleDiffAbs = Math.Abs(angleDiffShort);
                    int angleAway = angleDiffAbs - 0x2AAA;
                    return angleAway;
                },
                (double angleAway, bool allowToggle, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                        marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                    double oldAngleDiff = marioPos.Angle - angleToObj;
                    int oldAngleDiffShort = MoreMath.NormalizeAngleShort(oldAngleDiff);
                    int signMultiplier = oldAngleDiffShort >= 0 ? 1 : -1;

                    double angleDiffAbs = angleAway + 0x2AAA;
                    double angleDiff = angleDiffAbs * signMultiplier;
                    double marioAngleDouble = angleToObj + angleDiff;
                    ushort marioAngleUShort = MoreMath.NormalizeAngleUshort(marioAngleDouble);

                    return Config.Stream.SetValue(marioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("ObjectProcessGroup",
                ((uint processGroupUint) =>
                {
                    sbyte processGroupByte = processGroupUint == uint.MaxValue ? (sbyte)(-1) : (sbyte)processGroupUint;
                    return processGroupByte;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectProcessGroupDescription",
                ((uint processGroupUint) =>
                {
                    return ProcessGroupUtilities.GetProcessGroupDescription(processGroupUint);
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectRngIndex",
                ((uint objAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUShort(objAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    return coinRngIndex;
                },
                (int rngIndex, bool allowToggle, uint objAddress) =>
                {
                    ushort coinRngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(coinRngValue, objAddress + ObjectConfig.YawMovingOffset);
                }));

            _dictionary.Add("ObjectRngIndexDiff",
                ((uint objAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUShort(objAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    int rngIndexDiff = coinRngIndex - SpecialConfig.GoalRngIndex;
                    return rngIndexDiff;
                },
                (int rngIndexDiff, bool allowToggle, uint objAddress) =>
                {
                    int coinRngIndex = SpecialConfig.GoalRngIndex + rngIndexDiff;
                    ushort coinRngValue = RngIndexer.GetRngValue(coinRngIndex);
                    return Config.Stream.SetValue(coinRngValue, objAddress + ObjectConfig.YawMovingOffset);
                }));

            // Object specific vars - Pendulum

            _dictionary.Add("PendulumCountdown",
                ((uint objAddress) =>
                {
                    int pendulumCountdown = GetPendulumCountdown(objAddress);
                    return pendulumCountdown;
                },
                DEFAULT_SETTER));

            _dictionary.Add("PendulumAmplitude",
                ((uint objAddress) =>
                {
                    float pendulumAmplitude = GetPendulumAmplitude(objAddress);
                    return pendulumAmplitude;
                },
                (double amplitude, bool allowToggle, uint objAddress) =>
                {
                    float accelerationDirection = amplitude > 0 ? -1 : 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                    success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                    success &= Config.Stream.SetValue((float)amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                    return success;
                }));

            _dictionary.Add("PendulumSwingIndex",
                ((uint objAddress) =>
                {
                    float pendulumAmplitudeFloat = GetPendulumAmplitude(objAddress);
                    int? pendulumAmplitudeIntNullable = ParsingUtilities.ParseIntNullable(pendulumAmplitudeFloat);
                    if (!pendulumAmplitudeIntNullable.HasValue) return Double.NaN.ToString();
                    int pendulumAmplitudeInt = pendulumAmplitudeIntNullable.Value;
                    return TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(pendulumAmplitudeInt);
                },
                (int index, bool allowToggle, uint objAddress) =>
                {
                    float amplitude = TableConfig.PendulumSwings.GetPendulumAmplitude(index);
                    float accelerationDirection = amplitude > 0 ? -1 : 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                    success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                    success &= Config.Stream.SetValue(amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                    return success;
                }));

            // Object specific vars - Cog

            _dictionary.Add("CogCountdown",
                ((uint objAddress) =>
                {
                    int cogCountdown = GetCogNumFramesInRotation(objAddress);
                    return cogCountdown;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CogEndingYaw",
                ((uint objAddress) =>
                {
                    ushort cogEndingYaw = GetCogEndingYaw(objAddress);
                    return cogEndingYaw;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CogRotationIndex",
                ((uint objAddress) =>
                {
                    ushort yawFacing = Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    double rotationIndex = CogUtilities.GetRotationIndex(yawFacing) ?? Double.NaN;
                    return rotationIndex;
                },
                DEFAULT_SETTER));

            // Object specific vars - Waypoint

            _dictionary.Add("ObjectDotProductToWaypoint",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return dotProduct;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectDistanceToWaypointPlane",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypointPlane;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectDistanceToWaypoint",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypoint;
                },
                DEFAULT_SETTER));

            // Object specific vars - Racing Penguin

            _dictionary.Add("RacingPenguinEffortTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinEffortChange",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortChange;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinMinHSpeed",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return minHSpeed;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinDiffHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    float hSpeed = Config.Stream.GetFloat(objAddress + ObjectConfig.HSpeedOffset);
                    double hSpeedDiff = hSpeed - hSpeedTarget;
                    return hSpeedDiff;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinProgress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            // Object specific vars - Koopa the Quick

            _dictionary.Add("KoopaTheQuickHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuickHSpeedChange",
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedChange;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick1Progress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick2Progress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick2Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick1ProgressOld",
                ((uint objAddress) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    double progressOld = PlushUtilities.GetProgress(globalTimer);
                    return progressOld;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick1ProgressDiff",
                ((uint objAddress) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    double progressOld = PlushUtilities.GetProgress(globalTimer);
                    double progressNew = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                    return progressNew - progressOld;
                },
                DEFAULT_SETTER));

            // Object specific vars - Fly Guy

            _dictionary.Add("FlyGuyZone",
                ((uint objAddress) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double heightDiff = marioY - objY;
                    if (heightDiff < -400) return "Low";
                    if (heightDiff > -200) return "High";
                    return "Medium";
                },
                DEFAULT_SETTER));

            _dictionary.Add("FlyGuyRelativeHeight",
                ((uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double relativeHeight = TableConfig.FlyGuyData.GetRelativeHeight(oscillationTimer);
                    return relativeHeight;
                },
                DEFAULT_SETTER));

            _dictionary.Add("FlyGuyMinHeight",
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double minHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, objY);
                    return minHeight;
                },
                (double newMinHeight, bool allowToggle, uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    float oldHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double oldMinHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, oldHeight);
                    double heightDiff = newMinHeight - oldMinHeight;
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("FlyGuyMaxHeight",
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double maxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, objY);
                    return maxHeight;
                },
                (double newMaxHeight, bool allowToggle, uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    float oldHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double oldMaxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, oldHeight);
                    double heightDiff = newMaxHeight - oldMaxHeight;
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("FlyGuyActivationDistanceDiff",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 4000;
                    return distDiff;
                },
                (double distDiff, bool allowToggle, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = distDiff + 4000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                }));

            // Object specific vars - Bob-omb

            _dictionary.Add("BobombBloatSize",
                ((uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
                    float bloatSize = (hitboxRadius - 65) / 13;
                    return bloatSize;
                },
                (float bloatSize, bool allowToggle, uint objAddress) =>
                {
                    float hitboxRadius = bloatSize * 13 + 65;
                    float hitboxHeight = bloatSize * 22.6f + 113;
                    float scale = bloatSize / 5 + 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(hitboxRadius, objAddress + ObjectConfig.HitboxRadiusOffset);
                    success &= Config.Stream.SetValue(hitboxHeight, objAddress + ObjectConfig.HitboxHeightOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                    return success;
                }));

            _dictionary.Add("BobombRadius",
                ((uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
                    float radius = hitboxRadius + 32;
                    return radius;
                },
                (float radius, bool allowToggle, uint objAddress) =>
                {
                    float bloatSize = (radius - 97) / 13;
                    float hitboxRadius = bloatSize * 13 + 65;
                    float hitboxHeight = bloatSize * 22.6f + 113;
                    float scale = bloatSize / 5 + 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(hitboxRadius, objAddress + ObjectConfig.HitboxRadiusOffset);
                    success &= Config.Stream.SetValue(hitboxHeight, objAddress + ObjectConfig.HitboxHeightOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                    success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                    return success;
                }));

            _dictionary.Add("BobombSpaceBetween",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double hDist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                    float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
                    float radius = hitboxRadius + 32;
                    double spaceBetween = hDist - radius;
                    return spaceBetween;
                },
                (double spaceBetween, bool allowToggle, uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
                    float radius = hitboxRadius + 32;
                    double distAway = spaceBetween + radius;

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    (double newMarioX, double newMarioZ) =
                        MoreMath.ExtrapolateLine2D(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("BobombHomeRadiusDiff",
                ((uint objAddress) =>
                {
                    return GetRadiusDiff(PositionAngle.Mario, PositionAngle.ObjHome(objAddress), 400);
                },
                (double dist, bool allowToggle, uint objAddress) =>
                {
                    return SetRadiusDiff(PositionAngle.Mario, PositionAngle.ObjHome(objAddress), 400, dist);
                }));

            // Object specific vars - Chuckya

            _dictionary.Add("ChuckyaAngleMod1024",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUShort(objAddress + ObjectConfig.YawMovingOffset);
                    int mod = angle % 1024;
                    return mod;
                },
                DEFAULT_SETTER));

            // Object specific vars - Scuttlebug

            _dictionary.Add("ScuttlebugDeltaAngleToTarget",
                ((uint objAddress) =>
                {
                    ushort facingAngle = Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    ushort targetAngle = Config.Stream.GetUShort(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                    int angleDiff = facingAngle - targetAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint objAddress) =>
                {
                    ushort targetAngle = Config.Stream.GetUShort(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                    double newObjAngleDouble = targetAngle + angleDiff;
                    ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                    return PositionAngle.Obj(objAddress).SetAngle(newObjAngleUShort);
                }));

            // Object specific vars - Goomba Triplet Spawner

            _dictionary.Add("GoombaTripletLoadingRadiusDiff",
                ((uint objAddress) =>
                {
                    return GetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), 3000);
                },
                (double dist, bool allowToggle, uint objAddress) =>
                {
                    return SetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), 3000, dist);
                }));

            _dictionary.Add("GoombaTripletUnloadingRadiusDiff",
                ((uint objAddress) =>
                {
                    return GetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), 4000);
                },
                (double dist, bool allowToggle, uint objAddress) =>
                {
                    return SetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), 4000, dist);
                }));

            // Object specific vars - BitFS Platform

            _dictionary.Add("BitfsPlatformGroupMinHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMinHeight(timer, height);
                },
                (double newMinHeight, bool allowToggle, uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double oldMinHeight = BitfsPlatformGroupTable.GetMinHeight(timer, height);
                    double heightDiff = newMinHeight - oldMinHeight;
                    float oldHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("BitfsPlatformGroupMaxHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                },
                (double newMaxHeight, bool allowToggle, uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double oldMaxHeight = BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                    double heightDiff = newMaxHeight - oldMaxHeight;
                    float oldHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("BitfsPlatformGroupRelativeHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    return BitfsPlatformGroupTable.GetRelativeHeightFromMin(timer);
                },
                DEFAULT_SETTER));

            _dictionary.Add("BitfsPlatformGroupDisplacedHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float homeHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HomeYOffset);
                    return BitfsPlatformGroupTable.GetDisplacedHeight(timer, height, homeHeight);
                },
                (double displacedHeight, bool allowToggle, uint objAddress) =>
                {
                    float homeHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HomeYOffset);
                    double newMaxHeight = homeHeight + displacedHeight;
                    int timer = Config.Stream.GetInt(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float relativeHeightFromMax = BitfsPlatformGroupTable.GetRelativeHeightFromMax(timer);
                    double newHeight = newMaxHeight + relativeHeightFromMax;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            // Object specific vars - Hoot

            _dictionary.Add("HootReleaseTimer",
                ((uint objAddress) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    uint lastReleaseTime = Config.Stream.GetUInt(objAddress + ObjectConfig.HootLastReleaseTimeOffset);
                    int diff = (int)(globalTimer - lastReleaseTime);
                    return diff;
                },
                (int newDiff, bool allowToggle, uint objAddress) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    uint newLastReleaseTime = (uint)(globalTimer - newDiff);
                    return Config.Stream.SetValue(newLastReleaseTime, objAddress + ObjectConfig.HootLastReleaseTimeOffset);
                }));

            // Object specific vars - Power Star

            _dictionary.Add("PowerStarMissionName",
                ((uint objAddress) =>
                {
                    int courseIndex = Config.Stream.GetShort(MiscConfig.LevelIndexAddress);
                    int missionIndex = Config.Stream.GetByte(objAddress + ObjectConfig.PowerStarMissionIndexOffset);
                    return TableConfig.Missions.GetInGameMissionName(courseIndex, missionIndex);
                },
                DEFAULT_SETTER));

            // Object specific vars - Coordinates

            _dictionary.Add("MinXCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinX());
                },
                (float newMinX, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minX = tris.Min(tri => tri.GetMinX());
                    float diff = newMinX - minX;
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float newObjX = objX + diff;
                    return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                }));

            _dictionary.Add("MaxXCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxX());
                },
                (float newMaxX, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxX = tris.Max(tri => tri.GetMaxX());
                    float diff = newMaxX - maxX;
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float newObjX = objX + diff;
                    return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                }));

            _dictionary.Add("MinYCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinY());
                },
                (float newMinY, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minY = tris.Min(tri => tri.GetMinY());
                    float diff = newMinY - minY;
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float newObjY = objY + diff;
                    return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("MaxYCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxY());
                },
                (float newMaxY, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxY = tris.Max(tri => tri.GetMaxY());
                    float diff = newMaxY - maxY;
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float newObjY = objY + diff;
                    return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("MinZCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinZ());
                },
                (float newMinZ, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minZ = tris.Min(tri => tri.GetMinZ());
                    float diff = newMinZ - minZ;
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float newObjZ = objZ + diff;
                    return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                }));

            _dictionary.Add("MaxZCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxZ());
                },
                (float newMaxZ, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxZ = tris.Max(tri => tri.GetMaxZ());
                    float diff = newMaxZ - maxZ;
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float newObjZ = objZ + diff;
                    return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                }));

            _dictionary.Add("RangeXCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxX()) - tris.Min(tri => tri.GetMinX());
                },
                (float newXRange, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float xRange = tris.Max(tri => tri.GetMaxX()) - tris.Min(tri => tri.GetMinX());
                    float ratio = newXRange / xRange;
                    float scaleX = Config.Stream.GetFloat(objAddress + ObjectConfig.ScaleWidthOffset);
                    float newScaleX = scaleX * ratio;
                    return Config.Stream.SetValue(newScaleX, objAddress + ObjectConfig.ScaleWidthOffset);
                }));

            _dictionary.Add("RangeYCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxY()) - tris.Min(tri => tri.GetMinY());
                },
                (float newYRange, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float yRange = tris.Max(tri => tri.GetMaxY()) - tris.Min(tri => tri.GetMinY());
                    float ratio = newYRange / yRange;
                    float scaleY = Config.Stream.GetFloat(objAddress + ObjectConfig.ScaleHeightOffset);
                    float newScaleY = scaleY * ratio;
                    return Config.Stream.SetValue(newScaleY, objAddress + ObjectConfig.ScaleHeightOffset);
                }));

            _dictionary.Add("RangeZCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxZ()) - tris.Min(tri => tri.GetMinZ());
                },
                (float newZRange, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float zRange = tris.Max(tri => tri.GetMaxZ()) - tris.Min(tri => tri.GetMinZ());
                    float ratio = newZRange / zRange;
                    float scaleZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ScaleDepthOffset);
                    float newScaleZ = scaleZ * ratio;
                    return Config.Stream.SetValue(newScaleZ, objAddress + ObjectConfig.ScaleDepthOffset);
                }));

            _dictionary.Add("MidpointXCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return (tris.Max(tri => tri.GetMaxX()) + tris.Min(tri => tri.GetMinX())) / 2.0;
                },
                (float newMidpointX, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float midpointX = (tris.Max(tri => tri.GetMaxX()) + tris.Min(tri => tri.GetMinX())) / 2f;
                    float diff = newMidpointX - midpointX;
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float newObjX = objX + diff;
                    return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                }));

            _dictionary.Add("MidpointYCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return (tris.Max(tri => tri.GetMaxY()) + tris.Min(tri => tri.GetMinY())) / 2.0;
                },
                (float newMidpointY, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float midpointY = (tris.Max(tri => tri.GetMaxY()) + tris.Min(tri => tri.GetMinY())) / 2f;
                    float diff = newMidpointY - midpointY;
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float newObjY = objY + diff;
                    return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("MidpointZCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return (tris.Max(tri => tri.GetMaxZ()) + tris.Min(tri => tri.GetMinZ())) / 2.0;
                },
                (float newMidpointZ, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    float midpointZ = (tris.Max(tri => tri.GetMaxZ()) + tris.Min(tri => tri.GetMinZ())) / 2f;
                    float diff = newMidpointZ - midpointZ;
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float newObjZ = objZ + diff;
                    return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                }));

            _dictionary.Add("FarthestCoordinateDistance",
                ((uint objAddress) =>
                {
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);

                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;

                    List<(int, int, int)> coordinates = new List<(int, int, int)>();
                    tris.ForEach(tri =>
                    {
                        coordinates.Add((tri.X1, tri.Y1, tri.Z1));
                        coordinates.Add((tri.X2, tri.Y2, tri.Z2));
                        coordinates.Add((tri.X3, tri.Y3, tri.Z3));
                    });
                    return coordinates.Max(coord => MoreMath.GetDistanceBetween(objX, objY, objZ, coord.Item1, coord.Item2, coord.Item3));
                },
                DEFAULT_SETTER));

            _dictionary.Add("MinXFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinX());
                },
                (float newMinX, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minX = tris.Min(tri => tri.GetMinX());
                    float diff = newMinX - minX;
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float newObjX = objX + diff;
                    return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                }
            ));

            _dictionary.Add("MaxXFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxX());
                },
                (float newMaxX, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxX = tris.Max(tri => tri.GetMaxX());
                    float diff = newMaxX - maxX;
                    float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float newObjX = objX + diff;
                    return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                }
            ));

            _dictionary.Add("MinYFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinY());
                },
                (float newMinY, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minY = tris.Min(tri => tri.GetMinY());
                    float diff = newMinY - minY;
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float newObjY = objY + diff;
                    return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                }
            ));

            _dictionary.Add("MaxYFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxY());
                },
                (float newMaxY, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxY = tris.Max(tri => tri.GetMaxY());
                    float diff = newMaxY - maxY;
                    float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    float newObjY = objY + diff;
                    return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                }
            ));

            _dictionary.Add("MinZFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Min(tri => tri.GetMinZ());
                },
                (float newMinZ, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int minZ = tris.Min(tri => tri.GetMinZ());
                    float diff = newMinZ - minZ;
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float newObjZ = objZ + diff;
                    return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                }
            ));

            _dictionary.Add("MaxZFloorCoordinate",
                ((uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return double.NaN;
                    return tris.Max(tri => tri.GetMaxZ());
                },
                (float newMaxZ, bool allowToggle, uint objAddress) =>
                {
                    List<TriangleDataModel> tris = TriangleUtilities.GetObjectFloorTrianglesForObject(objAddress);
                    if (tris.Count == 0) return false;
                    int maxZ = tris.Max(tri => tri.GetMaxZ());
                    float diff = newMaxZ - maxZ;
                    float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float newObjZ = objZ + diff;
                    return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                }
            ));

            // Object specific vars - Rolling Log

            _dictionary.Add("RollingLogDistLimit",
                ((uint objAddress) =>
                {
                    float distLimitSquared = Config.Stream.GetFloat(objAddress + ObjectConfig.RollingLogDistLimitSquaredOffset);
                    double distLimit = Math.Sqrt(distLimitSquared);
                    return distLimit;
                },
                (double newDistLimit, bool allowToggle, uint objAddress) =>
                {
                    double newDistLimitSquared = newDistLimit * newDistLimit;
                    return Config.Stream.SetValue((float)newDistLimitSquared, objAddress + ObjectConfig.RollingLogDistLimitSquaredOffset);
                }));

            _dictionary.Add("RollingLogDist",
                ((uint objAddress) =>
                {
                    float x = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    float z = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    float xCenter = Config.Stream.GetFloat(objAddress + ObjectConfig.RollingLogXCenterOffset);
                    float zCenter = Config.Stream.GetFloat(objAddress + ObjectConfig.RollingLogZCenterOffset);
                    double dist = MoreMath.GetDistanceBetween(xCenter, zCenter, x, z);
                    return dist;
                },
                DEFAULT_SETTER));

            // Object specific vars - Object Spawner

            _dictionary.Add("ObjectSpawnerRadiusDiff",
                ((uint objAddress) =>
                {
                    float radius = Config.Stream.GetFloat(objAddress + ObjectConfig.ObjectSpawnerRadiusOffset);
                    return GetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), radius);
                },
                (double dist, bool allowToggle, uint objAddress) =>
                {
                    float radius = Config.Stream.GetFloat(objAddress + ObjectConfig.ObjectSpawnerRadiusOffset);
                    return SetRadiusDiff(PositionAngle.Mario, PositionAngle.Obj(objAddress), radius, dist);
                }));

            // Object specific vars - WDW Rotating Platform

            _dictionary.Add("WdwRotatingPlatformCurrentIndex",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    return TableConfig.WdwRotatingPlatformTable.GetIndex(angle) ?? double.NaN;
                },
                (int index, bool allowToggle, uint objAddress) =>
                {
                    ushort angle = TableConfig.WdwRotatingPlatformTable.GetAngle(index);
                    return Config.Stream.SetValue(angle, objAddress + ObjectConfig.YawFacingOffset);
                }));

            _dictionary.Add("WdwRotatingPlatformGoalIndex",
                ((uint dummy) =>
                {
                    return TableConfig.WdwRotatingPlatformTable.GetIndex(TableConfig.WdwRotatingPlatformTable.GoalAngle) ?? double.NaN;
                },
                (int index, bool allowToggle, uint dummy) =>
                {
                    TableConfig.WdwRotatingPlatformTable.GoalAngle = TableConfig.WdwRotatingPlatformTable.GetAngle(index);
                    return true;
                }));

            _dictionary.Add("WdwRotatingPlatformGoalAngle",
                ((uint dummy) =>
                {
                    return TableConfig.WdwRotatingPlatformTable.GoalAngle;
                },
                (ushort goalAngle, bool allowToggle, uint dummy) =>
                {
                    TableConfig.WdwRotatingPlatformTable.GoalAngle = goalAngle;
                    return true;
                }));

            _dictionary.Add("WdwRotatingPlatformFramesUntilGoal",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    return TableConfig.WdwRotatingPlatformTable.GetFramesToGoalAngle(angle);
                },
                (int numFrames, bool allowToggle, uint objAddress) =>
                {
                    ushort? newAngle = TableConfig.WdwRotatingPlatformTable.GetAngleNumFramesBeforeGoal(numFrames);
                    if (!newAngle.HasValue) return false;
                    return Config.Stream.SetValue(newAngle.Value, objAddress + ObjectConfig.YawFacingOffset);
                }));

            // Object specific vars - Elevator Axle

            _dictionary.Add("ElevatorAxleCurrentIndex",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUShort(objAddress + ObjectConfig.RollFacingOffset);
                    return TableConfig.ElevatorAxleTable.GetIndex(angle) ?? double.NaN;
                },
                (int index, bool allowToggle, uint objAddress) =>
                {
                    ushort angle = TableConfig.ElevatorAxleTable.GetAngle(index);
                    return Config.Stream.SetValue(angle, objAddress + ObjectConfig.RollFacingOffset);
                }));
            
            _dictionary.Add("ElevatorAxleGoalIndex",
                ((uint dummy) =>
                {
                    return TableConfig.ElevatorAxleTable.GetIndex(TableConfig.ElevatorAxleTable.GoalAngle) ?? double.NaN;
                },
                (int index, bool allowToggle, uint dummy) =>
                {
                    TableConfig.ElevatorAxleTable.GoalAngle = TableConfig.ElevatorAxleTable.GetAngle(index);
                    return true;
                }));

            _dictionary.Add("ElevatorAxleGoalAngle",
                ((uint dummy) =>
                {
                    return TableConfig.ElevatorAxleTable.GoalAngle;
                },
                (ushort goalAngle, bool allowToggle, uint dummy) =>
                {
                    TableConfig.ElevatorAxleTable.GoalAngle = goalAngle;
                    return true;
                }));

            _dictionary.Add("ElevatorAxleFramesUntilGoal",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUShort(objAddress + ObjectConfig.RollFacingOffset);
                    return TableConfig.ElevatorAxleTable.GetFramesToGoalAngle(angle);
                },
                (int numFrames, bool allowToggle, uint objAddress) =>
                {
                    ushort? newAngle = TableConfig.ElevatorAxleTable.GetAngleNumFramesBeforeGoal(numFrames);
                    if (!newAngle.HasValue) return false;
                    return Config.Stream.SetValue(newAngle.Value, objAddress + ObjectConfig.RollFacingOffset);
                }));

            // Object specific vars - Swooper

            _dictionary.Add("SwooperEffectiveTargetYaw",
                ((uint objAddress) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    int targetAngle = Config.Stream.GetInt(objAddress + ObjectConfig.SwooperTargetYawOffset);
                    return targetAngle + (short)(3000 * InGameTrigUtilities.InGameCosine(4000 * (int)globalTimer));
                },
                DEFAULT_SETTER));

            // Mario vars

            _dictionary.Add("RotationDisplacementX",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item1;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RotationDisplacementY",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item2;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RotationDisplacementZ",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item3;
                },
                DEFAULT_SETTER));

            _dictionary.Add("SpeedMultiplier",
                ((uint dummy) =>
                {
                    /*
                    intended dyaw = intended yaw - slide yaw (idk what this is called in stroop)
                    if cos(intended dyaw) < 0 and fspeed >= 0:
                      K = 0.5 + 0.5 * fspeed / 100
                    else:
                      K = 1

                    multiplier = (intended mag / 32) * cos(intended dyaw) * K * 0.02 + A

                    slide: A = 0.98
                    slippery: A = 0.96
                    default: A = 0.92
                    not slippery: A = 0.92
                    */

                    ushort intendedYaw = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
                    ushort movingYaw = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.MovingYawOffset);
                    float scaledMagnitude = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ScaledMagnitudeOffset);
                    float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    uint floorAddress = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    TriangleDataModel floorStruct = TriangleDataModel.Create(floorAddress);
                    double A = floorStruct.FrictionMultiplier;

                    int intendedDYaw = intendedYaw - movingYaw;
                    double K = InGameTrigUtilities.InGameCosine(intendedDYaw) < 0 && hSpeed >= 0 ? 0.5 + 0.5 * hSpeed / 100 : 1;
                    double multiplier = (scaledMagnitude / 32) * InGameTrigUtilities.InGameCosine(intendedDYaw) * K * 0.02 + A;

                    return multiplier;
                },
                DEFAULT_SETTER));

            _dictionary.Add("DeFactoSpeed",
                ((uint dummy) =>
                {
                    return GetMarioDeFactoSpeed();
                },
                (double newDefactoSpeed, bool allowToggle, uint dummy) =>
                {
                    double newHSpeed = newDefactoSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("SidewaysSpeed",
                ((uint dummy) =>
                {
                    float xSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
                    float zSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
                    double speedMagnitude = MoreMath.GetHypotenuse(xSpeed, zSpeed);
                    double speedAngle = MoreMath.AngleTo_AngleUnits(xSpeed, zSpeed);
                    ushort angle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    (double sidewaysDist, double forwardsDist) = MoreMath.GetComponentsFromVectorRelatively(speedMagnitude, speedAngle, angle);
                    return sidewaysDist;
                },
                DEFAULT_SETTER));

            _dictionary.Add("SlidingSpeed",
                ((uint dummy) =>
                {
                    return GetMarioSlidingSpeed();
                },
                (double newHSlidingSpeed, bool allowToggle, uint dummy) =>
                {
                    float xSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    float zSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    if (xSlidingSpeed == 0 && zSlidingSpeed == 0) xSlidingSpeed = 1;
                    double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                    double multiplier = newHSlidingSpeed / hSlidingSpeed;
                    double newXSlidingSpeed = xSlidingSpeed * multiplier;
                    double newZSlidingSpeed = zSlidingSpeed * multiplier;

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    return success;
                }));

            _dictionary.Add("SlidingAngle",
                ((uint dummy) =>
                {
                    return GetMarioSlidingAngle();
                },
                (double newHSlidingAngle, bool allowToggle, uint dummy) =>
                {
                    float xSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    float zSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                    (double newXSlidingSpeed, double newZSlidingSpeed) =
                        MoreMath.GetComponentsFromVector(hSlidingSpeed, newHSlidingAngle);

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    return success;
                }));

            _dictionary.Add("TwirlYawMod2048",
                ((uint dummy) =>
                {
                    ushort twirlYaw = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.TwirlYawOffset);
                    return twirlYaw % 2048;
                },
                DEFAULT_SETTER));

            _dictionary.Add("FlyingEnergy",
                ((uint dummy) =>
                {
                    return FlyingUtilities.GetEnergy();
                },
                DEFAULT_SETTER));

            _dictionary.Add("BobombTrajectoryFramesToPoint",
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointPA.Y - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (double frames, bool allowToggle, uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetBobombTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointPA.Y - yDist;
                    (double newX, double newZ) = MoreMath.AddVectorToPoint(
                        hDist,
                        MoreMath.ReverseAngle(SpecialConfig.PointPA.Angle),
                        SpecialConfig.PointPA.X,
                        SpecialConfig.PointPA.Z);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                }));

            _dictionary.Add("CorkBoxTrajectoryFramesToPoint",
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointPA.Y - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (double frames, bool allowToggle, uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetCorkBoxTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointPA.Y - yDist;
                    (double newX, double newZ) = MoreMath.AddVectorToPoint(
                        hDist,
                        MoreMath.ReverseAngle(SpecialConfig.PointPA.Angle),
                        SpecialConfig.PointPA.X,
                        SpecialConfig.PointPA.Z);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                }));

            _dictionary.Add("TrajectoryRemainingHeight",
                ((uint dummy) =>
                {
                    float vSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                    double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    return remainingHeight;
                },
                (double newRemainingHeight, bool allowToggle, uint dummy) =>
                {
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                    return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                }));

            _dictionary.Add("TrajectoryPeakHeight",
                ((uint dummy) =>
                {
                    float vSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                    double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double peakHeight = marioY + remainingHeight;
                    return peakHeight;
                },
                (double newPeakHeight, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newRemainingHeight = newPeakHeight - marioY;
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                    return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpVerticalSpeed",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    return vSpeed;
                },
                (double newVSpeed, bool allowToggle, uint dummy) =>
                {
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(newVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpHeight",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    return doubleJumpHeight;
                },
                (double newHeight, bool allowToggle, uint dummy) =>
                {
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpPeakHeight",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double doubleJumpPeakHeight = marioY + doubleJumpHeight;
                    return doubleJumpPeakHeight;
                },
                (double newPeakHeight, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newHeight = newPeakHeight - marioY;
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("DeltaYawIntendedFacing",
                ((uint dummy) =>
                {
                    return GetDeltaYawIntendedFacing();
                },
                DEFAULT_SETTER));

            _dictionary.Add("DeltaYawIntendedBackwards",
                ((uint dummy) =>
                {
                    return GetDeltaYawIntendedBackwards();
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioInGameDeltaYaw",
                ((uint dummy) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    return GetDeltaInGameAngle(marioAngle);
                },
                DEFAULT_SETTER));

            _dictionary.Add("FallHeight",
                ((uint dummy) =>
                {
                    float peakHeight = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float fallHeight = peakHeight - floorY;
                    return fallHeight;
                },
                (double fallHeight, bool allowToggle, uint dummy) =>
                {
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    double newPeakHeight = floorY + fallHeight;
                    return Config.Stream.SetValue((float)newPeakHeight, MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                }));

            _dictionary.Add("FSDistPointToSelf",
                ((uint dummy) =>
                {
                    double fDist = PositionAngle.GetFDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    double sDist = PositionAngle.GetSDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    return "(" + fDist + "," + sDist + ")";
                },
                (string value, bool allowToggle, uint dummy) =>
                {
                    List<double?> values = ParsingUtilities.ParseDoubleList(value);
                    if (values.Count < 2) return false;
                    if (!values[0].HasValue || !values[1].HasValue) return false;
                    double fDist = values[0].Value;
                    double sDist = values[1].Value;
                    (double relX, double relZ) =
                        MoreMath.RotatePointAboutPointAnAngularDistance(
                            sDist, -1 * fDist, 0, 0, SpecialConfig.PointPA.Angle);
                    SpecialConfig.SelfPA.SetX(SpecialConfig.PointPA.X + relX);
                    SpecialConfig.SelfPA.SetZ(SpecialConfig.PointPA.Z + relZ);
                    return true;
                }));

            _dictionary.Add("FSDistPoint2ToSelf2",
                ((uint dummy) =>
                {
                    double fDist = PositionAngle.GetFDistance(SpecialConfig.Point2PA, SpecialConfig.Self2PA);
                    double sDist = PositionAngle.GetSDistance(SpecialConfig.Point2PA, SpecialConfig.Self2PA);
                    return "(" + fDist + "," + sDist + ")";
                },
                (string value, bool allowToggle, uint dummy) =>
                {
                    List<double?> values = ParsingUtilities.ParseDoubleList(value);
                    if (values.Count < 2) return false;
                    if (!values[0].HasValue || !values[1].HasValue) return false;
                    double fDist = values[0].Value;
                    double sDist = values[1].Value;
                    (double relX, double relZ) =
                        MoreMath.RotatePointAboutPointAnAngularDistance(
                            sDist, -1 * fDist, 0, 0, SpecialConfig.Point2PA.Angle);
                    SpecialConfig.Self2PA.SetX(SpecialConfig.Point2PA.X + relX);
                    SpecialConfig.Self2PA.SetZ(SpecialConfig.Point2PA.Z + relZ);
                    return true;
                }
            ));

            _dictionary.Add("PitchSelfToPoint",
                ((uint dummy) =>
                {
                    return MoreMath.GetPitch(
                        SpecialConfig.SelfPA.X, SpecialConfig.SelfPA.Y, SpecialConfig.SelfPA.Z,
                        SpecialConfig.PointPA.X, SpecialConfig.PointPA.Y, SpecialConfig.PointPA.Z);
                },
                DEFAULT_SETTER));

            _dictionary.Add("WalkingDistance",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    float remainder = hSpeed % 1;
                    int numFrames = (int)Math.Abs(Math.Truncate(hSpeed)) + 1;
                    float sum = (hSpeed + remainder) * numFrames / 2;
                    float distance = sum - hSpeed;
                    return distance;
                },
                DEFAULT_SETTER));

            _dictionary.Add("WalkingDistanceDifferenceMarioToPoint",
                ((uint dummy) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle pointPos = SpecialConfig.PointPA;
                    float walkingDistance = (float)_dictionary.Get("WalkingDistance").Item1(0);
                    double diff = walkingDistance - PositionAngle.GetHDistance(marioPos, pointPos);
                    return diff;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ScheduleOffset",
                ((uint dummy) =>
                {
                    return PositionAngle.ScheduleOffset;
                },
                (int value, bool allowToggle, uint dummy) =>
                {
                    PositionAngle.ScheduleOffset = value;
                    return true;
                }));

            // HUD vars

            _dictionary.Add("HudTimeText",
                ((uint dummy) =>
                {
                    ushort time = Config.Stream.GetUShort(MarioConfig.StructAddress + HudConfig.TimeOffset);
                    int totalDeciSeconds = time / 3;
                    int deciSecondComponent = totalDeciSeconds % 10;
                    int secondComponent = (totalDeciSeconds / 10) % 60;
                    int minuteComponent = (totalDeciSeconds / 600);
                    return minuteComponent + "'" + secondComponent.ToString("D2") + "\"" + deciSecondComponent;
                },
                (string timerString, bool allowToggle, uint dummy) =>
                {
                    if (timerString.Length == 0) timerString = "0" + timerString;
                    if (timerString.Length == 1) timerString = "\"" + timerString;
                    if (timerString.Length == 2) timerString = "0" + timerString;
                    if (timerString.Length == 3) timerString = "0" + timerString;
                    if (timerString.Length == 4) timerString = "'" + timerString;
                    if (timerString.Length == 5) timerString = "0" + timerString;

                    string minuteComponentString = timerString.Substring(0, timerString.Length - 5);
                    string leftMarker = timerString.Substring(timerString.Length - 5, 1);
                    string secondComponentString = timerString.Substring(timerString.Length - 4, 2);
                    string rightMarker = timerString.Substring(timerString.Length - 2, 1);
                    string deciSecondComponentString = timerString.Substring(timerString.Length - 1, 1);

                    if (leftMarker != "\"" && leftMarker != "'" && leftMarker != ".") return false;
                    if (rightMarker != "\"" && rightMarker != "'" && rightMarker != ".") return false;

                    int? minuteComponentNullable = ParsingUtilities.ParseIntNullable(minuteComponentString);
                    int? secondComponentNullable = ParsingUtilities.ParseIntNullable(secondComponentString);
                    int? deciSecondComponentNullable = ParsingUtilities.ParseIntNullable(deciSecondComponentString);

                    if (!minuteComponentNullable.HasValue ||
                        !secondComponentNullable.HasValue ||
                        !deciSecondComponentNullable.HasValue) return false;

                    int totalDeciSeconds =
                        deciSecondComponentNullable.Value +
                        secondComponentNullable.Value * 10 +
                        minuteComponentNullable.Value * 600;

                    int time = totalDeciSeconds * 3;
                    ushort timeUShort = ParsingUtilities.ParseUShortRoundingCapping(time);
                    return Config.Stream.SetValue(timeUShort, MarioConfig.StructAddress + HudConfig.TimeOffset);
                }));

            // Triangle vars

            _dictionary.Add("Classification",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.Classification.ToString();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleTypeDescription",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.Description;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleSlipperiness",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.Slipperiness;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleSlipperinessDescription",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.SlipperinessDescription;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleFrictionMultiplier",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.FrictionMultiplier;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleExertion",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    return triStruct.Exertion ? 1 : 0;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleHorizontalNormal",
                ((uint triAddress) =>
                {
                    float normalX = Config.Stream.GetFloat(triAddress + TriangleOffsetsConfig.NormX);
                    float normalZ = Config.Stream.GetFloat(triAddress + TriangleOffsetsConfig.NormZ);
                    float normalH = (float) Math.Sqrt(normalX * normalX + normalZ * normalZ);
                    return normalH;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexIndex",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexIndex(triAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexX",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).X;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexY",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Y;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexZ",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Z;
                },
                DEFAULT_SETTER));

            _dictionary.Add("Steepness",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double steepness = MoreMath.RadiansToAngleUnits(Math.Acos(triStruct.NormY));
                    return steepness;
                },
                DEFAULT_SETTER));

            _dictionary.Add("UpHillAngle",
                ((uint triAddress) =>
                {
                    return GetTriangleUphillAngle(triAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("DownHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.ReverseAngle(uphillAngle);
                },
                DEFAULT_SETTER));

            _dictionary.Add("LeftHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER));

            _dictionary.Add("RightHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER));

            _dictionary.Add("UpHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double angleDiff = marioAngle - uphillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double newMarioAngleDouble = uphillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DownHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                    double angleDiff = marioAngle - downhillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                    double newMarioAngleDouble = downhillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("LeftHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                    double angleDiff = marioAngle - lefthillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                    double newMarioAngleDouble = lefthillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("RightHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                    double angleDiff = marioAngle - righthillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                    double newMarioAngleDouble = righthillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("HillStatus",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    if (Double.IsNaN(uphillAngle)) return "No Hill";
                    double angleDiff = marioAngle - uphillAngle;
                    angleDiff = MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    bool uphill = angleDiff >= -16384 && angleDiff <= 16384;
                    return uphill ? "Uphill" : "Downhill";
                },
                DEFAULT_SETTER));

            _dictionary.Add("WallKickAngleAway",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double angleDiff = marioPos.Angle - uphillAngle;
                    int angleDiffShort = MoreMath.NormalizeAngleShort(angleDiff);
                    int angleDiffAbs = Math.Abs(angleDiffShort);
                    int angleAway = angleDiffAbs - 8192;
                    return angleAway;
                },
                (double angleAway, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double oldAngleDiff = marioPos.Angle - uphillAngle;
                    int oldAngleDiffShort = MoreMath.NormalizeAngleShort(oldAngleDiff);
                    int signMultiplier = oldAngleDiffShort >= 0 ? 1 : -1;

                    double angleDiffAbs = angleAway + 8192;
                    double angleDiff = angleDiffAbs * signMultiplier;
                    double marioAngleDouble = uphillAngle + angleDiff;
                    ushort marioAngleUShort = MoreMath.NormalizeAngleUshort(marioAngleDouble);

                    return Config.Stream.SetValue(marioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("WallKickPostAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    float normX = Config.Stream.GetFloat(triAddress + TriangleOffsetsConfig.NormX);
                    float normZ = Config.Stream.GetFloat(triAddress + TriangleOffsetsConfig.NormZ);
                    ushort wallAngle = InGameTrigUtilities.InGameATan(normZ, normX);
                    return MoreMath.NormalizeAngleUshort(wallAngle - (marioAngle - wallAngle) + 32768);
                },
                DEFAULT_SETTER));

            _dictionary.Add("DistanceAboveFloor",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    return distAboveFloor;
                },
                (double distAbove, bool allowToggle, uint dummy) =>
                {
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    double newMarioY = floorY + distAbove;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("DistanceBelowCeiling",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float ceilingY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                    float distBelowCeiling = ceilingY - marioY;
                    return distBelowCeiling;
                },
                (double distBelow, bool allowToggle, uint dummy) =>
                {
                    float ceilingY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                    double newMarioY = ceilingY - distBelow;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("NormalDistAway",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double normalDistAway =
                        marioPos.X * triStruct.NormX +
                        marioPos.Y * triStruct.NormY +
                        marioPos.Z * triStruct.NormZ +
                        triStruct.NormOffset;
                    return normalDistAway;
                },
                (double distAway, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);

                    double missingDist = distAway -
                        marioPos.X * triStruct.NormX -
                        marioPos.Y * triStruct.NormY -
                        marioPos.Z * triStruct.NormZ -
                        triStruct.NormOffset;

                    double xDiff = missingDist * triStruct.NormX;
                    double yDiff = missingDist * triStruct.NormY;
                    double zDiff = missingDist * triStruct.NormZ;

                    double newMarioX = marioPos.X + xDiff;
                    double newMarioY = marioPos.Y + yDiff;
                    double newMarioZ = marioPos.Z + zDiff;

                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                }));

            _dictionary.Add("VerticalDistAway",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double verticalDistAway =
                        marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return verticalDistAway;
                },
                (double distAbove, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double newMarioY = distAbove - (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("HeightOnTriangle",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double heightOnTriangle = triStruct.GetHeightOnTriangle(marioPos.X, marioPos.Z);
                    return heightOnTriangle;
                },
                DEFAULT_SETTER));

            _dictionary.Add("SelfNormalDistAway",
                ((uint triAddress) =>
                {
                    PositionAngle self = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double normalDistAway =
                        self.X * triStruct.NormX +
                        self.Y * triStruct.NormY +
                        self.Z * triStruct.NormZ +
                        triStruct.NormOffset;
                    return normalDistAway;
                },
                (double distAway, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle self = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);

                    double missingDist = distAway -
                        self.X * triStruct.NormX -
                        self.Y * triStruct.NormY -
                        self.Z * triStruct.NormZ -
                        triStruct.NormOffset;

                    double xDiff = missingDist * triStruct.NormX;
                    double yDiff = missingDist * triStruct.NormY;
                    double zDiff = missingDist * triStruct.NormZ;

                    double newSelfX = self.X + xDiff;
                    double newSelfY = self.Y + yDiff;
                    double newSelfZ = self.Z + zDiff;

                    return self.SetValues(x: newSelfX, y: newSelfY, z: newSelfZ);
                }
            ));

            _dictionary.Add("SelfVerticalDistAway",
                ((uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double verticalDistAway =
                        selfPos.Y + (selfPos.X * triStruct.NormX + selfPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return verticalDistAway;
                },
                (double distAbove, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double newSelfY = distAbove - (selfPos.X * triStruct.NormX + selfPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return selfPos.SetY(newSelfY);
                }
            ));

            _dictionary.Add("SelfHeightOnTriangle",
                ((uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double heightOnTriangle = triStruct.GetHeightOnTriangle(selfPos.X, selfPos.Z);
                    return heightOnTriangle;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedUphill",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, false);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedUphillAtAngle",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, true);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedDownhill",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, false);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedDownhillAtAngle",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, true);
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleCells",
                ((uint triAddress) =>
                {
                    TriangleDataModel tri = TriangleDataModel.Create(triAddress);
                    short minCellX = CellUtilities.lower_cell_index(tri.GetMinX());
                    short maxCellX = CellUtilities.upper_cell_index(tri.GetMaxX());
                    short minCellZ = CellUtilities.lower_cell_index(tri.GetMinZ());
                    short maxCellZ = CellUtilities.upper_cell_index(tri.GetMaxZ());
                    return string.Format("X:{0}-{1},Z:{2}-{3}",
                        minCellX, maxCellX, minCellZ, maxCellZ);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioCell",
                ((uint dummy) =>
                {
                    (int cellX, int cellZ) = CellUtilities.GetMarioCell();
                    return string.Format("X:{0},Z:{1}", cellX, cellZ);
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectTriCount",
                ((uint dummy) =>
                {
                    int totalTriangleCount = Config.Stream.GetInt(TriangleConfig.TotalTriangleCountAddress);
                    int levelTriangleCount = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
                    int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                    return objectTriangleCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CurrentTriangleIndex",
                ((uint triAddress) =>
                {
                    uint triangleListStartAddress = Config.Stream.GetUInt(TriangleConfig.TriangleListPointerAddress);
                    uint structSize = TriangleConfig.TriangleStructSize;
                    int addressDiff = triAddress >= triangleListStartAddress
                        ? (int)(triAddress - triangleListStartAddress)
                        : (int)(-1 * (triangleListStartAddress - triAddress));
                    int indexGuess = (int)(addressDiff / structSize);
                    if (triangleListStartAddress + indexGuess * structSize == triAddress) return indexGuess;
                    return Double.NaN;
                },
                (int index, bool allowToggle, uint triAddress) =>
                {
                    uint triangleListStartAddress = Config.Stream.GetUInt(TriangleConfig.TriangleListPointerAddress);
                    uint structSize = TriangleConfig.TriangleStructSize;
                    uint newTriAddress = (uint)(triangleListStartAddress + index * structSize);
                    Config.TriangleManager.SetCustomTriangleAddresses(newTriAddress);
                    return true;
                }));

            _dictionary.Add("CurrentTriangleObjectIndex",
                ((uint triAddress) =>
                {
                    uint objAddress = Config.Stream.GetUInt(triAddress + TriangleOffsetsConfig.AssociatedObject);
                    if (objAddress == 0) return double.NaN;
                    List<TriangleDataModel> objTris = TriangleUtilities.GetObjectTrianglesForObject(objAddress);
                    for (int i = 0; i < objTris.Count; i++)
                    {
                        if (objTris[i].Address == triAddress) return i;
                    }
                    return double.NaN;
                },
                (int index, bool allowToggle, uint triAddress) =>
                {
                    return false;
                }
            ));

            _dictionary.Add("CurrentTriangleAddress",
                ((uint triAddress) =>
                {
                    return triAddress;
                },
                (uint address, bool allowToggle, uint triAddress) =>
                {
                    Config.TriangleManager.SetCustomTriangleAddresses(address);
                    return true;
                }));

            _dictionary.Add("CurrentCellsTriangleAddress",
                ((uint dummy) =>
                {
                    return Config.CellsManager.TriangleAddress;
                },
                (uint address, bool allowToggle, uint dummy) =>
                {
                    Config.CellsManager.TriangleAddress = address;
                    return true;
                }));

            _dictionary.Add("ObjectNodeCount",
                ((uint dummy) =>
                {
                    int totalNodeCount = Config.Stream.GetInt(TriangleConfig.TotalNodeCountAddress);
                    int levelNodeCount = Config.Stream.GetInt(TriangleConfig.LevelNodeCountAddress);
                    int objectNodeCount = totalNodeCount - levelNodeCount;
                    return objectNodeCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMinX",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMinX();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMaxX",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMaxX();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMinY",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMinY();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMaxY",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMaxY();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMinZ",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMinZ();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMaxZ",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMaxZ();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriRangeX",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetRangeX();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriRangeY",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetRangeY();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriRangeZ",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetRangeZ();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMidpointX",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMidpointX();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMidpointY",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMidpointY();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriMidpointZ",
                ((uint triAddress) =>
                {
                    return TriangleDataModel.Create(triAddress).GetMidpointZ();
                },
                DEFAULT_SETTER));

            _dictionary.Add("DistanceToLine12",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 1, 2,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine12;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 1, 2,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine12;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("DistanceToLine23",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 2, 3,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine23;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 2, 3,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine23;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("DistanceToLine31",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 3, 1,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine31;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 3, 1,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine31;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("SelfDistanceToLine12",
                ((uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 1, 2,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine12;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 1, 2,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine12;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newSelfX = selfPos.X + xDiff;
                    double newSelfZ = selfPos.Z + zDiff;
                    return selfPos.SetValues(x: newSelfX, z: newSelfZ);
                }
            ));

            _dictionary.Add("SelfDistanceToLine23",
                ((uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 2, 3,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine23;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 2, 3,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine23;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newSelfX = selfPos.X + xDiff;
                    double newSelfZ = selfPos.Z + zDiff;
                    return selfPos.SetValues(x: newSelfX, z: newSelfZ);
                }
            ));

            _dictionary.Add("SelfDistanceToLine31",
                ((uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 3, 1,
                        TriangleDataModel.Create(triAddress).Classification);
                    return signedDistToLine31;
                },
                (double dist, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle selfPos = PositionAngle.Self;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                        selfPos.X, selfPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 3, 1,
                        TriangleDataModel.Create(triAddress).Classification);

                    double missingDist = dist - signedDistToLine31;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newSelfX = selfPos.X + xDiff;
                    double newSelfZ = selfPos.Z + zDiff;
                    return selfPos.SetValues(x: newSelfX, z: newSelfZ);
                }
            ));

            _dictionary.Add("DeltaAngleLine12",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV1ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);;
                    double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double newMarioAngleDouble = angleV1ToV2 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine21",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV2ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                    double newMarioAngleDouble = angleV2ToV1 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine23",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV2ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    double newMarioAngleDouble = angleV2ToV3 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine32",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV3ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                    double newMarioAngleDouble = angleV3ToV2 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine31",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV3ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    double newMarioAngleDouble = angleV3ToV1 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine13",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV1ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, bool allowToggle, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
                    double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                    double newMarioAngleDouble = angleV1ToV3 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("TriangleX1",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetX1(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetX1(value, triAddress);
                }));

            _dictionary.Add("TriangleY1",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetY1(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetY1(value, triAddress);
                }));

            _dictionary.Add("TriangleZ1",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetZ1(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetZ1(value, triAddress);
                }));

            _dictionary.Add("TriangleX2",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetX2(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetX2(value, triAddress);
                }));

            _dictionary.Add("TriangleY2",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetY2(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetY2(value, triAddress);
                }));

            _dictionary.Add("TriangleZ2",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetZ2(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetZ2(value, triAddress);
                }));

            _dictionary.Add("TriangleX3",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetX3(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetX3(value, triAddress);
                }));

            _dictionary.Add("TriangleY3",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetY3(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetY3(value, triAddress);
                }));

            _dictionary.Add("TriangleZ3",
                ((uint triAddress) =>
                {
                    return TriangleOffsetsConfig.GetZ3(triAddress);
                },
                (short value, bool allowToggle, uint triAddress) =>
                {
                    return TriangleOffsetsConfig.SetZ3(value, triAddress);
                }));

            // File vars

            _dictionary.Add("StarsInFile",
                ((uint fileAddress) =>
                {
                    return Config.FileManager.CalculateNumStars(fileAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("FileChecksumCalculated",
                ((uint fileAddress) =>
                {
                    return Config.FileManager.GetChecksum(fileAddress);
                },
                DEFAULT_SETTER));

            // Main Save vars

            _dictionary.Add("MainSaveChecksumCalculated",
                ((uint mainSaveAddress) =>
                {
                    return Config.MainSaveManager.GetChecksum(mainSaveAddress);
                },
                DEFAULT_SETTER));

            // Action vars

            _dictionary.Add("ActionDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetActionName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("PrevActionDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetPrevActionName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("ActionGroupDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetGroupName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("AnimationDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioAnimations.GetAnimationName();
                },
                DEFAULT_SETTER));

            // Water vars

            _dictionary.Add("WaterAboveMedian",
                ((uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    short waterLevelMedian = Config.Stream.GetShort(MiscConfig.WaterLevelMedianAddress);
                    double waterAboveMedian = waterLevel - waterLevelMedian;
                    return waterAboveMedian;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioAboveWater",
                ((uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float marioAboveWater = marioY - waterLevel;
                    return marioAboveWater;
                },
                (double goalMarioAboveWater, bool allowToggle, uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double goalMarioY = waterLevel + goalMarioAboveWater;
                    return Config.Stream.SetValue((float)goalMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("CurrentWater",
                ((uint dummy) =>
                {
                    return WaterUtilities.GetCurrentWater();
                },
                DEFAULT_SETTER));

            // Cam Hack Vars

            _dictionary.Add("CamHackYaw",
                ((uint dummy) =>
                {
                    float camX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    float camZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    float focusX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    float focusZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    return MoreMath.AngleTo_AngleUnits(camX, camZ, focusX, focusZ);
                },
                (double yaw, bool allowToggle, uint dummy) =>
                {
                    float camX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    float camZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    float focusX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    float focusZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    (double newFocusX, double newFocusZ) = MoreMath.RotatePointAboutPointToAngle(focusX, focusZ, camX, camZ, yaw);

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newFocusX, CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    success &= Config.Stream.SetValue((float)newFocusZ, CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    return success;
                }));

            _dictionary.Add("CamHackPitch",
                ((uint dummy) =>
                {
                    float camX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    float camY = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    float camZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    float focusX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    float focusY = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                    float focusZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    (double radius, double theta, double phi) = MoreMath.EulerToSpherical_AngleUnits(focusX - camX, focusY - camY, focusZ - camZ);
                    return phi;
                },
                (double pitch, bool allowToggle, uint dummy) =>
                {
                    float camX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    float camY = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    float camZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    float focusX = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    float focusY = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                    float focusZ = Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    (double radius, double theta, double phi) = MoreMath.EulerToSpherical_AngleUnits(focusX - camX, focusY - camY, focusZ - camZ);
                    (double diffX, double diffY, double diffZ) = MoreMath.SphericalToEuler_AngleUnits(radius, theta, pitch);
                    (double newFocusX, double newFocusY, double newFocusZ) = (camX + diffX, camY + diffY, camZ + diffZ);

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newFocusX, CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    success &= Config.Stream.SetValue((float)newFocusY, CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                    success &= Config.Stream.SetValue((float)newFocusZ, CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    return success;
                }
            ));

            // PU vars

            _dictionary.Add("MarioXQpuIndex",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    double qpuXIndex = puXIndex / 4d;
                    return qpuXIndex;
                },
                (double newQpuXIndex, bool allowToggle, uint dummy) =>
                {
                    int newPuXIndex = (int)Math.Round(newQpuXIndex * 4);
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYQpuIndex",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    double qpuYIndex = puYIndex / 4d;
                    return qpuYIndex;
                },
                (double newQpuYIndex, bool allowToggle, uint dummy) =>
                {
                    int newPuYIndex = (int)Math.Round(newQpuYIndex * 4);
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZQpuIndex",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    double qpuZIndex = puZIndex / 4d;
                    return qpuZIndex;
                },
                (double newQpuZIndex, bool allowToggle, uint dummy) =>
                {
                    int newPuZIndex = (int)Math.Round(newQpuZIndex * 4);
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("MarioXPuIndex",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    return puXIndex;
                },
                (int newPuXIndex, bool allowToggle, uint dummy) =>
                {
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYPuIndex",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    return puYIndex;
                },
                (int newPuYIndex, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZPuIndex",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    return puZIndex;
                },
                (int newPuZIndex, bool allowToggle, uint dummy) =>
                {
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("MarioXPuRelative",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double relX = PuUtilities.GetRelativeCoordinate(marioX);
                    return relX;
                },
                (double newRelX, bool allowToggle, uint dummy) =>
                {
                    float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    double newMarioX = PuUtilities.GetCoordinateInPu(newRelX, puXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYPuRelative",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double relY = PuUtilities.GetRelativeCoordinate(marioY);
                    return relY;
                },
                (double newRelY, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    double newMarioY = PuUtilities.GetCoordinateInPu(newRelY, puYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZPuRelative",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double relZ = PuUtilities.GetRelativeCoordinate(marioZ);
                    return relZ;
                },
                (double newRelZ, bool allowToggle, uint dummy) =>
                {
                    float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(newRelZ, puZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("DeFactoMultiplier",
                ((uint dummy) =>
                {
                    return GetDeFactoMultiplier();
                },
                (double newDeFactoMultiplier, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    if (distAboveFloor != 0) return false;

                    uint floorTri = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    if (floorTri == 0) return false;
                    return Config.Stream.SetValue((float)newDeFactoMultiplier, floorTri + TriangleOffsetsConfig.NormY);
                }));

            _dictionary.Add("SyncingSpeed",
                ((uint dummy) =>
                {
                    return GetSyncingSpeed();
                },
                (double newSyncingSpeed, bool allowToggle, uint dummy) =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    if (distAboveFloor != 0) return false;

                    uint floorTri = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    if (floorTri == 0) return false;
                    double newYnorm = PuUtilities.QpuSpeed / newSyncingSpeed * SpecialConfig.PuHypotenuse;
                    return Config.Stream.SetValue((float)newYnorm, floorTri + TriangleOffsetsConfig.NormY);
                }));

            _dictionary.Add("QpuSpeed",
                ((uint dummy) =>
                {
                    return GetQpuSpeed();
                },
                (double newQpuSpeed, bool allowToggle, uint dummy) =>
                {
                    double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("PuSpeed",
                ((uint dummy) =>
                {
                    double puSpeed = GetQpuSpeed() * 4;
                    return puSpeed;
                },
                (double newPuSpeed, bool allowToggle, uint dummy) =>
                {
                    double newQpuSpeed = newPuSpeed / 4;
                    double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("QpuSpeedComponent",
                ((uint dummy) =>
                {
                    return Math.Round(GetQpuSpeed());
                },
                (int newQpuSpeedComp, bool allowToggle, uint dummy) =>
                {
                    double relativeSpeed = GetRelativePuSpeed();
                    double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("PuSpeedComponent",
                ((uint dummy) =>
                {
                    return Math.Round(GetQpuSpeed() * 4);
                },
                (int newPuSpeedComp, bool allowToggle, uint dummy) =>
                {
                    double newQpuSpeedComp = newPuSpeedComp / 4d;
                    double relativeSpeed = GetRelativePuSpeed();
                    double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("RelativeSpeed",
                ((uint dummy) =>
                {
                    return GetRelativePuSpeed();
                },
                (double newRelativeSpeed, bool allowToggle, uint dummy) =>
                {
                    double puSpeed = GetQpuSpeed() * 4;
                    double puSpeedRounded = Math.Round(puSpeed);
                    double newHSpeed = (puSpeedRounded / 4) * GetSyncingSpeed() + newRelativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("Qs1RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, true, true);
                }));

            _dictionary.Add("Qs1RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, false, true);
                }));

            _dictionary.Add("Qs1RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, true, false);
                }));

            _dictionary.Add("Qs1RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, false, false);
                }));

            _dictionary.Add("Qs2RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, true, true);
                }));

            _dictionary.Add("Qs2RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, false, true);
                }));

            _dictionary.Add("Qs2RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, true, false);
                }));

            _dictionary.Add("Qs2RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, false, false);
                }));

            _dictionary.Add("Qs3RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, true, true);
                }));

            _dictionary.Add("Qs3RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, false, true);
                }));

            _dictionary.Add("Qs3RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, true, false);
                }));

            _dictionary.Add("Qs3RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, false, false);
                }));

            _dictionary.Add("Qs4RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, true, true);
                }));

            _dictionary.Add("Qs4RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, false, true);
                }));

            _dictionary.Add("Qs4RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, true);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, true, false);
                }));

            _dictionary.Add("Qs4RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, false);
                },
                (double newValue, bool allowToggle, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, false, false);
                }));

            _dictionary.Add("PuParams",
                ((uint dummy) =>
                {
                    return "(" + SpecialConfig.PuParam1 + "," + SpecialConfig.PuParam2 + ")";
                },
                (string puParamsString, bool allowToggle, uint dummy) =>
                {
                    List<string> stringList = ParsingUtilities.ParseStringList(puParamsString);
                    List<int?> intList = stringList.ConvertAll(
                        stringVal => ParsingUtilities.ParseIntNullable(stringVal));
                    if (intList.Count == 1) intList.Insert(0, 0);
                    if (intList.Count != 2 || intList.Exists(intValue => !intValue.HasValue)) return false;
                    SpecialConfig.PuParam1 = intList[0].Value;
                    SpecialConfig.PuParam2 = intList[1].Value;
                    return true;
                }));

            // Misc vars

            _dictionary.Add("GlobalTimerMod64",
                ((uint dummy) =>
                {
                    uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    return globalTimer % 64;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RngIndex",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUShort(MiscConfig.RngAddress);
                    return RngIndexer.GetRngIndex(rngValue);
                },
                (int rngIndex, bool allowToggle, uint dummy) =>
                {
                    ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                }));

            _dictionary.Add("RngIndexMod4",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUShort(MiscConfig.RngAddress);
                    int rngIndex = RngIndexer.GetRngIndex();
                    return rngIndex % 4;
                },
                DEFAULT_SETTER));

            _dictionary.Add("LastCoinRngIndex",
                ((uint coinAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUShort(coinAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    return coinRngIndex;
                },
                (int rngIndex, bool allowToggle, uint coinAddress) =>
                {
                    ushort coinRngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(coinRngValue, coinAddress + ObjectConfig.YawMovingOffset);
                }));

            _dictionary.Add("LastCoinRngIndexDiff",
                ((uint coinAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUShort(coinAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    int rngIndexDiff = coinRngIndex - SpecialConfig.GoalRngIndex;
                    return rngIndexDiff;
                },
                (int rngIndexDiff, bool allowToggle, uint coinAddress) =>
                {
                    int coinRngIndex = SpecialConfig.GoalRngIndex + rngIndexDiff;
                    ushort coinRngValue = RngIndexer.GetRngValue(coinRngIndex);
                    return Config.Stream.SetValue(coinRngValue, coinAddress + ObjectConfig.YawMovingOffset);
                }));
            
            _dictionary.Add("GoalRngValue",
                ((uint dummy) =>
                {
                    return SpecialConfig.GoalRngValue;
                },
                (ushort goalRngValue, bool allowToggle, uint coinAddress) =>
                {
                    SpecialConfig.GoalRngValue = goalRngValue;
                    return true;
                }));

            _dictionary.Add("GoalRngIndex",
                ((uint dummy) =>
                {
                    return SpecialConfig.GoalRngIndex;
                },
                (ushort goalRngIndex, bool allowToggle, uint coinAddress) =>
                {
                    SpecialConfig.GoalRngIndex = goalRngIndex;
                    return true;
                }));

            _dictionary.Add("GoalRngIndexDiff",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUShort(MiscConfig.RngAddress);
                    int rngIndex = RngIndexer.GetRngIndex(rngValue);
                    int rngIndexDiff = rngIndex - SpecialConfig.GoalRngIndex;
                    return rngIndexDiff;
                },
                (int rngIndexDiff, bool allowToggle, uint dummy) =>
                {
                    int rngIndex = SpecialConfig.GoalRngIndex + rngIndexDiff;
                    ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                }));

            _dictionary.Add("NumRngCalls",
                ((uint dummy) =>
                {
                    return ObjectRngUtilities.GetNumRngUsages();
                },
                DEFAULT_SETTER));

            _dictionary.Add("NumberOfLoadedObjects",
                ((uint dummy) =>
                {
                    return DataModels.ObjectProcessor.ActiveObjectCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("PlayTime",
                ((uint dummy) =>
                {
                    uint totalFrames = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                    return GetRealTime(totalFrames);
                },
                DEFAULT_SETTER));

            _dictionary.Add("DemoCounterDescription",
                ((uint dummy) =>
                {
                    return DemoCounterUtilities.GetDemoCounterDescription();
                },
                (string description, bool allowToggle, uint dummy) =>
                {
                    short? demoCounterNullable = DemoCounterUtilities.GetDemoCounter(description);
                    if (!demoCounterNullable.HasValue) return false;
                    return Config.Stream.SetValue(demoCounterNullable.Value, MiscConfig.DemoCounterAddress);
                }
            ));

            _dictionary.Add("TtcSpeedSettingDescription",
                ((uint dummy) =>
                {
                    return TtcSpeedSettingUtilities.GetTtcSpeedSettingDescription();
                },
                (string description, bool allowToggle, uint dummy) =>
                {
                    short? ttcSpeedSettingNullable = TtcSpeedSettingUtilities.GetTtcSpeedSetting(description);
                    if (!ttcSpeedSettingNullable.HasValue) return false;
                    return Config.Stream.SetValue(ttcSpeedSettingNullable.Value, MiscConfig.TtcSpeedSettingAddress);
                }));

            _dictionary.Add("TtcSaveState",
                ((uint dummy) =>
                {
                    return new TtcSaveState().ToString();
                },
                (string saveStateString, bool allowToggle, uint dummy) =>
                {
                    TtcSaveState saveState = new TtcSaveState(saveStateString);
                    TtcUtilities.ApplySaveState(saveState);
                    return true;
                }
            ));

            _dictionary.Add("GfxBufferSpace",
                ((uint dummy) =>
                {
                    uint gfxBufferStart = Config.Stream.GetUInt(MiscConfig.GfxBufferStartAddress);
                    uint gfxBufferEnd = Config.Stream.GetUInt(MiscConfig.GfxBufferEndAddress);
                    return gfxBufferEnd - gfxBufferStart;
                },
                DEFAULT_SETTER));

            _dictionary.Add("SegmentedToVirtualAddress",
                ((uint dummy) =>
                {
                    return SpecialConfig.SegmentedToVirtualAddress;
                },
                (uint value, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.SegmentedToVirtualAddress = value;
                    return true;
                }));

            _dictionary.Add("SegmentedToVirtualOutput",
                ((uint dummy) =>
                {
                    return SpecialConfig.SegmentedToVirtualOutput;
                },
                DEFAULT_SETTER));

            _dictionary.Add("VirtualToSegmentedSegment",
                ((uint dummy) =>
                {
                    return SpecialConfig.VirtualToSegmentedSegment;
                },
                (uint value, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.VirtualToSegmentedSegment = value;
                    return true;
                }));

            _dictionary.Add("VirtualToSegmentedAddress",
                ((uint dummy) =>
                {
                    return SpecialConfig.VirtualToSegmentedAddress;
                },
                (uint value, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.VirtualToSegmentedAddress = value;
                    return true;
                }));

            _dictionary.Add("VirtualToSegmentedOutput",
                ((uint dummy) =>
                {
                    return SpecialConfig.VirtualToSegmentedOutput;
                },
                DEFAULT_SETTER));

            // Options vars

            _dictionary.Add("GotoAboveOffset",
                ((uint dummy) =>
                {
                    return GotoRetrieveConfig.GotoAboveOffset;
                },
                (float value, bool allowToggle, uint dummy) =>
                {
                    GotoRetrieveConfig.GotoAboveOffset = value;
                    return true;
                }));

            _dictionary.Add("GotoInfrontOffset",
                ((uint dummy) =>
                {
                    return GotoRetrieveConfig.GotoInfrontOffset;
                },
                (float value, bool allowToggle, uint dummy) =>
                {
                    GotoRetrieveConfig.GotoInfrontOffset = value;
                    return true;
                }));

            _dictionary.Add("RetrieveAboveOffset",
                ((uint dummy) =>
                {
                    return GotoRetrieveConfig.RetrieveAboveOffset;
                },
                (float value, bool allowToggle, uint dummy) =>
                {
                    GotoRetrieveConfig.RetrieveAboveOffset = value;
                    return true;
                }));

            _dictionary.Add("RetrieveInfrontOffset",
                ((uint dummy) =>
                {
                    return GotoRetrieveConfig.RetrieveInfrontOffset;
                },
                (float value, bool allowToggle, uint dummy) =>
                {
                    GotoRetrieveConfig.RetrieveInfrontOffset = value;
                    return true;
                }));

            _dictionary.Add("FramesPerSecond",
                ((uint dummy) =>
                {
                    return RefreshRateConfig.RefreshRateFreq;
                },
                (uint value, bool allowToggle, uint dummy) =>
                {
                    RefreshRateConfig.RefreshRateFreq = value;
                    return true;
                }));

            _dictionary.Add("PositionControllerRelativity",
                ((uint dummy) =>
                {
                    return PositionControllerRelativityConfig.RelativityPA.ToString();
                },
                (PositionAngle value, bool allowToggle, uint dummy) =>
                {
                    PositionControllerRelativityConfig.RelativityPA = value;
                    return true;
                }));

            _dictionary.Add("ObjectSlotSize",
                ((uint dummy) =>
                {
                    return Config.ObjectSlotsManager.GetObjectSlotSize();
                },
                (int value, bool allowToggle, uint dummy) =>
                {
                    Config.StroopMainForm.ChangeObjectSlotSize(value);
                    return true;
                }));

            _dictionary.Add("CustomReleaseStatus",
                ((uint dummy) =>
                {
                    return SpecialConfig.CustomReleaseStatus;
                },
                (uint value, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.CustomReleaseStatus = value;
                    return true;
                }));

            // Area vars

            _dictionary.Add("CurrentAreaIndexMario",
                ((uint dummy) =>
                {
                    uint currentAreaMario = Config.Stream.GetUInt(
                        MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                    double currentAreaIndexMario = AreaUtilities.GetAreaIndex(currentAreaMario) ?? Double.NaN;
                    return currentAreaIndexMario;
                },
                (int currentAreaIndexMario, bool allowToggle, uint dummy) =>
                {
                    if (currentAreaIndexMario < 0 || currentAreaIndexMario >= 8) return false;
                    uint currentAreaAddressMario = AreaUtilities.GetAreaAddress(currentAreaIndexMario);
                    return Config.Stream.SetValue(
                        currentAreaAddressMario, MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                }));

            _dictionary.Add("CurrentAreaIndex",
                ((uint dummy) =>
                {
                    uint currentArea = Config.Stream.GetUInt(AreaConfig.CurrentAreaPointerAddress);
                    double currentAreaIndex = AreaUtilities.GetAreaIndex(currentArea) ?? Double.NaN;
                    return currentAreaIndex;
                },
                (int currentAreaIndex, bool allowToggle, uint dummy) =>
                {
                    if (currentAreaIndex < 0 || currentAreaIndex >= 8) return false;
                    uint currentAreaAddress = AreaUtilities.GetAreaAddress(currentAreaIndex);
                    return Config.Stream.SetValue(currentAreaAddress, AreaConfig.CurrentAreaPointerAddress);
                }));

            _dictionary.Add("AreaTerrainDescription",
                ((uint dummy) =>
                {
                    short terrainType = Config.Stream.GetShort(
                        Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                    string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                    return terrainDescription;
                },
                (short terrainType, bool allowToggle, uint dummy) =>
                {
                    return Config.Stream.SetValue(
                        terrainType, Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                }));

            // Warp vars

            _dictionary.Add("WarpNodesAddress",
                ((uint dummy) =>
                {
                    return GetWarpNodesAddress();
                },
                DEFAULT_SETTER));

            _dictionary.Add("NumWarpNodes",
                ((uint dummy) =>
                {
                    return GetNumWarpNodes();
                },
                DEFAULT_SETTER));

            // Custom point

            _dictionary.Add("SelfPosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPosPA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    if (posAngle.DependsOnSelf()) return false;
                    SpecialConfig.SelfPosPA = posAngle;
                    return true;
                }));

            _dictionary.Add("SelfX",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPA.X;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.SelfPA.SetX(doubleValue);
                }));

            _dictionary.Add("SelfY",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPA.Y;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.SelfPA.SetY(doubleValue);
                }));

            _dictionary.Add("SelfZ",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPA.Z;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.SelfPA.SetZ(doubleValue);
                }));

            _dictionary.Add("SelfAngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfAnglePA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    if (posAngle.DependsOnSelf()) return false;
                    SpecialConfig.SelfAnglePA = posAngle;
                    return true;
                }));

            _dictionary.Add("SelfAngle",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPA.Angle;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.SelfPA.SetAngle(doubleValue);
                }));

            _dictionary.Add("PointPosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.PointPosPA = posAngle;
                    return true;
                }));

            _dictionary.Add("PointX",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPA.X;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.PointPA.SetX(doubleValue);
                }));

            _dictionary.Add("PointY",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPA.Y;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.PointPA.SetY(doubleValue);
                }));

            _dictionary.Add("PointZ",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPA.Z;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.PointPA.SetZ(doubleValue);
                }));

            _dictionary.Add("PointAngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointAnglePA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.PointAnglePA = posAngle;
                    return true;
                }));

            _dictionary.Add("PointAngle",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPA.Angle;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.PointPA.SetAngle(doubleValue);
                }));

            _dictionary.Add("Self2PosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2PosPA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.Self2PosPA = posAngle;
                    return true;
                }
            ));

            _dictionary.Add("Self2X",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2PA.X;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Self2PA.SetX(doubleValue);
                }
            ));

            _dictionary.Add("Self2Y",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2PA.Y;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Self2PA.SetY(doubleValue);
                }
            ));

            _dictionary.Add("Self2Z",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2PA.Z;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Self2PA.SetZ(doubleValue);
                }
            ));

            _dictionary.Add("Self2AngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2AnglePA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.Self2AnglePA = posAngle;
                    return true;
                }
            ));

            _dictionary.Add("Self2Angle",
                ((uint dummy) =>
                {
                    return SpecialConfig.Self2PA.Angle;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Self2PA.SetAngle(doubleValue);
                }
            ));

            _dictionary.Add("Point2PosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2PosPA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.Point2PosPA = posAngle;
                    return true;
                }
            ));

            _dictionary.Add("Point2X",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2PA.X;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Point2PA.SetX(doubleValue);
                }
            ));

            _dictionary.Add("Point2Y",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2PA.Y;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Point2PA.SetY(doubleValue);
                }
            ));

            _dictionary.Add("Point2Z",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2PA.Z;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Point2PA.SetZ(doubleValue);
                }
            ));

            _dictionary.Add("Point2AngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2AnglePA.ToString();
                },
                (PositionAngle posAngle, bool allowToggle, uint dummy) =>
                {
                    SpecialConfig.Point2AnglePA = posAngle;
                    return true;
                }
            ));

            _dictionary.Add("Point2Angle",
                ((uint dummy) =>
                {
                    return SpecialConfig.Point2PA.Angle;
                },
                (double doubleValue, bool allowToggle, uint dummy) =>
                {
                    return SpecialConfig.Point2PA.SetAngle(doubleValue);
                }
            ));

            // Ghost vars

            _dictionary.Add("GhostActionDescription",
                ((uint dummy) =>
                {
                    uint action = Config.Stream.GetUInt(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.ActionOffset);
                    return TableConfig.MarioActions.GetActionName(action);
                },
                DEFAULT_SETTER));

            _dictionary.Add("GhostDeltaHSpeed",
                ((uint dummy) =>
                {
                    float marioHSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    float ghostHSpeed = Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.HSpeedOffset);
                    return marioHSpeed - ghostHSpeed;
                },
                (float deltaHSpeed, bool allowToggle, uint dummy) =>
                {
                    float ghostHSpeed = Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.HSpeedOffset);
                    float newMarioHSpeed = ghostHSpeed + deltaHSpeed;
                    return Config.Stream.SetValue(newMarioHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("GhostDeltaYSpeed",
                ((uint dummy) =>
                {
                    float marioYSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                    float ghostYSpeed = Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YSpeedOffset);
                    return marioYSpeed - ghostYSpeed;
                },
                (float deltaYSpeed, bool allowToggle, uint dummy) =>
                {
                    float ghostYSpeed = Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YSpeedOffset);
                    float newMarioYSpeed = ghostYSpeed + deltaYSpeed;
                    return Config.Stream.SetValue(newMarioYSpeed, MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                }));

            _dictionary.Add("GhostDeltaYawFacing",
                ((uint dummy) =>
                {
                    ushort marioYawFacing = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    ushort ghostYawFacing = Config.Stream.GetUShort(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YawFacingOffset);
                    return MoreMath.NormalizeAngleShort(marioYawFacing - ghostYawFacing);
                },
                (short deltaYawFacing, bool allowToggle, uint dummy) =>
                {
                    ushort ghostYawFacing = Config.Stream.GetUShort(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YawFacingOffset);
                    ushort newMarioYawFacing = MoreMath.NormalizeAngleUshort(ghostYawFacing + deltaYawFacing);
                    return Config.Stream.SetValue(newMarioYawFacing, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("GhostDeltaYawIntended",
                ((uint dummy) =>
                {
                    ushort marioYawIntended = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
                    ushort ghostYawIntended = Config.Stream.GetUShort(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YawIntendedOffset);
                    return MoreMath.NormalizeAngleShort(marioYawIntended - ghostYawIntended);
                },
                (short deltaYawIntended, bool allowToggle, uint dummy) =>
                {
                    ushort ghostYawIntended = Config.Stream.GetUShort(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YawIntendedOffset);
                    ushort newMarioYawIntended = MoreMath.NormalizeAngleUshort(ghostYawIntended + deltaYawIntended);
                    return Config.Stream.SetValue(newMarioYawIntended, MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
                }
            ));

            _dictionary.Add("HorizontalMovement",
                ((uint dummy) =>
                {
                    float pos01X = Config.Stream.GetFloat(0x80372F00);
                    float pos01Z = Config.Stream.GetFloat(0x80372F08);
                    float pos15X = Config.Stream.GetFloat(0x80372FE0);
                    float pos15Z = Config.Stream.GetFloat(0x80372FE8);
                    return MoreMath.GetDistanceBetween(pos01X, pos01Z, pos15X, pos15Z);
                },
                DEFAULT_SETTER));

            // Mupen vars

            _dictionary.Add("MupenLag",
                ((uint objAddress) =>
                {
                    if (!MupenUtilities.IsUsingMupen()) return Double.NaN;
                    int lag = MupenUtilities.GetLagCount() + SpecialConfig.MupenLagOffset;
                    return lag;
                },
                (string stringValue, bool allowToggle, uint dummy) =>
                {
                    if (!MupenUtilities.IsUsingMupen()) return false;

                    if (stringValue.ToLower() == "x")
                    {
                        SpecialConfig.MupenLagOffset = 0;
                        return true;
                    }

                    int? newLagNullable = ParsingUtilities.ParseIntNullable(stringValue);
                    if (!newLagNullable.HasValue) return false;
                    int newLag = newLagNullable.Value;
                    int newLagOffset = newLag - MupenUtilities.GetLagCount();
                    SpecialConfig.MupenLagOffset = newLagOffset;
                    return true;
                }));
        }

        // Triangle utilitiy methods

        public static int GetClosestTriangleVertexIndex(uint triAddress)
        {
            PositionAngle marioPos = PositionAngle.Mario;
            TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
            double distToV1 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X1, triStruct.Y1, triStruct.Z1);
            double distToV2 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X2, triStruct.Y2, triStruct.Z2);
            double distToV3 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X3, triStruct.Y3, triStruct.Z3);

            if (distToV1 <= distToV2 && distToV1 <= distToV3) return 1;
            else return distToV2 <= distToV3 ? 2 : 3;
        }

        private static PositionAngle GetClosestTriangleVertexPosition(uint triAddress)
        {
            int closestTriangleVertexIndex = GetClosestTriangleVertexIndex(triAddress);
            TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
            if (closestTriangleVertexIndex == 1) return PositionAngle.Tri(triAddress, 1);
            if (closestTriangleVertexIndex == 2) return PositionAngle.Tri(triAddress, 2);
            if (closestTriangleVertexIndex == 3) return PositionAngle.Tri(triAddress, 3);
            throw new ArgumentOutOfRangeException();
        }

        private static double GetTriangleUphillAngleRadians(uint triAddress)
        {
            double angle = GetTriangleUphillAngle(triAddress);
            return MoreMath.AngleUnitsToRadians(angle);
        }

        public static double GetTriangleUphillAngle(uint triAddress)
        {
            TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
            return GetTriangleUphillAngle(triStruct);
        }

        public static double GetTriangleUphillAngle(TriangleDataModel triStruct)
        {
            double uphillAngle = 32768 + InGameTrigUtilities.InGameAngleTo(triStruct.NormX, triStruct.NormZ);
            if (triStruct.NormX == 0 && triStruct.NormZ == 0) uphillAngle = double.NaN;
            if (triStruct.IsCeiling()) uphillAngle += 32768;
            return MoreMath.NormalizeAngleDouble(uphillAngle);
        }

        private static double GetMaxHorizontalSpeedOnTriangle(uint triAddress, bool uphill, bool atAngle)
        {
            TriangleDataModel triStruct = TriangleDataModel.Create(triAddress);
            double vDist = uphill ? 78 : 100;
            if (atAngle)
            {
                ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                double marioAngleRadians = MoreMath.AngleUnitsToRadians(marioAngle);
                double uphillAngleRadians = GetTriangleUphillAngleRadians(triAddress);
                double deltaAngle = marioAngleRadians - uphillAngleRadians;
                double multiplier = Math.Abs(Math.Cos(deltaAngle));
                vDist /= multiplier;
            }
            double steepnessRadians = Math.Acos(triStruct.NormY);
            double hDist = vDist / Math.Tan(steepnessRadians);
            double hSpeed = hDist * 4 / triStruct.NormY;
            return hSpeed;
        }

        // Mario special methods

        public static double GetMarioSlidingSpeed()
        {
            float xSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
            float zSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
            double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);
            return hSlidingSpeed;
        }

        public static double GetMarioSlidingAngle()
        {
            float xSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
            float zSlidingSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
            double slidingAngle = MoreMath.AngleTo_AngleUnits(xSlidingSpeed, zSlidingSpeed);
            return slidingAngle;
        }

        // Radius distance utility methods

        private static double GetRadiusDiff(PositionAngle self, PositionAngle point, double radius)
        {
            double dist = MoreMath.GetDistanceBetween(
                self.X, self.Y, self.Z, point.X, point.Y, point.Z);
            return dist - radius;
        }

        private static bool SetRadiusDiff(PositionAngle self, PositionAngle point, double radius, double value)
        {
            double totalDist = radius + value;
            (double newSelfX, double newSelfY, double newSelfZ) =
                MoreMath.ExtrapolateLine3D(
                    point.X, point.Y, point.Z, self.X, self.Y, self.Z, totalDist);
            return self.SetValues(x: newSelfX, y: newSelfY, z: newSelfZ);
        }

        // Object specific utilitiy methods

        private static (double dotProduct, double distToWaypointPlane, double distToWaypoint)
            GetWaypointSpecialVars(uint objAddress)
        {
            float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
            float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
            float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);

            uint prevWaypointAddress = Config.Stream.GetUInt(objAddress + ObjectConfig.WaypointOffset);
            short prevWaypointIndex = Config.Stream.GetShort(prevWaypointAddress + WaypointConfig.IndexOffset);
            short prevWaypointX = Config.Stream.GetShort(prevWaypointAddress + WaypointConfig.XOffset);
            short prevWaypointY = Config.Stream.GetShort(prevWaypointAddress + WaypointConfig.YOffset);
            short prevWaypointZ = Config.Stream.GetShort(prevWaypointAddress + WaypointConfig.ZOffset);
            uint nextWaypointAddress = prevWaypointAddress + WaypointConfig.StructSize;
            short nextWaypointIndex = Config.Stream.GetShort(nextWaypointAddress + WaypointConfig.IndexOffset);
            short nextWaypointX = Config.Stream.GetShort(nextWaypointAddress + WaypointConfig.XOffset);
            short nextWaypointY = Config.Stream.GetShort(nextWaypointAddress + WaypointConfig.YOffset);
            short nextWaypointZ = Config.Stream.GetShort(nextWaypointAddress + WaypointConfig.ZOffset);

            float objToWaypointX = nextWaypointX - objX;
            float objToWaypointY = nextWaypointY - objY;
            float objToWaypointZ = nextWaypointZ - objZ;
            float prevToNextX = nextWaypointX - prevWaypointX;
            float prevToNextY = nextWaypointY - prevWaypointY;
            float prevToNextZ = nextWaypointZ - prevWaypointZ;

            double dotProduct = MoreMath.GetDotProduct(objToWaypointX, objToWaypointY, objToWaypointZ, prevToNextX, prevToNextY, prevToNextZ);
            double prevToNextDist = MoreMath.GetDistanceBetween(prevWaypointX, prevWaypointY, prevWaypointZ, nextWaypointX, nextWaypointY, nextWaypointZ);
            double distToWaypointPlane = dotProduct / prevToNextDist;
            double distToWaypoint = MoreMath.GetDistanceBetween(objX, objY, objZ, nextWaypointX, nextWaypointY, nextWaypointZ);

            return (dotProduct, distToWaypointPlane, distToWaypoint);
        }

        private static (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget)
            GetRacingPenguinSpecialVars(uint racingPenguinAddress)
        {
            double marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            double objectY = Config.Stream.GetFloat(racingPenguinAddress + ObjectConfig.YOffset);
            double heightDiff = marioY - objectY;

            uint prevWaypointAddress = Config.Stream.GetUInt(racingPenguinAddress + ObjectConfig.WaypointOffset);
            short prevWaypointIndex = Config.Stream.GetShort(prevWaypointAddress);
            double effort = Config.Stream.GetFloat(racingPenguinAddress + ObjectConfig.RacingPenguinEffortOffset);

            double effortTarget;
            double effortChange;
            double minHSpeed = 70;
            if (heightDiff > -100 || prevWaypointIndex >= 35)
            {
                if (prevWaypointIndex >= 35) minHSpeed = 60;
                effortTarget = -500;
                effortChange = 100;
            }
            else
            {
                effortTarget = 1000;
                effortChange = 30;
            }
            effort = MoreMath.MoveNumberTowards(effort, effortTarget, effortChange);

            double hSpeedTarget = (effort - heightDiff) * 0.1;
            hSpeedTarget = MoreMath.Clamp(hSpeedTarget, minHSpeed, 150);

            return (effortTarget, effortChange, minHSpeed, hSpeedTarget);
        }

        private static (double hSpeedTarget, double hSpeedChange)
            GetKoopaTheQuickSpecialVars(uint koopaTheQuickAddress)
        {
            double hSpeedMultiplier = Config.Stream.GetFloat(koopaTheQuickAddress + ObjectConfig.KoopaTheQuickHSpeedMultiplierOffset);
            short pitchToWaypointAngleUnits = Config.Stream.GetShort(koopaTheQuickAddress + ObjectConfig.PitchToWaypointOffset);
            double pitchToWaypointRadians = MoreMath.AngleUnitsToRadians(pitchToWaypointAngleUnits);

            double hSpeedTarget = hSpeedMultiplier * (Math.Sin(pitchToWaypointRadians) + 1) * 6;
            double hSpeedChange = hSpeedMultiplier * 0.1;

            return (hSpeedTarget, hSpeedChange);
        }

        public static int GetPendulumCountdown(uint pendulumAddress)
        {
            // Get pendulum variables
            float accelerationDirection = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
            float accelerationMagnitude = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAccelerationMagnitudeOffset);
            float angularVelocity = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAngularVelocityOffset);
            float angle = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAngleOffset);
            int waitingTimer = Config.Stream.GetInt(pendulumAddress + ObjectConfig.PendulumWaitingTimerOffset);
            return GetPendulumCountdown(accelerationDirection, accelerationMagnitude, angularVelocity, angle, waitingTimer);
        }

        public static int GetPendulumCountdown(
             float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle, int waitingTimer)
        {
            return GetPendulumVars(accelerationDirection, accelerationMagnitude, angularVelocity, angle).ToTuple().Item2 + waitingTimer;
        }

        public static float GetPendulumAmplitude(uint pendulumAddress)
        {
            // Get pendulum variables
            float accelerationDirection = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
            float accelerationMagnitude = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAccelerationMagnitudeOffset);
            float angularVelocity = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAngularVelocityOffset);
            float angle = Config.Stream.GetFloat(pendulumAddress + ObjectConfig.PendulumAngleOffset);
            return GetPendulumAmplitude(accelerationDirection, accelerationMagnitude, angularVelocity, angle);
        }

        public static float GetPendulumAmplitude(
            float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle)
        {
            return GetPendulumVars(accelerationDirection, accelerationMagnitude, angularVelocity, angle).ToTuple().Item1;
        }

        public static float GetPendulumAmplitude(float angle, float accelerationMagnitude)
        {
            float accelerationDirection = -1 * Math.Sign(angle);
            float angularVelocity = 0;
            return GetPendulumAmplitude(accelerationDirection, accelerationMagnitude, angularVelocity, angle);
        }

        public static (float amplitude, int countdown) GetPendulumVars(
            float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle)
        {
            // Get pendulum variables
            float acceleration = accelerationDirection * accelerationMagnitude;

            // Calculate one frame forwards to see if pendulum is speeding up or slowing down
            float nextAccelerationDirection = accelerationDirection;
            if (angle > 0) nextAccelerationDirection = -1;
            if (angle < 0) nextAccelerationDirection = 1;
            float nextAcceleration = nextAccelerationDirection * accelerationMagnitude;
            float nextAngularVelocity = angularVelocity + nextAcceleration;
            float nextAngle = angle + nextAngularVelocity;
            bool speedingUp = Math.Abs(nextAngularVelocity) > Math.Abs(angularVelocity);

            // Calculate duration of speeding up phase
            float inflectionAngle = angle;
            float inflectionAngularVelocity = nextAngularVelocity;
            float speedUpDistance = 0;
            int speedUpDuration = 0;

            if (speedingUp)
            {
                // d = t * v + t(t-1)/2 * a
                // d = tv + (t^2)a/2-ta/2
                // d = t(v-a/2) + (t^2)a/2
                // 0 = (t^2)a/2 + t(v-a/2) + -d
                // t = (-B +- sqrt(B^2 - 4AC)) / (2A)
                float tentativeSlowDownStartAngle = nextAccelerationDirection;
                float tentativeSpeedUpDistance = tentativeSlowDownStartAngle - angle;
                float A = nextAcceleration / 2;
                float B = nextAngularVelocity - nextAcceleration / 2;
                float C = -1 * tentativeSpeedUpDistance;
                double tentativeSpeedUpDuration = (-B + nextAccelerationDirection * Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
                speedUpDuration = (int)Math.Ceiling(tentativeSpeedUpDuration);

                // d = t * v + t(t-1)/2 * a
                speedUpDistance = speedUpDuration * nextAngularVelocity + speedUpDuration * (speedUpDuration - 1) / 2 * nextAcceleration;
                inflectionAngle = angle + speedUpDistance;

                // v_f = v_i + t * a
                inflectionAngularVelocity = nextAngularVelocity + (speedUpDuration - 2) * nextAcceleration;
            }

            // Calculate duration of slowing down phase

            // v_f = v_i + t * a
            // 0 = v_i + t * a
            // t = v_i / a
            int slowDownDuration = (int)Math.Abs(inflectionAngularVelocity / accelerationMagnitude);

            // d = t * (v_i + v_f)/2
            // d = t * (v_i + 0)/2
            // d = t * v_i/2
            float slowDownDistance = (slowDownDuration + 1) * inflectionAngularVelocity / 2;

            // Combine the results from the speeding up phase and the slowing down phase
            int totalDuration = speedUpDuration + slowDownDuration;
            float totalDistance = speedUpDistance + slowDownDistance;
            float amplitude = angle + totalDistance;
            return (amplitude, totalDuration);
        }

        public static int GetCogNumFramesInRotation(uint cogAddress)
        {
            ushort yawFacing = Config.Stream.GetUShort(cogAddress + ObjectConfig.YawFacingOffset);
            int currentYawVel = (int)Config.Stream.GetFloat(cogAddress + ObjectConfig.CogCurrentYawVelocity);
            int targetYawVel = (int)Config.Stream.GetFloat(cogAddress + ObjectConfig.CogTargetYawVelocity);
            return GetCogNumFramesInRotation(yawFacing, currentYawVel, targetYawVel);
        }

        public static int GetCogNumFramesInRotation(ushort yawFacing, int currentYawVel, int targetYawVel)
        {
            int diff = Math.Abs(targetYawVel - currentYawVel);
            int numFrames = diff / 50;
            if (numFrames == 0) numFrames = 1;
            return numFrames;
        }

        public static ushort GetCogEndingYaw(uint cogAddress)
        {
            ushort yawFacing = Config.Stream.GetUShort(cogAddress + ObjectConfig.YawFacingOffset);
            int currentYawVel = (int)Config.Stream.GetFloat(cogAddress + ObjectConfig.CogCurrentYawVelocity);
            int targetYawVel = (int)Config.Stream.GetFloat(cogAddress + ObjectConfig.CogTargetYawVelocity);
            return GetCogEndingYaw(yawFacing, currentYawVel, targetYawVel);
        }

        public static ushort GetCogEndingYaw(ushort yawFacing, int currentYawVel, int targetYawVel)
        {
            int numFrames = GetCogNumFramesInRotation(yawFacing, currentYawVel, targetYawVel);
            int remainingRotation = (currentYawVel + targetYawVel) * (numFrames + 1) / 2 - currentYawVel;
            int endingYaw = yawFacing + remainingRotation;
            return MoreMath.NormalizeAngleUshort(endingYaw);
        }

        private static double GetObjectTrajectoryFramesToYDist(double frames)
        {
            bool reflected = false;
            if (frames < 7.5)
            {
                frames = MoreMath.ReflectValueAboutValue(frames, 7.5);
                reflected = true;
            }
            double yDist;
            if (frames <= 38)
            {
                yDist = -1.25 * frames * frames + 18.75 * frames;
            }
            else
            {
                yDist = -75 * (frames - 38) - 1092.5;
            }
            if (reflected) yDist = MoreMath.ReflectValueAboutValue(yDist, 70.3125);
            return yDist;
        }

        private static double GetObjectTrajectoryYDistToFrames(double yDist)
        {
            bool reflected = false;
            if (yDist > 70.3125)
            {
                yDist = MoreMath.ReflectValueAboutValue(yDist, 70.3125);
                reflected = true;
            }
            double frames;
            if (yDist >= -1092.5)
            {
                double radicand = 351.5625 - 5 * yDist;
                frames = 7.5 + 0.4 * Math.Sqrt(radicand);
            }
            else
            {
                frames = (yDist + 1092.5) / -75 + 38;
            }
            if (reflected) frames = MoreMath.ReflectValueAboutValue(frames, 7.5);
            return frames;
        }

        private static double GetBobombTrajectoryFramesToHDist(double frames)
        {
            return 32 + frames * 25;
        }

        private static double GetBobombTrajectoryHDistToFrames(double hDist)
        {
            return (hDist - 32) / 25;
        }

        private static double GetCorkBoxTrajectoryFramesToHDist(double frames)
        {
            return 32 + frames * 40;
        }

        private static double GetCorkBoxTrajectoryHDistToFrames(double hDist)
        {
            return (hDist - 32) / 40;
        }

        // PU methods

        private static float GetDeFactoMultiplier()
        {
            uint floorTri = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            float yNorm = floorTri == 0 ? 1 : Config.Stream.GetFloat(floorTri + TriangleOffsetsConfig.NormY);

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            float distAboveFloor = marioY - floorY;

            float defactoMultiplier = distAboveFloor == 0 ? yNorm : 1;
            return defactoMultiplier;
        }

        public static float GetMarioDeFactoSpeed()
        {
            float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            float defactoSpeed = hSpeed * GetDeFactoMultiplier();
            return defactoSpeed;
        }

        public static double GetSyncingSpeed()
        {
            return PuUtilities.QpuSpeed / GetDeFactoMultiplier() * SpecialConfig.PuHypotenuse;
        }

        public static double GetQpuSpeed()
        {
            float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            return hSpeed / GetSyncingSpeed();
        }

        public static double GetRelativePuSpeed()
        {
            double puSpeed = GetQpuSpeed() * 4;
            double puSpeedRounded = Math.Round(puSpeed);
            double relativeSpeed = (puSpeed - puSpeedRounded) / 4 * GetSyncingSpeed() * GetDeFactoMultiplier();
            return relativeSpeed;
        }

        public static (double x, double z) GetIntendedNextPosition(double numFrames)
        {
            double deFactoSpeed = GetMarioDeFactoSpeed();
            ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(marioAngle);
            (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(deFactoSpeed * numFrames, marioAngleTruncated);

            float currentX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float currentZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            return (currentX + xDiff, currentZ + zDiff);
        }

        private static double GetQsRelativeSpeed(double numFrames, bool xComp)
        {
            uint compOffset = xComp ? MarioConfig.XOffset : MarioConfig.ZOffset;
            float currentComp = Config.Stream.GetFloat(MarioConfig.StructAddress + compOffset);
            double relCurrentComp = PuUtilities.GetRelativeCoordinate(currentComp);
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            double relIntendedComp = PuUtilities.GetRelativeCoordinate(intendedComp);
            double compDiff = relIntendedComp - relCurrentComp;
            return compDiff;
        }
        
        private static double GetQsRelativeIntendedNextComponent(double numFrames, bool xComp)
        {
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            double relIntendedComp = PuUtilities.GetRelativeCoordinate(intendedComp);
            return relIntendedComp;
        }
        
        private static bool GetQsRelativeIntendedNextComponent(double newValue, double numFrames, bool xComp, bool relativePosition)
        {
            float currentX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float currentZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float currentComp = xComp ? currentX : currentZ;
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            int intendedPuCompIndex = PuUtilities.GetPuIndex(intendedComp);
            double newRelativeComp = relativePosition ? currentComp + newValue : newValue;
            double newIntendedComp = PuUtilities.GetCoordinateInPu(newRelativeComp, intendedPuCompIndex);

            float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            double intendedXComp = xComp ? newIntendedComp : intendedX;
            double intendedZComp = xComp ? intendedZ : newIntendedComp;
            (double newDeFactoSpeed, double newAngle) =
                MoreMath.GetVectorFromCoordinates(
                    currentX, currentZ, intendedXComp, intendedZComp, hSpeed >= 0);
            double newHSpeed = newDeFactoSpeed / GetDeFactoMultiplier() / numFrames;
            ushort newAngleRounded = MoreMath.NormalizeAngleUshort(newAngle);

            bool success = true;
            success &= Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            success &= Config.Stream.SetValue(newAngleRounded, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            return success;
        }

        // Angle methods

        public static short GetDeltaYawIntendedFacing()
        {
            ushort marioYawFacing = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioYawIntended = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
            ushort diff = MoreMath.NormalizeAngleTruncated(marioYawIntended - marioYawFacing);
            return MoreMath.NormalizeAngleShort(diff);
        }

        public static short GetDeltaYawIntendedBackwards()
        {
            short forwards = GetDeltaYawIntendedFacing();
            return MoreMath.NormalizeAngleShort(forwards + 32768);
        }

        // Mario trajectory methods

        public static double ConvertDoubleJumpHSpeedToVSpeed(double hSpeed)
        {
            return (hSpeed / 4) + 52;
        }

        public static double ConvertDoubleJumpVSpeedToHSpeed(double vSpeed)
        {
            return (vSpeed - 52) * 4;
        }

        public static double ComputeHeightChangeFromInitialVerticalSpeed(double initialVSpeed)
        {
            int numFrames = (int) Math.Ceiling(initialVSpeed / 4);
            double finalVSpeed = initialVSpeed - (numFrames - 1) * 4;
            double heightChange = numFrames * (initialVSpeed + finalVSpeed) / 2;
            return heightChange;
        }

        public static double ComputeInitialVerticalSpeedFromHeightChange(double heightChange)
        {
            int numFrames = (int) Math.Ceiling((-2 + Math.Sqrt(4 + 8 * heightChange)) / 4);
            double triangleConstant = 2 * numFrames * (numFrames - 1);
            double initialSpeed = (heightChange + triangleConstant) / numFrames;
            return initialSpeed;
        }

        // Rotation methods

        private static (float x, float y, float z) GetRotationDisplacement()
        {
            uint stoodOnObject = Config.Stream.GetUInt(MarioConfig.StoodOnObjectPointerAddress);
            if (stoodOnObject == 0)
            {
                return (0, 0, 0);
            }

            float[] currentObjectPos = new float[]
            {
                Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset),
                Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset),
                Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset),
            };

            float[] platformPos = new float[]
            {
                Config.Stream.GetFloat(stoodOnObject + ObjectConfig.XOffset),
                Config.Stream.GetFloat(stoodOnObject + ObjectConfig.YOffset),
                Config.Stream.GetFloat(stoodOnObject + ObjectConfig.ZOffset),
            };

            float[] currentObjectOffset = new float[]
            {
                currentObjectPos[0] - platformPos[0],
                currentObjectPos[1] - platformPos[1],
                currentObjectPos[2] - platformPos[2],
            };

            short[] platformAngularVelocity = new short[]
            {
                (short)Config.Stream.GetInt(stoodOnObject + ObjectConfig.PitchVelocityOffset),
                (short)Config.Stream.GetInt(stoodOnObject + ObjectConfig.YawVelocityOffset),
                (short)Config.Stream.GetInt(stoodOnObject + ObjectConfig.RollVelocityOffset),
            };

            short[] platformFacingAngle = new short[]
            {
                Config.Stream.GetShort(stoodOnObject + ObjectConfig.PitchFacingOffset),
                Config.Stream.GetShort(stoodOnObject + ObjectConfig.YawFacingOffset),
                Config.Stream.GetShort(stoodOnObject + ObjectConfig.RollFacingOffset),
            };

            short[] rotation = new short[]
            {
                (short)(platformFacingAngle[0] - platformAngularVelocity[0]),
                (short)(platformFacingAngle[1] - platformAngularVelocity[1]),
                (short)(platformFacingAngle[2] - platformAngularVelocity[2]),
            };

            float[,] displaceMatrix = new float[4,4];
            float[] relativeOffset = new float[3];
            float[] newObjectOffset = new float[3];

            mtxf_rotate_zxy_and_translate(displaceMatrix, currentObjectOffset, rotation);
            linear_mtxf_transpose_mul_vec3f(displaceMatrix, relativeOffset, currentObjectOffset);

            rotation[0] = platformFacingAngle[0];
            rotation[1] = platformFacingAngle[1];
            rotation[2] = platformFacingAngle[2];

            mtxf_rotate_zxy_and_translate(displaceMatrix, currentObjectOffset, rotation);
            linear_mtxf_transpose_mul_vec3f(displaceMatrix, newObjectOffset, relativeOffset);

            float[] netDisplacement = new float[]
            {
                newObjectOffset[0] - currentObjectOffset[0],
                newObjectOffset[1] - currentObjectOffset[1],
                newObjectOffset[2] - currentObjectOffset[2],
            };

            return (netDisplacement[0], netDisplacement[1], netDisplacement[2]);
        }

        private static void mtxf_rotate_zxy_and_translate(float[,] dest, float[] translate, short[] rotate)
        {
            float sx = InGameTrigUtilities.InGameSine(rotate[0]);
            float cx = InGameTrigUtilities.InGameCosine(rotate[0]);

            float sy = InGameTrigUtilities.InGameSine(rotate[1]);
            float cy = InGameTrigUtilities.InGameCosine(rotate[1]);

            float sz = InGameTrigUtilities.InGameSine(rotate[2]);
            float cz = InGameTrigUtilities.InGameCosine(rotate[2]);

            dest[0,0] = cy * cz + sx * sy * sz;
            dest[1,0] = -cy * sz + sx * sy * cz;
            dest[2,0] = cx * sy;
            dest[3,0] = translate[0];

            dest[0,1] = cx * sz;
            dest[1,1] = cx * cz;
            dest[2,1] = -sx;
            dest[3,1] = translate[1];

            dest[0,2] = -sy * cz + sx * cy * sz;
            dest[1,2] = sy * sz + sx * cy * cz;
            dest[2,2] = cx * cy;
            dest[3,2] = translate[2];

            dest[0,3] = dest[1,3] = dest[2,3] = 0.0f;
            dest[3,3] = 1.0f;
        }

        private static void linear_mtxf_transpose_mul_vec3f(float[,] m, float[] dst, float[] v)
        {
            for (int i = 0; i < 3; i++)
            {
                dst[i] = m[i,0] * v[0] + m[i,1] * v[1] + m[i,2] * v[2];
            }
        }

        // Triangle methods

        public static uint GetWarpNodesAddress()
        {
            uint gAreas = Config.Stream.GetUInt(0x8032DDC8);
            short currentAreaIndex = Config.Stream.GetShort(0x8033BACA);
            uint warpNodesAddress = Config.Stream.GetUInt(gAreas + (uint)currentAreaIndex * AreaConfig.AreaStructSize + 0x14);
            return warpNodesAddress;
        }

        public static int GetNumWarpNodes()
        {
            uint address = GetWarpNodesAddress();
            int numWarpNodes = 0;
            while (address != 0)
            {
                numWarpNodes++;
                address = Config.Stream.GetUInt(address + 0x8);
            }
            return numWarpNodes;
        }

        public static List<uint> GetWarpNodeAddresses()
        {
            List<uint> addresses = new List<uint>();
            uint address = GetWarpNodesAddress();
            while (address != 0)
            {
                addresses.Add(address);
                address = Config.Stream.GetUInt(address + 0x8);
            }
            return addresses;
        }

        // In Game Angle Methods

        public static int GetDeltaInGameAngle(ushort angle)
        {
            (double x, double z) = MoreMath.GetComponentsFromVector(1, angle);
            int inGameAngle = InGameTrigUtilities.InGameAngleTo(x, z);
            return angle - inGameAngle;
        }

        // Play Time

        public static string GetRealTime(uint totalFrames)
        {
            uint frameConst = 30;
            uint secondConst = 60;
            uint minuteConst = 60;
            uint hourConst = 24;
            uint dayConst = 365;

            uint totalSeconds = totalFrames / frameConst;
            uint totalMinutes = totalSeconds / secondConst;
            uint totalHours = totalMinutes / minuteConst;
            uint totalDays = totalHours / hourConst;
            uint totalYears = totalDays / dayConst;

            uint frames = totalFrames % frameConst;
            uint seconds = totalSeconds % secondConst;
            uint minutes = totalMinutes % minuteConst;
            uint hours = totalHours % hourConst;
            uint days = totalDays % dayConst;
            uint years = totalYears;

            List<uint> values = new List<uint> { years, days, hours, minutes, seconds, frames };
            int firstNonZeroIndex = values.FindIndex(value => value != 0);
            if (firstNonZeroIndex == -1) firstNonZeroIndex = values.Count - 1;
            int numValuesToShow = values.Count - firstNonZeroIndex;

            StringBuilder builder = new StringBuilder();
            if (numValuesToShow >= 6) builder.Append(years + "y ");
            if (numValuesToShow >= 5) builder.Append(days + "d ");
            if (numValuesToShow >= 4) builder.Append(hours + "h ");
            if (numValuesToShow >= 3) builder.Append(minutes + "m ");
            if (numValuesToShow >= 2) builder.Append(seconds + "s ");
            if (numValuesToShow >= 1) builder.Append(String.Format("{0:D2}", frames) + "f");
            return builder.ToString();
        }

        // Hitbox vars

        public static int IsMarioHitboxOverlapping(uint objAddress)
        {
            uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
            float mObjX = Config.Stream.GetFloat(marioObjRef + ObjectConfig.XOffset);
            float mObjY = Config.Stream.GetFloat(marioObjRef + ObjectConfig.YOffset);
            float mObjZ = Config.Stream.GetFloat(marioObjRef + ObjectConfig.ZOffset);
            float mObjHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);
            float mObjHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);
            float mObjHitboxDownOffset = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxDownOffsetOffset);
            float mObjHitboxBottom = mObjY - mObjHitboxDownOffset;
            float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

            float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
            float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
            float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
            float objHitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
            float objHitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
            float objHitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float objHitboxBottom = objY - objHitboxDownOffset;
            float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

            double marioHitboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
            double marioHitboxAboveObject = mObjHitboxBottom - objHitboxTop;
            double marioHitboxBelowObject = objHitboxBottom - mObjHitboxTop;

            bool overlap = marioHitboxAwayFromObject < 0 && marioHitboxAboveObject <= 0 && marioHitboxBelowObject <= 0;
            return overlap ? 1 : 0;
        }
    }
}