using STROOP.Forms;
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
        private static List<IUpdatableForm> _forms = new List<IUpdatableForm>();

        public static void AddForm(IUpdatableForm form)
        {
            _forms.Add(form);
        }

        public static void RemoveForm(IUpdatableForm form)
        {
            _forms.Remove(form);
        }

        public static void Update()
        {
            foreach (IUpdatableForm form in _forms)
            {
                form.UpdateForm();
            }
        }

        public static List<VariablePopOutForm> GetPopOutForms()
        {
            return _forms.FindAll(form => form is VariablePopOutForm)
                .ConvertAll(form => form as VariablePopOutForm);
        }
    }
}
