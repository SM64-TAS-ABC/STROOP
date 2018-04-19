using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class M64Config
    {
        public static readonly Color NewRowColor = Color.FromArgb(186, 255, 166);
        public static readonly Color EditedCellColor = Color.Pink;

        public static readonly int HeaderSize = 0x400;
        public static readonly byte[] SignatureBytes = new byte[] { 0x4D, 0x36, 0x34, 0x1A };

        public static readonly ushort CountryCodeUS = 69;
        public static readonly ushort CountryCodeJP = 74;

        public static readonly uint CrcUS = 4281031267;
        public static readonly uint CrcJP = 238922318;

        public static FrameInputRelationType FrameInputRelation = FrameInputRelationType.FrameAfterInput;
    }
}
