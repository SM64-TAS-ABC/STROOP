namespace SM64_Diagnostic
{
    partial class VarHackContainerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VarHackContainerForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this._textBoxGetSet = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox1 = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox2 = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox3 = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox4 = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox5 = new SM64_Diagnostic.BetterTextbox();
            this.betterTextbox6 = new SM64_Diagnostic.BetterTextbox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.52316F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.32153F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 305F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.04242F));
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._textBoxGetSet, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.betterTextbox6, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(778, 277);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(167, 165);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // _textBoxGetSet
            // 
            this._textBoxGetSet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxGetSet.BackColor = System.Drawing.Color.White;
            this._textBoxGetSet.Location = new System.Drawing.Point(129, 24);
            this._textBoxGetSet.Name = "_textBoxGetSet";
            this._textBoxGetSet.Size = new System.Drawing.Size(91, 20);
            this._textBoxGetSet.TabIndex = 10;
            this._textBoxGetSet.Text = "Mario X";
            this._textBoxGetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox1
            // 
            this.betterTextbox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox1.BackColor = System.Drawing.SystemColors.Control;
            this.betterTextbox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.betterTextbox1.Location = new System.Drawing.Point(3, 28);
            this.betterTextbox1.Name = "betterTextbox1";
            this.betterTextbox1.ReadOnly = true;
            this.betterTextbox1.Size = new System.Drawing.Size(84, 13);
            this.betterTextbox1.TabIndex = 10;
            this.betterTextbox1.Text = "Name:";
            this.betterTextbox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox2
            // 
            this.betterTextbox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox2.BackColor = System.Drawing.SystemColors.Control;
            this.betterTextbox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.betterTextbox2.Location = new System.Drawing.Point(3, 97);
            this.betterTextbox2.Name = "betterTextbox2";
            this.betterTextbox2.ReadOnly = true;
            this.betterTextbox2.Size = new System.Drawing.Size(84, 13);
            this.betterTextbox2.TabIndex = 10;
            this.betterTextbox2.Text = "Address:";
            this.betterTextbox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox3
            // 
            this.betterTextbox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox3.BackColor = System.Drawing.Color.White;
            this.betterTextbox3.Location = new System.Drawing.Point(129, 93);
            this.betterTextbox3.Name = "betterTextbox3";
            this.betterTextbox3.Size = new System.Drawing.Size(91, 20);
            this.betterTextbox3.TabIndex = 10;
            this.betterTextbox3.Text = "0x8033B1AC";
            this.betterTextbox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox4
            // 
            this.betterTextbox4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox4.BackColor = System.Drawing.SystemColors.Control;
            this.betterTextbox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.betterTextbox4.Location = new System.Drawing.Point(3, 166);
            this.betterTextbox4.Name = "betterTextbox4";
            this.betterTextbox4.ReadOnly = true;
            this.betterTextbox4.Size = new System.Drawing.Size(84, 13);
            this.betterTextbox4.TabIndex = 10;
            this.betterTextbox4.Text = "Use Pointer:";
            this.betterTextbox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox5
            // 
            this.betterTextbox5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox5.BackColor = System.Drawing.SystemColors.Control;
            this.betterTextbox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.betterTextbox5.Location = new System.Drawing.Point(3, 235);
            this.betterTextbox5.Name = "betterTextbox5";
            this.betterTextbox5.ReadOnly = true;
            this.betterTextbox5.Size = new System.Drawing.Size(84, 13);
            this.betterTextbox5.TabIndex = 10;
            this.betterTextbox5.Text = "Pointer Offset:";
            this.betterTextbox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // betterTextbox6
            // 
            this.betterTextbox6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.betterTextbox6.BackColor = System.Drawing.Color.White;
            this.betterTextbox6.Location = new System.Drawing.Point(129, 232);
            this.betterTextbox6.Name = "betterTextbox6";
            this.betterTextbox6.Size = new System.Drawing.Size(91, 20);
            this.betterTextbox6.TabIndex = 10;
            this.betterTextbox6.Text = "0x10";
            this.betterTextbox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VarHackContainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 277);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VarHackContainerForm";
            this.ShowIcon = false;
            this.Text = "Variable Controller";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private BetterTextbox _textBoxGetSet;
        private BetterTextbox betterTextbox1;
        private BetterTextbox betterTextbox2;
        private BetterTextbox betterTextbox3;
        private BetterTextbox betterTextbox4;
        private BetterTextbox betterTextbox5;
        private BetterTextbox betterTextbox6;
    }
}