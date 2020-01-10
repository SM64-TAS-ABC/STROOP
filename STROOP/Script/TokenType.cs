using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Script
{
    public enum TokenType
    {
        EOF,

        ID,

        IF,
        VAR,
        FUNCTION,

        NUMBER,

        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,

        LEFT_PAREN,
        RIGHT_PAREN,

        ASSIGN,
        SEMI,
    };
}
