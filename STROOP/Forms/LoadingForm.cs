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
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {

        }

        public void UpdateStatus(int progressValue, int progressMax, string status)
        {


            /*
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
            */
        }
    }
}
