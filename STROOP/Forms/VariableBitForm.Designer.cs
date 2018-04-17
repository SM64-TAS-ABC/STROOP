namespace STROOP.Forms
{
    partial class VariableBitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableBitForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._textBoxGetSet = new STROOP.BetterTextbox();
            this._textBoxAddSubtract = new STROOP.BetterTextbox();
            this._textBoxCurrentValue = new STROOP.BetterTextbox();
            this._textBoxVarName = new STROOP.BetterTextbox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._textBoxAddSubtract, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._textBoxCurrentValue, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._textBoxVarName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._textBoxGetSet, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 254);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // _textBoxGetSet
            // 
            this._textBoxGetSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxGetSet.BackColor = System.Drawing.Color.White;
            this._textBoxGetSet.Location = new System.Drawing.Point(3, 88);
            this._textBoxGetSet.Name = "_textBoxGetSet";
            this._textBoxGetSet.Size = new System.Drawing.Size(430, 20);
            this._textBoxGetSet.TabIndex = 9;
            this._textBoxGetSet.Text = "200";
            this._textBoxGetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxAddSubtract
            // 
            this._textBoxAddSubtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxAddSubtract.BackColor = System.Drawing.Color.White;
            this._textBoxAddSubtract.Location = new System.Drawing.Point(3, 60);
            this._textBoxAddSubtract.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxAddSubtract.Name = "_textBoxAddSubtract";
            this._textBoxAddSubtract.Size = new System.Drawing.Size(430, 20);
            this._textBoxAddSubtract.TabIndex = 9;
            this._textBoxAddSubtract.Text = "100";
            this._textBoxAddSubtract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxCurrentValue
            // 
            this._textBoxCurrentValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxCurrentValue.BackColor = System.Drawing.Color.White;
            this._textBoxCurrentValue.Location = new System.Drawing.Point(3, 32);
            this._textBoxCurrentValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxCurrentValue.Name = "_textBoxCurrentValue";
            this._textBoxCurrentValue.Size = new System.Drawing.Size(430, 20);
            this._textBoxCurrentValue.TabIndex = 9;
            this._textBoxCurrentValue.Text = "0x12345678";
            this._textBoxCurrentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxVarName
            // 
            this._textBoxVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxVarName.BackColor = System.Drawing.SystemColors.Control;
            this._textBoxVarName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this._textBoxVarName.Location = new System.Drawing.Point(3, 3);
            this._textBoxVarName.Name = "_textBoxVarName";
            this._textBoxVarName.Size = new System.Drawing.Size(430, 24);
            this._textBoxVarName.TabIndex = 9;
            this._textBoxVarName.Text = "Variable Name";
            this._textBoxVarName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VariableBitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 254);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableBitForm";
            this.ShowIcon = false;
            this.Text = "Variable Bits";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BetterTextbox _textBoxCurrentValue;
        private BetterTextbox _textBoxAddSubtract;
        private BetterTextbox _textBoxGetSet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BetterTextbox _textBoxVarName;
    }
}