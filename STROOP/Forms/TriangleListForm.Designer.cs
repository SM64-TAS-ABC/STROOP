namespace STROOP.Forms
{
    partial class TriangleListForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriangleListForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonInject = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonSort = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.labelNumTriangles = new System.Windows.Forms.Label();
            this.buttonAnnihilate = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.buttonAnnihilate, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonInject, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonSort, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelNumTriangles, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 181);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonInject
            // 
            this.buttonInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInject.Location = new System.Drawing.Point(165, 33);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(75, 24);
            this.buttonInject.TabIndex = 8;
            this.buttonInject.Text = "Inject";
            this.buttonInject.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.Location = new System.Drawing.Point(246, 33);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(76, 24);
            this.buttonRemove.TabIndex = 7;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // buttonSort
            // 
            this.buttonSort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSort.Location = new System.Drawing.Point(3, 33);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(75, 24);
            this.buttonSort.TabIndex = 6;
            this.buttonSort.Text = "Sort";
            this.buttonSort.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Distance});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView, 4);
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 60);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(325, 121);
            this.dataGridView.TabIndex = 5;
            // 
            // labelNumTriangles
            // 
            this.labelNumTriangles.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelNumTriangles, 4);
            this.labelNumTriangles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumTriangles.Location = new System.Drawing.Point(3, 0);
            this.labelNumTriangles.Name = "labelNumTriangles";
            this.labelNumTriangles.Size = new System.Drawing.Size(319, 30);
            this.labelNumTriangles.TabIndex = 0;
            this.labelNumTriangles.Text = "Num Triangles";
            this.labelNumTriangles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAnnihilate
            // 
            this.buttonAnnihilate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAnnihilate.Location = new System.Drawing.Point(84, 33);
            this.buttonAnnihilate.Name = "buttonAnnihilate";
            this.buttonAnnihilate.Size = new System.Drawing.Size(75, 24);
            this.buttonAnnihilate.TabIndex = 9;
            this.buttonAnnihilate.Text = "Annihilate";
            this.buttonAnnihilate.UseVisualStyleBackColor = true;
            // 
            // Address
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Address.DefaultCellStyle = dataGridViewCellStyle2;
            this.Address.HeaderText = "Address";
            this.Address.MinimumWidth = 2;
            this.Address.Name = "Address";
            // 
            // Distance
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Distance.DefaultCellStyle = dataGridViewCellStyle3;
            this.Distance.HeaderText = "Distance";
            this.Distance.MinimumWidth = 2;
            this.Distance.Name = "Distance";
            // 
            // TriangleListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 181);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.Name = "TriangleListForm";
            this.ShowIcon = false;
            this.Text = "Triangle List";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelNumTriangles;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonInject;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonSort;
        private System.Windows.Forms.Button buttonAnnihilate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
    }
}