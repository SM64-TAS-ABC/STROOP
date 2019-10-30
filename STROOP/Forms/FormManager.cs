using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class FormManager
    {
        private static List<IForm> _forms = new List<IForm>();

        public static void AddForm(IForm form)
        {
            _forms.Add(form);
        }

        public static void RemoveForm(IForm form)
        {
            _forms.Remove(form);
        }

        public static void Update()
        {
            foreach (IForm form in _forms)
            {
                form.UpdateForm();
            }
        }
    }
}
