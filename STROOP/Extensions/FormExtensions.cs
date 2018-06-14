using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Extensions
{
    public static class FormExtensions
    {
        public static void TryInvoke(this Form form, Delegate method)
        {
            try
            {
                form.Invoke(method);
            }
            catch (ObjectDisposedException) { }
        }
    }
}
