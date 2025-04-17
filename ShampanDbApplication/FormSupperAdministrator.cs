using PFViewModel.DTOs;
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
    public partial class FormSupperAdministrator : Form
    {
        public FormSupperAdministrator()
        {
            InitializeComponent();
            connVM = Program.OrdinaryLoad();
        }
        private SysDBInfoVMTemp connVM = new SysDBInfoVMTemp();
        private bool ChangeData = false;
        private bool SuperAdministrator = false;
        private static bool SymphonyUser = false;
        private const string LineDelimeter = DBConstant.LineDelimeter;
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private static string PassPhrase = DBConstant.PassPhrase;
        private static string EnKey = DBConstant.EnKey;
        private string[] sqlResults;
        private bool SAVE_DOWORK_SUCCESS = false;
        private bool DELETE_DOWORK_SUCCESS = false;
        private bool UPDATE_DOWORK_SUCCESS = false;
        private string SuperLoginName = string.Empty;
        private string SuperLoginPWD = string.Empty;
        private DataTable SuperNameResult;
        private string result = string.Empty;
        private string LoginPWD = string.Empty;
        private string LoginName = string.Empty;
        private string uName = string.Empty;
        private string pwd = string.Empty;
        private string DbSource = string.Empty;

        //private void btnLogIn_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        #region Statement

        //        if (Program.CheckLicence(DateTime.Now) == true)
        //        {
        //            MessageBox.Show("Fiscal year not create or your system date not ok, Transaction not complete");
        //            return;
        //        }

        //        //SuperNamelist();
        //        this.btnLogIn.Enabled = false;
        //        SuperAdministrator = false;
        //        this.progressBar1.Visible = true;
        //        bgwLogin.RunWorkerAsync();


        //        #endregion

        //    }
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
        //    #endregion

        //}
    }
}
