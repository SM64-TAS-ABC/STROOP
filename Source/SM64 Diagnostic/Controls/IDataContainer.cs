using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public interface IDataContainer
    {
        string Name
        {
            get;
            set;
        }
        string SpecialName
        {
            get;
        }

        Control Control
        {
            get;
        }


        void Update();
    }
}
