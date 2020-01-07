using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Graphics;

namespace STROOP.Script
{
    public class NumberNode : Node
    {
        private Token _token;
        private double _value;

        public NumberNode(Token token)
        {
            _token = token;
            double? parsed = ParsingUtilities.ParseDoubleNullable(token.Value);
            if (!parsed.HasValue) throw new Exception("could not parse double: " + token.Value);
            _value = parsed.Value;
        }

        public override object Evaluate()
        {
            return _value;
        }
    }
}
