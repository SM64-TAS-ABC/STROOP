namespace SM64_Diagnostic
{
    partial class TriangleInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriangleInfoForm));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTriangleInfo = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTriangleInfo, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.92308F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(317, 182);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOk.Location = new System.Drawing.Point(121, 155);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(119, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(79, 13);
            this.labelTitle.TabIndex = 8;
            this.labelTitle.Text = "Triangle Info";
            // 
            // textBoxTriangleInfo
            // 
            this.textBoxTriangleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTriangleInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTriangleInfo.Location = new System.Drawing.Point(3, 38);
            this.textBoxTriangleInfo.Multiline = true;
            this.textBoxTriangleInfo.Name = "textBoxTriangleInfo";
            this.textBoxTriangleInfo.ReadOnly = true;
            this.textBoxTriangleInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTriangleInfo.Size = new System.Drawing.Size(311, 110);
            this.textBoxTriangleInfo.TabIndex = 3;
            this.textBoxTriangleInfo.Text = "\r\n";
            // 
            // TriangleInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 206);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 244);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(357, 244);
            this.Name = "TriangleInfoForm";
            this.ShowIcon = false;
            this.Text = "Triangle Info";
            this.Load += new System.EventHandler(this.VariableViewerForm_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTriangleInfo;
    }
}