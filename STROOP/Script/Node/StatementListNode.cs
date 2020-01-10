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
    public class StatementListNode : Node
    {
        private List<Node> _statementList;

        public StatementListNode(List<Node> statementList)
        {
            _statementList = new List<Node>(statementList);
        }

        public override object Evaluate()
        {
            List<object> values = new List<object>();
            foreach (Node statement in _statementList)
            {
                values.Add(statement.Evaluate());
            }
            return string.Join(",", values);
        }
    }
}
