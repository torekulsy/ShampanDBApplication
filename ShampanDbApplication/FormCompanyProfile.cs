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

        private void FiscalYearSave()
        {
            try
            {
                fiscalyears.Clear();
                for (int i = 0; i < dgvFYear.RowCount; i++)
                {
                    FiscalYearVM detail = new FiscalYearVM();

                    detail.FiscalYearName = dtpFYearStart.Value.ToString("dd/MMM/yyyy") + " To " + dtpFYearEnd.Value.ToString("dd/MMM/yyyy");
                    detail.CurrentYear = dtpFYearEnd.Value.ToString("yyyy");
                    detail.PeriodID = Convert.ToDateTime(dgvFYear.Rows[i].Cells["PeriodStart"].Value).ToString("MMyyyy");
                    detail.PeriodName = Convert.ToDateTime(dgvFYear.Rows[i].Cells["MonthName"].Value).ToString("MMMM-yyyy");
                    detail.PeriodStart = Convert.ToDateTime(dgvFYear.Rows[i].Cells["PeriodStart"].Value).ToString("yyyy-MMM-dd");
                    detail.PeriodEnd = Convert.ToDateTime(dgvFYear.Rows[i].Cells["PeriodEnd"].Value).ToString("yyyy-MMM-dd");
                    detail.PeriodLock = Convert.ToString("N");
                    detail.GLLock = Convert.ToString("N");
                    detail.CreatedBy = Program.CurrentUser;
                    detail.CreatedOn = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
                    detail.LastModifiedBy = Program.CurrentUser;
                    detail.LastModifiedOn = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
                    fiscalyears.Add(detail);

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
           
        }

        private void AddFiscalYear()
        {
            try
            {
                dtpFYearStart.Value = Convert.ToDateTime(dtpFYearStart.Value.ToString("dd/MMM/yyyy"));
                dgvFYear.Rows.Clear();
                for (int j = 0; j < Convert.ToInt32(12); j++)
                {
                    DateTime a =
                        Convert.ToDateTime(dtpFYearStart.Value.ToString("MMMM-yyyy")).AddMonths(j);


                    DataGridViewRow NewRow = new DataGridViewRow();
                    dgvFYear.Rows.Add(NewRow);

                    dgvFYear.Rows[j].Cells["LineNo"].Value = Convert.ToDecimal(j + 1);
                    dgvFYear.Rows[j].Cells["MonthName"].Value = Convert.ToDateTime(dtpFYearStart.Value.ToString("MMMM-yyyy")).AddMonths(j);
                    dgvFYear.Rows[j].Cells["PeriodStart"].Value = Convert.ToDateTime(dtpFYearStart.Value.ToString("dd/MMM/yyyy")).AddMonths(j);
                    dgvFYear.Rows[j].Cells["PeriodEnd"].Value = Convert.ToDateTime(dtpFYearStart.Value.ToString("dd/MMM/yyyy")).AddMonths(j + 1).AddDays(-1);

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
        }

        private SysDBInfoVMTemp connVM = new SysDBInfoVMTemp();

       private void btnNewCompany_Click(object sender, EventArgs e)
       {
           try
           {             
               if (DialogResult.No != MessageBox.Show("Is Fiscal Year Okay?", this.Text, MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question,
                                                          MessageBoxDefaultButton.Button2))
               {

               }
               else
               {
                   return;
               }
               if (txtCompanyID.Text != "")
               {
                   MessageBox.Show(
                       "Data already saved" + "\n" + "To change click update button or for new click refresh button",
                       this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                   return;
               }
             
               progressBar1.Visible = true;
               btnNewCompany.Enabled = false;

              string  NextID = "0";

               NextID = DateTime.Now.ToString("yyMMddHHmmss");
               companyProfiles = new CompanyProfileVM();
               companyProfiles.CompanyID = NextID;
               companyProfiles.CompanyName = txtCompanyName.Text.Trim();
               companyProfiles.CompanyLegalName = txtCompanyLegalName.Text.Trim();
               companyProfiles.Address1 = txtAddress1.Text.Trim();
               companyProfiles.Address2 = "-";
               companyProfiles.Address3 = "-";
               companyProfiles.City = txtCity.Text.Trim();
               companyProfiles.ZipCode = txtZipCode.Text.Trim();
               companyProfiles.TelephoneNo = txtTelephoneNo.Text.Trim();
               companyProfiles.FaxNo = txtFaxNo.Text.Trim();
               companyProfiles.Email = txtEmail.Text.Trim();
               companyProfiles.ContactPerson = txtContactPerson.Text.Trim();
               companyProfiles.ContactPersonDesignation = txtContactPersonDesignation.Text.Trim();
               companyProfiles.ContactPersonTelephone = txtContactPersonTelephone.Text.Trim();
               companyProfiles.ContactPersonEmail = txtContactPersonEmail.Text.Trim();
               companyProfiles.TINNo = txtTINNo.Text.Trim();
               companyProfiles.VatRegistrationNo = txtVatRegistrationNo.Text.Trim();
               companyProfiles.Comments = txtComments.Text.Trim();
               companyProfiles.ActiveStatus = "Y";// active status
               companyProfiles.CreatedBy = Program.CurrentUser;
               companyProfiles.CreatedOn = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
               companyProfiles.StartDateTime = dtpStartDate.Value.ToString("yyyy-MMM-dd HH:mm:ss");
               companyProfiles.FYearStart = dtpFYearStart.Value.ToString("yyyy-MMM-dd");
               companyProfiles.FYearEnd = dtpFYearEnd.Value.ToString("yyyy-MMM-dd");
               companyProfiles.BIN = txtBIN.Text;
               companyProfiles.Section = txtSection.Text.Trim();
                              
               AddFiscalYear();
               FiscalYearSave();
                              
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
               progressBar1.Visible = true;
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
               progressBar1.Visible = false;
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
           txtAddress1.Text = "";
           //txtAddress2.Text = "-";
           //txtAddress3.Text = "-";
           txtCity.Text = "";
           txtZipCode.Text = "";
           txtTelephoneNo.Text = "";
           txtFaxNo.Text = "";
           txtEmail.Text = "";
           txtContactPerson.Text = "";
           txtContactPersonDesignation.Text = "";
           txtContactPersonTelephone.Text = "";
           txtContactPersonEmail.Text = "";
           txtTINNo.Text = "";
           txtVatRegistrationNo.Text = "";
           txtComments.Text = "";

           DateTime now = DateTime.Now;
           DateTime startDate = new DateTime(now.Year, now.Month, 1);

           dtpStartDate.Value = startDate;
           dtpFYearStart.Value = startDate;
       }
       private void dtpFYearStart_ValueChanged(object sender, EventArgs e)
       {
           dtpFYearEnd.Value = dtpFYearStart.Value.AddYears(1).AddDays(-1);
           ChangeData = true;

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
