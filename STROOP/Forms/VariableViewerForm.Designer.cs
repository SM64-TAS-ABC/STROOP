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
            this.textBoxN64BaseAddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxN64BaseAddressValue = new System.Windows.Forms.TextBox();
            this.textBoxVariableName = new System.Windows.Forms.TextBox();
            this.textBoxEmulatorAddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxEmulatorAddressValue = new System.Windows.Forms.TextBox();
            this.textBoxN64AddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxN64AddressValue = new System.Windows.Forms.TextBox();
            this.textBoxBaseTypeOffsetLabel = new System.Windows.Forms.TextBox();
            this.textBoxBaseTypeOffsetValue = new System.Windows.Forms.TextBox();
            this.textBoxTypeLabel = new System.Windows.Forms.TextBox();
            this.textBoxTypeValue = new System.Windows.Forms.TextBox();
            this.textBoxClassLabel = new System.Windows.Forms.TextBox();
            this.textBoxClassValue = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxEmulatorBaseAddressLabel = new System.Windows.Forms.TextBox();
            this.textBoxEmulatorBaseAddressValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.textBoxVariableName, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseTypeOffsetLabel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseTypeOffsetValue, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeLabel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeValue, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxClassLabel, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxClassValue, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressLabel, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressValue, 1, 7);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressLabel, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressValue, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64BaseAddressLabel, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64BaseAddressValue, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorBaseAddressLabel, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorBaseAddressValue, 1, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 9;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11148F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11106F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(320, 220);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBoxN64BaseAddressLabel
            // 
            this.textBoxN64BaseAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64BaseAddressLabel.Location = new System.Drawing.Point(3, 99);
            this.textBoxN64BaseAddressLabel.Name = "textBoxN64BaseAddressLabel";
            this.textBoxN64BaseAddressLabel.ReadOnly = true;
            this.textBoxN64BaseAddressLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxN64BaseAddressLabel.TabIndex = 19;
            this.textBoxN64BaseAddressLabel.Text = "N64 Base Address:";
            this.textBoxN64BaseAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxN64BaseAddressValue
            // 
            this.textBoxN64BaseAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64BaseAddressValue.Location = new System.Drawing.Point(133, 99);
            this.textBoxN64BaseAddressValue.Name = "textBoxN64BaseAddressValue";
            this.textBoxN64BaseAddressValue.ReadOnly = true;
            this.textBoxN64BaseAddressValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxN64BaseAddressValue.TabIndex = 18;
            this.textBoxN64BaseAddressValue.Text = "N64 Base Address Value";
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
            this.textBoxVariableName.Size = new System.Drawing.Size(314, 13);
            this.textBoxVariableName.TabIndex = 16;
            this.textBoxVariableName.Text = "Variable Name";
            this.textBoxVariableName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxEmulatorAddressLabel
            // 
            this.textBoxEmulatorAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressLabel.Location = new System.Drawing.Point(3, 171);
            this.textBoxEmulatorAddressLabel.Name = "textBoxEmulatorAddressLabel";
            this.textBoxEmulatorAddressLabel.ReadOnly = true;
            this.textBoxEmulatorAddressLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxEmulatorAddressLabel.TabIndex = 13;
            this.textBoxEmulatorAddressLabel.Text = "Emulator Address:";
            this.textBoxEmulatorAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxEmulatorAddressValue
            // 
            this.textBoxEmulatorAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressValue.Location = new System.Drawing.Point(133, 171);
            this.textBoxEmulatorAddressValue.Name = "textBoxEmulatorAddressValue";
            this.textBoxEmulatorAddressValue.ReadOnly = true;
            this.textBoxEmulatorAddressValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxEmulatorAddressValue.TabIndex = 9;
            this.textBoxEmulatorAddressValue.Text = "Emulator Address Value";
            // 
            // textBoxN64AddressLabel
            // 
            this.textBoxN64AddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressLabel.Location = new System.Drawing.Point(3, 147);
            this.textBoxN64AddressLabel.Name = "textBoxN64AddressLabel";
            this.textBoxN64AddressLabel.ReadOnly = true;
            this.textBoxN64AddressLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxN64AddressLabel.TabIndex = 12;
            this.textBoxN64AddressLabel.Text = "N64 Address:";
            this.textBoxN64AddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxN64AddressValue
            // 
            this.textBoxN64AddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressValue.Location = new System.Drawing.Point(133, 147);
            this.textBoxN64AddressValue.Name = "textBoxN64AddressValue";
            this.textBoxN64AddressValue.ReadOnly = true;
            this.textBoxN64AddressValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxN64AddressValue.TabIndex = 3;
            this.textBoxN64AddressValue.Text = "N64 Address Value";
            // 
            // textBoxBaseTypeOffsetLabel
            // 
            this.textBoxBaseTypeOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseTypeOffsetLabel.Location = new System.Drawing.Point(3, 75);
            this.textBoxBaseTypeOffsetLabel.Name = "textBoxBaseTypeOffsetLabel";
            this.textBoxBaseTypeOffsetLabel.ReadOnly = true;
            this.textBoxBaseTypeOffsetLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxBaseTypeOffsetLabel.TabIndex = 15;
            this.textBoxBaseTypeOffsetLabel.Text = "BaseType + Offset:";
            this.textBoxBaseTypeOffsetLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxBaseTypeOffsetValue
            // 
            this.textBoxBaseTypeOffsetValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseTypeOffsetValue.Location = new System.Drawing.Point(133, 75);
            this.textBoxBaseTypeOffsetValue.Name = "textBoxBaseTypeOffsetValue";
            this.textBoxBaseTypeOffsetValue.ReadOnly = true;
            this.textBoxBaseTypeOffsetValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxBaseTypeOffsetValue.TabIndex = 10;
            this.textBoxBaseTypeOffsetValue.Text = "BaseType + Offset Value";
            // 
            // textBoxTypeLabel
            // 
            this.textBoxTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeLabel.Location = new System.Drawing.Point(3, 51);
            this.textBoxTypeLabel.Name = "textBoxTypeLabel";
            this.textBoxTypeLabel.ReadOnly = true;
            this.textBoxTypeLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxTypeLabel.TabIndex = 14;
            this.textBoxTypeLabel.Text = "Type:";
            this.textBoxTypeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTypeValue
            // 
            this.textBoxTypeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeValue.Location = new System.Drawing.Point(133, 51);
            this.textBoxTypeValue.Name = "textBoxTypeValue";
            this.textBoxTypeValue.ReadOnly = true;
            this.textBoxTypeValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxTypeValue.TabIndex = 11;
            this.textBoxTypeValue.Text = "Type Value";
            // 
            // textBoxClassLabel
            // 
            this.textBoxClassLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassLabel.Location = new System.Drawing.Point(3, 27);
            this.textBoxClassLabel.Name = "textBoxClassLabel";
            this.textBoxClassLabel.ReadOnly = true;
            this.textBoxClassLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxClassLabel.TabIndex = 14;
            this.textBoxClassLabel.Text = "Class:";
            this.textBoxClassLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxClassValue
            // 
            this.textBoxClassValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassValue.Location = new System.Drawing.Point(133, 27);
            this.textBoxClassValue.Name = "textBoxClassValue";
            this.textBoxClassValue.ReadOnly = true;
            this.textBoxClassValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxClassValue.TabIndex = 11;
            this.textBoxClassValue.Text = "Class Value";
            // 
            // buttonOk
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOk, 2);
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(3, 195);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(314, 22);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // textBoxEmulatorBaseAddressLabel
            // 
            this.textBoxEmulatorBaseAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorBaseAddressLabel.Location = new System.Drawing.Point(3, 123);
            this.textBoxEmulatorBaseAddressLabel.Name = "textBoxEmulatorBaseAddressLabel";
            this.textBoxEmulatorBaseAddressLabel.ReadOnly = true;
            this.textBoxEmulatorBaseAddressLabel.Size = new System.Drawing.Size(124, 20);
            this.textBoxEmulatorBaseAddressLabel.TabIndex = 13;
            this.textBoxEmulatorBaseAddressLabel.Text = "Emulator Base Address:";
            this.textBoxEmulatorBaseAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxEmulatorBaseAddressValue
            // 
            this.textBoxEmulatorBaseAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorBaseAddressValue.Location = new System.Drawing.Point(133, 123);
            this.textBoxEmulatorBaseAddressValue.Name = "textBoxEmulatorBaseAddressValue";
            this.textBoxEmulatorBaseAddressValue.ReadOnly = true;
            this.textBoxEmulatorBaseAddressValue.Size = new System.Drawing.Size(184, 20);
            this.textBoxEmulatorBaseAddressValue.TabIndex = 18;
            this.textBoxEmulatorBaseAddressValue.Text = "Emulator Base Address Value";
            // 
            // VariableViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 220);
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
        private System.Windows.Forms.TextBox textBoxBaseTypeOffsetValue;
        private System.Windows.Forms.TextBox textBoxTypeValue;
        private System.Windows.Forms.TextBox textBoxN64AddressLabel;
        private System.Windows.Forms.TextBox textBoxEmulatorAddressLabel;
        private System.Windows.Forms.TextBox textBoxTypeLabel;
        private System.Windows.Forms.TextBox textBoxBaseTypeOffsetLabel;
        private System.Windows.Forms.TextBox textBoxVariableName;
        private System.Windows.Forms.TextBox textBoxClassLabel;
        private System.Windows.Forms.TextBox textBoxClassValue;
        private System.Windows.Forms.TextBox textBoxN64BaseAddressLabel;
        private System.Windows.Forms.TextBox textBoxN64BaseAddressValue;
        private System.Windows.Forms.TextBox textBoxEmulatorBaseAddressLabel;
        private System.Windows.Forms.TextBox textBoxEmulatorBaseAddressValue;
    }
}