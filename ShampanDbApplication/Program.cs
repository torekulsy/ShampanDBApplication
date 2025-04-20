using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;
using SymphonySofttech.Utilities;
using System.IO;
using System.Reflection;
using System;
using PFViewModel.DTOs;


namespace ShampanDbApplication
{
    internal static class Program
    {
        #region PublicDatafields
        public static FormLogIn LoginForm { get; set; }
        public static int BranchId = 0;
        public static string BranchCode = "";
        public static string CurrentUser { get; set; }
        public static string CurrentUserID { get; set; }
        public static bool IsLoading { get; set; }
        public static string R_F { get; set; }
        public static string fromOpen { get; set; }
        public static string SalesType { get; set; }
        public static string Trading { get; set; }
        public static string DatabaseName { get; set; }
        public static string[] PublicRollLines { get; set; }
        public static DateTime SessionDate { get; set; }
        public static DateTime SessionTime { get; set; }
        public static int ChangeTime { get; set; }
        public static DateTime ServerDateTime { get; set; }
        public static DateTime vMinDate = Convert.ToDateTime("1753/01/02");
        public static DateTime vMaxDate = Convert.ToDateTime("9998/12/30");
        public static bool successLogin = false;
        public static string FontSize = "8";
        public static string Access { get; set; }
        public static string Post { get; set; }
        public static DateTime LicenceDate { get; set; }
        public static DateTime serverDate { get; set; }
        public static bool IsTrial = false;
        public static string Trial = "";
        public static string TrialComments = "Unregister Version";
        public static string ImportFileName { get; set; }
        public static string ItemType = "Other";
        private const string LineDelimeter = DBConstant.LineDelimeter;
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private static string PassPhrase = DBConstant.PassPhrase;
        private static string EnKey = DBConstant.EnKey;
        public static bool IsAlpha = false;
        public static string Alpha = "";
        public static string AlphaComments = "Alpha Version";
        public static bool IsBeta = false;
        public static string Beta = "";
        public static string BetaComments = "Beta Version";
        public static bool IsBureau = false;
        public static string Add { get; set; }
        public static string Edit { get; set; }
        public static DataSet publicDataSet { get; set; }
        public static DataTable UserMenuAllRolls { get; set; }
        public static DataTable UserMenuRolls { get; set; }
        public static string IsWCF { get; set; }
        #region CompanyLicenseVM
        public static string LicenseKey = "NA";
        public static bool IsCentralBIN = true;
        public static bool IsManufacturing = true;
        public static bool IsTrading = true;
        public static bool IsService = true;
        public static bool IsTender = true;
        public static bool IsTDS = true;
        public static bool IsBandroll = true;
        public static bool IsTollClient = true;
        public static bool IsTollContractor = true;
        public static bool IsIntegrationExcel = true;
        public static bool IsIntegrationOthers = true;
        public static bool IsIntegrationAPI = true;
        public static int Depos = 0;
        #endregion CompanyLicenseVM
        #region Company Profile
        public static string CompanyID { get; set; }
        public static string CompanyNameLog { get; set; }
        public static string CompanyName { get; set; }
        public static string CompanyLegalName { get; set; }
        public static string Address1 { get; set; }
        public static string Address2 { get; set; }
        public static string Address3 { get; set; }
        public static string City { get; set; }
        public static string ZipCode { get; set; }
        public static string TelephoneNo { get; set; }
        public static string FaxNo { get; set; }
        public static string Email { get; set; }
        public static string ContactPerson { get; set; }
        public static string ContactPersonDesignation { get; set; }
        public static string ContactPersonTelephone { get; set; }
        public static string ContactPersonEmail { get; set; }
        public static string TINNo { get; set; }
        public static string VatRegistrationNo { get; set; }
        public static string Comments { get; set; }
        public static string Section { get; set; }
        public static string ActiveStatus { get; set; }
        public static DateTime FMonthStart { get; set; }
        public static DateTime FMonthEnd { get; set; }
        public static decimal VATAmount { get; set; }
        #endregion
        #endregion
        public static DataSet publicDsCompanyProfile = new DataSet();
        public static string AppPath
        {

            get
            {
                string directory = string.Empty;
                try
                {
                    directory = AppDomain.CurrentDomain.BaseDirectory;
                }
                #region Catch
                catch (IndexOutOfRangeException ex)
                {
                   
                    MessageBox.Show(ex.Message, "Program", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                
                #endregion Catch

                return directory;
            }

        }
       
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MDIMainInterface());
        }

        internal static SysDBInfoVMTemp OrdinaryLoad()
        {
            throw new NotImplementedException();
        }
        public static bool SaveToSuperFile(string tom, string jery, string mini, string DatabaseName)
        {
            bool result = false;

            try
            {


                //if (DeleteFile() == false)
                //{
                //    MessageBox.Show("File not Deleted");
                //    result = false;
                //}

               
                XElement xml = null;
                XDocument Xdoc = new XDocument();
                if (System.IO.File.Exists(AppPath + "/SuperInformation.xml"))
                {
                    Xdoc = XDocument.Load(AppPath + "/SuperInformation.xml");
                    xml = new XElement("SuperInfo",

                                       new XAttribute("tom", tom),
                                       new XElement("jery", jery),
                                       new XElement("mini", mini),
                                       new XElement("doremon", DatabaseName),
                                       new XElement("DateTime", DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"))
                        );
                }
                else
                {
                    xml = new XElement("Super",
                                       new XElement("SuperInfo",
                                                    new XAttribute("tom", tom),
                                                    new XElement("jery", jery),
                                                    new XElement("mini", mini),
                                                    new XElement("doremon", DatabaseName),
                                                    new XElement("DateTime", DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"))
                                           ));


                }
                if (Xdoc.Descendants().Count() > 0)
                    Xdoc.Descendants().First().Add(xml);
                else
                {
                    Xdoc.Add(xml);
                }
                Xdoc.Save(AppPath + "/SuperInformation.xml");

                #region Save data into System Database
                #endregion Save data into System Database

                result = true;

            }
            #region Catch
            catch (IndexOutOfRangeException ex)
            {               
            }
           
            #endregion Catch
            return result;

        }
    }
}
