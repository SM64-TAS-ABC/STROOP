namespace STROOP.Forms
{
    partial class ImageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageForm));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.trackBarTransparency = new System.Windows.Forms.TrackBar();
            this.buttonOpenImage = new System.Windows.Forms.Button();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.trackBarTransparency, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenImage, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxImage, 0, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(260, 150);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // trackBarTransparency
            // 
            this.trackBarTransparency.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarTransparency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarTransparency.Location = new System.Drawing.Point(130, 1);
            this.trackBarTransparency.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarTransparency.Maximum = 100;
            this.trackBarTransparency.Minimum = 15;
            this.trackBarTransparency.Name = "trackBarTransparency";
            this.trackBarTransparency.Size = new System.Drawing.Size(129, 30);
            this.trackBarTransparency.TabIndex = 4;
            this.trackBarTransparency.TickFrequency = 10;
            this.trackBarTransparency.Value = 100;
            // 
            // buttonOpenImage
            // 
            this.buttonOpenImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenImage.Location = new System.Drawing.Point(1, 1);
            this.buttonOpenImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenImage.Name = "buttonOpenImage";
            this.buttonOpenImage.Size = new System.Drawing.Size(128, 30);
            this.buttonOpenImage.TabIndex = 1;
            this.buttonOpenImage.Text = "Open Image";
            this.buttonOpenImage.UseVisualStyleBackColor = true;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanelMain.SetColumnSpan(this.pictureBoxImage, 2);
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.Location = new System.Drawing.Point(1, 32);
            this.pictureBoxImage.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(258, 117);
            this.pictureBoxImage.TabIndex = 5;
            this.pictureBoxImage.TabStop = false;
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightPink;
            this.ClientSize = new System.Drawing.Size(284, 174);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "ImageForm";
            this.ShowIcon = false;
            this.Text = "Image Form";
            this.TransparencyKey = System.Drawing.Color.LightPink;
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOpenImage;
        private System.Windows.Forms.TrackBar trackBarTransparency;
        private System.Windows.Forms.PictureBox pictureBoxImage;
    }
}