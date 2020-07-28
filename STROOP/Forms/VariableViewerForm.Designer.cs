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
            this.textBoxBaseAddressValue = new System.Windows.Forms.TextBox();
            this.textBoxBaseAddressLabel = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseAddressLabel, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseAddressValue, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxVariableName, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressLabel, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxEmulatorAddressValue, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressLabel, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxN64AddressValue, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseTypeOffsetLabel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBaseTypeOffsetValue, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeLabel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTypeValue, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxClassLabel, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxClassValue, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 0, 7);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 8;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50041F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49994F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(304, 183);
            this.tableLayoutPanelMain.TabIndex = 0;
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
            this.textBoxVariableName.Size = new System.Drawing.Size(298, 13);
            this.textBoxVariableName.TabIndex = 16;
            this.textBoxVariableName.Text = "Variable Name";
            this.textBoxVariableName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxEmulatorAddressLabel
            // 
            this.textBoxEmulatorAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressLabel.Location = new System.Drawing.Point(3, 135);
            this.textBoxEmulatorAddressLabel.Name = "textBoxEmulatorAddressLabel";
            this.textBoxEmulatorAddressLabel.ReadOnly = true;
            this.textBoxEmulatorAddressLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxEmulatorAddressLabel.TabIndex = 13;
            this.textBoxEmulatorAddressLabel.Text = "Emulator Address:";
            this.textBoxEmulatorAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxEmulatorAddressValue
            // 
            this.textBoxEmulatorAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmulatorAddressValue.Location = new System.Drawing.Point(103, 135);
            this.textBoxEmulatorAddressValue.Name = "textBoxEmulatorAddressValue";
            this.textBoxEmulatorAddressValue.ReadOnly = true;
            this.textBoxEmulatorAddressValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxEmulatorAddressValue.TabIndex = 9;
            this.textBoxEmulatorAddressValue.Text = "Emulator Address Value";
            // 
            // textBoxN64AddressLabel
            // 
            this.textBoxN64AddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressLabel.Location = new System.Drawing.Point(3, 113);
            this.textBoxN64AddressLabel.Name = "textBoxN64AddressLabel";
            this.textBoxN64AddressLabel.ReadOnly = true;
            this.textBoxN64AddressLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxN64AddressLabel.TabIndex = 12;
            this.textBoxN64AddressLabel.Text = "N64 Address:";
            this.textBoxN64AddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxN64AddressValue
            // 
            this.textBoxN64AddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxN64AddressValue.Location = new System.Drawing.Point(103, 113);
            this.textBoxN64AddressValue.Name = "textBoxN64AddressValue";
            this.textBoxN64AddressValue.ReadOnly = true;
            this.textBoxN64AddressValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxN64AddressValue.TabIndex = 3;
            this.textBoxN64AddressValue.Text = "N64 Address Value";
            // 
            // textBoxBaseTypeOffsetLabel
            // 
            this.textBoxBaseTypeOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseTypeOffsetLabel.Location = new System.Drawing.Point(3, 69);
            this.textBoxBaseTypeOffsetLabel.Name = "textBoxBaseTypeOffsetLabel";
            this.textBoxBaseTypeOffsetLabel.ReadOnly = true;
            this.textBoxBaseTypeOffsetLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxBaseTypeOffsetLabel.TabIndex = 15;
            this.textBoxBaseTypeOffsetLabel.Text = "BaseType + Offset:";
            this.textBoxBaseTypeOffsetLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxBaseTypeOffsetValue
            // 
            this.textBoxBaseTypeOffsetValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseTypeOffsetValue.Location = new System.Drawing.Point(103, 69);
            this.textBoxBaseTypeOffsetValue.Name = "textBoxBaseTypeOffsetValue";
            this.textBoxBaseTypeOffsetValue.ReadOnly = true;
            this.textBoxBaseTypeOffsetValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxBaseTypeOffsetValue.TabIndex = 10;
            this.textBoxBaseTypeOffsetValue.Text = "BaseType + Offset Value";
            // 
            // textBoxTypeLabel
            // 
            this.textBoxTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeLabel.Location = new System.Drawing.Point(3, 47);
            this.textBoxTypeLabel.Name = "textBoxTypeLabel";
            this.textBoxTypeLabel.ReadOnly = true;
            this.textBoxTypeLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxTypeLabel.TabIndex = 14;
            this.textBoxTypeLabel.Text = "Type:";
            this.textBoxTypeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTypeValue
            // 
            this.textBoxTypeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTypeValue.Location = new System.Drawing.Point(103, 47);
            this.textBoxTypeValue.Name = "textBoxTypeValue";
            this.textBoxTypeValue.ReadOnly = true;
            this.textBoxTypeValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxTypeValue.TabIndex = 11;
            this.textBoxTypeValue.Text = "Type Value";
            // 
            // textBoxClassLabel
            // 
            this.textBoxClassLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassLabel.Location = new System.Drawing.Point(3, 25);
            this.textBoxClassLabel.Name = "textBoxClassLabel";
            this.textBoxClassLabel.ReadOnly = true;
            this.textBoxClassLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxClassLabel.TabIndex = 14;
            this.textBoxClassLabel.Text = "Class:";
            this.textBoxClassLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxClassValue
            // 
            this.textBoxClassValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassValue.Location = new System.Drawing.Point(103, 25);
            this.textBoxClassValue.Name = "textBoxClassValue";
            this.textBoxClassValue.ReadOnly = true;
            this.textBoxClassValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxClassValue.TabIndex = 11;
            this.textBoxClassValue.Text = "Class Value";
            // 
            // buttonOk
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonOk, 2);
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(3, 157);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(298, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // textBoxBaseAddressValue
            // 
            this.textBoxBaseAddressValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseAddressValue.Location = new System.Drawing.Point(103, 91);
            this.textBoxBaseAddressValue.Name = "textBoxBaseAddressValue";
            this.textBoxBaseAddressValue.ReadOnly = true;
            this.textBoxBaseAddressValue.Size = new System.Drawing.Size(198, 20);
            this.textBoxBaseAddressValue.TabIndex = 18;
            this.textBoxBaseAddressValue.Text = "Base Address Value";
            // 
            // textBoxBaseAddressLabel
            // 
            this.textBoxBaseAddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseAddressLabel.Location = new System.Drawing.Point(3, 91);
            this.textBoxBaseAddressLabel.Name = "textBoxBaseAddressLabel";
            this.textBoxBaseAddressLabel.ReadOnly = true;
            this.textBoxBaseAddressLabel.Size = new System.Drawing.Size(94, 20);
            this.textBoxBaseAddressLabel.TabIndex = 19;
            this.textBoxBaseAddressLabel.Text = "Base Address:";
            this.textBoxBaseAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // VariableViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 183);
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
        private System.Windows.Forms.TextBox textBoxBaseAddressLabel;
        private System.Windows.Forms.TextBox textBoxBaseAddressValue;
    }
}