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

namespace STROOP.Forms
{
    public partial class LoadingForm : Form
    {
        int _maxStatus;

        public LoadingForm(int maxStatus)
        {
            InitializeComponent();
            _maxStatus = maxStatus;
            textBoxLoadingHelpfulHint.Text = HelpfulHintUtilities.GetRandomHelpfulHint();
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
    }
}
