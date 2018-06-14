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
    public static class DialogUtilities
    {
        private static string GetFilterString(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Xml:
                    return "*.xml";
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

        public static bool AskQuestionAboutM64Pasting(int numInputs)
        {
            return AskQuestion(
                String.Format("You are about to paste {0} inputs. " +
                    "Pasting more than {1} inputs at a time can be slow. " +
                    "Are you sure you wish to proceed?", numInputs, M64Config.PasteWarningLimit),
                "High Paste Count Warning");
        }

        public static bool AskQuestionAboutSavingVariableFileInPlace()
        {
            return AskQuestion(
                "You are about to save the variables in place. " +
                    "This action will replace the default variables of this tab with the current set of variables. " +
                    "Then from now on, STROOP will open with this set of variables in this tab. " +
                    "This action cannot be undone, except by re-downloading STROOP. " +
                    "Are you sure you wish to proceed?",
                "Save Variables In Place Warning");
        }

        public static bool AskQuestion(string message, string title)
        {
            DialogResult result = MessageBox.Show(
                message,
                title,
                MessageBoxButtons.YesNoCancel);
            return result == DialogResult.Yes;
        }

        public static void DisplayMessage(string message, string title)
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK);
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

        public static List<XElement> OpenXmlElements(FileType fileType, string fileName = null)
        {
            if (fileName == null)
            {
                OpenFileDialog openFileDialog = CreateOpenFileDialog(fileType);
                DialogResult result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK) return new List<XElement>();
                fileName = openFileDialog.FileName;
            }
            XDocument varXml = XDocument.Load(fileName);
            return ConvertDocumentIntoElements(varXml);
        }

        public static void SaveXmlElements(
            FileType fileType, string xmlName, List<XElement> elements, string fileName = null)
        {
            if (fileName == null)
            {
                SaveFileDialog saveFileDialog = CreateSaveFileDialog(fileType);
                DialogResult result = saveFileDialog.ShowDialog();
                if (result != DialogResult.OK) return;
                fileName = saveFileDialog.FileName;
            }
            XDocument document = ConvertElementsIntoDocument(xmlName, elements);
            document.Save(fileName);
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
