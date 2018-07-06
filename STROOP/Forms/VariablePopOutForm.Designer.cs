namespace STROOP.Forms
{
    partial class VariablePopOutForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariablePopOutForm));
            this._watchVariablePanel = new STROOP.Controls.WatchVariableFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // _watchVariablePanel
            // 
            this._watchVariablePanel.AutoScroll = true;
            this._watchVariablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._watchVariablePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._watchVariablePanel.Location = new System.Drawing.Point(0, 0);
            this._watchVariablePanel.Margin = new System.Windows.Forms.Padding(0);
            this._watchVariablePanel.Name = "_watchVariablePanel";
            this._watchVariablePanel.Padding = new System.Windows.Forms.Padding(2);
            this._watchVariablePanel.Size = new System.Drawing.Size(284, 174);
            this._watchVariablePanel.TabIndex = 3;
            // 
            // VariablePopOutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 174);
            this.Controls.Add(this._watchVariablePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariablePopOutForm";
            this.ShowIcon = false;
            this.Text = "Variables";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.WatchVariableFlowLayoutPanel _watchVariablePanel;
    }
}