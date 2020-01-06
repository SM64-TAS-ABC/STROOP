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
    public class Token
    {
        public readonly TokenType Type;
        public readonly object Value;

        public Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return "[" + Type + "," + Value + "]";
        }
    }
}
