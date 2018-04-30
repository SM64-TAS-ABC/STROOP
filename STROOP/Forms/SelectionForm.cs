using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Extensions;
using STROOP.Utilities;

namespace STROOP.Forms
{
    public partial class SelectionForm<E> : Form
    {
        public SelectionForm(string selectionText, List<E> items, Action<E> action)
        {
            InitializeComponent();
            textBoxSelect.Text = selectionText;
            listBoxSelections.DataSource = items;
            buttonSet.Click += (sender, e) =>
            {
                E selection = (E) listBoxSelections.SelectedItem;
                action(selection);
                Close();
            };
        }
        
        public static void ShowText(string formTitle, string textTitle, string text)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.Show();
        }
    }
}
