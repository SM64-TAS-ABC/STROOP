namespace STROOP.Forms
{
    partial class MainLoadingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainLoadingForm));
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.labelLoading = new System.Windows.Forms.Label();
            this.labelLoadingStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLoadingHelpfulHint = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLoading.Location = new System.Drawing.Point(2, 27);
            this.progressBarLoading.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(423, 51);
            this.progressBarLoading.TabIndex = 0;
            // 
            // labelLoading
            // 
            this.labelLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoading.Location = new System.Drawing.Point(2, 0);
            this.labelLoading.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(423, 25);
            this.labelLoading.TabIndex = 1;
            this.labelLoading.Text = "Loading STROOP";
            this.labelLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLoadingStatus
            // 
            this.labelLoadingStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelLoadingStatus.AutoSize = true;
            this.labelLoadingStatus.Location = new System.Drawing.Point(186, 82);
            this.labelLoadingStatus.Margin = new System.Windows.Forms.Padding(2);
            this.labelLoadingStatus.Name = "labelLoadingStatus";
            this.labelLoadingStatus.Size = new System.Drawing.Size(54, 13);
            this.labelLoadingStatus.TabIndex = 2;
            this.labelLoadingStatus.Text = "Loading...";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxLoadingHelpfulHint, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelLoading, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLoadingStatus, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBarLoading, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(427, 179);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // textBoxLoadingHelpfulHint
            // 
            this.textBoxLoadingHelpfulHint.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLoadingHelpfulHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLoadingHelpfulHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoadingHelpfulHint.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLoadingHelpfulHint.Location = new System.Drawing.Point(3, 103);
            this.textBoxLoadingHelpfulHint.Name = "textBoxLoadingHelpfulHint";
            this.textBoxLoadingHelpfulHint.ReadOnly = true;
            this.textBoxLoadingHelpfulHint.Size = new System.Drawing.Size(421, 73);
            this.textBoxLoadingHelpfulHint.TabIndex = 3;
            this.textBoxLoadingHelpfulHint.Text = "Helpful Hint: Do something.\nLine 2\nLine 3\nLine 4";
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 199);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LoadingForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "LoadingForm";
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.Label labelLoadingStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox textBoxLoadingHelpfulHint;
    }
}