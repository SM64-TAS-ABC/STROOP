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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableBitForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._textBoxGetSet = new STROOP.BetterTextbox();
            this._textBoxAddSubtract = new STROOP.BetterTextbox();
            this._textBoxCurrentValue = new STROOP.BetterTextbox();
            this._textBoxVarName = new STROOP.BetterTextbox();
            this.dataGridViewM64Inputs = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Binary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bit7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Bit0 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewM64Inputs)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewM64Inputs, 0, 4);
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
            // dataGridViewM64Inputs
            // 
            this.dataGridViewM64Inputs.AllowUserToDeleteRows = false;
            this.dataGridViewM64Inputs.AllowUserToResizeColumns = false;
            this.dataGridViewM64Inputs.AllowUserToResizeRows = false;
            this.dataGridViewM64Inputs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewM64Inputs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewM64Inputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewM64Inputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.Dec,
            this.Hex,
            this.Binary,
            this.Bit7,
            this.Bit6,
            this.Bit5,
            this.Bit4,
            this.Bit3,
            this.Bit2,
            this.Bit1,
            this.Bit0});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewM64Inputs.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewM64Inputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewM64Inputs.EnableHeadersVisualStyles = false;
            this.dataGridViewM64Inputs.Location = new System.Drawing.Point(0, 112);
            this.dataGridViewM64Inputs.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewM64Inputs.Name = "dataGridViewM64Inputs";
            this.dataGridViewM64Inputs.RowHeadersVisible = false;
            this.dataGridViewM64Inputs.RowTemplate.Height = 20;
            this.dataGridViewM64Inputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewM64Inputs.Size = new System.Drawing.Size(436, 142);
            this.dataGridViewM64Inputs.TabIndex = 10;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            this.Index.FillWeight = 200F;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Dec
            // 
            this.Dec.DataPropertyName = "Dec";
            this.Dec.FillWeight = 200F;
            this.Dec.HeaderText = "Dec";
            this.Dec.Name = "Dec";
            this.Dec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Hex
            // 
            this.Hex.DataPropertyName = "Hex";
            this.Hex.FillWeight = 200F;
            this.Hex.HeaderText = "Hex";
            this.Hex.Name = "Hex";
            this.Hex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Binary
            // 
            this.Binary.DataPropertyName = "Binary";
            this.Binary.FillWeight = 200F;
            this.Binary.HeaderText = "Binary";
            this.Binary.Name = "Binary";
            this.Binary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Bit7
            // 
            this.Bit7.DataPropertyName = "Bit7";
            this.Bit7.HeaderText = "7";
            this.Bit7.Name = "Bit7";
            this.Bit7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit6
            // 
            this.Bit6.DataPropertyName = "Bit6";
            this.Bit6.HeaderText = "6";
            this.Bit6.Name = "Bit6";
            this.Bit6.ReadOnly = true;
            this.Bit6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit5
            // 
            this.Bit5.DataPropertyName = "Bit5";
            this.Bit5.HeaderText = "5";
            this.Bit5.Name = "Bit5";
            this.Bit5.ReadOnly = true;
            this.Bit5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit4
            // 
            this.Bit4.DataPropertyName = "Bit4";
            this.Bit4.HeaderText = "4";
            this.Bit4.Name = "Bit4";
            this.Bit4.ReadOnly = true;
            this.Bit4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit3
            // 
            this.Bit3.DataPropertyName = "Bit3";
            this.Bit3.HeaderText = "3";
            this.Bit3.Name = "Bit3";
            this.Bit3.ReadOnly = true;
            this.Bit3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit2
            // 
            this.Bit2.DataPropertyName = "Bir2";
            this.Bit2.HeaderText = "2";
            this.Bit2.Name = "Bit2";
            this.Bit2.ReadOnly = true;
            this.Bit2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit1
            // 
            this.Bit1.DataPropertyName = "Bit1";
            this.Bit1.HeaderText = "1";
            this.Bit1.Name = "Bit1";
            this.Bit1.ReadOnly = true;
            this.Bit1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Bit0
            // 
            this.Bit0.DataPropertyName = "Bir0";
            this.Bit0.HeaderText = "0";
            this.Bit0.Name = "Bit0";
            this.Bit0.ReadOnly = true;
            this.Bit0.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewM64Inputs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BetterTextbox _textBoxCurrentValue;
        private BetterTextbox _textBoxAddSubtract;
        private BetterTextbox _textBoxGetSet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BetterTextbox _textBoxVarName;
        private System.Windows.Forms.DataGridView dataGridViewM64Inputs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Binary;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bit0;
    }
}