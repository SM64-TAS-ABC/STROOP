using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;
using System.ComponentModel;
using STROOP.Utilities;

namespace STROOP.M64Editor
{
    public class M64Stats
    {
        private readonly M64Header _header;
        private readonly BindingList<M64InputFrame> _inputs;

        public M64Stats(M64Header header, BindingList<M64InputFrame> inputs)
        {
            _header = header;
            _inputs = inputs;
        }
    }
}
