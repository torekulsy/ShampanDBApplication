using PFServer.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShampanDbApplication
{
    public partial class FormLogIn : Form
    {
        public FormLogIn()
        {
            InitializeComponent();
        }

        //private void btnLogIn_Click(object sender, EventArgs e)
        //{
        //    #region try

        //    try
        //    {

        //        CommonDAL commonDal = new CommonDAL();


        //        this.btnLogIn.Enabled = false;
        //        this.progressBar1.Visible = true;

        //        if (ErrorReturn() != 0)
        //        {
        //            return;
        //        }

        //        if (SuperLoginInfo() == false)
        //        {

        //            FormSuperInfo frms = new FormSuperInfo();
        //            frms.Show();
        //            return;
        //        }

        //        BDName();

        //        SessionDate();


        //        Program.BranchCode = "login";
        //        bgwUserHas.RunWorkerAsync();


        //    }

        //    #endregion

        //    #region catch

        //    catch (Exception ex)
        //    {
        //        string exMessage = ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            exMessage = exMessage + Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
        //                        ex.StackTrace;

        //        }

        //        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //    }
        //    finally
        //    {
        //        this.btnLogIn.Enabled = true;
        //        this.progressBar1.Visible = false;
        //    }
        //    #endregion
           
        //}

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            FormSuperInfo fmi = new FormSuperInfo();
           
            fmi.Show(this);


        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            FormCompanyProfile fmi = new FormCompanyProfile();

            fmi.Show(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogIn_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Program.AppPath + "/SuperInformation.xml"))
            {
                btnLogIn.Visible = false;
                lblMessage.Text = "Please delete SuperInformation.xml";
            }
            else
            {
                btnLogIn.Visible = true;
                lblMessage.Text = "";
            }
        }
    }
}
