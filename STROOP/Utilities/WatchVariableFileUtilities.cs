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

        public static void SaveVariables(List<XElement> elements, string xmlName = null)
        {
            DialogResult result = _saveFileDialogCustom.ShowDialog();
            if (result != DialogResult.OK)
                return;

            XDocument document = AggregateElementsIntoDocument(elements, xmlName);
            document.Save(_saveFileDialogCustom.FileName);
        }

        private static XDocument AggregateElementsIntoDocument(List<XElement> elements, string xmlName = null)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement(XName.Get(xmlName ?? "CustomData"));
            doc.Add(root);

            foreach (XElement element in elements)
                root.Add(element);

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
