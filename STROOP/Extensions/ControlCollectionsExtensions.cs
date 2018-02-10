using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Extensions
{
    public static class ControlCollectionsExtensions
    {
        public static void Insert(this Control.ControlCollection collection, Control control, int pos)
        {
            collection.Add(control);
            collection.SetChildIndex(control, pos);
        }
    }
}
