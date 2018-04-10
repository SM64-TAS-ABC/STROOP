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
    public static class FileUtilities
    {
        private static string GetFilterString(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.StroopVariables:
                    return "STROOP Variables|*.stv";
                case FileType.StroopVarHackVariables:
                    return "STROOP Var Hack Variables|*.stvhv";
                case FileType.MupenMovie:
                    return "Mupen Movies|*.m64|All Files|*.*";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static OpenFileDialog CreateOpenFileDialog(FileType fileType)
        {
            return new OpenFileDialog()
            {
                Filter = GetFilterString(fileType),
            };
        }

        public static SaveFileDialog CreateSaveFileDialog(FileType fileType)
        {
            return new SaveFileDialog()
            {
                Filter = GetFilterString(fileType),
            };
        }

        public static List<XElement> OpenXmlElements(FileType fileType)
        {
            OpenFileDialog openFileDialog = CreateOpenFileDialog(fileType);
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return new List<XElement>();

            XDocument varXml = XDocument.Load(openFileDialog.FileName);
            return ConvertDocumentIntoElements(varXml);
        }

        public static void SaveXmlElements(
            FileType fileType, string xmlName, List<XElement> elements)
        {
            SaveFileDialog saveFileDialog = CreateSaveFileDialog(fileType);
            DialogResult result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            XDocument document = ConvertElementsIntoDocument(xmlName, elements);
            document.Save(saveFileDialog.FileName);
        }

        private static XDocument ConvertElementsIntoDocument(
            string xmlName, List<XElement> elements)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement(XName.Get(xmlName));
            doc.Add(root);

            foreach (XElement element in elements)
                root.Add(element);

            return doc;
        }

        private static List<XElement> ConvertDocumentIntoElements(XDocument doc)
        {
            XElement root = doc.Root;
            return root.Elements().ToList();
        }

    }
}
