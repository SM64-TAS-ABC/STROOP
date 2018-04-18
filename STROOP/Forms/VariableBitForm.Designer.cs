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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewBits = new System.Windows.Forms.DataGridView();
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
            this.textBoxHexValue = new STROOP.BetterTextbox();
            this.textBoxDecValue = new STROOP.BetterTextbox();
            this.textBoxVarName = new STROOP.BetterTextbox();
            this.textBoxBinaryValue = new STROOP.BetterTextbox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBits)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.dataGridViewBits, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxHexValue, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxDecValue, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxVarName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxBinaryValue, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(436, 254);
            this.tableLayoutPanel.TabIndex = 39;
            // 
            // dataGridViewBits
            // 
            this.dataGridViewBits.AllowUserToDeleteRows = false;
            this.dataGridViewBits.AllowUserToResizeColumns = false;
            this.dataGridViewBits.AllowUserToResizeRows = false;
            this.dataGridViewBits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dataGridViewBits.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBits.EnableHeadersVisualStyles = false;
            this.dataGridViewBits.Location = new System.Drawing.Point(0, 112);
            this.dataGridViewBits.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewBits.Name = "dataGridViewBits";
            this.dataGridViewBits.RowHeadersVisible = false;
            this.dataGridViewBits.RowTemplate.Height = 20;
            this.dataGridViewBits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewBits.Size = new System.Drawing.Size(436, 142);
            this.dataGridViewBits.TabIndex = 10;
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
            // textBoxHexValue
            // 
            this.textBoxHexValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHexValue.BackColor = System.Drawing.Color.White;
            this.textBoxHexValue.Location = new System.Drawing.Point(3, 60);
            this.textBoxHexValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.textBoxHexValue.Name = "textBoxHexValue";
            this.textBoxHexValue.Size = new System.Drawing.Size(430, 20);
            this.textBoxHexValue.TabIndex = 9;
            this.textBoxHexValue.Text = "100";
            this.textBoxHexValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxDecValue
            // 
            this.textBoxDecValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDecValue.BackColor = System.Drawing.Color.White;
            this.textBoxDecValue.Location = new System.Drawing.Point(3, 32);
            this.textBoxDecValue.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.textBoxDecValue.Name = "textBoxDecValue";
            this.textBoxDecValue.Size = new System.Drawing.Size(430, 20);
            this.textBoxDecValue.TabIndex = 9;
            this.textBoxDecValue.Text = "0x12345678";
            this.textBoxDecValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxVarName
            // 
            this.textBoxVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVarName.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxVarName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.textBoxVarName.Location = new System.Drawing.Point(3, 3);
            this.textBoxVarName.Name = "textBoxVarName";
            this.textBoxVarName.Size = new System.Drawing.Size(430, 24);
            this.textBoxVarName.TabIndex = 9;
            this.textBoxVarName.Text = "Variable Name";
            this.textBoxVarName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBinaryValue
            // 
            this.textBoxBinaryValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBinaryValue.BackColor = System.Drawing.Color.White;
            this.textBoxBinaryValue.Location = new System.Drawing.Point(3, 88);
            this.textBoxBinaryValue.Name = "textBoxBinaryValue";
            this.textBoxBinaryValue.Size = new System.Drawing.Size(430, 20);
            this.textBoxBinaryValue.TabIndex = 9;
            this.textBoxBinaryValue.Text = "200";
            this.textBoxBinaryValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VariableBitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 254);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableBitForm";
            this.ShowIcon = false;
            this.Text = "Variable Bits";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BetterTextbox textBoxDecValue;
        private BetterTextbox textBoxHexValue;
        private BetterTextbox textBoxBinaryValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private BetterTextbox textBoxVarName;
        private System.Windows.Forms.DataGridView dataGridViewBits;
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