using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class MainLoadingForm : Form
    {
        int _maxStatus;
        Point lastclickedpoint;

        public MainLoadingForm(int maxStatus)
        {
            InitializeComponent();
            _maxStatus = maxStatus;
            textBoxLoadingHelpfulHint.Text = HelpfulHintUtilities.GetRandomHelpfulHint();
            ControlUtilities.AddContextMenuStripFunctions(
                textBoxLoadingHelpfulHint,
                new List<string>() { "Show All Helpful Hints" },
                new List<Action>() { () => HelpfulHintUtilities.ShowAllHelpfulHints() });
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            progressBarLoading.Maximum = _maxStatus;
            CenterToScreen();
        }

        public void UpdateStatus(string status, int number)
        {
            this.Invoke(new Action(() =>
            {
                progressBarLoading.Value = number;
                if (number == _maxStatus)
                {
                    labelLoadingStatus.Text = status;
                    return;
                }

                labelLoadingStatus.Text = String.Format(status + " [{0} / {1}]", number + 1, _maxStatus);
            }));
        }

        private void MainLoadingForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastclickedpoint.X = e.X;
            lastclickedpoint.Y = e.Y;
        }

        private void MainLoadingForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastclickedpoint.X;
                this.Top += e.Y - lastclickedpoint.Y;
            }   
        }

        private void progressBarLoading_MouseDown(object sender, MouseEventArgs e)
        {
            // This has 4 references (each control)
            lastclickedpoint = new Point(e.X, e.Y);
        }

        private void progressBarLoading_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastclickedpoint.X;
                this.Top += e.Y - lastclickedpoint.Y;
            }
        }
    }
}
