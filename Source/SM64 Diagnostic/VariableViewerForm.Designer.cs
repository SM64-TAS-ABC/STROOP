namespace SM64_Diagnostic
{
    partial class VariableViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableViewerForm));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelEmuAdd = new System.Windows.Forms.Label();
            this.labelN64Add = new System.Windows.Forms.Label();
            this.textBoxN64AddValue = new System.Windows.Forms.TextBox();
            this.labelVarType = new System.Windows.Forms.Label();
            this.labelVarName = new System.Windows.Forms.Label();
            this.textBoxEmuAddValue = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxVarTypeValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.54574F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.45426F));
            this.tableLayoutPanelMain.Controls.Add(this.labelVarType, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmuAddValue, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelVarName, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelEmuAdd, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelN64Add, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddValue, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBox1, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxVarTypeValue, 1, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(317, 238);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonOk.Location = new System.Drawing.Point(103, 205);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelEmuAdd
            // 
            this.labelEmuAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmuAdd.AutoSize = true;
            this.labelEmuAdd.Location = new System.Drawing.Point(5, 156);
            this.labelEmuAdd.Name = "labelEmuAdd";
            this.labelEmuAdd.Size = new System.Drawing.Size(92, 13);
            this.labelEmuAdd.TabIndex = 1;
            this.labelEmuAdd.Text = "Emulator Address:";
            // 
            // labelN64Add
            // 
            this.labelN64Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelN64Add.AutoSize = true;
            this.labelN64Add.Location = new System.Drawing.Point(26, 117);
            this.labelN64Add.Name = "labelN64Add";
            this.labelN64Add.Size = new System.Drawing.Size(71, 13);
            this.labelN64Add.TabIndex = 4;
            this.labelN64Add.Text = "N64 Address:";
            // 
            // textBoxN64AddValue
            // 
            this.textBoxN64AddValue.Location = new System.Drawing.Point(103, 120);
            this.textBoxN64AddValue.Name = "textBoxN64AddValue";
            this.textBoxN64AddValue.ReadOnly = true;
            this.textBoxN64AddValue.Size = new System.Drawing.Size(159, 20);
            this.textBoxN64AddValue.TabIndex = 3;
            this.textBoxN64AddValue.Text = "N64 Address";
            // 
            // labelVarType
            // 
            this.labelVarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVarType.AutoSize = true;
            this.labelVarType.Location = new System.Drawing.Point(63, 39);
            this.labelVarType.Name = "labelVarType";
            this.labelVarType.Size = new System.Drawing.Size(34, 13);
            this.labelVarType.TabIndex = 5;
            this.labelVarType.Text = "Type:";
            // 
            // labelVarName
            // 
            this.labelVarName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelVarName.AutoSize = true;
            this.labelVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVarName.Location = new System.Drawing.Point(103, 13);
            this.labelVarName.Name = "labelVarName";
            this.labelVarName.Size = new System.Drawing.Size(89, 13);
            this.labelVarName.TabIndex = 8;
            this.labelVarName.Text = "Variable Name";
            // 
            // textBoxEmuAddValue
            // 
            this.textBoxEmuAddValue.Location = new System.Drawing.Point(103, 159);
            this.textBoxEmuAddValue.Name = "textBoxEmuAddValue";
            this.textBoxEmuAddValue.ReadOnly = true;
            this.textBoxEmuAddValue.Size = new System.Drawing.Size(159, 20);
            this.textBoxEmuAddValue.TabIndex = 9;
            this.textBoxEmuAddValue.Text = "Emulator Address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 81);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(159, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "N64 Address";
            // 
            // textBoxVarTypeValue
            // 
            this.textBoxVarTypeValue.Location = new System.Drawing.Point(103, 42);
            this.textBoxVarTypeValue.Name = "textBoxVarTypeValue";
            this.textBoxVarTypeValue.ReadOnly = true;
            this.textBoxVarTypeValue.Size = new System.Drawing.Size(159, 20);
            this.textBoxVarTypeValue.TabIndex = 11;
            this.textBoxVarTypeValue.Text = "N64 Address";
            // 
            // VariableViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 262);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(357, 300);
            this.Name = "VariableViewerForm";
            this.ShowIcon = false;
            this.Text = "Variable Info";
            this.Load += new System.EventHandler(this.VariableViewerForm_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelEmuAdd;
        private System.Windows.Forms.Label labelN64Add;
        private System.Windows.Forms.TextBox textBoxN64AddValue;
        private System.Windows.Forms.Label labelVarType;
        private System.Windows.Forms.Label labelVarName;
        private System.Windows.Forms.TextBox textBoxEmuAddValue;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxVarTypeValue;
    }
}