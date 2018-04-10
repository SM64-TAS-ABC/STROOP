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
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.M64Editor
{
    public class M64CopiedData
    {

        public M64CopiedData(DataGridViewSelectedCellCollection tableCells)
        {
            if (tableCells.Count == 0) throw new ArgumentOutOfRangeException();

        }

        public M64CopiedData(DataGridViewSelectedRowCollection tableRows)
        {
            if (tableRows.Count == 0) throw new ArgumentOutOfRangeException();
        }

    }
}
