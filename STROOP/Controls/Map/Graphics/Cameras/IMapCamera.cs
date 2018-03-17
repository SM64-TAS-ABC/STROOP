using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace STROOP.Controls.Map.Graphics
{
    public interface IMapCamera
    {
        Matrix4 Matrix { get; }
    }
}
