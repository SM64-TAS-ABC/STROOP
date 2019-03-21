using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class MathOperationUtilities
    {
        public static string GetSymbol(MathOperation operation, bool useX = true, bool useSlash = true)
        {
            switch (operation)
            {
                case MathOperation.Add:
                    return "+";
                case MathOperation.Subtract:
                    return "-";
                case MathOperation.Multiply:
                    return useX ? "×" : "*";
                case MathOperation.Divide:
                    return useSlash ? "/" : "÷";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetNoun(MathOperation operation)
        {
            switch (operation)
            {
                case MathOperation.Add:
                    return "Addition";
                case MathOperation.Subtract:
                    return "Subtraction";
                case MathOperation.Multiply:
                    return "Multiplication";
                case MathOperation.Divide:
                    return "Division";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetResultName(MathOperation operation)
        {
            switch (operation)
            {
                case MathOperation.Add:
                    return "Sum";
                case MathOperation.Subtract:
                    return "Difference";
                case MathOperation.Multiply:
                    return "Product";
                case MathOperation.Divide:
                    return "Quotient";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
