using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PFViewModel.DTOs;
using System.Text.RegularExpressions;
using PFServer.Library;
using System.Xml;
using System.IO;
using SymphonySofttech.Utilities;

namespace ShampanDbApplication
{
    public partial class FormCompanyProfile : Form
    {
        public FormCompanyProfile()
        {
            InitializeComponent();
        }
        private CompanyProfileVM companyProfiles = new CompanyProfileVM();
        private List<FiscalYearVM> fiscalyears = new List<FiscalYearVM>();

        // ----------- Declare from DBConstant End--------//
        private string[] sqlResults;
        private bool SAVE_DOWORK_SUCCESS = false;       
        private static string PassPhrase = DBConstant.PassPhrase;
        private static string EnKey = DBConstant.EnKey;

        private string encriptedFiscalYearData;
        private string encriptedCompanyProfileData;
        private string CompanyProfileData;
        private bool ChangeData = false;
        //string NextID;
        private string result = string.Empty;
        public string VFIN = "181";
        private DataTable companyResult;
        private DataSet ReportResult;
        private string DBName = string.Empty;
        private string DBName2 = string.Empty;
        private string NextID = string.Empty;
        private bool IsSymphonyUser = false;
        #region sql save, update, delete

         #endregion

       private SysDBInfoVMTemp connVM = new SysDBInfoVMTemp();

       private void btnNewCompany_Click(object sender, EventArgs e)
       {
           try
           {                                        
               btnNewCompany.Enabled = false;

              string  NextID = "0";

               NextID = DateTime.Now.ToString("yyMMddHHmmss");
               companyProfiles = new CompanyProfileVM();
               companyProfiles.CompanyID = NextID;
               companyProfiles.CompanyName = txtCompanyName.Text.Trim();
               companyProfiles.CompanyLegalName = txtCompanyLegalName.Text.Trim();              
               companyProfiles.Address2 = "-";
               companyProfiles.Address3 = "-";                      
               companyProfiles.ActiveStatus = "Y";// active status
               companyProfiles.CreatedBy = Program.CurrentUser;
               companyProfiles.CreatedOn = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
               companyProfiles.CompanyType = cmbCompanyType.SelectedItem.ToString();
                              
               //DBName = txtCompanyName.Text.Replace(".", " ");
               //DBName = DBName.Replace(".", " ");
               //DBName = DBName.Replace(" ", "_");
               var result = txtCompanyName.Text.Trim();
               DBName = Regex.Replace(result, @"[0-9\-]", "_");
               DBName = Regex.Replace(result, "[^a-zA-Z0-9_.]+", "_", RegexOptions.Compiled);
               DBName = DBName.Replace(".", "_");
               DBName = DBName.Replace(" ", "_");
               DBName = DBName.Replace("@", "_");
               DBName = DBName.Replace("$", "_");
               DBName = DBName.Replace("#", "_");
               DBName = DBName.Replace(",", "_");
               DBName = DBName.Replace("'", "_");
              
               btnNewCompany.Enabled = false;
               bgwNew.RunWorkerAsync();                           
           }
           #region catch
            catch (Exception ex)
            {
                string exMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    exMessage = exMessage + Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
                                ex.StackTrace;

                }
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            #endregion
          
       }

    

       private void bgwNew_DoWork(object sender, DoWorkEventArgs e)
       {
           try
           {
               #region Statement
               #region Statement
               SAVE_DOWORK_SUCCESS = false;
               sqlResults = new string[3];
               CommonDAL commonDal = new CommonDAL();
               //ICommon commonDal = OrdinaryVATDesktop.GetObject<CommonDAL, CommonRepo, ICommon>(OrdinaryVATDesktop.IsWCF);


               sqlResults = commonDal.NewDBCreate(companyProfiles, DBName + "_DB", fiscalyears, connVM);
               //done
               SAVE_DOWORK_SUCCESS = true;

               #endregion

               // Start DoWork

               // End DoWork

               #endregion

           }
           #region catch              
           catch (Exception ex)
           {
               string exMessage = ex.Message + Environment.NewLine + ex.StackTrace;
               if (ex.InnerException != null)
               {
                   exMessage = exMessage + Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
                               ex.StackTrace;

               }
               MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
           }
           #endregion
       }

       private void bgwNew_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
       {
           try
           {
               if (SAVE_DOWORK_SUCCESS)
               {
                   if (sqlResults.Length > 0)
                   {
                       string result = sqlResults[0];
                       string message = sqlResults[1];
                       string newId = sqlResults[2];
                       if (string.IsNullOrEmpty(result))
                       {
                           throw new ArgumentNullException("bgwNewCompany_RunWorkerCompleted", "Unexpected error.");
                       }
                       else if (result == "Success" || result == "Fail")
                       {
                           MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           txtCompanyID.Text = newId;
                       }

                   }
               }

           }
           #region catch     
           catch (Exception ex)
           {
               string exMessage = ex.Message;
               if (ex.InnerException != null)
               {
                   exMessage = exMessage + Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
                               ex.StackTrace;

               }
               MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
           }
           #endregion
           finally
           {
               ChangeData = false;
              
               btnNewCompany.Enabled = true;
           }

       }

       private void FormCompanyProfile_Load(object sender, EventArgs e)
       {           
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "SuperInformation.xml");
            XmlDocument doc = new XmlDocument();

            try
            {               
                doc.Load(filePath);
                XmlNode superInfoNode = doc.SelectSingleNode("/Super/SuperInfo");

                SysDBInfoVM.SysUserName = Converter.DESDecrypt(PassPhrase, EnKey, superInfoNode.Attributes["tom"].Value);
                SysDBInfoVM.SysPassword = Converter.DESDecrypt(PassPhrase, EnKey, superInfoNode["jery"].InnerText);
                SysDBInfoVM.SysdataSource = Converter.DESDecrypt(PassPhrase, EnKey, superInfoNode["mini"].InnerText);
      
            }
            catch (Exception ex)
            {               
                MessageBox.Show("Error loading XML: " + ex.Message);
            }
       }

       private void btnClose_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       private void btnCancel_Click(object sender, EventArgs e)
       {
           if (ChangeData == true)
           {
               if (DialogResult.No != MessageBox.Show(

                   "Recent changes have not been saved ." + "\n" + " Want to refresh without saving?",
                   this.Text,
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question,
                   MessageBoxDefaultButton.Button2))
               {
                   ClearAll();
                   ChangeData = false;
                   //return;
               }
           }
           if (ChangeData == false)
           {

               ClearAll();
               ChangeData = false;
           }
       }
       private void ClearAll()
       {
           txtCompanyID.Text = "";
           txtCompanyName.Text = "";
           txtCompanyLegalName.Text = "";
          

           DateTime now = DateTime.Now;
           DateTime startDate = new DateTime(now.Year, now.Month, 1);

         
       }     
     
       private void dtpFYearEnd_ValueChanged(object sender, EventArgs e)
       {
           ChangeData = true;
       }

       private void button1_Click(object sender, EventArgs e)
       {
           bgwNew_DefaultDataSave();
       }
       private void bgwNew_DefaultDataSave()
       {
           try
           {
               #region Statement

               #region Statement


               SAVE_DOWORK_SUCCESS = false;
               sqlResults = new string[3];
               CommonDAL commonDal = new CommonDAL();

               sqlResults = commonDal.DefaultDataSave();
               //done
               SAVE_DOWORK_SUCCESS = true;

               #endregion

               #endregion

           }
           #region catch
           catch (Exception ex)
           {
               string exMessage = ex.Message + Environment.NewLine + ex.StackTrace;
               if (ex.InnerException != null)
               {
                   exMessage = exMessage + Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
                               ex.StackTrace;

               }
               MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

           }
           #endregion
       }

    }
}
