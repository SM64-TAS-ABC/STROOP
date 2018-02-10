namespace STROOP.Forms
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
            this.textBoxEmulatorAddressValue = new System.Windows.Forms.TextBox();
            this.textBoxN64AddressValue = new System.Windows.Forms.TextBox();
            this.textBoxBaseOffsetValue = new System.Windows.Forms.TextBox();
            this.textBoxTypeValue = new System.Windows.Forms.TextBox();
            this.textBoxN64AddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxEmulatorAddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxTypeLabel = new System.Windows.Forms.TextBox();
            this.textBoxBaseOffsetLabel = new System.Windows.Forms.TextBox();
            this.textBoxVariableName = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressValue, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressValue, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseOffsetValue, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeValue, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressLabel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressLabel, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeLabel, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseOffsetLabel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxVariableName, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 0, 5);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(277, 149);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBoxEmulatorAddressValue
            // 
            this.textBoxEmulatorAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressValue.Location = new System.Drawing.Point(103, 99);
            this.textBoxEmulatorAddressValue.Name = "textBoxEmulatorAddressValue";
            this.textBoxEmulatorAddressValue.ReadOnly = true;
            this.textBoxEmulatorAddressValue.Size = new System.Drawing.Size(171, 20);
            this.textBoxEmulatorAddressValue.TabIndex = 9;
            this.textBoxEmulatorAddressValue.Text = "Emulator Address Value";
            // 
            // textBoxN64AddressValue
            // 
            this.textBoxN64AddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressValue.Location = new System.Drawing.Point(103, 75);
            this.textBoxN64AddressValue.Name = "textBoxN64AddressValue";
            this.textBoxN64AddressValue.ReadOnly = true;
            this.textBoxN64AddressValue.Size = new System.Drawing.Size(171, 20);
            this.textBoxN64AddressValue.TabIndex = 3;
            this.textBoxN64AddressValue.Text = "N64 Address Value";
            // 
            // textBoxBaseOffsetValue
            // 
            this.textBoxBaseOffsetValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseOffsetValue.Location = new System.Drawing.Point(103, 51);
            this.textBoxBaseOffsetValue.Name = "textBoxBaseOffsetValue";
            this.textBoxBaseOffsetValue.ReadOnly = true;
            this.textBoxBaseOffsetValue.Size = new System.Drawing.Size(171, 20);
            this.textBoxBaseOffsetValue.TabIndex = 10;
            this.textBoxBaseOffsetValue.Text = "Base + Offset Value";
            // 
            // textBoxTypeValue
            // 
            this.textBoxTypeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeValue.Location = new System.Drawing.Point(103, 27);
            this.textBoxTypeValue.Name = "textBoxTypeValue";
            this.textBoxTypeValue.ReadOnly = true;
            this.textBoxTypeValue.Size = new System.Drawing.Size(171, 20);
            this.textBoxTypeValue.TabIndex = 11;
            this.textBoxTypeValue.Text = "Type Value";
            // 
            // textBoxN64AddressLabel
            // 
            this.textBoxN64AddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressLabel.Location = new System.Drawing.Point(3, 75);
            this.textBoxN64AddressLabel.Name = "textBoxN64AddressLabel";
            this.textBoxN64AddressLabel.ReadOnly = true;
            this.textBoxN64AddressLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxN64AddressLabel.TabIndex = 12;
            this.textBoxN64AddressLabel.Text = "N64 Address:";
            this.textBoxN64AddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxEmulatorAddressLabel
            // 
            this.textBoxEmulatorAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressLabel.Location = new System.Drawing.Point(3, 99);
            this.textBoxEmulatorAddressLabel.Name = "textBoxEmulatorAddressLabel";
            this.textBoxEmulatorAddressLabel.ReadOnly = true;
            this.textBoxEmulatorAddressLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxEmulatorAddressLabel.TabIndex = 13;
            this.textBoxEmulatorAddressLabel.Text = "Emulator Address:";
            this.textBoxEmulatorAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTypeLabel
            // 
            this.textBoxTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeLabel.Location = new System.Drawing.Point(3, 27);
            this.textBoxTypeLabel.Name = "textBoxTypeLabel";
            this.textBoxTypeLabel.ReadOnly = true;
            this.textBoxTypeLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxTypeLabel.TabIndex = 14;
            this.textBoxTypeLabel.Text = "Type:";
            this.textBoxTypeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxBaseOffsetLabel
            // 
            this.textBoxBaseOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseOffsetLabel.Location = new System.Drawing.Point(3, 51);
            this.textBoxBaseOffsetLabel.Name = "textBoxBaseOffsetLabel";
            this.textBoxBaseOffsetLabel.ReadOnly = true;
            this.textBoxBaseOffsetLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxBaseOffsetLabel.TabIndex = 15;
            this.textBoxBaseOffsetLabel.Text = "Base + Offset:";
            this.textBoxBaseOffsetLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxVariableName
            // 
            this.textBoxVariableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVariableName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxVariableName, 2);
            this.textBoxVariableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVariableName.Location = new System.Drawing.Point(3, 3);
            this.textBoxVariableName.Name = "textBoxVariableName";
            this.textBoxVariableName.ReadOnly = true;
            this.textBoxVariableName.Size = new System.Drawing.Size(271, 13);
            this.textBoxVariableName.TabIndex = 16;
            this.textBoxVariableName.Text = "Variable Name";
            this.textBoxVariableName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOk, 2);
            this.buttonOk.Location = new System.Drawing.Point(101, 123);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // VariableViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 173);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "VariableViewerForm";
            this.ShowIcon = false;
            this.Text = "Variable Info";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxN64AddressValue;
        private System.Windows.Forms.TextBox textBoxEmulatorAddressValue;
        private System.Windows.Forms.TextBox textBoxBaseOffsetValue;
        private System.Windows.Forms.TextBox textBoxTypeValue;
        private System.Windows.Forms.TextBox textBoxN64AddressLabel;
        private System.Windows.Forms.TextBox textBoxEmulatorAddressLabel;
        private System.Windows.Forms.TextBox textBoxTypeLabel;
        private System.Windows.Forms.TextBox textBoxBaseOffsetLabel;
        private System.Windows.Forms.TextBox textBoxVariableName;
    }
}