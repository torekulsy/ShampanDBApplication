using PFServer.Library;
using PFViewModel.DTOs;
using SymphonySofttech.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace ShampanDbApplication
{
    public partial class FormSuperInfo : Form
    {

        private CompanyProfileVM companyProfiles = new CompanyProfileVM();
        private List<FiscalYearVM> fiscalyears = new List<FiscalYearVM>();

        // ----------- Declare from DBConstant End--------//
        private string[] sqlResults;
        private bool SAVE_DOWORK_SUCCESS = false;
     
        private string encriptedFiscalYearData;
        private string encriptedCompanyProfileData;
        private string CompanyProfileData;    
        //string NextID;
        private string result = string.Empty;
        public string VFIN = "181";
        private DataTable companyResult;
        private DataSet ReportResult;
        private string DBName = string.Empty;
        private string DBName2 = string.Empty;
        private string NextID = string.Empty;
        private bool IsSymphonyUser = false;


        private bool ChangeData = false;
        public FormSuperInfo()
        {
            InitializeComponent();
           
        }
        private SysDBInfoVMTemp connVM = new SysDBInfoVMTemp();
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private static string PassPhrase = DBConstant.PassPhrase;
        private static string EnKey = DBConstant.EnKey;
        private void btnTestConn_Click(object sender, EventArgs e)
        {
            try
            {
                btnTestConn.Enabled = false;
                progressBar1.Visible = true;
                CommonDAL commonDal = new CommonDAL();
                if (
                    commonDal.TestConnection(txtUserName.Text.Trim(), txtUserPassword.Text.Trim(),
                                             txtDataSource.Text.Trim(), true))
                {
                    MessageBox.Show("Database Connection Stablished successfully", this.Text);
                }
                else
                {
                    MessageBox.Show("Database Connection not Stablish", this.Text);

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
                btnTestConn.Enabled = true;
                progressBar1.Visible = false;
            }

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;

                if (string.IsNullOrEmpty(txtDatabaseName.Text.Trim()) || string.IsNullOrEmpty(txtDataSource.Text.Trim()) || string.IsNullOrEmpty(txtUserName.Text.Trim()) || string.IsNullOrEmpty(txtUserPassword.Text.Trim()))
                {
                    MessageBox.Show("All field must fillup", this.Text);
                    return;
                }               

                SysDBInfoVM.SysUserName = txtUserName.Text.Trim();
                SysDBInfoVM.SysPassword = txtUserPassword.Text.Trim();
                SysDBInfoVM.SysdataSource = txtDataSource.Text.Trim();
                SysDBInfoVM.DatabaseName = txtDatabaseName.Text.Trim();
                if (Program.SaveToSuperFile(Converter.DESEncrypt(PassPhrase, EnKey, txtUserName.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, txtUserPassword.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, txtDataSource.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, txtDatabaseName.Text.Trim() + "_DB")
                                        ) == false)
                {
                    MessageBox.Show("Data not save ", this.Text);

                }
                else
                {
                  
                    DataSave();

                    MessageBox.Show("Data save successfully", this.Text);

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
                progressBar1.Visible = false;
            }


        }

        public void DataSave()
        {
            try
            {
                btnLogIn.Enabled = false;

                string NextID = "0";

                NextID = DateTime.Now.ToString("yyMMddHHmmss");
                companyProfiles = new CompanyProfileVM();
                companyProfiles.CompanyID = NextID;
                companyProfiles.CompanyName = txtDatabaseName.Text.Trim();
                companyProfiles.CompanyLegalName = txtDatabaseName.Text.Trim();
                companyProfiles.Address2 = "-";
                companyProfiles.Address3 = "-";
                companyProfiles.ActiveStatus = "Y";// active status
                companyProfiles.CreatedBy = Program.CurrentUser;
                companyProfiles.CreatedOn = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
                companyProfiles.CompanyType = cmbCompanyType.SelectedItem.ToString();

                var result = txtDatabaseName.Text.Trim();
                DBName = Regex.Replace(result, @"[0-9\-]", "_");
                DBName = Regex.Replace(result, "[^a-zA-Z0-9_.]+", "_", RegexOptions.Compiled);
                DBName = DBName.Replace(".", "_");
                DBName = DBName.Replace(" ", "_");
                DBName = DBName.Replace("@", "_");
                DBName = DBName.Replace("$", "_");
                DBName = DBName.Replace("#", "_");
                DBName = DBName.Replace(",", "_");
                DBName = DBName.Replace("'", "_");

                btnLogIn.Enabled = false;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

                btnLogIn.Enabled = true;
            }
        }

        private void FormSuperInfo_Load(object sender, EventArgs e)
        {
          
        }

    }
}
