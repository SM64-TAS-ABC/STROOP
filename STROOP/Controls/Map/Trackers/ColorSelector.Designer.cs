namespace STROOP.Controls.Map.Trackers
{
    partial class ColorSelector
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelColorSelector = new System.Windows.Forms.Panel();
            this.textBoxColorSelector = new STROOP.BetterTextbox();
            this.panelColorSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelColorSelector
            // 
            this.panelColorSelector.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorSelector.Controls.Add(this.textBoxColorSelector);
            this.panelColorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColorSelector.Location = new System.Drawing.Point(0, 0);
            this.panelColorSelector.Margin = new System.Windows.Forms.Padding(0);
            this.panelColorSelector.Name = "panelColorSelector";
            this.panelColorSelector.Size = new System.Drawing.Size(114, 37);
            this.panelColorSelector.TabIndex = 0;
            // 
            // textBoxColorSelector
            // 
            this.textBoxColorSelector.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxColorSelector.Location = new System.Drawing.Point(19, 8);
            this.textBoxColorSelector.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxColorSelector.Name = "textBoxColorSelector";
            this.textBoxColorSelector.Size = new System.Drawing.Size(77, 20);
            this.textBoxColorSelector.TabIndex = 34;
            this.textBoxColorSelector.Text = "240,240,240";
            this.textBoxColorSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ColorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelColorSelector);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ColorSelector";
            this.Size = new System.Drawing.Size(114, 37);
            this.panelColorSelector.ResumeLayout(false);
            this.panelColorSelector.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelColorSelector;
        private BetterTextbox textBoxColorSelector;
    }
}
