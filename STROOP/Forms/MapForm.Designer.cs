namespace STROOP.Forms
{
    partial class MapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.glControlMap2D = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // glControlMap2D
            // 
            this.glControlMap2D.BackColor = System.Drawing.Color.Black;
            this.glControlMap2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControlMap2D.Location = new System.Drawing.Point(0, 0);
            this.glControlMap2D.Margin = new System.Windows.Forms.Padding(0);
            this.glControlMap2D.Name = "glControlMap2D";
            this.glControlMap2D.Size = new System.Drawing.Size(284, 174);
            this.glControlMap2D.TabIndex = 1;
            this.glControlMap2D.VSync = false;
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 174);
            this.Controls.Add(this.glControlMap2D);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "MapForm";
            this.ShowIcon = false;
            this.Text = "Map";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControlMap2D;
    }
}