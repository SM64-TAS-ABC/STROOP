using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using System.Reflection;
using SM64_Diagnostic.Managers;
using static SM64_Diagnostic.Structs.WatchVariable;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Structs.VarXUtilities;

namespace SM64_Diagnostic.Controls
{
    public class AddressHolder
    {
        public readonly uint? AddressUS;
        public readonly uint? AddressJP;
        public readonly uint? AddressPAL;
        public readonly uint? AddressOffset;

        public uint Address
        {
            get
            {
                switch (Config.Version)
                {
                    case Config.RomVersion.US:
                        if (AddressUS != null) return AddressUS.Value;
                        break;
                    case Config.RomVersion.JP:
                        if (AddressJP != null) return AddressJP.Value;
                        break;
                    case Config.RomVersion.PAL:
                        if (AddressPAL != null) return AddressPAL.Value;
                        break;
                }
                if (AddressOffset != null) return AddressOffset.Value;
                return 0;
            }
        }

        public AddressHolder(uint? addressUS, uint? addressJP, uint? addressPAL, uint? addressOffset)
        {
            if (addressUS == null && addressJP == null && addressPAL == null && addressOffset == null)
            {
                //TODO add this back in after var refactor
                //throw new ArgumentOutOfRangeException("Cannot instantiate Address with all null values");
            }

            AddressUS = addressUS;
            AddressJP = addressJP;
            AddressPAL = addressPAL;
            AddressOffset = addressOffset;
        }
    }
}
