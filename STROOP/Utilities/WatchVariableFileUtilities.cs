using STROOP.Controls;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Structs
{
    public static class WatchVariableFileUtilities
    {
        private static SaveFileDialog _saveFileDialogCustom =
            new SaveFileDialog()
            {
                CheckPathExists = true,
                Filter = "STROOP Variables|*.stv",
            };

        private static OpenFileDialog _openFileDialogCustom =
            new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "STROOP Variables|*.stv",
            };

        public static List<WatchVariableControlPrecursor> OpenVariables()
        {
            DialogResult result = _openFileDialogCustom.ShowDialog();
            if (result != DialogResult.OK)
                return new List<WatchVariableControlPrecursor>();

            XDocument varXml = XDocument.Load(_openFileDialogCustom.FileName);
            return WatchVariablesFromXML(varXml);
        }

        public static void SaveVariables(List<WatchVariableControlPrecursor> precursors, string xmlName = null)
        {
            DialogResult result = _saveFileDialogCustom.ShowDialog();
            if (result != DialogResult.OK)
                return;

            WatchVariablesToXML(precursors, xmlName).Save(_saveFileDialogCustom.FileName);
        }

        private static XDocument WatchVariablesToXML(List<WatchVariableControlPrecursor> watchVars, string xmlName = null)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement(XName.Get(xmlName ?? "CustomData"));
            doc.Add(root);

            foreach (WatchVariableControlPrecursor watchVar in watchVars)
                root.Add(watchVar.ToXML());

            return doc;
        }

        private static List<WatchVariableControlPrecursor> WatchVariablesFromXML(XContainer xml)
        {
            // Retreive the root node
            if (xml is XDocument)
                xml = (xml as XDocument).Root;

            return xml.Elements().ToList().ConvertAll(e => new WatchVariableControlPrecursor(e));
        }

    }
}
