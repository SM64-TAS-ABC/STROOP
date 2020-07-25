namespace STROOP.Forms
{
    partial class CoinRingDisplayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoinRingDisplayForm));
            this.coinRingDisplayPanel = new CoinRingDisplayPanel();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.coinRingDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coinRingDisplayPanel.Location = new System.Drawing.Point(0, 0);
            this.coinRingDisplayPanel.Margin = new System.Windows.Forms.Padding(0);
            this.coinRingDisplayPanel.Name = "coinRingDisplayPanel";
            this.coinRingDisplayPanel.Size = new System.Drawing.Size(584, 161);
            this.coinRingDisplayPanel.TabIndex = 0;
            // 
            // CoinRingDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 161);
            this.Controls.Add(this.coinRingDisplayPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "CoinRingDisplayForm";
            this.ShowIcon = false;
            this.Text = "Coin Ring Display Form";
            this.ResumeLayout(false);

        }

        #endregion

        private CoinRingDisplayPanel coinRingDisplayPanel;
    }
}