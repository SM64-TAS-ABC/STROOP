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
            this.buttonSubtract = new System.Windows.Forms.Button();
            this.textBoxAddSubtract = new System.Windows.Forms.TextBox();
            this.textBoxGetSet = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonGet = new System.Windows.Forms.Button();
            this.buttonSet = new System.Windows.Forms.Button();
            this.labelVarName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSubtract
            // 
            this.buttonSubtract.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSubtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSubtract.Location = new System.Drawing.Point(24, 59);
            this.buttonSubtract.Name = "buttonSubtract";
            this.buttonSubtract.Size = new System.Drawing.Size(89, 33);
            this.buttonSubtract.TabIndex = 0;
            this.buttonSubtract.Text = "-";
            this.buttonSubtract.UseVisualStyleBackColor = true;
            this.buttonSubtract.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // textBoxAddSubtract
            // 
            this.textBoxAddSubtract.BackColor = System.Drawing.Color.White;
            this.textBoxAddSubtract.Location = new System.Drawing.Point(119, 66);
            this.textBoxAddSubtract.Name = "textBoxAddSubtract";
            this.textBoxAddSubtract.ReadOnly = true;
            this.textBoxAddSubtract.Size = new System.Drawing.Size(99, 20);
            this.textBoxAddSubtract.TabIndex = 9;
            this.textBoxAddSubtract.Text = "100";
            this.textBoxAddSubtract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxGetSet
            // 
            this.textBoxGetSet.BackColor = System.Drawing.Color.White;
            this.textBoxGetSet.Location = new System.Drawing.Point(119, 120);
            this.textBoxGetSet.Name = "textBoxGetSet";
            this.textBoxGetSet.ReadOnly = true;
            this.textBoxGetSet.Size = new System.Drawing.Size(99, 20);
            this.textBoxGetSet.TabIndex = 9;
            this.textBoxGetSet.Text = "200";
            this.textBoxGetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(224, 59);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(89, 33);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonGet
            // 
            this.buttonGet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonGet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGet.Location = new System.Drawing.Point(24, 113);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(89, 33);
            this.buttonGet.TabIndex = 0;
            this.buttonGet.Text = "Get";
            this.buttonGet.UseVisualStyleBackColor = true;
            this.buttonGet.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonSet
            // 
            this.buttonSet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSet.Location = new System.Drawing.Point(224, 113);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(89, 33);
            this.buttonSet.TabIndex = 0;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelVarName
            // 
            this.labelVarName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelVarName.AutoSize = true;
            this.labelVarName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVarName.Location = new System.Drawing.Point(87, 18);
            this.labelVarName.Name = "labelVarName";
            this.labelVarName.Size = new System.Drawing.Size(166, 25);
            this.labelVarName.TabIndex = 10;
            this.labelVarName.Text = "Variable Name";
            // 
            // VariableControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 162);
            this.Controls.Add(this.labelVarName);
            this.Controls.Add(this.textBoxGetSet);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.buttonGet);
            this.Controls.Add(this.buttonSubtract);
            this.Controls.Add(this.textBoxAddSubtract);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(357, 200);
            this.Name = "VariableControllerForm";
            this.ShowIcon = false;
            this.Text = "Variable Controller";
            this.Load += new System.EventHandler(this.VariableViewerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSubtract;
        private System.Windows.Forms.TextBox textBoxAddSubtract;
        private System.Windows.Forms.TextBox textBoxGetSet;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonGet;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.Label labelVarName;
    }
}