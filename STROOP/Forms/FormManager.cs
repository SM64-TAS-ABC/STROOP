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
using System.Xml.Linq;

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

        public static (int num2DTopDown, int num2DOrthographic, int num3D) GetMapCounts()
        {
            int num2DTopDown = 0;
            int num2DOrthographic = 0;
            int num3D = 0;

            if (Config.MapGui.checkBoxMapOptionsEnable3D.Checked)
            {
                num3D++;
            }
            else if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {
                num2DOrthographic++;
            }
            else
            {
                num2DTopDown++;
            }

            foreach (IUpdatableForm form in _forms)
            {
                if (form is MapPopOutForm mapPopOutForm)
                {
                    if (mapPopOutForm.IsOrthographicViewEnabled())
                    {
                        num2DOrthographic++;
                    }
                    else
                    {
                        num2DTopDown++;
                    }
                }
            }

            return (num2DTopDown, num2DOrthographic, num3D);
        }

        public static void SavePopOuts()
        {
            DialogUtilities.SaveXmlElements(
                FileType.StroopVariables, "VarData", GetPopOutData());
        }

        public static List<XElement> GetPopOutData()
        {
            return _forms
                .FindAll(form => form is VariablePopOutForm)
                .ConvertAll(form => (form as VariablePopOutForm).GetData());
        }
    }
}
