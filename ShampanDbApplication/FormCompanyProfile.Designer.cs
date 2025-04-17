namespace ShampanDbApplication
{
    partial class FormCompanyProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompanyProfile));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbCompanyType = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtCompanyLegalName = new System.Windows.Forms.TextBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCompanyID = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewCompany = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bgwNew = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbCompanyType);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtCompanyLegalName);
            this.groupBox1.Controls.Add(this.txtCompanyName);
            this.groupBox1.Controls.Add(this.txtCompanyID);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 256);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic";
            // 
            // cmbCompanyType
            // 
            this.cmbCompanyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompanyType.FormattingEnabled = true;
            this.cmbCompanyType.Items.AddRange(new object[] {
            "HRM",
            "PF",
            "GF",
            "TAX"});
            this.cmbCompanyType.Location = new System.Drawing.Point(112, 94);
            this.cmbCompanyType.Name = "cmbCompanyType";
            this.cmbCompanyType.Size = new System.Drawing.Size(267, 21);
            this.cmbCompanyType.TabIndex = 292;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(9, 94);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(78, 13);
            this.label29.TabIndex = 291;
            this.label29.Text = "Company Type";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(92, 93);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 14);
            this.label19.TabIndex = 286;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(92, 70);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(14, 14);
            this.label18.TabIndex = 285;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(92, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 14);
            this.label17.TabIndex = 284;
            this.label17.Text = "*";
            // 
            // txtCompanyLegalName
            // 
            this.txtCompanyLegalName.Location = new System.Drawing.Point(107, 64);
            this.txtCompanyLegalName.MaximumSize = new System.Drawing.Size(4, 20);
            this.txtCompanyLegalName.MinimumSize = new System.Drawing.Size(250, 20);
            this.txtCompanyLegalName.Name = "txtCompanyLegalName";
            this.txtCompanyLegalName.Size = new System.Drawing.Size(250, 20);
            this.txtCompanyLegalName.TabIndex = 3;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(107, 40);
            this.txtCompanyName.MaximumSize = new System.Drawing.Size(4, 20);
            this.txtCompanyName.MinimumSize = new System.Drawing.Size(250, 20);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(250, 20);
            this.txtCompanyName.TabIndex = 2;
            // 
            // txtCompanyID
            // 
            this.txtCompanyID.Location = new System.Drawing.Point(107, 16);
            this.txtCompanyID.MaximumSize = new System.Drawing.Size(4, 20);
            this.txtCompanyID.MinimumSize = new System.Drawing.Size(125, 20);
            this.txtCompanyID.Name = "txtCompanyID";
            this.txtCompanyID.ReadOnly = true;
            this.txtCompanyID.Size = new System.Drawing.Size(125, 20);
            this.txtCompanyID.TabIndex = 1;
            this.txtCompanyID.TabStop = false;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(9, 67);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(64, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "Legal Name";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(9, 43);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(63, 13);
            this.label31.TabIndex = 7;
            this.label31.Text = "Short Name";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(9, 19);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(18, 13);
            this.label32.TabIndex = 6;
            this.label32.Text = "ID";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.btnNewCompany);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(-10, 289);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 40);
            this.panel1.TabIndex = 218;
            // 
            // btnNewCompany
            // 
            this.btnNewCompany.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNewCompany.Image = ((System.Drawing.Image)(resources.GetObject("btnNewCompany.Image")));
            this.btnNewCompany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewCompany.Location = new System.Drawing.Point(17, 9);
            this.btnNewCompany.Name = "btnNewCompany";
            this.btnNewCompany.Size = new System.Drawing.Size(96, 28);
            this.btnNewCompany.TabIndex = 28;
            this.btnNewCompany.Text = "&New Company";
            this.btnNewCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewCompany.UseVisualStyleBackColor = false;
            this.btnNewCompany.Click += new System.EventHandler(this.btnNewCompany_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(321, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 28);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(174, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "&Refresh";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bgwNew
            // 
            this.bgwNew.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwNew_DoWork);
            this.bgwNew.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwNew_RunWorkerCompleted);
            // 
            // FormCompanyProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(192)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(446, 345);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCompanyProfile";
            this.Text = "Company";
            this.Load += new System.EventHandler(this.FormCompanyProfile_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.ComboBox cmbCompanyType;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtCompanyLegalName;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtCompanyID;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button btnNewCompany;
        private System.Windows.Forms.Button btnClose;
        protected System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgwNewCompany;
        private System.ComponentModel.BackgroundWorker bgwNew;
    }
}