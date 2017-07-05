namespace SM64_Diagnostic
{
    partial class TriangleCoordinatesForm
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelVarName = new System.Windows.Forms.Label();
            this.textBoxN64AddValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.56467F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.43533F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelVarName, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddValue, 1, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(317, 182);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOk, 2);
            this.buttonOk.Location = new System.Drawing.Point(121, 155);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelVarName
            // 
            this.labelVarName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelVarName.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelVarName, 2);
            this.labelVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVarName.Location = new System.Drawing.Point(96, 24);
            this.labelVarName.Name = "labelVarName";
            this.labelVarName.Size = new System.Drawing.Size(124, 13);
            this.labelVarName.TabIndex = 8;
            this.labelVarName.Text = "Triangle Coordinates";
            // 
            // textBoxN64AddValue
            // 
            this.textBoxN64AddValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxN64AddValue.Location = new System.Drawing.Point(45, 65);
            this.textBoxN64AddValue.Multiline = true;
            this.textBoxN64AddValue.Name = "textBoxN64AddValue";
            this.textBoxN64AddValue.ReadOnly = true;
            this.tableLayoutPanelMain.SetRowSpan(this.textBoxN64AddValue, 3);
            this.textBoxN64AddValue.Size = new System.Drawing.Size(226, 84);
            this.textBoxN64AddValue.TabIndex = 3;
            this.textBoxN64AddValue.Text = "\r\n";
            // 
            // TriangleCoordinatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 206);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 244);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(357, 244);
            this.Name = "TriangleCoordinatesForm";
            this.ShowIcon = false;
            this.Text = "Triangle Coordinates";
            this.Load += new System.EventHandler(this.VariableViewerForm_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelVarName;
        private System.Windows.Forms.TextBox textBoxN64AddValue;
    }
}