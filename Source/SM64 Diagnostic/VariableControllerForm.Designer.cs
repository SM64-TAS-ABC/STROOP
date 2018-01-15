namespace SM64_Diagnostic
{
    partial class VariableControllerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableControllerForm));
            this._textBoxCurrentValue = new SM64_Diagnostic.BetterTextbox();
            this._textBoxAddSubtract = new SM64_Diagnostic.BetterTextbox();
            this._buttonSubtract = new System.Windows.Forms.Button();
            this._textBoxGetSet = new SM64_Diagnostic.BetterTextbox();
            this._buttonGet = new System.Windows.Forms.Button();
            this._buttonAdd = new System.Windows.Forms.Button();
            this._buttonSet = new System.Windows.Forms.Button();
            this._checkBoxLock = new System.Windows.Forms.CheckBox();
            this._textBoxVarName = new SM64_Diagnostic.BetterTextbox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _textBoxCurrentValue
            // 
            this._textBoxCurrentValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxCurrentValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._textBoxCurrentValue.Location = new System.Drawing.Point(55, 44);
            this._textBoxCurrentValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxCurrentValue.Name = "_textBoxCurrentValue";
            this._textBoxCurrentValue.Size = new System.Drawing.Size(69, 20);
            this._textBoxCurrentValue.TabIndex = 9;
            this._textBoxCurrentValue.Text = "0x12345678";
            this._textBoxCurrentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxAddSubtract
            // 
            this._textBoxAddSubtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxAddSubtract.BackColor = System.Drawing.Color.White;
            this._textBoxAddSubtract.Location = new System.Drawing.Point(55, 80);
            this._textBoxAddSubtract.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxAddSubtract.Name = "_textBoxAddSubtract";
            this._textBoxAddSubtract.Size = new System.Drawing.Size(69, 20);
            this._textBoxAddSubtract.TabIndex = 9;
            this._textBoxAddSubtract.Text = "100";
            this._textBoxAddSubtract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _buttonSubtract
            // 
            this._buttonSubtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this._buttonSubtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonSubtract.Location = new System.Drawing.Point(0, 72);
            this._buttonSubtract.Margin = new System.Windows.Forms.Padding(0);
            this._buttonSubtract.Name = "_buttonSubtract";
            this._buttonSubtract.Size = new System.Drawing.Size(52, 36);
            this._buttonSubtract.TabIndex = 0;
            this._buttonSubtract.Text = "-";
            this._buttonSubtract.UseVisualStyleBackColor = true;
            // 
            // _textBoxGetSet
            // 
            this._textBoxGetSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxGetSet.BackColor = System.Drawing.Color.White;
            this._textBoxGetSet.Location = new System.Drawing.Point(55, 116);
            this._textBoxGetSet.Name = "_textBoxGetSet";
            this._textBoxGetSet.Size = new System.Drawing.Size(69, 20);
            this._textBoxGetSet.TabIndex = 9;
            this._textBoxGetSet.Text = "200";
            this._textBoxGetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _buttonGet
            // 
            this._buttonGet.Dock = System.Windows.Forms.DockStyle.Fill;
            this._buttonGet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonGet.Location = new System.Drawing.Point(0, 108);
            this._buttonGet.Margin = new System.Windows.Forms.Padding(0);
            this._buttonGet.Name = "_buttonGet";
            this._buttonGet.Size = new System.Drawing.Size(52, 36);
            this._buttonGet.TabIndex = 0;
            this._buttonGet.Text = "Get";
            this._buttonGet.UseVisualStyleBackColor = true;
            // 
            // _buttonAdd
            // 
            this._buttonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this._buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAdd.Location = new System.Drawing.Point(127, 72);
            this._buttonAdd.Margin = new System.Windows.Forms.Padding(0);
            this._buttonAdd.Name = "_buttonAdd";
            this._buttonAdd.Size = new System.Drawing.Size(54, 36);
            this._buttonAdd.TabIndex = 0;
            this._buttonAdd.Text = "+";
            this._buttonAdd.UseVisualStyleBackColor = true;
            // 
            // _buttonSet
            // 
            this._buttonSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this._buttonSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonSet.Location = new System.Drawing.Point(127, 108);
            this._buttonSet.Margin = new System.Windows.Forms.Padding(0);
            this._buttonSet.Name = "_buttonSet";
            this._buttonSet.Size = new System.Drawing.Size(54, 36);
            this._buttonSet.TabIndex = 0;
            this._buttonSet.Text = "Set";
            this._buttonSet.UseVisualStyleBackColor = true;
            // 
            // _checkBoxLock
            // 
            this._checkBoxLock.AutoSize = true;
            this._checkBoxLock.Dock = System.Windows.Forms.DockStyle.Fill;
            this._checkBoxLock.Location = new System.Drawing.Point(130, 39);
            this._checkBoxLock.Name = "_checkBoxLock";
            this._checkBoxLock.Size = new System.Drawing.Size(48, 30);
            this._checkBoxLock.TabIndex = 38;
            this._checkBoxLock.Text = "Lock";
            this._checkBoxLock.UseVisualStyleBackColor = true;
            // 
            // _textBoxVarName
            // 
            this._textBoxVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxVarName.BackColor = System.Drawing.SystemColors.Control;
            this._textBoxVarName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this._textBoxVarName, 3);
            this._textBoxVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this._textBoxVarName.Location = new System.Drawing.Point(3, 6);
            this._textBoxVarName.Name = "_textBoxVarName";
            this._textBoxVarName.Size = new System.Drawing.Size(175, 24);
            this._textBoxVarName.TabIndex = 9;
            this._textBoxVarName.Text = "Variable Name";
            this._textBoxVarName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.04243F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.91515F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.04242F));
            this.tableLayoutPanel1.Controls.Add(this._checkBoxLock, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._buttonSet, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this._buttonAdd, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this._buttonGet, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._textBoxGetSet, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._buttonSubtract, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._textBoxAddSubtract, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._textBoxCurrentValue, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._textBoxVarName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(181, 144);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // VariableControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 144);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableControllerForm";
            this.ShowIcon = false;
            this.Text = "Variable Controller";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BetterTextbox _textBoxCurrentValue;
        private BetterTextbox _textBoxAddSubtract;
        private System.Windows.Forms.Button _buttonSubtract;
        private BetterTextbox _textBoxGetSet;
        private System.Windows.Forms.Button _buttonGet;
        private System.Windows.Forms.Button _buttonAdd;
        private System.Windows.Forms.Button _buttonSet;
        private System.Windows.Forms.CheckBox _checkBoxLock;
        private BetterTextbox _textBoxVarName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}