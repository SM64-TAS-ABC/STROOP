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
            this._buttonSubtract = new System.Windows.Forms.Button();
            this._textBoxAddSubtract = new SM64_Diagnostic.BetterTextbox();
            this._textBoxGetSet = new SM64_Diagnostic.BetterTextbox();
            this._buttonAdd = new System.Windows.Forms.Button();
            this._buttonGet = new System.Windows.Forms.Button();
            this._buttonSet = new System.Windows.Forms.Button();
            this._labelVarName = new System.Windows.Forms.Label();
            this._textBoxCurrentValue = new SM64_Diagnostic.BetterTextbox();
            this.SuspendLayout();
            // 
            // _buttonSubtract
            // 
            this._buttonSubtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonSubtract.Location = new System.Drawing.Point(11, 63);
            this._buttonSubtract.Name = "_buttonSubtract";
            this._buttonSubtract.Size = new System.Drawing.Size(50, 33);
            this._buttonSubtract.TabIndex = 0;
            this._buttonSubtract.Text = "-";
            this._buttonSubtract.UseVisualStyleBackColor = true;
            // 
            // _textBoxAddSubtract
            // 
            this._textBoxAddSubtract.BackColor = System.Drawing.Color.White;
            this._textBoxAddSubtract.Location = new System.Drawing.Point(67, 69);
            this._textBoxAddSubtract.Name = "_textBoxAddSubtract";
            this._textBoxAddSubtract.Size = new System.Drawing.Size(67, 20);
            this._textBoxAddSubtract.TabIndex = 9;
            this._textBoxAddSubtract.Text = "100";
            this._textBoxAddSubtract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _textBoxGetSet
            // 
            this._textBoxGetSet.BackColor = System.Drawing.Color.White;
            this._textBoxGetSet.Location = new System.Drawing.Point(67, 107);
            this._textBoxGetSet.Name = "_textBoxGetSet";
            this._textBoxGetSet.Size = new System.Drawing.Size(67, 20);
            this._textBoxGetSet.TabIndex = 9;
            this._textBoxGetSet.Text = "200";
            this._textBoxGetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _buttonAdd
            // 
            this._buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAdd.Location = new System.Drawing.Point(140, 63);
            this._buttonAdd.Name = "_buttonAdd";
            this._buttonAdd.Size = new System.Drawing.Size(50, 33);
            this._buttonAdd.TabIndex = 0;
            this._buttonAdd.Text = "+";
            this._buttonAdd.UseVisualStyleBackColor = true;
            // 
            // _buttonGet
            // 
            this._buttonGet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonGet.Location = new System.Drawing.Point(11, 101);
            this._buttonGet.Name = "_buttonGet";
            this._buttonGet.Size = new System.Drawing.Size(50, 33);
            this._buttonGet.TabIndex = 0;
            this._buttonGet.Text = "Get";
            this._buttonGet.UseVisualStyleBackColor = true;
            // 
            // _buttonSet
            // 
            this._buttonSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonSet.Location = new System.Drawing.Point(140, 101);
            this._buttonSet.Name = "_buttonSet";
            this._buttonSet.Size = new System.Drawing.Size(50, 33);
            this._buttonSet.TabIndex = 0;
            this._buttonSet.Text = "Set";
            this._buttonSet.UseVisualStyleBackColor = true;
            // 
            // _labelVarName
            // 
            this._labelVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelVarName.Location = new System.Drawing.Point(19, 5);
            this._labelVarName.Name = "_labelVarName";
            this._labelVarName.Size = new System.Drawing.Size(166, 25);
            this._labelVarName.TabIndex = 10;
            this._labelVarName.Text = "Variable Name";
            this._labelVarName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _textBoxCurrentValue
            // 
            this._textBoxCurrentValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._textBoxCurrentValue.Location = new System.Drawing.Point(67, 37);
            this._textBoxCurrentValue.Name = "_textBoxCurrentValue";
            this._textBoxCurrentValue.Size = new System.Drawing.Size(67, 20);
            this._textBoxCurrentValue.TabIndex = 9;
            this._textBoxCurrentValue.Text = "100";
            this._textBoxCurrentValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VariableControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 142);
            this.Controls.Add(this._labelVarName);
            this.Controls.Add(this._textBoxGetSet);
            this.Controls.Add(this._buttonAdd);
            this.Controls.Add(this._buttonSet);
            this.Controls.Add(this._buttonGet);
            this.Controls.Add(this._buttonSubtract);
            this.Controls.Add(this._textBoxCurrentValue);
            this.Controls.Add(this._textBoxAddSubtract);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(220, 180);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(220, 180);
            this.Name = "VariableControllerForm";
            this.ShowIcon = false;
            this.Text = "Variable Controller";
            this.Load += new System.EventHandler(this.VariableViewerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _buttonSubtract;
        private BetterTextbox _textBoxAddSubtract;
        private BetterTextbox _textBoxGetSet;
        private System.Windows.Forms.Button _buttonAdd;
        private System.Windows.Forms.Button _buttonGet;
        private System.Windows.Forms.Button _buttonSet;
        private System.Windows.Forms.Label _labelVarName;
        private BetterTextbox _textBoxCurrentValue;
    }
}