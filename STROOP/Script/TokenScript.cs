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
    public class TokenScript
    {
        private bool _isEnabled = false;

        public TokenScript()
        {
        }

        public void SetScript(string script)
        {
            // set script
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void Update()
        {
            if (!_isEnabled) return;

            // run script
        }
    }
}
