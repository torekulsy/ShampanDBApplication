using PFServer.Library;
using PFViewModel.DTOs;
using SymphonySofttech.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ShampanDbApplication
{
    public partial class FormSuperInfo : Form
    {
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
                                             txtDataSource.Text.Trim(), chkIsWindowsAuthentication.Checked, connVM) == true)
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

                if (chkIsWindowsAuthentication.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtDataSource.Text.Trim()))
                    {
                        MessageBox.Show("All field must fillup", this.Text);
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text.Trim())
                        || string.IsNullOrEmpty(txtUserPassword.Text.Trim())
                        || string.IsNullOrEmpty(txtDataSource.Text.Trim()))
                    {
                        MessageBox.Show("All field must fillup", this.Text);
                        return;
                    }
                }

                SysDBInfoVM.SysUserName = txtUserName.Text.Trim();
                SysDBInfoVM.SysPassword = txtUserPassword.Text.Trim();
                SysDBInfoVM.SysdataSource = txtDataSource.Text.Trim();
                SysDBInfoVM.IsWindowsAuthentication = chkIsWindowsAuthentication.Checked;
                if (Program.SaveToSuperFile(Converter.DESEncrypt(PassPhrase, EnKey, txtUserName.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, txtUserPassword.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, txtDataSource.Text.Trim()),
                                        Converter.DESEncrypt(PassPhrase, EnKey, chkIsWindowsAuthentication.Checked == true ? "Y" : "N".ToString())
                                        ) == false)
                {
                    MessageBox.Show("Data not save ", this.Text);

                }
                else
                {
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
