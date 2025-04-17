namespace ShampanDbApplication
{
    partial class FormSupperAdministrator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupperAdministrator));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSysDBCreate = new System.Windows.Forms.Button();
            this.btnDBBackup = new System.Windows.Forms.Button();
            this.btnSuperInfo = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnNewCompany = new System.Windows.Forms.Button();
            this.txtUserPassword = new System.Windows.Forms.TextBox();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSysDBCreate);
            this.groupBox1.Controls.Add(this.btnDBBackup);
            this.groupBox1.Controls.Add(this.btnSuperInfo);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.btnNewCompany);
            this.groupBox1.Controls.Add(this.txtUserPassword);
            this.groupBox1.Controls.Add(this.btnLogIn);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Location = new System.Drawing.Point(6, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 226);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Super Administrator Login";
            // 
            // btnSysDBCreate
            // 
            this.btnSysDBCreate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSysDBCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSysDBCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnSysDBCreate.Image")));
            this.btnSysDBCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSysDBCreate.Location = new System.Drawing.Point(183, 144);
            this.btnSysDBCreate.Name = "btnSysDBCreate";
            this.btnSysDBCreate.Size = new System.Drawing.Size(143, 28);
            this.btnSysDBCreate.TabIndex = 212;
            this.btnSysDBCreate.Text = "Sys  DB Create";
            this.btnSysDBCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSysDBCreate.UseVisualStyleBackColor = false;
            // 
            // btnDBBackup
            // 
            this.btnDBBackup.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDBBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnDBBackup.Image = ((System.Drawing.Image)(resources.GetObject("btnDBBackup.Image")));
            this.btnDBBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDBBackup.Location = new System.Drawing.Point(37, 175);
            this.btnDBBackup.Name = "btnDBBackup";
            this.btnDBBackup.Size = new System.Drawing.Size(143, 28);
            this.btnDBBackup.TabIndex = 211;
            this.btnDBBackup.Text = "Backup/Restore";
            this.btnDBBackup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDBBackup.UseVisualStyleBackColor = false;
            // 
            // btnSuperInfo
            // 
            this.btnSuperInfo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSuperInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSuperInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnSuperInfo.Image")));
            this.btnSuperInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSuperInfo.Location = new System.Drawing.Point(37, 144);
            this.btnSuperInfo.Name = "btnSuperInfo";
            this.btnSuperInfo.Size = new System.Drawing.Size(143, 28);
            this.btnSuperInfo.TabIndex = 3;
            this.btnSuperInfo.Text = "Sys Login Info";
            this.btnSuperInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSuperInfo.UseVisualStyleBackColor = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(37, 95);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(288, 13);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 210;
            this.progressBar1.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(166, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "&Change PWD";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 125;
            this.label4.Text = "PWD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 124;
            this.label3.Text = "User";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(276, 112);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(49, 28);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "B";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnNewCompany
            // 
            this.btnNewCompany.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNewCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnNewCompany.Image = ((System.Drawing.Image)(resources.GetObject("btnNewCompany.Image")));
            this.btnNewCompany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewCompany.Location = new System.Drawing.Point(183, 175);
            this.btnNewCompany.Name = "btnNewCompany";
            this.btnNewCompany.Size = new System.Drawing.Size(143, 28);
            this.btnNewCompany.TabIndex = 6;
            this.btnNewCompany.Text = "&New Company";
            this.btnNewCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewCompany.UseVisualStyleBackColor = false;
            // 
            // txtUserPassword
            // 
            this.txtUserPassword.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserPassword.Location = new System.Drawing.Point(103, 67);
            this.txtUserPassword.MinimumSize = new System.Drawing.Size(175, 22);
            this.txtUserPassword.Name = "txtUserPassword";
            this.txtUserPassword.PasswordChar = '*';
            this.txtUserPassword.Size = new System.Drawing.Size(222, 22);
            this.txtUserPassword.TabIndex = 1;
            this.txtUserPassword.UseSystemPasswordChar = true;
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLogIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnLogIn.Image = ((System.Drawing.Image)(resources.GetObject("btnLogIn.Image")));
            this.btnLogIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogIn.Location = new System.Drawing.Point(37, 112);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(124, 28);
            this.btnLogIn.TabIndex = 2;
            this.btnLogIn.Text = "&Super Login ";
            this.btnLogIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogIn.UseVisualStyleBackColor = false;
            //this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(103, 37);
            this.txtUserName.MinimumSize = new System.Drawing.Size(175, 22);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(222, 22);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.UseSystemPasswordChar = true;
            // 
            // FormSupperAdministrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(192)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(385, 255);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSupperAdministrator";
            this.Text = "Super Administrator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSysDBCreate;
        private System.Windows.Forms.Button btnDBBackup;
        private System.Windows.Forms.Button btnSuperInfo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNewCompany;
        private System.Windows.Forms.TextBox txtUserPassword;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.TextBox txtUserName;
    }
}