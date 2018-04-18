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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableBitForm));
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._dataGridViewBits = new System.Windows.Forms.DataGridView();
            this._textBoxHexValue = new STROOP.BetterTextbox();
            this._textBoxDecValue = new STROOP.BetterTextbox();
            this._textBoxVarName = new STROOP.BetterTextbox();
            this._textBoxBinaryValue = new STROOP.BetterTextbox();
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
            this._tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewBits)).BeginInit();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._dataGridViewBits, 0, 4);
            this._tableLayoutPanel.Controls.Add(this._textBoxHexValue, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._textBoxDecValue, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._textBoxVarName, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._textBoxBinaryValue, 0, 3);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(436, 254);
            this._tableLayoutPanel.TabIndex = 39;
            // 
            // _dataGridViewBits
            // 
            this._dataGridViewBits.AllowUserToDeleteRows = false;
            this._dataGridViewBits.AllowUserToResizeColumns = false;
            this._dataGridViewBits.AllowUserToResizeRows = false;
            this._dataGridViewBits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridViewBits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dataGridViewBits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridViewBits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dataGridViewBits.DefaultCellStyle = dataGridViewCellStyle3;
            this._dataGridViewBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGridViewBits.EnableHeadersVisualStyles = false;
            this._dataGridViewBits.Location = new System.Drawing.Point(0, 112);
            this._dataGridViewBits.Margin = new System.Windows.Forms.Padding(0);
            this._dataGridViewBits.Name = "_dataGridViewBits";
            this._dataGridViewBits.RowHeadersVisible = false;
            this._dataGridViewBits.RowTemplate.Height = 20;
            this._dataGridViewBits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dataGridViewBits.Size = new System.Drawing.Size(436, 142);
            this._dataGridViewBits.TabIndex = 10;
            // 
            // _textBoxHexValue
            // 
            this._textBoxHexValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxHexValue.BackColor = System.Drawing.Color.White;
            this._textBoxHexValue.Location = new System.Drawing.Point(3, 60);
            this._textBoxHexValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxHexValue.Name = "_textBoxHexValue";
            this._textBoxHexValue.Size = new System.Drawing.Size(430, 20);
            this._textBoxHexValue.TabIndex = 9;
            this._textBoxHexValue.Text = "100";
            this._textBoxHexValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxDecValue
            // 
            this._textBoxDecValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxDecValue.BackColor = System.Drawing.Color.White;
            this._textBoxDecValue.Location = new System.Drawing.Point(3, 32);
            this._textBoxDecValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this._textBoxDecValue.Name = "_textBoxDecValue";
            this._textBoxDecValue.Size = new System.Drawing.Size(430, 20);
            this._textBoxDecValue.TabIndex = 9;
            this._textBoxDecValue.Text = "0x12345678";
            this._textBoxDecValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // _textBoxBinaryValue
            // 
            this._textBoxBinaryValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxBinaryValue.BackColor = System.Drawing.Color.White;
            this._textBoxBinaryValue.Location = new System.Drawing.Point(3, 88);
            this._textBoxBinaryValue.Name = "_textBoxBinaryValue";
            this._textBoxBinaryValue.Size = new System.Drawing.Size(430, 20);
            this._textBoxBinaryValue.TabIndex = 9;
            this._textBoxBinaryValue.Text = "200";
            this._textBoxBinaryValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.Index.DefaultCellStyle = dataGridViewCellStyle2;
            this.Index.FillWeight = 200F;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Dec
            // 
            this.Dec.DataPropertyName = "Dec";
            this.Dec.FillWeight = 200F;
            this.Dec.HeaderText = "Dec";
            this.Dec.Name = "Dec";
            this.Dec.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Dec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Hex
            // 
            this.Hex.DataPropertyName = "Hex";
            this.Hex.FillWeight = 200F;
            this.Hex.HeaderText = "Hex";
            this.Hex.Name = "Hex";
            this.Hex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Hex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Binary
            // 
            this.Binary.DataPropertyName = "Binary";
            this.Binary.FillWeight = 400F;
            this.Binary.HeaderText = "Binary";
            this.Binary.Name = "Binary";
            this.Binary.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Binary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Bit7
            // 
            this.Bit7.DataPropertyName = "Bit7";
            this.Bit7.HeaderText = "7";
            this.Bit7.Name = "Bit7";
            this.Bit7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit6
            // 
            this.Bit6.DataPropertyName = "Bit6";
            this.Bit6.HeaderText = "6";
            this.Bit6.Name = "Bit6";
            this.Bit6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit5
            // 
            this.Bit5.DataPropertyName = "Bit5";
            this.Bit5.HeaderText = "5";
            this.Bit5.Name = "Bit5";
            this.Bit5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit4
            // 
            this.Bit4.DataPropertyName = "Bit4";
            this.Bit4.HeaderText = "4";
            this.Bit4.Name = "Bit4";
            this.Bit4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit3
            // 
            this.Bit3.DataPropertyName = "Bit3";
            this.Bit3.HeaderText = "3";
            this.Bit3.Name = "Bit3";
            this.Bit3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit2
            // 
            this.Bit2.DataPropertyName = "Bit2";
            this.Bit2.HeaderText = "2";
            this.Bit2.Name = "Bit2";
            this.Bit2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit1
            // 
            this.Bit1.DataPropertyName = "Bit1";
            this.Bit1.HeaderText = "1";
            this.Bit1.Name = "Bit1";
            this.Bit1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bit0
            // 
            this.Bit0.DataPropertyName = "Bit0";
            this.Bit0.HeaderText = "0";
            this.Bit0.Name = "Bit0";
            this.Bit0.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // VariableBitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 254);
            this.Controls.Add(this._tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableBitForm";
            this.ShowIcon = false;
            this.Text = "Variable Bits";
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewBits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BetterTextbox _textBoxDecValue;
        private BetterTextbox _textBoxHexValue;
        private BetterTextbox _textBoxBinaryValue;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private BetterTextbox _textBoxVarName;
        private System.Windows.Forms.DataGridView _dataGridViewBits;
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