using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using PFViewModel.DTOs;
using SymphonySofttech.Utilities;
namespace PFServer.Library
{
    public class CommonDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private static string PassPhrase = DBConstant.PassPhrase;
        private static string EnKey = DBConstant.EnKey;       
        DataTable ApiDt = new DataTable();
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();
        #endregion

        public bool TestConnection(string userName, string Password, string Datasource, bool IsWindowsAuthentication = false, SysDBInfoVMTemp connVM = null)
        {
            #region Initializ

            bool result = false;
            SqlConnection conn = null;
            string ConnectionString = "";

            #endregion

            #region try

            try
            {
                #region open connection and transaction
                if (IsWindowsAuthentication)
                {
                    ConnectionString = "Data Source=" + Datasource + ";Trusted_Connection=True;" +
                             ";connect Timeout=120;";
                }
                else
                {
                    ConnectionString = "Data Source=" + Datasource + ";" +
                                "user id=" + userName + ";password=" + Password + ";connect Timeout=120;";
                }

                conn = new SqlConnection(ConnectionString);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    result = true;
                }


                #endregion open connection and transaction

            }
            #endregion

            #region catch

            catch (SqlException sqlex)
            {
               
                throw sqlex;
            }
           
            #endregion

            #region finally

            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

            }
            #endregion

            return result;

        }
        
        public string[] NewDBCreate(CompanyProfileVM companyProfiles, string databaseName, List<FiscalYearVM> fiscalDetails, SysDBInfoVMTemp connVM = null)
        {
            
            #region Initializ

            string[] retResults = new string[3];
            retResults[0] = "Fail";
            retResults[1] = "Fail";
            retResults[2] = "";

            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            int countId = 0;
            string sqlText = "";
            string nextId = "";
            string newID = "";

            #endregion Initializ

            #region Try
            try
            {
                #region Validation

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new ArgumentNullException(MessageVM.dbMsgMethodName, MessageVM.dbMsgNoCompanyName);
                }
                if (fiscalDetails.Count() <= 0)
                {
                    throw new ArgumentNullException(MessageVM.dbMsgMethodName, MessageVM.dbMsgNoFiscalYear);
                }
                if (companyProfiles == null)
                {
                    throw new ArgumentNullException(MessageVM.dbMsgMethodName, MessageVM.dbMsgNoCompanyInformation);
                }

                #endregion Validation

                #region open connection and transaction sys / Master

                SysDBInfoVM.SysDatabaseName = "";
             
                currConn = _dbsqlConnection.GetConnectionSys(connVM);//start
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region check Database

                sqlText = "";
                sqlText += " USE [master]";
                sqlText += " select COUNT(NAME) from sys.databases where name = '" + databaseName + "'";

                SqlCommand cmdDBExist = new SqlCommand(sqlText, currConn);
                transResult = (int)cmdDBExist.ExecuteScalar();
                if (transResult > 0)
                {
                    throw new ArgumentNullException("DeleteDB", MessageVM.dbMsgDBExist);
                }

                #endregion check Database

                #region CreateDatabase

                sqlText = "";
                sqlText += " USE [master]";
                sqlText += " CREATE DATABASE " + databaseName + "";

                SqlCommand cmdIdExist = new SqlCommand(sqlText, currConn);
                transResult = (int)cmdIdExist.ExecuteNonQuery();
                if (transResult != -1)
                {
                    throw new ArgumentNullException("Create Database('" + databaseName + "')", MessageVM.dbMsgDBNotCreate);
                }

                #endregion CreateDatabase

                #region Change Database for New DB
                currConn.ChangeDatabase(databaseName);
                transaction = currConn.BeginTransaction(MessageVM.dbMsgMethodName);
                #endregion open connection and transaction

                #region TableCreate
                string top1;

                #region CreateTable Back
                //              sqlText = @"
                //
                //";
                #endregion CreateTable
                #region CreateTable Back
                sqlText = @"
CREATE TABLE [dbo].[AccountTypes](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Asset]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Asset](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsVehicle] [bit] NULL,
	[RegNo] [nvarchar](100) NULL,
	[EngineNo] [nvarchar](100) NULL,
	[ChassisNo] [nvarchar](100) NULL,
	[Model] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bank]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Bank](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankBranchs]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[BankBranchs](
	[Id] [int] NOT NULL,
	[BankId] [int] NOT NULL,
	[BranchName] [nvarchar](200) NOT NULL,
	[BranchAddress] [nvarchar](200) NOT NULL,
	[BankAccountType] [nvarchar](200) NOT NULL,
	[BankAccountNo] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankCharge]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[BankCharge](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](20) NULL,
	[BankBranchId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NOT NULL,
	[TotalValue] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_BankCharge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankNames]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[BankNames](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Branch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [varchar](500) NULL,
	[District] [nvarchar](200) NULL,
	[Division] [nvarchar](200) NULL,
	[Country] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[PostalCode] [varchar](50) NULL,
	[Phone] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Fax] [nvarchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COAGroups]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[COAGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupSL] [int] NULL,
	[Code] [varchar](500) NULL,
	[Name] [varchar](500) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[GroupType] [varchar](100) NULL,
	[ReportType] [varchar](100) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
	[COATypeOfReportId] [int] NULL,
	[COAGroupTypeId] [int] NULL,
	[GroupNature] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COAs]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[COAs](
	[Id] [int] NOT NULL,
	[COASL] [int] NULL,
	[COAGroupId] [int] NULL,
	[Code] [varchar](500) NULL,
	[Name] [varchar](500) NULL,
	[Nature] [varchar](10) NULL,
	[COAType] [varchar](100) NULL,
	[ReportType] [varchar](100) NULL,
	[OpeningBalance] [decimal](18, 2) NULL,
	[TransType] [varchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsRetainedEarning] [bit] NULL,
	[IsNetProfit] [bit] NULL,
	[IsDepreciation] [bit] NULL,
	[TransactionType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COAsBackup]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[COAsBackup](
	[Id] [int] NOT NULL,
	[COASL] [int] NULL,
	[COAGroupId] [int] NULL,
	[Code] [varchar](500) NULL,
	[Name] [varchar](500) NULL,
	[Nature] [varchar](10) NULL,
	[OpeningBalance] [decimal](18, 2) NULL,
	[TransType] [varchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsRetainedEarning] [bit] NULL,
	[TransactionType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COAType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[COAType](
	[Id] [int] NOT NULL,
	[Name] [varchar](500) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_COAGroupType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COATypeOfReport]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[COATypeOfReport](
	[Id] [int] NOT NULL,
	[TypeOfReportSL] [int] NULL,
	[TypeOfReportShortName] [varchar](500) NULL,
	[Name] [varchar](500) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_COATypeOfReport] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodeGenerations]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[CodeGenerations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CYear] [int] NULL,
	[TransactionTypeGroup] [nvarchar](50) NULL,
	[TransactionType] [nvarchar](50) NULL,
	[Prefix] [nvarchar](50) NULL,
	[LastNumber] [int] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_CodeGenerations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [varchar](500) NULL,
	[District] [nvarchar](200) NULL,
	[Division] [nvarchar](200) NULL,
	[Country] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[PostalCode] [varchar](50) NULL,
	[Phone] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Fax] [nvarchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TaxId] [nvarchar](50) NULL,
	[RegistrationNumber] [nvarchar](50) NULL,
	[Mail] [nvarchar](50) NULL,
	[NumberOfEmployees] [int] NOT NULL,
	[YearStart] [nvarchar](20) NULL,
	[Year] [nvarchar](20) NULL,
	[VATNo] [nvarchar](20) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Department](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[OrderNo] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Designation](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[AttendenceBonus] [decimal](18, 2) NULL,
	[EPZ] [decimal](18, 2) NULL,
	[Other] [decimal](18, 2) NULL,
	[DinnerAmount] [decimal](18, 2) NULL,
	[IfterAmount] [decimal](18, 2) NULL,
	[TiffinAmount] [decimal](18, 2) NULL,
	[ETiffinAmount] [decimal](18, 2) NULL,
	[OTAlloawance] [bit] NULL,
	[OTOrginal] [bit] NULL,
	[OTBayer] [bit] NULL,
	[ExtraOT] [bit] NULL,
	[PriorityLevel] [int] NULL,
	[OrderNo] [int] NULL,
	[DesignationGroupId] [nvarchar](20) NULL,
	[GradeId] [nvarchar](50) NULL,
	[HospitalPlanC1] [nvarchar](50) NULL,
	[HospitalPlanC2] [nvarchar](50) NULL,
	[HospitalPlanC3] [nvarchar](50) NULL,
	[HospitalPlanC4] [nvarchar](50) NULL,
	[HospitalPlanC5] [nvarchar](50) NULL,
	[DeathCoveragePlanC6] [nvarchar](50) NULL,
	[MaternityPlanC7] [nvarchar](50) NULL,
	[MaternityPlanC8] [nvarchar](50) NULL,
	[MaternityPlanC9] [nvarchar](50) NULL,
	[EntitlementC1] [nvarchar](50) NULL,
	[EntitlementC2] [nvarchar](50) NULL,
	[EntitlementC3] [nvarchar](50) NULL,
	[EntitlementC4] [nvarchar](50) NULL,
	[EntitlementC5] [nvarchar](50) NULL,
	[MobileExpenseC1] [nvarchar](50) NULL,
	[MobileExpenseC2] [nvarchar](50) NULL,
	[MobileExpenseC3] [nvarchar](50) NULL,
	[MobileExpenseC4] [nvarchar](50) NULL,
	[InternationalTravelC1] [nvarchar](50) NULL,
	[InternationalTravelC2] [nvarchar](50) NULL,
	[InternationalTravelC3] [nvarchar](50) NULL,
	[DomesticlTravelC1] [nvarchar](50) NULL,
	[DomesticTravelC2] [nvarchar](50) NULL,
	[DomesticTravelC3] [nvarchar](50) NULL,
	[DomesticTravelC4] [nvarchar](50) NULL,
	[DomesticTravelC5] [nvarchar](50) NULL,
 CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignationGroup]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[DesignationGroup](
	[Id] [nvarchar](20) NOT NULL,
	[Serial] [int] NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnLeaveEncashmentStatement]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EarnLeaveEncashmentStatement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nchar](10) NULL,
	[Year] [int] NULL,
	[EncashmentBalance] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[EncashmentRatio] [decimal](18, 2) NULL,
 CONSTRAINT [PK_EarnLeaveEncashmentStatement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EarnLeaveStatement]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EarnLeaveStatement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nchar](10) NULL,
	[Year] [int] NULL,
	[CFBalance] [decimal](18, 2) NULL,
	[AnnualLeaveEntitle] [decimal](18, 2) NULL,
	[AnnualLeaveTaken] [decimal](18, 2) NULL,
	[AnnualBalance] [decimal](18, 2) NULL,
 CONSTRAINT [PK_EarnLeaveStatement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EEHeads]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EEHeads](
	[Id] [int] NOT NULL,
	[Name] [varchar](120) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EETransactionDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EETransactionDetails](
	[Id] [int] NOT NULL,
	[BranchId] [int] NULL,
	[SL] [int] NULL,
	[EETransactionId] [int] NOT NULL,
	[EEHeadId] [int] NULL,
	[TransactionDateTime] [nvarchar](14) NULL,
	[SubTotal] [decimal](25, 9) NULL,
	[ReferenceNo1] [varchar](50) NULL,
	[ReferenceNo2] [varchar](50) NULL,
	[ReferenceNo3] [varchar](50) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsPS] [bit] NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EETransactions]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EETransactions](
	[Id] [int] NOT NULL,
	[BranchId] [int] NULL,
	[Code] [varchar](20) NOT NULL,
	[EEHeadId] [int] NULL,
	[TransactionDateTime] [nvarchar](14) NULL,
	[GrandTotal] [decimal](25, 9) NULL,
	[ReferenceNo1] [varchar](50) NULL,
	[ReferenceNo2] [varchar](50) NULL,
	[ReferenceNo3] [varchar](50) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsPS] [bit] NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeBreakMonthPF]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeBreakMonthPF](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmployeeBreakMonthContribution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeCompensatoryLeave]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeCompensatoryLeave](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[LeaveYear] [int] NOT NULL,
	[LeaveType_E] [nvarchar](200) NOT NULL,
	[FromDate] [nvarchar](14) NOT NULL,
	[ToDate] [nvarchar](14) NOT NULL,
	[TotalLeave] [decimal](18, 1) NOT NULL,
	[ApprovedBy] [nvarchar](20) NULL,
	[ApproveDate] [nvarchar](14) NULL,
	[IsApprove] [bit] NOT NULL,
	[RejectedBy] [nvarchar](20) NULL,
	[RejectDate] [nvarchar](14) NULL,
	[IsReject] [bit] NULL,
	[IsHalfDay] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmployeeCompensatoryLeave_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeFile]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[EmployeePersonalDetail_NIDFile] [nvarchar](150) NULL,
	[EmployeePersonalDetail_PassportFile] [nvarchar](150) NULL,
	[EmployeePersonalDetail_Fingerprint] [nvarchar](150) NULL,
	[EmployeePersonalDetail_VaccineFile1] [nvarchar](150) NULL,
	[EmployeePersonalDetail_VaccineFiles2] [nvarchar](150) NULL,
	[EmployeePersonalDetail_VaccineFile3] [nvarchar](150) NULL,
	[EmployeeInfo_PhotoName] [nvarchar](150) NULL,
	[EmployeePersonalDetail_DisabilityFile] [nvarchar](150) NULL,
	[EmployeePersonalDetail_Signature] [nvarchar](150) NULL,
	[EmployeeNominee_VaccineFile1] [nvarchar](150) NULL,
	[EmployeeNominee_VaccineFile2] [nvarchar](150) NULL,
	[EmployeeNominee_VaccineFile3] [nvarchar](150) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsArchive] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[EmployeePersonalDetail_TINFiles] [nvarchar](150) NULL,
	[SignatureFiles] [nvarchar](150) NULL,
	[FileName] [nvarchar](150) NULL,
	[Employeedependent_VaccineFile3] [nvarchar](150) NULL,
	[Employeedependent_VaccineFile2] [nvarchar](150) NULL,
	[Employeedependent_VaccineFile1] [nvarchar](150) NULL,
	[Extra_FileName] [nvarchar](150) NULL,
	[Experience_Certificate] [nvarchar](150) NULL,
	[Lng_Achivement] [nvarchar](150) NULL,
	[edu_Certificate] [nvarchar](150) NULL,
	[PassportVisa] [nvarchar](150) NULL,
	[BillVoucher] [nvarchar](150) NULL,
	[AssetFileName] [nvarchar](150) NULL,
	[Certificate] [nvarchar](150) NULL,
 CONSTRAINT [PK_EmployeeFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeForfeiture]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeForfeiture](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmployeeForfeiture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeForFeiture_New]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeForFeiture_New](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[ForFeitureDate] [nvarchar](14) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeInfo]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Department] [nvarchar](200) NULL,
	[Designation] [nvarchar](200) NULL,
	[Project] [nvarchar](200) NULL,
	[Section] [nvarchar](150) NULL,
	[DateOfBirth] [nvarchar](150) NULL,
	[JoinDate] [nvarchar](150) NULL,
	[ResignDate] [nvarchar](150) NULL,
	[Remarks] [nvarchar](150) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NULL,
	[CreatedAt] [nvarchar](14) NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PhotoName] [nvarchar](50) NULL,
	[NomineeDateofBirth] [nvarchar](14) NULL,
	[NomineeName] [varchar](500) NULL,
	[NomineeRelation] [nvarchar](200) NULL,
	[NomineeAddress] [varchar](500) NULL,
	[NomineeDistrict] [nvarchar](200) NULL,
	[NomineeDivision] [nvarchar](200) NULL,
	[NomineeCountry] [nvarchar](200) NULL,
	[NomineeCity] [nvarchar](200) NULL,
	[NomineePostalCode] [varchar](50) NULL,
	[NomineePhone] [nvarchar](100) NULL,
	[NomineeMobile] [nvarchar](100) NULL,
	[NomineeBirthCertificateNo] [nvarchar](50) NULL,
	[NomineeFax] [nvarchar](100) NULL,
	[NomineeFileName] [nchar](50) NULL,
	[NomineeRemarks] [nvarchar](500) NULL,
	[NomineeNID] [nvarchar](50) NULL,
	[GrossSalary] [decimal](18, 2) NULL,
	[BasicSalary] [decimal](18, 2) NULL,
	[LeftDate] [nvarchar](14) NULL,
	[Grade] [nvarchar](200) NULL,
	[Branch] [nvarchar](200) NULL,
	[ProjectId] [nvarchar](20) NULL,
	[SectionId] [nvarchar](20) NULL,
	[DepartmentId] [nvarchar](20) NULL,
	[DesignationId] [nvarchar](20) NULL,
	[Other1] [nvarchar](200) NULL,
	[EmployeeId] [nvarchar](20) NULL,
	[EmpName] [nvarchar](601) NULL,
	[Email] [nvarchar](50) NULL,
	[ContactNo] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[IsNoProfit] [bit] NULL,
 CONSTRAINT [PK_EmployeeInfoForPF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeLoan]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeLoan](
	[Id] [varchar](50) NOT NULL,
	[BranchId] [int] NOT NULL,
	[LoanType_E] [nvarchar](200) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[PrincipalAmount] [decimal](18, 3) NOT NULL,
	[IsFixed] [bit] NOT NULL,
	[InterestPolicy] [varchar](50) NULL,
	[InterestRate] [decimal](18, 3) NULL,
	[InterestAmount] [decimal](18, 3) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[NumberOfInstallment] [int] NOT NULL,
	[ApprovedDate] [nvarchar](14) NULL,
	[StartDate] [nvarchar](14) NOT NULL,
	[EndDate] [nvarchar](14) NOT NULL,
	[ApplicationDate] [nvarchar](14) NULL,
	[IsApproved] [bit] NULL,
	[IsHold] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[RefundAmount] [decimal](18, 2) NULL,
	[RefundDate] [nvarchar](14) NULL,
	[PayrollProcessDate] [varchar](100) NULL,
	[IsEarlySellte] [bit] NULL,
	[EarlySellteDate] [varchar](100) NULL,
	[EarlySelltePrincipleAmount] [decimal](18, 2) NULL,
	[EarlySellteInterestAmount] [decimal](18, 2) NULL,
	[LoanNo] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeLoanDetail]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeLoanDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeLoanId] [varchar](50) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[InstallmentAmount] [decimal](18, 2) NOT NULL,
	[InstallmentPaidAmount] [decimal](18, 2) NOT NULL,
	[PaymentScheduleDate] [nvarchar](20) NOT NULL,
	[PaymentDate] [nvarchar](20) NULL,
	[IsHold] [bit] NOT NULL,
	[IsManual] [bit] NULL,
	[IsPaid] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PrincipalAmount] [decimal](18, 3) NOT NULL,
	[InterestAmount] [decimal](18, 3) NOT NULL,
	[HaveDuplicate] [bit] NULL,
	[DuplicateID] [int] NULL,
	[InstallmentSLNo] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeePFOpeinig]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeePFOpeinig](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmployeePFOpeinig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeePFPayment]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeePFPayment](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[PaymentDate] [nvarchar](14) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[FiscalYearDetailId] [int] NULL,
 CONSTRAINT [PK_EmployeePFPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeProfessionalDegree]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeProfessionalDegree](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[Degree_E] [nvarchar](200) NOT NULL,
	[Institute] [nvarchar](500) NULL,
	[YearOfPassing] [nvarchar](4) NULL,
	[IsLast] [bit] NULL,
	[FileName] [nchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[Marks] [numeric](18, 2) NULL,
	[TotalYear] [int] NULL,
	[Level] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmployeeProfessionalDegree] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeTransfer]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EmployeeTransfer](
	[Id] [nvarchar](50) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[IsCurrent] [bit] NOT NULL,
	[FileName] [nchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransferDate] [nvarchar](14) NULL,
	[Other1] [nvarchar](200) NULL,
	[Other2] [nvarchar](200) NULL,
	[Other3] [nvarchar](200) NULL,
	[Other4] [nvarchar](200) NULL,
	[Other5] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumDistrict]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumDistrict](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Country_E] [nvarchar](200) NOT NULL,
	[Division_E] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnumDistrict] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumDivision]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumDivision](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Country_E] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnumDivision] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumEMPCategory]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumEMPCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnumEMPCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumEmpJobType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumEmpJobType](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnumEmpJobType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumInvestmentTypes]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumInvestmentTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumJournalTransactionType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumJournalTransactionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[NameTrim] [nvarchar](200) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_EnumJournalTransactionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumJournalType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumJournalType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActice] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_JournalEntryType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumLeaveType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumLeaveType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsWithoutPay] [bit] NULL,
	[LType] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsRegular] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumLoanType]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumLoanType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[GLAccountCode] [varchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsInterest] [bit] NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumOderBy]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumOderBy](
	[Id] [nvarchar](200) NOT NULL,
	[Module] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumProfessionalDegree]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumProfessionalDegree](
	[Id] [int] NOT NULL,
	[ProfessionalDegrees] [nvarchar](200) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnumProfessionalDegree] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumReport]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[EnumReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReportId] [varchar](50) NULL,
	[ReportName] [varchar](500) NULL,
	[ReportType] [varchar](500) NULL,
	[ReportFileName] [varchar](500) NULL,
	[IsVisible] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[ReportSL] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FiscalYear]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[FiscalYear](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[YearStart] [nvarchar](20) NULL,
	[YearEnd] [nvarchar](20) NULL,
	[YearLock] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FiscalYearDetail]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[FiscalYearDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FiscalYearId] [nvarchar](20) NOT NULL,
	[Year] [int] NOT NULL,
	[PeriodName] [varchar](50) NULL,
	[PeriodStart] [nvarchar](20) NULL,
	[PeriodEnd] [nvarchar](20) NULL,
	[PeriodLock] [bit] NULL,
	[PayrollLock] [bit] NULL,
	[PFLock] [bit] NULL,
	[TAXLock] [bit] NULL,
	[LoanLock] [bit] NULL,
	[SagePostComplete] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[PeriodId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ForfeitureAccounts]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ForfeitureAccounts](
	[Id] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[ForfeitDate] [nvarchar](14) NOT NULL,
	[ForfeitValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsTransferPDF] [bit] NULL,
	[Post] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TotalForfeitValue] [decimal](18, 2) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeeBreakMonth]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeeBreakMonth](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFEmployeeBreakMonth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeeForfeiture]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeeForfeiture](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFEmployeeForfeiture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeeOpeinig]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeeOpeinig](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[OpeningDate] [nvarchar](14) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFEmployeeOpeinig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeePayment]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeePayment](
	[Id] [int] NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[PaymentDate] [nvarchar](14) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[FiscalYearDetailId] [int] NULL,
 CONSTRAINT [PK_GFEmployeePayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeeProvisions]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeeProvisions](
	[Id] [int] NOT NULL,
	[GFHeaderId] [int] NULL,
	[FiscalYearDetailId] [int] NULL,
	[EmployeeId] [nvarchar](20) NULL,
	[ProjectId] [nvarchar](20) NULL,
	[DepartmentId] [nvarchar](20) NULL,
	[SectionId] [nvarchar](20) NULL,
	[DesignationId] [nvarchar](20) NULL,
	[JoinDate] [nvarchar](14) NULL,
	[GrossAmount] [decimal](18, 2) NULL,
	[BasicAmount] [decimal](18, 2) NULL,
	[ProvisionAmount] [decimal](18, 2) NULL,
	[IncrementArrear] [decimal](18, 2) NULL,
	[GFPolicyId] [int] NULL,
	[MultipicationFactor] [decimal](18, 2) NULL,
	[JobMonth] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[Post] [bit] NULL,
	[FiscalYearDetailStartDate] [nvarchar](20) NULL,
	[IsBreakMonth] [bit] NULL,
 CONSTRAINT [PK_GFEmployeeProvisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFEmployeeSettlements]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFEmployeeSettlements](
	[Id] [int] NOT NULL,
	[GFPolicyId] [int] NOT NULL,
	[PolicyJobDurationYearFrom] [int] NULL,
	[PolicyJobDurationYearTo] [int] NULL,
	[PolicyMultipicationFactor] [decimal](18, 2) NULL,
	[PolicyIsFixed] [bit] NULL,
	[PolicyLastBasicMultipication] [decimal](18, 2) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[JoinDate] [nvarchar](14) NOT NULL,
	[LeftDate] [nvarchar](14) NOT NULL,
	[TotalJobDurationYear] [int] NULL,
	[LastGross] [decimal](18, 2) NOT NULL,
	[LastBasic] [decimal](18, 2) NOT NULL,
	[SettlementDate] [nvarchar](14) NOT NULL,
	[GFValue] [decimal](18, 2) NOT NULL,
	[ServiceCharge] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFHeader]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFHeader](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[ProvisionAmount] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFPolicies]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFPolicies](
	[Id] [int] NOT NULL,
	[PolicyName] [nvarchar](200) NOT NULL,
	[JobDurationYearFrom] [int] NULL,
	[JobDurationYearTo] [int] NULL,
	[MultipicationFactor] [decimal](18, 2) NULL,
	[IsFixed] [bit] NULL,
	[LastBasicMultipication] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFPolicies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GFProfitDistributionNew]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GFProfitDistributionNew](
	[Id] [int] NOT NULL,
	[GFPreDistributionFundId] [nvarchar](200) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[MultiplicationFactor] [decimal](18, 9) NULL,
	[EmployeerProfitDistribution] [decimal](18, 2) NULL,
	[TotalProfit] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[IsPaid] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
 CONSTRAINT [PK_GFProfitDistributionNew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GLJournalDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GLJournalDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GLJournalId] [int] NULL,
	[COAId] [int] NULL,
	[TransactionDate] [int] NULL,
	[TransactionType] [int] NULL,
	[JournalType] [int] NULL,
	[IsDr] [bit] NULL,
	[DrAmount] [decimal](18, 3) NULL,
	[CrAmount] [decimal](18, 3) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransType] [varchar](100) NULL,
	[IsYearClosing] [bit] NULL,
 CONSTRAINT [PK_GLJournalDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GLJournals]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[GLJournals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[JournalType] [int] NULL,
	[TransactionType] [int] NULL,
	[TransactionValue] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL,
	[IsYearClosing] [bit] NULL,
 CONSTRAINT [PK_GLJournals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grade]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Grade](
	[Id] [nvarchar](20) NOT NULL,
	[SL] [int] NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[MinSalary] [decimal](18, 2) NULL,
	[MaxSalary] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsHouseRentFactorFromBasic] [bit] NULL,
	[IsTAFactorFromBasic] [bit] NULL,
	[TAFactor] [bit] NULL,
	[IsMedicalFactorFromBasic] [bit] NULL,
	[Area] [nvarchar](20) NULL,
	[GradeNo] [int] NULL,
	[CurrentBasic] [decimal](18, 5) NULL,
	[BasicNextYearFactor] [decimal](18, 5) NULL,
	[BasicNextStepFactor] [decimal](18, 5) NULL,
	[HouseRentFactor] [decimal](18, 5) NULL,
	[MedicalFactor] [decimal](18, 5) NULL,
	[IsFixedHouseRent] [bit] NULL,
	[HouseRentAllowance] [decimal](18, 5) NULL,
	[IsFixedSpecialAllowance] [bit] NULL,
	[SpecialAllowance] [decimal](18, 5) NULL,
	[LowerLimit] [decimal](18, 5) NULL,
	[MedianLimit] [decimal](18, 5) NULL,
	[UpperLimit] [decimal](18, 5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestmentAccrued]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[InvestmentAccrued](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvestmentNameId] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NOT NULL,
	[ReferenceNo] [nvarchar](500) NOT NULL,
	[InvestmentValue] [decimal](18, 2) NOT NULL,
	[AccruedMonth] [int] NULL,
	[InterestRate] [decimal](18, 2) NOT NULL,
	[AccruedInterest] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
	[AitInterest] [decimal](18, 2) NULL,
	[NetInterest] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvestmentsAccrued] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestmentDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[InvestmentDetails](
	[Id] [int] NOT NULL,
	[InvestmentId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[AccountId] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_InvestmentDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestmentNameDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[InvestmentNameDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvestmentNameId] [int] NOT NULL,
	[FromMonth] [nvarchar](14) NOT NULL,
	[ToMonth] [nvarchar](14) NOT NULL,
	[InterestRate] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_InvestmentNameDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestmentNames]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[InvestmentNames](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[Code] [nvarchar](200) NULL,
	[InvestmentTypeId] [int] NULL,
	[InvestmentDate] [nvarchar](14) NULL,
	[FromDate] [nvarchar](14) NULL,
	[ToDate] [nvarchar](14) NULL,
	[MaturityDate] [nvarchar](14) NULL,
	[BankBranchId] [int] NULL,
	[BankNameId] [int] NULL,
	[FiscalYearDetailId] [int] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
	[AitInterest] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvestmentNames] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestmentRenew]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[InvestmentRenew](
	[Id] [int] NOT NULL,
	[InvestmentId] [int] NOT NULL,
	[TransactionCode] [nvarchar](500) NULL,
	[InvestmentDate] [nvarchar](14) NOT NULL,
	[ReferenceNo] [nvarchar](500) NOT NULL,
	[FromDate] [nvarchar](14) NOT NULL,
	[ToDate] [nvarchar](14) NOT NULL,
	[MaturityDate] [nvarchar](14) NOT NULL,
	[InvestmentValue] [decimal](18, 2) NOT NULL,
	[BankCharge] [decimal](18, 2) NOT NULL,
	[BankExciseDuty] [decimal](18, 2) NULL,
	[Interest] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsEncashed] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
	[SourceTaxDeduct] [decimal](18, 2) NULL,
	[OtherCharge] [decimal](18, 2) NULL,
	[AditionAmount] [decimal](18, 2) NULL,
	[EncashAmount] [decimal](18, 2) NULL,
	[InterestRate] [decimal](18, 2) NULL,
	[AIT] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InvestmentRenew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Investments]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Investments](
	[Id] [int] NOT NULL,
	[TransactionCode] [nvarchar](500) NULL,
	[InvestmentTypeId] [int] NULL,
	[TransactionType] [nvarchar](500) NULL,
	[ReferenceNo] [nvarchar](500) NULL,
	[InvestmentAddress] [nvarchar](500) NULL,
	[InvestmentDate] [nvarchar](14) NOT NULL,
	[FromDate] [nvarchar](14) NOT NULL,
	[ToDate] [nvarchar](14) NOT NULL,
	[MaturityDate] [nvarchar](14) NOT NULL,
	[InvestmentRate] [decimal](18, 2) NOT NULL,
	[InvestmentValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[InvestmentNameId] [int] NULL,
	[ReferenceId] [varchar](500) NULL,
	[IsEncashed] [bit] NULL,
	[TransType] [varchar](100) NULL,
	[BankCharge] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Investments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveSchedule]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[LeaveSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [varchar](100) NULL,
	[LeaveDate] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loan]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Loan](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[Amount] [decimal](18, 2) NULL,
	[InterestAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_Loan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanMonthlyPayment]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[LoanMonthlyPayment](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[Amount] [decimal](18, 2) NULL,
	[InterestAmount] [decimal](18, 2) NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_LoanMonthlyPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanRepaymentToBank]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[LoanRepaymentToBank](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[Amount] [decimal](18, 2) NULL,
	[InterestAmount] [decimal](18, 2) NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_LoanRepaymentToBank] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanSattlement]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[LoanSattlement](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[Amount] [decimal](18, 2) NULL,
	[InterestAmount] [decimal](18, 2) NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_LoanSattlement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetProfitGFYearEnds]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[NetProfitGFYearEnds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransType] [nvarchar](50) NULL,
	[Year] [varchar](50) NULL,
	[YearStart] [varchar](50) NULL,
	[YearEnd] [varchar](50) NULL,
	[COAId] [int] NULL,
	[COAType] [varchar](50) NULL,
	[TransactionAmount] [decimal](18, 4) NULL,
	[NetProfit] [decimal](18, 4) NULL,
	[RetainedEarning] [decimal](18, 4) NULL,
 CONSTRAINT [PK_NetProfitGFYearEnds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetProfitGFYearEnds1]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[NetProfitGFYearEnds1](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransType] [nvarchar](50) NULL,
	[Year] [varchar](50) NULL,
	[YearStart] [varchar](50) NULL,
	[YearEnd] [varchar](50) NULL,
	[COAId] [int] NULL,
	[COAType] [varchar](50) NULL,
	[TransactionAmount] [decimal](18, 4) NULL,
	[NetProfit] [decimal](18, 4) NULL,
	[RetainedEarning] [decimal](18, 4) NULL,
 CONSTRAINT [pk_NetProfitGFYearEnds1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetProfitYearEnds]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[NetProfitYearEnds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransType] [nvarchar](50) NULL,
	[Year] [varchar](50) NULL,
	[YearStart] [varchar](50) NULL,
	[YearEnd] [varchar](50) NULL,
	[COAId] [int] NULL,
	[COAType] [varchar](50) NULL,
	[TransactionAmount] [decimal](18, 4) NULL,
	[NetProfit] [decimal](18, 4) NULL,
	[RetainedEarning] [decimal](18, 4) NULL,
 CONSTRAINT [PK_NetProfitYearEnds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[O_GLTransactionDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[O_GLTransactionDetails](
	[Id] [int] NOT NULL,
	[TransactionCode] [nvarchar](50) NULL,
	[TransactionMasterId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[TransactionType] [varchar](50) NULL,
	[AccountId] [int] NOT NULL,
	[IsDr] [bit] NULL,
	[IsSingle] [bit] NULL,
	[DebitAmount] [decimal](25, 9) NULL,
	[CreditAmount] [decimal](25, 9) NULL,
	[TransactionAmount] [decimal](18, 2) NOT NULL,
	[DrAccountIdForCredit] [int] NULL,
	[CrAccountIdForDebit] [int] NULL,
	[Post] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PostedBy] [nvarchar](20) NULL,
	[PostedAt] [nvarchar](14) NULL,
	[PostedFrom] [nvarchar](50) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_GLTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[O_ROIDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[O_ROIDetails](
	[Id] [int] NOT NULL,
	[ROIId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[AccountId] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[o_WithdrawDetails1]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[o_WithdrawDetails1](
	[Id] [int] NOT NULL,
	[WithdrawId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[AccountId] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[o_WithdrawTypes]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[o_WithdrawTypes](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[AccountType] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OProfitDistributionOfBankInterests]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[OProfitDistributionOfBankInterests](
	[Id] [int] NOT NULL,
	[ROBIId] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[TotalInterestValue] [decimal](18, 2) NOT NULL,
	[SelfInterestValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OProfitDistributionOfInvestments]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[OProfitDistributionOfInvestments](
	[Id] [int] NOT NULL,
	[ROIId] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[TotalProfitValue] [decimal](18, 2) NOT NULL,
	[SelfProfitValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OProfitDistributionOfReservedfunds]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[OProfitDistributionOfReservedfunds](
	[Id] [int] NOT NULL,
	[RFIId] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[TotalValue] [decimal](18, 2) NOT NULL,
	[SelfValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFBankDepositDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFBankDepositDetails](
	[Id] [int] NOT NULL,
	[PFBankDepositId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[AccountId] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PFBankDepositDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFBankDeposits]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFBankDeposits](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[FiscalYearDetailId] [int] NULL,
	[DepositAmount] [decimal](18, 2) NULL,
	[TotalEmployeePFValue] [decimal](18, 2) NULL,
	[TotalEmployeerPFValue] [decimal](18, 2) NULL,
	[DepositDate] [nvarchar](14) NULL,
	[BankBranchId] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[TransactionMediaId] [nvarchar](200) NULL,
	[ReferenceId] [int] NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PFBankDeposits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PFHeaderId] [int] NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[PFStructureId] [nvarchar](20) NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[EmployeePFValue] [decimal](18, 2) NOT NULL,
	[EmployeerPFValue] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[GrossSalary] [decimal](18, 2) NOT NULL,
	[IsDistribute] [bit] NULL,
	[IsBankDeposited] [bit] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PFDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFHeader]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFHeader](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[EmployeePFValue] [decimal](18, 2) NOT NULL,
	[EmployeerPFValue] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PFHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFLoanDetail]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFLoanDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeLoanId] [varchar](50) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[InstallmentAmount] [decimal](18, 2) NOT NULL,
	[InstallmentPaidAmount] [decimal](18, 2) NOT NULL,
	[PaymentScheduleDate] [nvarchar](20) NOT NULL,
	[PaymentDate] [nvarchar](20) NULL,
	[IsHold] [bit] NOT NULL,
	[IsManual] [bit] NULL,
	[IsPaid] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PrincipalAmount] [decimal](18, 3) NOT NULL,
	[InterestAmount] [decimal](18, 3) NOT NULL,
	[HaveDuplicate] [bit] NULL,
	[DuplicateID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFOpeningTIB]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFOpeningTIB](
	[SL] [int] NOT NULL,
	[EIN] [nvarchar](255) NULL,
	[EmployeeId] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[DOJ] [nvarchar](255) NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PFOpeningTIB] PRIMARY KEY CLUSTERED 
(
	[SL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFSettlementDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFSettlementDetails](
	[Id] [int] NOT NULL,
	[PFSettlementId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NULL,
	[AccountId] [int] NOT NULL,
	[DebitAmount] [decimal](18, 2) NULL,
	[CreditAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Post] [bit] NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PFSettlements]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PFSettlements](
	[Id] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[TransactionCode] [nvarchar](50) NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[EmployeeProfitValue] [decimal](18, 2) NOT NULL,
	[EmployerProfitValue] [decimal](18, 2) NOT NULL,
	[EmployeeTotalContribution] [decimal](18, 2) NULL,
	[EmployerTotalContribution] [decimal](18, 2) NULL,
	[EmpDOJ] [nvarchar](20) NOT NULL,
	[EmpResignDate] [nvarchar](20) NOT NULL,
	[SettlementDate] [nvarchar](20) NOT NULL,
	[SettlementPolicyId] [int] NOT NULL,
	[JobAgeInMonth] [decimal](18, 2) NOT NULL,
	[EmployeeContributionRatio] [decimal](18, 2) NULL,
	[EmployerContributionRatio] [decimal](18, 2) NULL,
	[EmployeeProfitRatio] [decimal](18, 2) NULL,
	[EmployerProfitRatio] [decimal](18, 2) NULL,
	[EmployeeActualContribution] [decimal](18, 2) NULL,
	[EmployerActualContribution] [decimal](18, 2) NULL,
	[EmployeeActualProfitValue] [decimal](18, 2) NULL,
	[EmployerActualProfitValue] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TotalPayableAmount] [decimal](18, 2) NULL,
	[AlreadyPaidAmount] [decimal](18, 2) NULL,
	[NetPayAmount] [decimal](18, 2) NULL,
	[PFStartDate] [nvarchar](14) NULL,
	[PFEndDate] [nvarchar](14) NULL,
	[EmployeeContributionForfeitValue] [decimal](18, 2) NULL,
	[EmployeeProfitForfeitValue] [decimal](18, 2) NULL,
	[EmployerContributionForfeitValue] [decimal](18, 2) NULL,
	[EmployerProfitForfeitValue] [decimal](18, 2) NULL,
	[TotalForfeitValue] [decimal](18, 2) NULL,
	[ProvidentFundAmount] [decimal](18, 2) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PreDistributionFunds]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[PreDistributionFunds](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](500) NOT NULL,
	[TransactionDate] [nvarchar](14) NOT NULL,
	[TotalValue] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsDistribute] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_PreDistributionFunds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfitDistributionDetails]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ProfitDistributionDetails](
	[Id] [int] NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[ProfitDistributionId] [int] NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[EmployeeProfitValue] [decimal](18, 2) NOT NULL,
	[EmployerProfitValue] [decimal](18, 2) NOT NULL,
	[EmployeeTotalContribution] [decimal](18, 2) NULL,
	[EmployerTotalContribution] [decimal](18, 2) NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsPaid] [bit] NULL,
	[FiscalYearDetailIdTo] [int] NULL,
	[IndividualTotalContribution] [decimal](18, 2) NULL,
	[ServiceLengthMonthWeight] [decimal](18, 2) NULL,
	[IndividualWeightedContribution] [decimal](18, 2) NULL,
	[MultiplicationFactor] [decimal](18, 9) NULL,
	[IndividualProfitValue] [decimal](18, 2) NULL,
	[ServiceLengthMonth] [decimal](18, 2) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfitDistributionNew]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ProfitDistributionNew](
	[Id] [int] NOT NULL,
	[PreDistributionFundId] [nvarchar](200) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[MultiplicationFactor] [decimal](18, 9) NULL,
	[EmployeeProfitDistribution] [decimal](18, 2) NULL,
	[EmployeerProfitDistribution] [decimal](18, 2) NULL,
	[TotalProfit] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[IsPaid] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_ProfitDistributionNew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfitDistributionNoProfit]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ProfitDistributionNoProfit](
	[Id] [int] NOT NULL,
	[PreDistributionFundId] [nvarchar](200) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[EmployeeContribution] [decimal](18, 2) NULL,
	[EmployerContribution] [decimal](18, 2) NULL,
	[EmployeeProfit] [decimal](18, 2) NULL,
	[EmployerProfit] [decimal](18, 2) NULL,
	[MultiplicationFactor] [decimal](18, 9) NULL,
	[EmployeeProfitDistribution] [decimal](18, 2) NULL,
	[EmployeerProfitDistribution] [decimal](18, 2) NULL,
	[TotalProfit] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[IsPaid] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfitDistributions]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ProfitDistributions](
	[Id] [int] NOT NULL,
	[PFDetailFiscalYearDetailIds] [nvarchar](200) NULL,
	[PreDistributionFundIds] [nvarchar](200) NOT NULL,
	[DistributionDate] [nvarchar](14) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[TotalEmployeeContribution] [decimal](18, 2) NULL,
	[TotalEmployerContribution] [decimal](18, 2) NULL,
	[TotalProfit] [decimal](18, 2) NOT NULL,
	[TransactionType] [varchar](50) NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsPaid] [bit] NULL,
	[FiscalYearDetailIdTo] [int] NULL,
	[TotalExpense] [decimal](18, 2) NULL,
	[AvailableDistributionAmount] [decimal](18, 2) NULL,
	[MultiplicationFactor] [decimal](18, 9) NULL,
	[TotalWeightedContribution] [decimal](18, 2) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Project](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Startdate] [nvarchar](14) NULL,
	[EndDate] [nvarchar](14) NULL,
	[ManpowerRequired] [int] NOT NULL,
	[ContactPerson] [nvarchar](500) NULL,
	[ContactPersonDesignation] [nvarchar](500) NULL,
	[Address] [varchar](500) NULL,
	[District] [nvarchar](200) NULL,
	[Division] [nvarchar](200) NULL,
	[Country] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[PostalCode] [varchar](50) NULL,
	[Phone] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Fax] [nvarchar](100) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [varchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [varchar](50) NULL,
	[OrderNo] [int] NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RawDataSummaryPF]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[RawDataSummaryPF](
	[EmployeeId] [nvarchar](255) NULL,
	[Code] [nvarchar](255) NULL,
	[PFAmount] [decimal](18, 2) NULL,
	[PeriodName] [nvarchar](255) NULL,
	[FiscalYearDetailId] [int] NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReservedFunds]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ReservedFunds](
	[Id] [int] NOT NULL,
	[ReservedDate] [nvarchar](14) NOT NULL,
	[ReservedValue] [decimal](18, 2) NOT NULL,
	[PDFId] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnOnBankInterests]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ReturnOnBankInterests](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](20) NULL,
	[BankBranchId] [int] NOT NULL,
	[TransactionDate] [nvarchar](14) NOT NULL,
	[TotalValue] [decimal](18, 2) NOT NULL,
	[Post] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL,
	[ActualInterestAmount] [decimal](18, 2) NULL,
	[ServiceChargeAmount] [decimal](18, 2) NULL,
	[IsBankDeposited] [bit] NULL,
 CONSTRAINT [PK_ReturnOnBankInterests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnOnInvestments]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[ReturnOnInvestments](
	[Id] [int] NOT NULL,
	[TransactionCode] [nvarchar](500) NULL,
	[TransactionType] [nvarchar](500) NULL,
	[ReferenceId] [nvarchar](500) NULL,
	[InvestmentId] [int] NOT NULL,
	[InvestmentTypeId] [int] NULL,
	[ROIDate] [nvarchar](14) NOT NULL,
	[ROIRate] [decimal](18, 2) NOT NULL,
	[ROITotalValue] [decimal](18, 2) NOT NULL,
	[TotalInterestValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[Post] [bit] NOT NULL,
	[IsTransferPDF] [bit] NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsFixed] [bit] NULL,
	[ActualInterestAmount] [decimal](18, 2) NULL,
	[ServiceChargeAmount] [decimal](18, 2) NULL,
	[IsBankDeposited] [bit] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_ReturnOnInvestments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalaryEmployee]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SalaryEmployee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[EmployeeStatus] [varchar](50) NULL,
	[GradeId] [nvarchar](20) NULL,
	[IsHold] [bit] NULL,
	[HoldBy] [nvarchar](20) NULL,
	[HoldAt] [nvarchar](14) NULL,
	[UnHoldBy] [nvarchar](20) NULL,
	[UnHoldAt] [nvarchar](14) NULL,
	[IsManual] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [varchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [varchar](50) NULL,
	[Other1] [nvarchar](100) NULL,
	[Other2] [nvarchar](100) NULL,
	[Other3] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalaryHeadMappings]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SalaryHeadMappings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SL] [int] NULL,
	[GroupType] [nvarchar](max) NULL,
	[FieldGroup] [nvarchar](max) NULL,
	[FieldName] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_SalaryHeadMappings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalaryPFDetail]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SalaryPFDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FiscalYearDetailId] [int] NOT NULL,
	[PFStructureId] [nvarchar](20) NOT NULL,
	[ProjectId] [nvarchar](20) NOT NULL,
	[DepartmentId] [nvarchar](20) NOT NULL,
	[SectionId] [nvarchar](20) NOT NULL,
	[DesignationId] [nvarchar](20) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[GradeId] [nvarchar](20) NULL,
	[PFValue] [decimal](18, 2) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[GrossSalary] [decimal](18, 2) NOT NULL,
	[EmployeeStatus] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Section]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Section](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[OrderNo] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [varchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [varchar](50) NULL,
 CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Setting](
	[Id] [varchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[SettingGroup] [varchar](120) NULL,
	[SettingName] [varchar](120) NULL,
	[SettingValue] [nvarchar](500) NULL,
	[SettingType] [varchar](120) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SettlementPolicies]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SettlementPolicies](
	[Id] [int] NOT NULL,
	[PolicyName] [nvarchar](500) NOT NULL,
	[JobAgeInMonth] [decimal](18, 2) NOT NULL,
	[EmployeeContributionRatio] [decimal](18, 2) NOT NULL,
	[EmployerContributionRatio] [decimal](18, 2) NULL,
	[EmployeeProfitRatio] [decimal](18, 2) NULL,
	[EmployerProfitRatio] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SymUserDefaultRoll]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SymUserDefaultRoll](
	[Id] [nvarchar](20) NOT NULL,
	[BranchId] [int] NOT NULL,
	[symArea] [nvarchar](100) NULL,
	[symController] [nvarchar](100) NULL,
	[IsIndex] [bit] NOT NULL,
	[IsAdd] [bit] NOT NULL,
	[IsEdit] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[IsReport] [bit] NOT NULL,
	[IsProcess] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SymUserRoll]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[SymUserRoll](
	[Id] [nvarchar](20) NOT NULL,
	[DefaultRollId] [nvarchar](20) NULL,
	[BranchId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsIndex] [bit] NOT NULL,
	[IsAdd] [bit] NOT NULL,
	[IsEdit] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[IsReport] [bit] NOT NULL,
	[IsProcess] [bit] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NULL,
	[CreatedAt] [nvarchar](14) NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TAX108ExEmployee]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TAX108ExEmployee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [nvarchar](20) NOT NULL,
	[basic] [decimal](18, 2) NULL,
	[gross] [decimal](18, 2) NULL,
	[Housing] [decimal](18, 2) NULL,
	[TA] [decimal](18, 2) NULL,
	[Medical] [decimal](18, 2) NULL,
	[ChildAllowance] [decimal](18, 2) NULL,
	[HardshipAllowance] [decimal](18, 2) NULL,
	[Overtime] [decimal](18, 2) NULL,
	[LeaveEncashment] [decimal](18, 2) NULL,
	[FestivalAllowance] [decimal](18, 2) NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TAXHeadMapping]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TAXHeadMapping](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HeadName] [varchar](500) NULL,
 CONSTRAINT [PK_TAXHeadMapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempNetChange]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TempNetChange](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RowSL] [int] NOT NULL,
	[AccountName] [nvarchar](500) NULL,
	[AccountCode] [nvarchar](500) NULL,
	[GroupName] [nvarchar](500) NULL,
	[GroupType] [nvarchar](500) NULL,
	[TypeOfReport] [nvarchar](500) NULL,
	[Nature] [nvarchar](2) NULL,
	[GLJournalDetailId] [int] NULL,
	[GLJournalId] [int] NULL,
	[Code] [nvarchar](500) NULL,
	[TransactionDate] [nvarchar](50) NULL,
	[JournalType] [nvarchar](50) NULL,
	[COAId] [int] NULL,
	[IsDr] [bit] NULL,
	[DrAmount] [decimal](18, 2) NULL,
	[CrAmount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](500) NULL,
	[TransType] [nvarchar](10) NULL,
	[TransactionAmount] [decimal](18, 2) NULL,
	[NetChange] [decimal](18, 2) NULL,
	[IsRetainedEarning] [bit] NULL,
	[COAGroupId] [int] NULL,
	[COAGroupTypeId] [int] NULL,
	[COATypeOfReportId] [int] NULL,
	[COASL] [int] NULL,
	[GroupSL] [int] NULL,
	[GroupTypeSL] [int] NULL,
	[TypeOfReportSL] [int] NULL,
	[GroupTypeShortName] [nvarchar](100) NULL,
	[TypeOfReportShortName] [nvarchar](100) NULL,
 CONSTRAINT [PK_TempNetChange] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempNetChangeNew]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TempNetChangeNew](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransType] [nvarchar](50) NULL,
	[OperationType] [nvarchar](50) NULL,
	[COAId] [int] NULL,
	[TransactionAmount] [decimal](18, 4) NULL,
	[OpeningAmount] [decimal](18, 4) NULL,
	[NetChange] [decimal](18, 4) NULL,
	[ClosingAmount] [decimal](18, 4) NULL,
 CONSTRAINT [PK_TempNetChangeNew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempSalary]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TempSalary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FiscalYearDetailId] [varchar](100) NULL,
	[EmployeeId] [varchar](100) NULL,
	[Basic] [decimal](18, 4) NULL,
	[HouseRent] [decimal](18, 4) NULL,
	[Medical] [decimal](18, 4) NULL,
	[TransportAllowance] [decimal](18, 4) NULL,
	[Gross] [decimal](18, 4) NULL,
	[TAX] [decimal](18, 4) NULL,
	[TransportBill] [decimal](18, 4) NULL,
	[Stamp] [decimal](18, 4) NULL,
	[PFEmployer] [decimal](18, 4) NULL,
	[PFLoan] [decimal](18, 4) NULL,
	[STAFFWELFARE] [decimal](18, 4) NULL,
	[DeductionTotal] [decimal](18, 4) NULL,
	[NetSalary] [decimal](18, 4) NULL,
	[Othere(OT)] [decimal](18, 4) NULL,
	[Vehicle(Adj)] [decimal](18, 4) NULL,
	[Other(Bonus)] [decimal](18, 4) NULL,
	[LeaveWOPay] [decimal](18, 4) NULL,
	[GP] [decimal](18, 4) NULL,
	[Travel] [decimal](18, 4) NULL,
	[ChildAllowance] [decimal](18, 4) NULL,
	[MOBILE(Allowance)] [decimal](18, 4) NULL,
	[OtherAdjustment] [decimal](18, 4) NULL,
	[TotalAdjustment] [decimal](18, 4) NULL,
	[NetPayment] [decimal](18, 4) NULL,
	[ProjectId] [varchar](100) NULL,
	[DepartmentId] [varchar](100) NULL,
	[SectionId] [varchar](100) NULL,
	[DesignationId] [varchar](100) NULL,
 CONSTRAINT [PK_TempSalary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionMedias]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[TransactionMedias](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[TransactionType] [varchar](100) NULL,
	[TransType] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[User](
	[Id] [nvarchar](50) NOT NULL,
	[GroupId] [int] NULL,
	[FullName] [nchar](100) NULL,
	[Email] [nchar](100) NULL,
	[LogId] [nchar](50) NOT NULL,
	[Password] [nchar](50) NOT NULL,
	[VerificationCode] [nchar](20) NULL,
	[BranchId] [int] NOT NULL,
	[EmployeeId] [nvarchar](50) NOT NULL,
	[IsAdmin] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsVerified] [bit] NULL,
	[IsArchived] [bit] NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](20) NOT NULL,
	[CreatedFrom] [varchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[UserGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](100) NULL,
	[IsSuper] [bit] NULL,
	[IsESS] [bit] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[IsAdmin] [bit] NULL,
	[IsHRM] [bit] NULL,
	[IsAttendance] [bit] NULL,
	[IsPayroll] [bit] NULL,
	[IsTAX] [bit] NULL,
	[IsPF] [bit] NULL,
	[IsGF] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BranchId] [int] NOT NULL,
	[UserInfoId] [nvarchar](128) NULL,
	[RoleInfoId] [nvarchar](128) NULL,
	[IsArchived] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Withdraws]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[Withdraws](
	[Id] [int] NOT NULL,
	[IsInvested] [bit] NULL,
	[Code] [nvarchar](50) NULL,
	[WithdrawAmount] [decimal](18, 2) NULL,
	[WithdrawDate] [nvarchar](14) NULL,
	[BankBranchId] [int] NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](20) NOT NULL,
	[CreatedAt] [nvarchar](14) NOT NULL,
	[CreatedFrom] [nvarchar](50) NOT NULL,
	[LastUpdateBy] [nvarchar](20) NULL,
	[LastUpdateAt] [nvarchar](14) NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[Post] [bit] NULL,
	[TransactionType] [nvarchar](100) NULL,
	[ReferenceNo] [nvarchar](100) NULL,
	[TransactionMediaId] [nvarchar](200) NULL,
	[TransactionTypeId] [int] NULL,
	[TransType] [varchar](100) NULL,
 CONSTRAINT [PK_Withdraws] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YearClosing]    Script Date: 4/13/2025 5:14:03 PM ******/
SET ANSI_NULLS ON
GO

GO
CREATE TABLE [dbo].[YearClosing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransType] [nvarchar](10) NULL,
	[COAId] [int] NULL,
	[ClosingAmount] [decimal](18, 4) NULL,
	[FiscalYear] [nvarchar](4) NULL,
 CONSTRAINT [PK_YearClosing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE VIEW [dbo].[ViewEmployeeInformation]
AS
SELECT        ei.Id AS EmployeeId, ei.DepartmentId, ei.DesignationId, ei.ProjectId, ei.SectionId, ei.Code, ei.Name AS EmpName, ei.DateOfBirth, ei.JoinDate, ei.LeftDate, ei.Branch, ei.Grade, ISNULL(ei.GrossSalary, 0) AS GrossSalary, 
                         ISNULL(ei.BasicSalary, 0) AS BasicSalary, ei.PhotoName, ei.IsActive, ei.IsArchive, ei.LastUpdateAt, ei.LastUpdateBy, ei.LastUpdateFrom, ei.CreatedBy, ei.CreatedAt, ei.CreatedFrom, ei.Other1, ei.Remarks, ei.Email, 
                         d.Name AS Department, dg.Name AS Designation, s.Name AS Section, p.Name AS Project
FROM            dbo.EmployeeInfo AS ei LEFT OUTER JOIN
                         dbo.Department AS d ON ei.Department = d.Id LEFT OUTER JOIN
                         dbo.Designation AS dg ON ei.DesignationId = dg.Id LEFT OUTER JOIN
                         dbo.Section AS s ON ei.SectionId = s.Id LEFT OUTER JOIN
                         dbo.Project AS p ON ei.ProjectId = p.Id
GO
/****** Object:  View [dbo].[View_LoanDetails]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[View_LoanDetails] AS

SELECT        I.Id, I.BranchId, I.LoanType_E, I.EmployeeId, ve.EmpName, ve.Code, ve.Designation, ve.Department, ve.Section, I.IsFixed, I.InterestPolicy, I.InterestRate, I.TotalAmount, I.NumberOfInstallment, I.StartDate, I.EndDate, I.IsHold, 
                         I.IsApproved, I.ApplicationDate, I.ApprovedDate, I.RefundAmount, ISNULL(I.RefundDate, '') AS RefundDate, t.Name AS LoanType, I.LoanNo, ELD.Id AS Expr1, ELD.EmployeeLoanId, ELD.EmployeeId AS Expr2, 
                         ELD.InstallmentAmount, ELD.InstallmentPaidAmount, ELD.PaymentScheduleDate, ELD.PaymentDate, CASE WHEN ELD.IsHold = 0 THEN 'N' ELSE 'Y' END AS Expr3, CASE WHEN ELD.IsPaid = 0 THEN 'N' ELSE 'Y' END AS IsPaid,
                          CASE WHEN ELD.HaveDuplicate = 0 THEN 'N' ELSE 'Y' END AS HaveDuplicate, ELD.DuplicateID, ELD.Remarks, ELD.PrincipalAmount, ELD.InterestAmount, 
                         CASE WHEN IsPaid = 1 THEN ELD.PrincipalAmount ELSE 0 END AS PrincipalAmountPaid, CASE WHEN IsPaid = 1 THEN ELD.InterestAmount ELSE 0 END AS InterestAmountPaid
FROM            dbo.EmployeeLoanDetail AS ELD LEFT OUTER JOIN
                         dbo.EmployeeLoan AS I ON ELD.EmployeeLoanId = I.Id LEFT OUTER JOIN
                         dbo.ViewEmployeeInformation AS ve ON I.EmployeeId = ve.EmployeeId LEFT OUTER JOIN
                         dbo.EnumLoanType AS t ON t.Id = I.LoanType_E
WHERE        (ELD.IsArchive = 0)
GO
/****** Object:  View [dbo].[View_COA_New]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_COA_New]
AS
SELECT c.COAGroupId, cg.GroupSL, cg.Name AS COAGroupName, c.Id AS COAId, c.Code AS COACode, c.Name AS COAName, c.TransactionType, c.TransType, c.COASL, c.Nature, c.COAType, c.IsRetainedEarning, c.IsNetProfit, 
                  c.IsDepreciation
FROM     dbo.COAs AS c LEFT OUTER JOIN
                  dbo.COAGroups AS cg ON c.COAGroupId = cg.Id
GO
/****** Object:  View [dbo].[View_GLJournalDetailNew]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_GLJournalDetailNew]
AS
SELECT j.Code, j.TransactionDate, j.JournalType, j.TransType, j.TransactionType, c.COACode, c.COAName, c.Nature AS COANature, c.COAType, jd.DrAmount, jd.CrAmount, ISNULL(jd.DrAmount, 0) - ISNULL(jd.CrAmount, 0) AS TransactionAmount, 
                  c.COAId, j.Id AS GLJournalId, jd.Id AS GLJournalDetailId, j.IsYearClosing, c.IsNetProfit, c.IsRetainedEarning
FROM     dbo.GLJournalDetails AS jd LEFT OUTER JOIN
                  dbo.GLJournals AS j ON jd.GLJournalId = j.Id LEFT OUTER JOIN
                  dbo.View_COA_New AS c ON jd.COAId = c.COAId
GO
/****** Object:  View [dbo].[View_IncomeStatement]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_IncomeStatement]
AS
SELECT TOP (100) PERCENT v.TypeOfReport, v.GroupType, v.GroupName, v.AccountCode, v.AccountName, v.Nature, v.TransType, CASE WHEN Nature = 'Dr' THEN NetChange ELSE 0 END AS Debit, 
                  CASE WHEN Nature = 'Cr' THEN NetChange * - 1 ELSE 0 END AS Credit, CASE WHEN Nature = 'Dr' THEN NetChange ELSE NetChange * - 1 END AS TransactionAmount, v.IsRetainedEarning, v.COAGroupId, v.COAGroupTypeId, 
                  v.COATypeOfReportId, v.COASL, v.GroupSL, v.GroupTypeSL, v.TypeOfReportSL, v.GroupTypeShortName, v.TypeOfReportShortName
FROM     dbo.TempNetChange AS v RIGHT OUTER JOIN
                      (SELECT DISTINCT COAId, MAX(RowSL) AS RowSL
                       FROM      dbo.TempNetChange
                       GROUP BY COAId) AS l ON l.COAId = v.COAId AND l.RowSL = v.RowSL
WHERE  (v.NetChange <> 0) AND (v.TypeOfReportShortName = 'IS')
ORDER BY v.COAId
GO
/****** Object:  View [dbo].[View_TrialBalance]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_TrialBalance]
AS
SELECT TOP (100) PERCENT v.TypeOfReport, v.GroupType, v.GroupName AS COAGroupName, v.AccountCode, v.AccountName, v.Nature, v.TransType, CASE WHEN Nature = 'Dr' AND NetChange >= 0 THEN NetChange WHEN Nature = 'Cr' AND 
                  NetChange > 0 THEN NetChange * + 1 ELSE 0 END AS Debit, CASE WHEN Nature = 'Cr' THEN NetChange * - 1 WHEN Nature = 'Dr' AND NetChange < 0 THEN NetChange * - 1 ELSE 0 END AS Credit, 
                  CASE WHEN Nature = 'Dr' THEN NetChange ELSE NetChange * - 1 END AS TransactionAmount, v.IsRetainedEarning, v.COAGroupId, v.COAGroupTypeId, v.COATypeOfReportId, v.COASL, v.GroupSL, v.GroupTypeSL, v.TypeOfReportSL, 
                  v.GroupTypeShortName, v.TypeOfReportShortName
FROM     dbo.TempNetChange AS v RIGHT OUTER JOIN
                      (SELECT DISTINCT COAId, MAX(RowSL) AS RowSL
                       FROM      dbo.TempNetChange
                       GROUP BY COAId) AS l ON l.COAId = v.COAId AND l.RowSL = v.RowSL
WHERE  (v.NetChange <> 0)
GO
/****** Object:  View [dbo].[ViewEmployeeStatementGF]    Script Date: 4/16/2025 5:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewEmployeeStatementGF]
AS
SELECT        EmployeeId, TransactionDate, EmployerContribution, EmployerProfit, Total, TransType, Remarks
FROM            (SELECT        'A' AS SL, '19000101' AS TransactionDate, EmployeeId, EmployerContribution, EmployerProfit, 'Opening' AS TransType, 'Opening' AS Remarks, ISNULL(EmployerContribution, 0) + ISNULL(EmployerProfit, 0) 
                                                    AS Total
                          FROM            dbo.GFEmployeeOpeinig
                          UNION ALL
                          SELECT        'B' AS SL, OpeningDate AS TransactionDate, EmployeeId, EmployerContribution, EmployerProfit, 'BreakMonth' AS TransType, ISNULL(Remarks, 'Break Month') AS Remarks, ISNULL(EmployerContribution, 0) 
                                                   + ISNULL(EmployerProfit, 0) AS Total
                          FROM            dbo.GFEmployeeBreakMonth
                          UNION ALL
                          SELECT        'C' AS SL, fd.PeriodStart AS TransactionDate, dbo.GFEmployeeProvisions.EmployeeId, dbo.GFEmployeeProvisions.ProvisionAmount, 0 AS EmployerProfit, 'MonthlyContribution' AS TransType, 
                                                   ISNULL(dbo.GFEmployeeProvisions.Remarks, 'Monthly Contribution') AS Remarks, ISNULL(ISNULL(dbo.GFEmployeeProvisions.ProvisionAmount, 0), 0) AS Total
                          FROM            dbo.GFEmployeeProvisions LEFT OUTER JOIN
                                                  FiscalYearDetail AS fd ON dbo.GFEmployeeProvisions.FiscalYearDetailId = fd.Id
                          UNION ALL
                          SELECT        'C' AS SL, fd.PeriodStart AS TransactionDate, GFEmployeeProvisions_1.EmployeeId, GFEmployeeProvisions_1.IncrementArrear, 0 AS EmployerProfit, 'IncrementArrear' AS TransType, 
                                                   ISNULL(GFEmployeeProvisions_1.Remarks, 'IncrementArrear') AS Remarks, ISNULL(ISNULL(GFEmployeeProvisions_1.ProvisionAmount, 0), 0) AS Total
                          FROM            dbo.GFEmployeeProvisions AS GFEmployeeProvisions_1 LEFT OUTER JOIN
                                               FiscalYearDetail AS fd ON GFEmployeeProvisions_1.FiscalYearDetailId = fd.Id
                          UNION ALL
                          SELECT        'C' AS SL, DistributionDate AS TransactionDate, EmployeeId, 0 AS EmployerContribution, EmployeerProfitDistribution AS EmployerProfit, 'ProfitDistribution' AS TransType, ISNULL(Remarks, 'Profit Distribution') 
                                                   AS Remarks, ISNULL(EmployeerProfitDistribution, 0) AS Total
                          FROM            dbo.GFProfitDistributionNew
                          UNION ALL
                          SELECT        'B' AS SL, PaymentDate AS TransactionDate, EmployeeId, 1 * EmployerContribution AS EmployerContribution, 1 * EmployerProfit AS EmployerProfit, 'Payment' AS TransType, ISNULL(Remarks, 'Payment') 
                                                   AS Remarks, ISNULL(EmployerContribution, 0) + ISNULL(EmployerProfit, 0) AS Total
                          FROM            dbo.GFEmployeePayment) AS pf
WHERE        (1 = 1) AND (Total <> 0)
GO
                ";
                #endregion CreateTable

                top1 = "go";

                IEnumerable<string> commandStrings = Regex.Split(sqlText, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        SqlCommand cmdIdExist1 = new SqlCommand(commandString, currConn);

                        //new SqlCommand(commandString, currConn).ExecuteNonQuery();
                        cmdIdExist1.Transaction = transaction;
                        transResult = (int)cmdIdExist1.ExecuteNonQuery();
                        if (transResult != -1)
                        {
                            throw new ArgumentNullException("Create Tables to database('" + databaseName + "')", MessageVM.dbMsgTableNotCreate);
                        }
                    }
                }

                #endregion TableCreate

                #region TableDefaultData
                string top2;
                #region TableDefaultData Back
                sqlText = @"

                ";
                #endregion TableDefaultData Back

                #region TableDefaultData Back
                                sqlText = @"
                INSERT [dbo].[EnumOderBy] ([Id], [Module], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'CODE', N'Salary', N'CODE', NULL, 1, 0, N'admin', N'19000101', N'local', NULL, NULL, NULL)
                INSERT [dbo].[EnumOderBy] ([Id], [Module], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'DCG', N'Salary', N'DEPT>CODE>GRADE', NULL, 1, 0, N'admin', N'19000101', N'local', NULL, NULL, NULL)
                INSERT [dbo].[EnumOderBy] ([Id], [Module], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'DDC', N'Salary', N'DEPT>DOJ>CODE', NULL, 1, 0, N'admin', N'19000101', N'local', NULL, NULL, NULL)
                INSERT [dbo].[EnumOderBy] ([Id], [Module], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'DGC', N'Salary', N'DEPT>GRADE>CODE', NULL, 1, 0, N'admin', N'19000101', N'local', NULL, NULL, NULL)
                INSERT [dbo].[EnumOderBy] ([Id], [Module], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'DGDC', N'Salary', N'DEPT>GRADE>DOJ>CODE', NULL, 1, 0, N'admin', N'19000101', N'local', NULL, NULL, NULL)
                GO
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (1, N'ASset101', N'Laptop', NULL, 1, 0, N'ADMIN', N'20210204160645', N'', N'IT-1002', N'20231122154823', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (2, N'PC-0001', N'Laptop', NULL, 1, 0, N'IT-1002', N'20231122154910', N'', N'IT-1002', N'20231122154916', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (3, N'PC-001', N'Laptop Computer', N'0', 1, 0, N'IT-1002', N'20231122155031', N'', N'IT-1002', N'20231122161915', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (4, N'PC-002', N'Desktop Computer', NULL, 1, 0, N'IT-1002', N'20231122155212', N'', N'IT-1002', N'20231122161909', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (5, N'ASST-0001', N'Pen Drive', NULL, 1, 0, N'IT-1002', N'20231122155323', N'', N'IT-1002', N'20231122162415', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (6, N'ASST-0002', N'Keyboard', NULL, 1, 0, N'IT-1002', N'20231122155534', N'', N'IT-1002', N'20231122162406', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (7, N'ASST-0003', N'Mouse Wireless', NULL, 1, 0, N'IT-1002', N'20231122155634', N'', N'IT-1002', N'20231122162415', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (8, N'ASST-0004', N'Mouse', NULL, 1, 0, N'IT-1002', N'20231122155708', N'', N'IT-1002', N'20231122162415', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (9, N'ASST-0005', N'Monitor', NULL, 1, 0, N'IT-1002', N'20231122155845', N'', N'IT-1002', N'20231122162415', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (10, N'ASST-0006', N'HDD', NULL, 1, 0, N'IT-1002', N'20231122155938', N'', N'IT-1002', N'20231122162359', N'', 0, N'0', N'0', N'0', N'0')
                INSERT [dbo].[Asset] ([Id], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsVehicle], [RegNo], [EngineNo], [ChassisNo], [Model]) VALUES (11, N'ASST-00011', N'HDD1', N'Test', 1, 0, N'Admin', N'20250320102505', N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                GO
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_1', 1, N'BRAC Bank Ltd.', N'BRAC Bank Ltd.', N'', 1, 0, N'Admin', N'20250322163253', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_2', 1, N'ISB101', N'Islami Bank Bangladesh', N'Testing 101', 1, 0, N'Admin', N'20250322163253', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_6', 1, N'IM', N'Test', N'Remarks', 1, 0, N'Admin', N'20250323094354', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_3', 1, N'CBL', N'City Bank Ltd.', N'', 1, 0, N'Admin', N'20250322163253', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_4', 1, N'DEV-1029', N'test', N'Remarks', 1, 0, N'Admin', N'20250322163253', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_5', 1, N'2323', N'testtest', N'Remarks', 1, 0, N'Admin', N'20250322163253', N'', NULL, NULL, NULL)
                INSERT [dbo].[Bank] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_7', 1, N'IM1', N'Test1', N'Remarks', 1, 0, N'Admin', N'20250323094612', N'', NULL, NULL, NULL)
                GO
                INSERT [dbo].[BankBranchs] ([Id], [BankId], [BranchName], [BranchAddress], [BankAccountType], [BankAccountNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (1, 1, N'Gulshan Branch ', N'Gulshan Dhaka', N'Savings', N'111123232300', NULL, 1, 0, N'Admin', N'20250322095119', N'', NULL, NULL, NULL, N'PF', N'PF')
                GO
                INSERT [dbo].[BankNames] ([Id], [Name], [Address], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (1, N'Dutch Bangla bank', N'Dhaka', NULL, 1, 0, N'Admin', N'20250322095043', N'', NULL, NULL, NULL, N'PF', N'PF')
                GO
                SET IDENTITY_INSERT [dbo].[Branch] ON 

                INSERT [dbo].[Branch] ([Id], [CompanyId], [Code], [Name], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (10, 1, N'HO', N'Head Office', N'', N'Dhaka', N'Dhaka', NULL, N'Dhaka', N'1207', N'02-9611894-5', N'', N'02-9673916', N'A', 1, 0, N'Admin', N'20250322145032', N'', NULL, NULL, NULL)
                INSERT [dbo].[Branch] ([Id], [CompanyId], [Code], [Name], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (11, 1, N'112', N'Pice', N'C-6/23 Bangladesh Bank Colony', N'', N'', NULL, N'Dhaka', N'1204', N'', N'01711222244', N'', N'', 1, 0, N'Admin', N'20250322145032', N'', NULL, NULL, NULL)
                INSERT [dbo].[Branch] ([Id], [CompanyId], [Code], [Name], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (12, 1, N'00036', N'Dhanmondi', N'', N'Brahmanbaria ', N'Chittagoang', NULL, N'New Dhaka', N'', N'', N'aaa', N'', N'', 1, 0, N'Admin', N'20250322145032', N'', NULL, NULL, NULL)
                INSERT [dbo].[Branch] ([Id], [CompanyId], [Code], [Name], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (13, 1, N'B1010', N'BranchDhaka', N'Dhaka2515,', N'Barguna ', N'Barisal', NULL, N'Dapna', N'1212', N'5484448', N'2154251', N'54444845', N'Test', 1, 0, N'Admin', N'20250322145032', N'', NULL, NULL, NULL)
                SET IDENTITY_INSERT [dbo].[Branch] OFF
                GO
                SET IDENTITY_INSERT [dbo].[COAGroups] ON 

                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (2, 0, N'10200', N'Current Asset', NULL, 1, 0, N'-', N'-', N'-', N'Admin', N'20250325140149', N'', N'Asset', N'BS', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (3, 2, N'10100', N'Non Current Assest', NULL, 1, 0, N'-', N'-', N'-', N'ADMIN', N'20240502130945', N'', N'Asset', N'BS', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (4, 3, NULL, N'Less accumulated depreciation', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173450', N'', N'Asset', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (5, 4, N'20100', N'Current Liabilities', NULL, 1, 0, N'-', N'-', N'-', N'-', N'-', N'-', N'Liability', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (6, 5, NULL, N'Long term liabilities', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173636', N'', N'Liability', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (7, 6, NULL, N'Shareholder''s equity', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173627', N'', N'OwnersEquity', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (8, 7, NULL, N'Sales Revenue', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173620', N'', N'Revenue', N'IS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (9, 8, NULL, N'Other Income', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173610', N'', N'Revenue', N'IS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (10, 9, NULL, N'COGS', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173600', N'', N'Expense', N'IS', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (11, 10, NULL, N'Administrative Expence', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173550', N'', N'Expense', N'IS', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (12, 9999, NULL, N'Retained Earnings', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173541', N'', N'RetainedEarnings', N'RetainedEarnings', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (13, 2, N'XX', N'Fixed Asset', NULL, 0, 0, N'-', N'-', N'-', N'ADMIN', N'20240815173531', N'', N'Asset', N'BS', N'PF', N'PF', 1, 1, N'Dr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (14, 4, N'20200', N'Non-Current Liabilitie', NULL, 1, 0, N'-', N'-', N'-', N'-', N'-', N'-', N'Liability', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (15, 4, N'20300', N'Funds', NULL, 1, 0, N'-', N'-', N'-', N'-', N'-', N'-', N'Liability', N'BS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (16, 7, N'40000', N'Income', NULL, 1, 0, N'-', N'-', N'-', N'-', N'-', N'-', N'Revenue', N'IS', N'PF', N'PF', 1, 1, N'Cr')
                INSERT [dbo].[COAGroups] ([Id], [GroupSL], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [GroupType], [ReportType], [TransactionType], [TransType], [COATypeOfReportId], [COAGroupTypeId], [GroupNature]) VALUES (17, 7, N'50000', N'Expense', NULL, 1, 0, N'-', N'-', N'-', N'-', N'-', N'-', N'Revenue', N'IS', N'PF', N'PF', 1, 1, N'Cr')
                SET IDENTITY_INSERT [dbo].[COAGroups] OFF
                GO
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (1, 0, 1, N'10210', N'Cash in Hand', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815173803', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (2, 0, 1, N'10211', N'Bank (08536000027)', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815173857', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (3, 0, 1, N'10212', N'Loan to Members', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815174450', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (4, 0, 1, N'10213', N'Advance Tax Deducted- Sanchaypatra', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815174520', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (5, 0, 1, N'10214', N'Advance Tax Deducted- FDR A/C', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815174551', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (6, 0, 1, N'10215', N'Advance Tax Deducted- Bank A/C', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815174623', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (7, 0, 1, N'10216', N'Advance against FDR', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240824152140', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (8, 0, 1, N'10217', N'Interest Receivable from Sanchaypatra', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815174815', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (9, 0, 1, N'10218', N'Other Receivables', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20241106115831', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (10, 0, 1, N'10219', N'Investment in FDR', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240921153836', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (11, 0, 2, N'10110', N'Investment in Sanchaypatra', N'Dr', N'Asset', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922102249', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (12, 0, 4, N'20110', N'Payable to outgoing Members', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815175553', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (13, 0, 4, N'20111', N'Welfare Fund', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815175626', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (14, 0, 4, N'20112', N'Payable to EGCBL (Laps & Forfeiture)', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20241106120127', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (15, 0, 4, N'20113', N'Laps & Forfeiture', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815175829', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (16, 0, 4, N'20114', N'Extra amount added as subscription', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815180359', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (17, 0, 4, N'20115', N'Extra amount deducted as loan installment', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240817124346', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (18, 0, 4, N'20117', N'Other Liabilities', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815180602', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (19, 0, 4, N'20118', N'AIT Payable- Sanchaypatra', N'Cr', N'Liability', N'BS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815180700', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (20, 0, 4, N'20119', N'Provision for Tax', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815180759', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (21, 0, 14, N'30100', N'Employee''s contribution', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922101059', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (22, 0, 14, N'30200', N'Company''s Contribution', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181116', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (23, 0, 14, N'30300', N'Employee''s contribution- Interest Portion', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922101127', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (24, 0, 14, N'30400', N'Company''s Contribution- Interest Portion', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181222', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (25, 0, 15, N'40100', N'Interest Income from Sanchaypatra', N'Cr', N'Revenue', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922102039', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (26, 0, 15, N'40200', N'Interest Income from FDR', N'Cr', N'Revenue', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181338', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (27, 0, 15, N'40300', N'Interest Income from SND Account (Bank Asia Ltd)', N'Cr', N'Revenue', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20241106124236', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (28, 0, 15, N'40400', N'Interest income from Member''s Loan', N'Cr', N'Revenue', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240921172456', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (29, 0, 15, N'40500', N'Interest Forfeiture', N'Cr', N'Revenue', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181546', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (30, 0, 16, N'50100', N'TDS on SND account (Bank Asia Ltd)', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181633', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (31, 0, 16, N'50200', N'TDS on Sanchaypatra', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922102306', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (32, 0, 16, N'50300', N'TDS on FDR', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181740', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (33, 0, 16, N'50400', N'Bank Charge on SND account', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20241106124428', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (34, 0, 16, N'50500', N'Other Expense', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181911', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (35, 0, 16, N'50600', N'Tax Expense', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240815181937', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (36, 0, 4, N'20116', N'Amount adjusted against loan in Final Settlement', N'Cr', N'Liability', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240817124706', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (37, 0, 16, N'51000', N'Bank charge & Excise Duty on FDR', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20241106125731', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (38, 0, 1, N'10220', N'Excess AIT Receivable', N'Dr', N'Asset', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240824153004', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (39, 0, 1, N'10221', N'Interest Receivable from FDR', N'Dr', N'Asset', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240824173811', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (40, 0, 16, N'50700 ', N'Income TAX- FDR', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240827183127', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (41, 0, 16, N'50800', N'Income TAX- Sanchaypatra', N'Dr', N'Expense', N'IS', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240922102346', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (42, 0, 4, N'20120', N'Provision Income TAX- FDR', N'Cr', N'Liability', N'NetProfit', CAST(0.00 AS Decimal(18, 2)), N'PF', N'NA', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240827183315', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (43, 0, 4, N'20121', N'Provision Income TAX- Sanchaypatra', N'Cr', N'Liability', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20240827183356', N'', N'ADMIN', N'20240922102915', N'', 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (44, 0, 6, N'20300', N'Profit and Loss A/C', N'Cr', N'OwnersEquity', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20240922130710', N'', N'ADMIN', N'20241106114510', N'', 0, 1, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (45, 0, 16, N'50900', N'Excise Duty on SND account', N'Dr', N'Expense', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20241106125617', N'', NULL, NULL, NULL, 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (46, 0, 6, N'20400', N'Excess of Income over Expenditure', N'Cr', N'OwnersEquity', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20241106143236', N'', NULL, NULL, NULL, 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (47, 0, 15, N'40600', N'Miscellaneous Income', N'Cr', N'Revenue', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20241111122747', N'', NULL, NULL, NULL, 0, 0, 0, N'PF')
                INSERT [dbo].[COAs] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [COAType], [ReportType], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [IsNetProfit], [IsDepreciation], [TransactionType]) VALUES (48, 0, 16, N'51100', N'Miscellaneous Expense', N'Dr', N'Expense', NULL, NULL, N'PF', NULL, 1, 0, N'ADMIN', N'20241111122812', N'', NULL, NULL, NULL, 0, 0, 0, N'PF')
                GO
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (3, NULL, 2, N'10210', N'Cash in hand', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Cash & Cash Equivalent', 1, 0, N'Admin', N'19000101', N'Local', N'ADMIN', N'20240316145822', N'', 0, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (92, NULL, 4, N'10110', N'Investment in Sanchayapatra', N'Dr', CAST(347548470.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240211170412', N'', N'ADMIN', N'20240319103536', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (93, NULL, 4, N'10111', N'Investment in FDR', N'Dr', CAST(58722065.07 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240310131551', N'', N'ADMIN', N'20240319103627', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (94, NULL, 2, N'10211', N'Cash at Bank (SND)', N'Dr', CAST(262238.89 AS Decimal(18, 2)), N'PF', N'Cash & Cash Equivalent', 1, 0, N'ADMIN', N'20240316132456', N'', N'ADMIN', N'20240319103659', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (95, NULL, 2, N'10212', N'Loan to Members', N'Dr', CAST(19180005.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316132645', N'', N'ADMIN', N'20240319103716', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (96, NULL, 2, N'10213', N'Advance Tax Deducted- Sanchaypatra', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Advance Tax Deducted', 1, 0, N'ADMIN', N'20240316132807', N'', N'ADMIN', N'20240316145944', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (97, NULL, 2, N'10214', N'Advance Tax Deducted- FDR A/C', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Advance Tax Deducted', 1, 0, N'ADMIN', N'20240316150242', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (98, NULL, 2, N'10215', N'Advance Tax Deducted- Bank A/C', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Advance Tax Deducted', 1, 0, N'ADMIN', N'20240316150334', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (99, NULL, 2, N'10216', N'Contribuition Receivable A/C', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Contribuition Receivable', 1, 0, N'ADMIN', N'20240316150432', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (100, NULL, 8, N'20110', N'Payable to outgoing Members', N'Cr', CAST(3573948.77 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150531', N'', N'ADMIN', N'20240319103741', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (101, NULL, 8, N'20111', N'Welfare Fund', N'Cr', CAST(11205505.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150616', N'', N'ADMIN', N'20240319103816', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (102, NULL, 8, N'20112', N'Payable to EGCBL ', N'Cr', CAST(159038.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150713', N'', N'ADMIN', N'20240319103837', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (103, NULL, 8, N'20113', N'Laps & Forfeiture', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150744', N'', N'ADMIN', N'20240316151200', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (104, NULL, 8, N'20114', N'Extra amount added as subscription', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150822', N'', N'ADMIN', N'20240316151212', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (105, NULL, 8, N'20115', N'Extra amount deducted as Loan installment', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150901', N'', N'ADMIN', N'20240316151224', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (106, NULL, 8, N'20116', N'Other Liabilities', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150930', N'', N'ADMIN', N'20240316151254', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (107, NULL, 8, N'20117', N'Provision for Tax', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316150949', N'', N'ADMIN', N'20240316151306', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (108, NULL, 6, N'20200', N'Other Non- Current Liabilities', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316151049', N'', N'ADMIN', N'20240316151344', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (109, NULL, 23, N'30100', N'Employees'' Contribution Portion', N'Cr', CAST(205387143.50 AS Decimal(18, 2)), N'PF', N'Employees'' Contribution', 1, 0, N'ADMIN', N'20240316151430', N'', N'ADMIN', N'20240319103901', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (110, NULL, 23, N'30200', N'Company''s Contribution Portion', N'Cr', CAST(205387143.50 AS Decimal(18, 2)), N'PF', N'Company''s Contribution', 1, 0, N'ADMIN', N'20240316151818', N'', N'ADMIN', N'20240319103918', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (111, NULL, 23, N'30300', N'Employees'' Contribution- Interest Portion', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', N'Employees'' Contribution', 1, 0, N'ADMIN', N'20240316151924', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (113, NULL, 9, N'40100', N'Interest Income Receivable from Sanchayapatra', N'Cr', CAST(28381470.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152040', N'', N'ADMIN', N'20240319103943', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (114, NULL, 9, N'40200', N'Interest Income from FDR', N'Cr', CAST(1312500.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152124', N'', N'ADMIN', N'20240319104005', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (115, NULL, 9, N'40300', N'Interest Income Receivable from FDR', N'Cr', CAST(760068.49 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152157', N'', N'ADMIN', N'20240319104103', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (116, NULL, 9, N'40400', N'Interest on SND Account (Bank Asia Ltd.)', N'Cr', CAST(227714.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152230', N'', N'ADMIN', N'20240319104132', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (117, NULL, 9, N'40500', N'Interest from Member''s Loan ', N'Cr', CAST(1147230.94 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152257', N'', N'ADMIN', N'20240319104154', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (118, NULL, 9, N'40600', N'Interest Forfeiture', N'Cr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152401', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (119, NULL, 7, N'50100', N'TDS on SND account (Bank Asia Ltd.)', N'Dr', CAST(11385.70 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152436', N'', N'ADMIN', N'20240319104216', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (120, NULL, 7, N'50200', N'TDS Payable on Sanchayapatra', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152509', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (121, NULL, 7, N'50300', N'TDS Deduction and TDS Payable on FDR', N'Dr', CAST(103628.42 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152536', N'', N'ADMIN', N'20240319104235', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (122, NULL, 7, N'50400', N'Bank Charge & Excise duty on FDR', N'Dr', CAST(48000.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152605', N'', N'ADMIN', N'20240319104255', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (123, NULL, 7, N'50500', N'Bank Charge (Bank Asia Ltd.)', N'Dr', CAST(3117.52 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152629', N'', N'ADMIN', N'20240319104321', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (124, NULL, 7, N'50600', N'Excise Duty (Bank Asia Ltd.)', N'Dr', CAST(15000.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152653', N'', N'ADMIN', N'20240319104339', N'', 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (125, NULL, 7, N'50700', N'Other Expense', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152728', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (126, NULL, 7, N'50800', N'Tax Expense', N'Dr', CAST(0.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240316152754', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (127, NULL, 23, N'30500', N'Distributed Profit (Members Contribution) ', N'Cr', CAST(13997945.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240319105120', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (128, NULL, 23, N'30600', N'Distributed Profit (Company''s Contribution) ', N'Cr', CAST(13997945.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240319105241', N'', NULL, NULL, NULL, 1, N'PF')
                INSERT [dbo].[COAsBackup] ([Id], [COASL], [COAGroupId], [Code], [Name], [Nature], [OpeningBalance], [TransType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsRetainedEarning], [TransactionType]) VALUES (129, NULL, 23, N'30700', N'Undistributed Profit (Welfare Fund)', N'Cr', CAST(3651964.00 AS Decimal(18, 2)), N'PF', NULL, 1, 0, N'ADMIN', N'20240319105323', N'', NULL, NULL, NULL, 1, N'PF')
                GO
                INSERT [dbo].[COAType] ([Id], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (1, N'Asset', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COAType] ([Id], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (2, N'Liability', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COAType] ([Id], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (3, N'OwnersEquity', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COAType] ([Id], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (4, N'Revenue', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COAType] ([Id], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (5, N'Expense', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                GO
                INSERT [dbo].[COATypeOfReport] ([Id], [TypeOfReportSL], [TypeOfReportShortName], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (1, 100, N'BS', N'Balance Sheet', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COATypeOfReport] ([Id], [TypeOfReportSL], [TypeOfReportShortName], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (2, 500, N'IS', N'Income Statement', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                INSERT [dbo].[COATypeOfReport] ([Id], [TypeOfReportSL], [TypeOfReportShortName], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TransactionType], [TransType]) VALUES (3, 300, N'RE', N'Retained Earning', N'', 1, 0, N'Admin', N'19000101', N'Local', NULL, NULL, NULL, N'PF', N'PF')
                GO
                SET IDENTITY_INSERT [dbo].[Company] ON 

                INSERT [dbo].[Company] ([Id], [Code], [Name], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [TaxId], [RegistrationNumber], [Mail], [NumberOfEmployees], [YearStart], [Year], [VATNo]) VALUES (1, N'SSL0', N'Symphony Softtech Ltd', N'Tridhara Tower (2nd Floor), 67, West Panthapath, Lake Circus, Kalabagan, Dhaka-1205', N'Dhaka', N'Dhaka', N'Bangladesh', N'Dhaka', N'1205', N'02-9611894-5', N'A', N'A', N'A', 1, 0, N'A', N'A', N'A', N'admin', N'20250116125617', N'192.168.15.100', N'Tax id', N'001976146-0402', N'symphonysoftt.com', 1500, N'20200701', N'2021', N'0')
                SET IDENTITY_INSERT [dbo].[Company] OFF
                GO
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_1', 1, N'DEV', N'Development', 7, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_35', 1, N'MK', N'Marketing1 000', 1, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_36', 1, N'DEV', N'Development 00', 2, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_37', 1, N'AD', N'Admin', 3, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_38', 1, N'MG', N'Management', 4, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_39', 1, N'SU', N'Support', 5, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_40', 1, N'MK1', N'Marketing1', 6, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_41', 1, N'IM', N'Implementation', 11, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_42', 1, N'MG', N'Management', 8, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_43', 1, N'AD', N'Admin & Accounts', 9, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_44', 1, N'IT', N'Information Technology', 10, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)
                GO
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_1', 1, N'HoPS', N'Head of Priority Services', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_10', 1, N'', N'In-Charge, Aparajita Female Trading Booth-Gulshan', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_100', 1, N'IMP-E', N'Executive -VAT & ERP', NULL, 1, 0, N'ADMIN', N'20210228121138', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_101', 1, N'IT-E', N'Executive IT & Networking', NULL, 1, 0, N'ADMIN', N'20210228121221', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_102', 1, N'DV-S', N'Sr. Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121254', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_103', 1, N'DV-D', N'Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121334', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_104', 1, N'A&A', N'Jr. Executive Admin & Accounts', NULL, 1, 0, N'ADMIN', N'20210228121409', N'', N'ADMIN', N'20210228130006', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_105', 1, N'GD', N'Graphics designer', NULL, 1, 0, N'ADMIN', N'20210228121437', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_106', 1, N'DV-Jr', N'Jr. Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121625', N'', N'ADMIN', N'20230105123016', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_107', 1, N'MK-E', N'Executive -Marketing', NULL, 1, 0, N'ADMIN', N'20210228121727', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_108', 1, N'OA', N'Office Assistant', NULL, 1, 0, N'ADMIN', N'20210228122349', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_109', 1, N'C', N'Cleaner', NULL, 1, 0, N'ADMIN', N'20210228122433', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_11', 1, N'', N'Senior Executive, Accounts & Finance', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_110', 1, N'D', N'Driver', NULL, 1, 0, N'ADMIN', N'20210228122501', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_111', 1, N'MGT', N'Director & COO', NULL, 1, 0, N'ADMIN', N'20210412144732', N'', N'ADMIN', N'20210412144937', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_112', 1, N'IMP-S', N'Executive- ERP Implementation', NULL, 1, 0, N'ADMIN', N'20211219134150', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_113', 1, N'EA', N'Executive Accounts', NULL, 1, 0, N'ADMIN', N'20221222170110', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_114', 1, N'Exe- Acc', N'Executive- Admin & Accounts', NULL, 1, 0, N'ADMIN', N'20230101123326', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_3', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_115', 1, N'IMP- Off', N'Officer- VAT & ERP', NULL, 1, 0, N'ADMIN', N'20230101123356', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_116', 1, N'AM-IT', N'Asst. Manager- Networking & IT', NULL, 1, 0, N'ADMIN', N'20230307120441', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_7', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_117', 1, N'IMP-Sr.', N'Sr. Executive- VAT & ERP', NULL, 1, 0, N'ADMIN', N'20230307120513', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_5', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_118', 1, N'MGT- ERP', N'Director & ERP Consultant', NULL, 1, 0, N'ADMIN', N'20230507120259', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_11', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_119', 1, N'Exe-VAT', N'Executive- VAT & Tax', NULL, 1, 0, N'ADMIN', N'20230622145150', N'', NULL, NULL, NULL, CAST(1.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_12', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_12', 1, N'DM-RTS', N'Deputy Manager, Sales & Trading', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_120', 1, N'Sage-1', N'Executive- Sage & ERP', NULL, 1, 0, N'ADMIN', N'20230622145531', N'', NULL, NULL, NULL, CAST(1.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_13', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_121', 1, N'IMP-HR', N'Business Executive- HRM Software Implementation', NULL, 1, 0, N'ADMIN', N'20230716104211', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_14', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_122', 1, N'DEV- ASST. MAN', N'Assistant Project Manager', NULL, 1, 0, N'Admin', N'20230731160334', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_15', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_123', 1, N'IMP-Jr.', N'Jr. Executive- VAT & ERP', NULL, 1, 0, N'Admin', N'20230803105953', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_16', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_124', 1, N'DEV-TRA', N'Trainee Software Developer', NULL, 1, 0, N'Admin', N'20240206170914', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_4', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_125', 1, N'IT-AM', N'Deputy Manager- Networking & IT', NULL, 1, 0, N'Admin', N'20240213101456', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_17', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_126', 1, N'MKT- Sr. Exe', N'Sr. Executive- Marketing & Sales', NULL, 1, 0, N'Admin', N'20240213102023', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_18', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_127', 1, N'DEV-QA', N'SQA Engineer', NULL, 1, 0, N'Admin', N'20240710124357', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_19', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_128', 1, N'Pr Cdr', N'Project Coordinator', NULL, 1, 0, N'Admin', N'20240821103139', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_20', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_129', 1, N'Jr MKT', N'Jr. Executive- Marketing & Sales', NULL, 1, 0, N'Admin', N'20240909121010', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_18', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_13', 1, N'AM-STM', N'Assistant Manager, Settlement', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_130', 1, N'DEV-1029', N'test', NULL, 1, 1, N'Admin', N'20241208153104', N'', N'Admin', N'20241208153120', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1', N'1_1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_14', 1, N'AM-ICC', N'Assistant Manager, Internal Control & Compliance', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_15', 1, N'', N'Manager, Accounts & Finance', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_16', 1, N'', N'Manager, International Trade & Sales', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_17', 1, N'BM', N'Branch Manager', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_18', 1, N'', N'Manager, Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_19', 1, N'', N'Senior Executive, Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_2', 1, N'HoSTM', N'Head of Settlement', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_20', 1, N'DHoHR', N'Deputy Head of HR & Admin', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_21', 1, N'MS-BS', N'Manager-Marketing & Sales', NULL, 1, 0, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228121010', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_22', 1, N'', N'Senior Executive, Customer Services', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_23', 1, N'DHoRS', N'Deputy Head of Research', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_24', 1, N'AM-IMP', N'Asst. Manager-VAT & ERP', NULL, 1, 0, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228115519', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_25', 1, N'', N'Senior Executive, Settlement', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_26', 1, N'', N'Senior Executive, Customer Services, Compliance & IT', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_27', 1, N'', N'Senior Executive, Admin', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_28', 1, N'DM1', N'Deputy Manager, Corporate & Strategic Business', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_29', 1, N'IT', N'Sr. Executive IT & Networking', NULL, 1, 0, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228121052', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_3', 1, N'HoAF', N'Head of Accounts & Finance (Acting)', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_30', 1, N'HoHRADCA', N'Head of HR, Admin & Corporate Affairs', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_31', 1, N'', N'Programmer', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_32', 1, N'AM-STC', N'Assistant Manager, Sales, Trading & Compliance', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_33', 1, N'', N'Senior Executive, Corporate Affairs and Administration', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_34', 1, N'', N'Research Associate', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_35', 1, N'', N'Senior Executive, Front Desk', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_36', 1, N'DM-ITS', N'Deputy Manager, International Trade & Sales', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_37', 1, N'', N'Unit Head, Group Business Development and Incharge, Aparajita', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_38', 1, N'', N'Software Engineer', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_39', 1, N'', N'Senior Executive, Sales & Trading and Compliance', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_4', 1, N'HoIT', N'Head of IT (Acting)', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_40', 1, N'', N'Network Engineer', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_41', 1, N'', N'Deputy Manager, Corporate Sales & Trade', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206112532', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_42', 1, N'', N'Senior Executive, International Trade & Sales', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_43', 1, N'E-ST', N'Executive, Sales & Trading', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_44', 1, N'E-CSD', N'Executive, Customer Services', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_45', 1, N'CEO', N'Chief Executive Officer', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_46', 1, N'HoRTS', N'Head of Retail Sales & Trading', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_47', 1, N'E-BS', N'Executive, Brokerage Services', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_48', 1, N'', N'Senior Executive, Human Resources', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_49', 1, N'DM-CS', N'Deputy Manager, Corporate Sales', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_5', 1, N'DM-AF', N'Deputy Manager, Accounts & Finance', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_50', 1, N'E-ITS', N'Executive, International Trade & Sales', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_51', 1, N'', N'Senior Executive, Learning and Development', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_52', 1, N'HoCSB', N'Head of Corporate & Strategic Business', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_53', 1, N'M-IMP', N'Manager-VAT & ERP', NULL, 1, 0, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228115631', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_54', 1, N'', N'Senior Executive, Retail Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111915', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_55', 1, N'', N'Executive, Retail Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111815', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_56', 1, N'', N'Junior Executive, Retail Sales and Trade', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111915', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_57', 1, N'E-PS-CBU', N'Executive, CBU (Priority Services Trader)', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_58', 1, N'MTO', N'Management Trainee Officer, Corporate & Strategic Business', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_59', 1, N'', N'Junior Executive, Retail Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111915', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_6', 1, N'HoIIETS', N'Head of International & Institutional Equity Sales & Trade', N'Head of International & Institutional Equity Sales & Trade', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_60', 1, N'', N'Junior Executive, Customer Services', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_61', 1, N'', N'Junior Executive, Front Desk', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_62', 1, N'', N'Junior Executive, Aparajita-Gulshan', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_63', 1, N'E-RTS', N'Executive, Retail Sales & Trade', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_64', 1, N'JE-RTS', N'Junior Executive, Sales & Trade', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_65', 1, N'', N'Senior Executive, Research', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_66', 1, N'', N'Senior Executive, Retail Sales and Trade', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_67', 1, N'E-ADCA', N'Executive, Corporate Affairs & Administration', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_68', 1, N'JE1', N'Junior Executive, Corporate & Strategic Business', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_69', 1, N'', N'Junior Executive, Information Technology', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_7', 1, N'', N'Assistant Manager, Sales & Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_70', 1, N'', N'Executive, Customer Sevices', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206110807', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_71', 1, N'SM-RS', N'Senior Manager, Research', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                GO
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_72', 1, N'', N'Senior Executive, Retail Sales and Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111915', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_73', 1, N'', N'Executive, Retail Sales and Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111836', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_74', 1, N'AM-RS', N'Assistant Manager, Research', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_75', 1, N'AM-RTS', N'Assistant Manager, Sales and Trade', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_76', 1, N'', N'Junior Executive, Retail Sales and Trading', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20180206111915', N'182.48.89.41~192.168.15.22', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_77', 1, N'Director', N'Director, Research', NULL, 1, 1, N'ADMIN', N'20180210151256', N'182.48.89.41~192.168.15.208', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_78', 1, N'M-2', N'Executive Director', NULL, 1, 0, N'ADMIN', N'20180210151315', N'182.48.89.41~192.168.15.208', N'ADMIN', N'20210228115203', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_79', 1, N'MRS', N'Manager, Retail Sales', NULL, 1, 1, N'ADMIN', N'20180210151404', N'182.48.89.41~192.168.15.208', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_8', 1, N'', N'Head of Brokerage Services and Branch Manager, DSE Annex', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_80', 1, N'AM-IT', N'Assistant Manager, Information Technology', NULL, 1, 1, N'ADMIN', N'20180322151421', N'', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_81', 1, N'DM', N'Research Analyst', N'Research Analyst', 1, 1, N'ADMIN', N'20180708140809', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_82', 1, N'0', N'Deputy Manager, Corporate Business Development', NULL, 1, 1, N'ADMIN', N'20180726135741', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20180726135821', N'202.40.180.26~10.20.242.92', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_83', 1, N'JE-ITS', N'Junior Executive, International Trade & Sales', NULL, 1, 1, N'ADMIN', N'20180801155109', N'119.148.4.123~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_84', 1, N'AM-A&F', N'Sr. Executive -Admin & Accounts', NULL, 1, 0, N'ADMIN', N'20180920143427', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228115421', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_85', 1, N'SM-Sales&Trade', N'Senior Manager, Sales & Trading', NULL, 1, 1, N'ADMIN', N'20180920143944', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_86', 1, N'M-3', N'CTO & Project Manager', NULL, 1, 0, N'ADMIN', N'20180920144720', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210412144922', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_87', 1, N'SE-IB', N'Senior Executive, Institutional Business', NULL, 1, 1, N'ADMIN', N'20181030172955', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_88', 1, N'DM-IBU', N'Deputy Manager, Institutional Business', NULL, 1, 1, N'ADMIN', N'20181119111414', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_89', 1, N'AM-IBU', N'Assistant Manager, Institutional Business', NULL, 1, 1, N'ADMIN', N'20181119111453', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_9', 1, N'DM-STM', N'Deputy Manager, Settlement', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_90', 1, N'E-IBU', N'Executive, Institutional Business', NULL, 1, 1, N'ADMIN', N'20181119111533', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_91', 1, N'JE-IBU', N'Junior Executive, Institutional Business', NULL, 1, 1, N'ADMIN', N'20181119111603', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_92', 1, N'UH-IBU', N'Unit Head, Institutional Business Unit', NULL, 1, 1, N'ADMIN', N'20181119112627', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_93', 1, N'M-1', N'Managing Director', NULL, 1, 0, N'ADMIN', N'20190101151817', N'119.148.4.123~10.20.242.92', N'ADMIN', N'20210228115136', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_94', 1, N'JE-CBU', N'Junior Executive, CBU-Sales & Trading', NULL, 1, 1, N'ADMIN', N'20190117150641', N'202.40.180.26~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_95', 1, N'AssoD-CBU', N'Associate Director, Central Business Unit', NULL, 1, 1, N'ADMIN', N'20190122135749', N'', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_96', 1, N'HoCBU', N'Head of CBU', NULL, 1, 1, N'ADMIN', N'20190122135952', N'', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_97', 1, N'JE-Trading', N'Junior Executive, Trading', NULL, 1, 1, N'ADMIN', N'20190305155605', N'119.148.4.123~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_98', 1, N'Mgr-IBU', N'Manager, Institutional Business Unit', N'Unit Head - IBU', 1, 1, N'ADMIN', N'20190423123134', N'119.148.4.126~10.20.242.92', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_99', 1, N'E-CA', N'Executive, Corporate Access', NULL, 1, 1, N'ADMIN', N'20190505152946', N'202.40.180.28~10.20.242.92', N'ADMIN', N'20210228114952', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
                GO
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1', 1, 1, N'All', N'All', NULL, 1, 0, N'admin', N'1900/01/01', N'local', N'admin', N'1900/01/01', N'local')
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_10', 10, 1, N'MGT-1', N'Chairman', NULL, 1, 0, N'ADMIN', N'20230323174617', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_11', 11, 1, N'MGT- ERP', N'Director & ERP Consultant', NULL, 1, 0, N'ADMIN', N'20230507120227', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_12', 12, 1, N'VAT-1', N'VAT & Tax', NULL, 1, 0, N'ADMIN', N'20230622145117', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_13', 13, 1, N'Sage-1', N'Sage & ERP', NULL, 1, 0, N'ADMIN', N'20230622145509', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_14', 14, 1, N'HRM-1', N'HRM Software Implementation', NULL, 1, 0, N'ADMIN', N'20230716104110', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_15', 15, 1, N'DEV-ASST. MAN', N'Assistant Project Manager', NULL, 1, 0, N'Admin', N'20230731160223', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_16', 16, 1, N'IMP-1001', N'Jr./Executive- VAT & ERP', NULL, 1, 0, N'Admin', N'20230803105838', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_17', 17, 1, N'IT-General', N'IT', NULL, 1, 0, N'Admin', N'20240213101419', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_18', 18, 1, N'MKT', N'MKT', NULL, 1, 0, N'Admin', N'20240213101937', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_19', 19, 1, N'DEV-QA', N'QA', NULL, 1, 0, N'Admin', N'20240710124312', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_2', 2, 1, N'IMP- Off', N'Officer- VAT & ERP', NULL, 1, 0, N'ADMIN', N'20230101123213', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_20', 20, 1, N'Pr Cdr', N'Project Coordinator', NULL, 1, 0, N'Admin', N'20240821103052', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_3', 3, 1, N'Exe- Acc', N'Executive- Admin & Accounts', NULL, 1, 0, N'ADMIN', N'20230101123240', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_4', 4, 1, N'DEV', N'Developer', NULL, 1, 0, N'ADMIN', N'20230105122950', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_5', 5, 1, N'IMP-Sr.', N'Sr. Executive- VAT & ERP', NULL, 1, 0, N'ADMIN', N'20230123105456', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_6', 6, 1, N'IMP-AM', N'Asst. Manager- VAT & ERP', NULL, 1, 0, N'ADMIN', N'20230205153653', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_7', 7, 1, N'IT-AM', N'Asst. Manager- Networking & IT', NULL, 1, 0, N'ADMIN', N'20230205154628', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_8', 8, 1, N'DRV-1', N'Driver', NULL, 1, 0, N'ADMIN', N'20230208175505', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_9', 9, 1, N'MGT', N'Mangement', NULL, 1, 0, N'ADMIN', N'20230319103143', N'', NULL, NULL, NULL)
                INSERT [dbo].[DesignationGroup] ([Id], [Serial], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_21', 21, 1, N'MMM', N'Management2', N'Test', 1, 0, N'Admin', N'20250323093609', N'', NULL, NULL, NULL)
                GO
                INSERT [dbo].[Project] ([Id], [BranchId], [Code], [Name], [Startdate], [EndDate], [ManpowerRequired], [ContactPerson], [ContactPersonDesignation], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [OrderNo]) VALUES (N'1_1', 1, N'Head Office', N'Head Office', N'19000101', N'19000101', 0, N'', N'', N'Symphony Tower, Plot No.S.E(F)-9 (3rd Floor), Road No.142,Gulshan-1,Dhaka-1212', N'Dhaka', N'Dhaka', N'Bangladesh', N'Gulshan-1', N'', N'', N'', N'', N'', 1, 0, N'Admin', N'20250322171912', N'', NULL, NULL, NULL, NULL)
                INSERT [dbo].[Project] ([Id], [BranchId], [Code], [Name], [Startdate], [EndDate], [ManpowerRequired], [ContactPerson], [ContactPersonDesignation], [Address], [District], [Division], [Country], [City], [PostalCode], [Phone], [Mobile], [Fax], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [OrderNo]) VALUES (N'1_2', 1, N'SYM-CTG', N'Chittagong', N'', N'', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 1, 0, N'Admin', N'20250322171912', N'', NULL, NULL, NULL, NULL)
                GO
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_1', 1, N'AD', N'Admin', N'Admin & Corporate Affairs', 7, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_10', 1, N'DRV', N'Driver', N'', 0, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_11', 1, N'IT', N'Information Technology', N'Information Technology', 6, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_12', 1, N'IMP', N'Sage & ERP', N'Implementation', 4, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_2', 1, N'DEV', N'Development', N'Development', 5, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_3', 1, N'MGT', N'Management', N'Management', 1, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_4', 1, N'AF', N'Accounts & Finance', N'Accounts & Finance', 2, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_5', 1, N'MKT', N'Marketing', N'Marketing', 3, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_6', 1, N'VERP', N'VAT & ERP', N'', 0, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_7', 1, N'OA', N'Office Assistance', N'', 0, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_8', 1, N'IMP-HR', N'HRM Software Implementation', N'Implementation', 0, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                INSERT [dbo].[Section] ([Id], [BranchId], [Code], [Name], [Remarks], [OrderNo], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_9', 1, N'CLN', N'Cleaner', N'', 0, 1, 0, N'Admin', N'20250322153045', N'', NULL, NULL, NULL)
                GO
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_97', 1, N'PF', N'FromPayroll', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20240206120603', N'', N'Admin', N'20250311144637', N'')
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_100', 1, N'PF', N'IsProfitCalculation', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20240710091848', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_101', 1, N'PF', N'AccruedByDay', N'N', N'Boolean', NULL, 1, 0, N'Admin', N'20240710091848', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_141', 1, N'PF', N'API_URL', N'http://192.168.15.100:8095/', N'string', NULL, 1, 0, N'Admin', N'20240206120603', N'', N'Admin', N'20250311144637', N'')
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_142', 0, N'PF', N'Response', N'api/Employee/GetAllEmployee', N'string', NULL, 1, 0, N'Admin', N'20240206120603', N'', N'Admin', N'20250413115419', N'')
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_143', 1, N'GF', N'GFStartFrom', N'20210701', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_105', 1, N'PF', N'IsWeightedAverageMonth', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20241118091956', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_144', 1, N'GF', N'BreakMonthCalculate', N'N', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_145', 1, N'Salary', N'DebitA/CNo', N'N/A', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_146', 1, N'Tax', N'FromDOJ', N'N', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_147', 1, N'Tax', N'InvestmentDeductionFromTax', N'N', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_148', 1, N'Salary', N'ApproverEmail', N'abhishek.srivastava@bollore.com', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_149', 1, N'HRM', N'ELBalance', N'0', N'decimal', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_150', 1, N'HRM', N'IsHolyDayLeaveSkip', N'N', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_151', 1, N'HRM', N'IsESSEditPermission', N'N', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_118', 1, N'Holiday', N'FirstHoliday', N'Friday', N'string', NULL, 1, 0, N'Admin', N'20250407111703', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_119', 1, N'Holiday', N'SecondHoliday', N'Friday', N'string', NULL, 1, 0, N'Admin', N'20250407111703', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_152', 1, N'Encashment', N'EncashmentRatio', N'50', N'decimal', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_153', 1, N'Tax', N'TaxPercentByEmployee', N'N', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_154', 1, N'Deduction', N'PunishmentFromBasic', N'Y', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_155', 1, N'Leave', N'HolidayCheck', N'N', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_156', 1, N'Attendance', N'AutoAttendanceMigration', N'Y', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_157', 1, N'Mail', N'MailSubjectTC', N'Computer Generate Tax Certificate for the preiod of vmonth', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_158', 1, N'Mail', N'MailBodyTC', N'Dear vname,Please find the attachment file of Tax Certificate for the preiod of vmonth. If you have any queries, please feel free to contact the Payroll and Reporting/ Payment Office. Kind Regards,Arifa Begum Deputy Coordinator - (Finance & Accounts)', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_159', 1, N'Attendance', N'MovementEarlyOutAllowMin', N'60', N'int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_160', 1, N'Attendance', N'MovementLateInAllowMin', N'60', N'int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_161', 1, N'Sage', N'Currency', N'BDT', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_162', 1, N'Sage', N'Username', N'-', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_163', 1, N'Sage', N'Password', N'-', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_164', 1, N'Sage', N'SourceType', N'JE', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_165', 1, N'OverTime', N'DailyOTRoundUp', N'30', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_166', 1, N'OverTime', N'MonthlyOTRoundUp', N'60', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_167', 1, N'Report', N'rptEmployeeInfo', N'rptEmployeeInfo_Kajol', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_168', 1, N'Database', N'HRMDB', N'KajolBrothersHRMDemo', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_169', 1, N'Database', N'TAXDB', N'TAX_DB', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_170', 1, N'Database', N'PFDB', N'PF_DB', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_139', 1, N'AutoUser', N'Employee', N'Y', N'string', NULL, 1, 0, N'Admin', N'20250407111703', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_140', 1, N'AutoPassword', N'Employee', N'123456', N'string', NULL, 1, 0, N'Admin', N'20250407111703', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_171', 1, N'Database', N'GFDB', N'GF_DB', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_172', 1, N'OverTime', N'CountFrom', N'Gross', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_173', 1, N'OverTime', N'CountFromDevided', N'270', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_174', 1, N'EmployeeJob', N'ProbationMonth', N'6', N'int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_175', 1, N'Leave', N'ParmanentCheck', N'Y', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_176', 1, N'Salary', N'HouseRentCalc', N'vBasic*50/100', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_177', 1, N'Salary', N'ConvenceCalc', N'vGross*8/100', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_178', 1, N'Salary', N'MedicalCalc', N'vGross*2/100', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_179', 1, N'Salary', N'SalaryFromMatrix', N'N', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_180', 1, N'SalarySheet', N'SalarySheet(1)', N'RptSalarySheetWithProject_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_181', 1, N'SalarySheet', N'SalarySheet(2)', N'RptSalarySheet_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_182', 1, N'SalarySheet', N'SalarySheet(3)', N'RptSalarySheetP1_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_183', 1, N'SalarySheet', N'SalarySheet(4)', N'RptSalarySheetP2_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_184', 1, N'SalarySheet', N'paySlip', N'RptPaySlipNew_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_185', 1, N'SalarySheet', N'paySlip(email)', N'RptPaySlipNew_Campe', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_186', 1, N'AutoCode', N'Employee', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_187', 1, N'Tax', N'AmountofExemptedIncome', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_188', 1, N'Tax', N'Divided', N'3', N'Int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_189', 1, N'Tax', N'Exempted', N'450000', N'Int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_190', 1, N'Tax', N'DividedBonus', N'2', N'Int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_191', 1, N'Tax', N'DividedMonth', N'2', N'Int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_192', 1, N'GF', N'DayWiseArear', N'Y', N'Boolean', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_193', 1, N'GF', N'YearDay', N'365', N'Int', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_194', 1, N'Appraisal', N'IncrementEffectOn', N'Basic', N'string', NULL, 1, 0, N'Admin', N'20250417094139', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_70', 1, N'PF', N'EntitleDate', N'20170701', N'varchar(14)', NULL, 1, 0, N'ADMIN', N'20191027110735', N'', N'ADMIN', N'20191027110755', N'')
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_73', 1, N'PFLoan', N'AvailableRate', N'80', N'decimal', NULL, 1, 0, N'ADMIN', N'20200322125449', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_74', 1, N'PFLoanRate', N'FromSetting', N'N', N'Boolean', NULL, 1, 0, N'ADMIN', N'20220301131241', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_75', 1, N'PFLoanRate', N'Upto12Month', N'5', N'int', NULL, 1, 0, N'ADMIN', N'20220301131241', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_76', 1, N'PFLoanRate', N'GetterThen12Month', N'6', N'int', NULL, 1, 0, N'ADMIN', N'20220301131241', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_80', 1, N'PFLoan', N'BothContributionJobAge', N'2', N'decimal', NULL, 1, 0, N'ADMIN', N'20220301131241', N'', NULL, NULL, NULL)
                INSERT [dbo].[Setting] ([Id], [BranchId], [SettingGroup], [SettingName], [SettingValue], [SettingType], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_81', 1, N'PF', N'FromDOJ', N'N', N'Boolean', NULL, 1, 0, N'ADMIN', N'20220301131241', N'', NULL, NULL, NULL)
                GO
                INSERT [dbo].[User] ([Id], [GroupId], [FullName], [Email], [LogId], [Password], [VerificationCode], [BranchId], [EmployeeId], [IsAdmin], [IsActive], [IsVerified], [IsArchived], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1', 2, N'Admin                                                                                               ', N'Admin@sym.com                                                                                       ', N'Admin                                             ', N'b1JXpoXo6qdggBV0qXQnBw==                          ', NULL, 1, N'1', 1, 1, 1, 0, N'rr                  ', N'20160315', N'192.168.15.2', N'Admin', N'20231002150017', N'')
                GO
                SET IDENTITY_INSERT [dbo].[UserGroup] ON 

                INSERT [dbo].[UserGroup] ([Id], [GroupName], [IsSuper], [IsESS], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsAdmin], [IsHRM], [IsAttendance], [IsPayroll], [IsTAX], [IsPF], [IsGF]) VALUES (2, N'Admin', 1, 0, NULL, 0, 1, N'Admin', N'11', N'11', N'Admin', N'20240825124719', N'', 1, 1, 1, 1, 1, 1, 1)
                INSERT [dbo].[UserGroup] ([Id], [GroupName], [IsSuper], [IsESS], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [IsAdmin], [IsHRM], [IsAttendance], [IsPayroll], [IsTAX], [IsPF], [IsGF]) VALUES (3, N'ESS', 0, 1, N'12', 1, 0, N'ADMIN', N'20170422', N'182.48.67.50~192.168.15.23', N'Admin', N'20240908142552', N'', 0, 0, 0, 0, 0, 0, 0)
                SET IDENTITY_INSERT [dbo].[UserGroup] OFF
                GO
                SET IDENTITY_INSERT [dbo].[UserRoles] ON 

                INSERT [dbo].[UserRoles] ([Id], [BranchId], [UserInfoId], [RoleInfoId], [IsArchived]) VALUES (1, 1, N'1_2', N'Admin', 0)
                SET IDENTITY_INSERT [dbo].[UserRoles] OFF
                GO
 ";
                #endregion TableDefaultData Back


                top2 = "go";

                IEnumerable<string> commandStringsDefaultData = Regex.Split(sqlText, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (string commandString in commandStringsDefaultData)
                {
                    if (commandString.Trim() != "")
                    {
                        SqlCommand cmdIdExist1 = new SqlCommand(commandString, currConn);

                        cmdIdExist1.Transaction = transaction;
                        transResult = (int)cmdIdExist1.ExecuteNonQuery();
                        if (transResult < 0)
                        {
                            throw new ArgumentNullException("Insert Default Data to Database'" + databaseName + "'", MessageVM.dbMsgTableDefaultData);
                        }
                    }
                }

                #endregion TableCreate



                #region Insert Fiscal Year
                //                foreach (var Item in fiscalDetails.ToList())
//                {

//                    #region Insert only DetailTable

//                    sqlText = "";
//                    sqlText += " insert into FiscalYear(";
//                    sqlText += " FiscalYearName,";
//                    sqlText += " CurrentYear,";
//                    sqlText += " PeriodID,";
//                    sqlText += " PeriodName,";
//                    sqlText += " PeriodStart,";
//                    sqlText += " PeriodEnd,";
//                    sqlText += " PeriodLock,";
//                    sqlText += " GLLock,";
//                    sqlText += " CreatedBy,";
//                    sqlText += " CreatedOn,";
//                    sqlText += " LastModifiedBy,";
//                    sqlText += " LastModifiedOn";

//                    sqlText += " )";
//                    sqlText += " values(	";

//                    sqlText += "'" + Item.FiscalYearName + "',";
//                    sqlText += "'" + Item.CurrentYear + "',";
//                    sqlText += "'" + Item.PeriodID + "',";
//                    sqlText += "'" + Item.PeriodName + "',";
//                    sqlText += "'" + Item.PeriodStart + "',";
//                    sqlText += "'" + Item.PeriodEnd + "',";
//                    sqlText += "'" + Item.PeriodLock + "',";
//                    sqlText += "'" + Item.GLLock + "',";
//                    sqlText += "'SuperAdmin',";
//                    sqlText += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
//                    sqlText += "'SuperAdmin',";
//                    sqlText += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

//                    sqlText += ")	";


//                    SqlCommand cmdInsDetail = new SqlCommand(sqlText, currConn);
//                    cmdInsDetail.Transaction = transaction;
//                    transResult = (int)cmdInsDetail.ExecuteNonQuery();

//                    if (transResult <= 0)
//                    {
//                        throw new ArgumentNullException("Insert Fiscal Year data to Database('" + databaseName + "')", MessageVM.dbMsgCFiscalYearNotSave);
//                    }
//                    #endregion Insert only DetailTable
//                }


                #endregion Insert Sys DB Information

                #region Commit

                if (transaction != null)
                {
                    transaction.Commit();
                    #region SuccessResult

                    retResults[0] = "Success";
                    retResults[1] = "Requested Company Created successfully.";
                    retResults[2] = newID;
                    #endregion SuccessResult

                }

                #endregion Commit
            }
            #endregion Try

            #region Catch and Finall
           
            catch (Exception ex)
            {

                currConn.Close();
                currConn.Open();
                currConn.ChangeDatabase("master");
                #region check Database and delete


                sqlText = "";
                sqlText += " USE [master]";
                sqlText += " drop DATABASE " + databaseName + "";

                SqlCommand cmdIdExist = new SqlCommand(sqlText, currConn);
                cmdIdExist.Transaction = transaction;
                cmdIdExist.ExecuteNonQuery();

                #endregion check Database
            

                throw ex;
            }
            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();

                    }
                }

            }
            #endregion Catch and Finall

            #region Result
            return retResults;
            #endregion Result
        }

        public int TableFieldAdd(string TableName, string FieldName, string DataType, SqlConnection VcurrConn, SqlTransaction Vtransaction, string defaultValue = "", SysDBInfoVMTemp connVM = null)
        {
            #region Initializ
            string sqlText = "";
            int transResult = 0;

            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            #endregion

            #region Try

            try
            {

                #region Validation
                if (string.IsNullOrEmpty(TableName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(1)");

                }
                else if (string.IsNullOrEmpty(FieldName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(2)");

                }
                else if (string.IsNullOrEmpty(DataType))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(3)");

                }

                #endregion Validation

                #region open connection and transaction

                #region New open connection and transaction

                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }

                #endregion New open connection and transaction

                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection(connVM);
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }

                #endregion open connection and transaction

                #region Prefetch

                sqlText = "";
                sqlText += " if not exists(select * from sys.columns ";
                sqlText += " where Name = N'" + FieldName.Replace("[", "").Replace("]", "") + "' and Object_ID = Object_ID(N'" + TableName + "'))   ";
                sqlText += " begin";
                sqlText += " ALTER TABLE " + TableName + " ADD " + FieldName + " " + DataType + "";
                if (!string.IsNullOrWhiteSpace(defaultValue))
                {
                    if (!defaultValue.Contains("'"))
                    {
                        defaultValue = "'" + defaultValue + "'";
                    }
                    sqlText += "  default " + defaultValue + " WITH VALUES";

                }
                sqlText += " END";

                SqlCommand cmdPrefetch = new SqlCommand(sqlText, currConn);

                //cmdPrefetch.ExecuteScalar();
                cmdPrefetch.CommandTimeout = 500;
                cmdPrefetch.Transaction = transaction;
                transResult = (int)cmdPrefetch.ExecuteNonQuery();

                #endregion Prefetch

                #region Commit

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }

                #endregion


            }

            #endregion try

            #region Catch and Finall

            catch (Exception ex)
            {
                throw new ArgumentNullException("", ex.Message.ToString());

            }

            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }

            }

            #endregion

            return transResult;
        }


        public void TableFieldAlter(string TableName, string FieldName, string DataType, SqlConnection VcurrConn, SqlTransaction Vtransaction, SysDBInfoVMTemp connVM = null)
        {
            #region Initializ
            string sqlText = "";

            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            #endregion

            #region Try

            try
            {

                #region Validation
                if (string.IsNullOrEmpty(TableName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(1)");

                }
                else if (string.IsNullOrEmpty(FieldName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(2)");

                }
                else if (string.IsNullOrEmpty(DataType))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(3)");

                }

                #endregion Validation


                #region open connection and transaction

                #region New open connection and transaction

                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }

                #endregion New open connection and transaction

                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection(connVM);
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }

                #endregion open connection and transaction


                #region Prefetch

                sqlText = "";
                sqlText += " ALTER TABLE " + TableName + " ALTER COLUMN " + FieldName + "   " + DataType + "";
                SqlCommand cmdPrefetch = new SqlCommand(sqlText, currConn);
                cmdPrefetch.CommandTimeout = 500;
                cmdPrefetch.Transaction = transaction;
                cmdPrefetch.ExecuteScalar();
                #endregion Prefetch

                #region Commit

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }

                #endregion

            }

            #endregion try

            #region Catch and Finall

            catch (SqlException sqlex)
            {              
                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
                //////throw sqlex;
            }
            catch (Exception ex)
            {              
                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
                //////throw ex;
            }

            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            #endregion
        }
        public int NewTableExistCheck(string TableName, SqlConnection VcurrConn, SqlTransaction Vtransaction, SysDBInfoVMTemp connVM = null)
        {
            #region Initializ
            string sqlText = "";
            int transResult = 0;
            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            #endregion

            #region Try

            try
            {

                #region Validation
                if (string.IsNullOrEmpty(TableName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(1)");

                }

                //else if (string.IsNullOrEmpty(DataType))
                //{
                //    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(3)");

                //}

                #endregion Validation
                #region open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }

                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnectionNoPooling(connVM);
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Prefetch

                sqlText = "";

                sqlText += " IF  EXISTS (SELECT * FROM sys.objects ";
                sqlText += " WHERE object_id = OBJECT_ID(N'" + TableName + "') AND type in (N'U'))";

                sqlText += " BEGIN Select 1 END";
                sqlText += " else BEGIN Select 0 END";

                SqlCommand cmdPrefetch = new SqlCommand(sqlText, currConn);

                //cmdPrefetch.ExecuteScalar();
                cmdPrefetch.Transaction = transaction;
                transResult = (int)cmdPrefetch.ExecuteScalar();

                #endregion Prefetch
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
            }

            #endregion try

            #region Catch and Finall

            catch (SqlException sqlex)
            {
                if (transaction != null && Vtransaction == null) { transaction.Rollback(); }             
                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
                //////throw sqlex;
            }
            catch (Exception ex)
            {
                if (transaction != null && Vtransaction == null) { transaction.Rollback(); }
             
                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
                //////throw ex;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion

            return transResult;
        }
        public int NewTableAdd(string TableName, string createQuery, SqlConnection VcurrConn, SqlTransaction Vtransaction, SysDBInfoVMTemp connVM = null)
        {
            #region Initializ
            string sqlText = "";
            int transResult = 0;
            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            #endregion

            #region Try

            try
            {

                #region Validation
                if (string.IsNullOrEmpty(TableName))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(1)");

                }
                else if (string.IsNullOrEmpty(createQuery))
                {
                    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(2)");

                }
                //else if (string.IsNullOrEmpty(DataType))
                //{
                //    throw new ArgumentNullException("TransactionCode", "Unable to Create ID(3)");

                //}

                #endregion Validation
                #region open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }

                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnectionNoPooling(connVM);
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Prefetch

                sqlText = "";

                sqlText += " IF  NOT EXISTS (SELECT * FROM sys.objects ";
                sqlText += " WHERE object_id = OBJECT_ID(N'" + TableName + "') AND type in (N'U'))";

                sqlText += " BEGIN";
                sqlText += " " + createQuery;
                sqlText += " END";

                SqlCommand cmdPrefetch = new SqlCommand(sqlText, currConn);
                cmdPrefetch.CommandTimeout = 500;
                //cmdPrefetch.ExecuteScalar();
                cmdPrefetch.Transaction = transaction;
                transResult = (int)cmdPrefetch.ExecuteNonQuery();

                #endregion Prefetch
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
            }

            #endregion try

            #region Catch and Finall

            catch (SqlException sqlex)
            {
                if (transaction != null && Vtransaction == null) { transaction.Rollback(); }

              

                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
                //////throw sqlex;
            }
            catch (Exception ex)
            {
                if (transaction != null && Vtransaction == null) { transaction.Rollback(); }

             

                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
                //////throw ex;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion

            return transResult;
        }

        public string GetServerHardwareId(SysDBInfoVMTemp connVM = null)
        {
            #region Initializ

            string retResults = string.Empty;
            SqlConnection currConn = null;
            string sqlText = @"EXEC xp_instance_regread
                                'HKEY_LOCAL_MACHINE',
                                'HARDWARE\DESCRIPTION\System\MultifunctionAdapter\0\DiskController\0\DiskPeripheral\0',
                                'Identifier'";
            #endregion Initializ

            #region Try
            try
            {
                currConn = _dbsqlConnection.GetConnection(connVM);
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                DataTable dt = new DataTable("ServerProcessor");

                //SqlCommand getHardware = new SqlCommand(sqlText, currConn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlText, currConn);
                adapter.Fill(dt);

                if (dt == null)
                {
                    retResults = string.Empty;
                }
                else if (dt.Columns.Count > 0)
                {
                    retResults = dt.Rows[0][1].ToString();
                }


            }
            #endregion Try

            #region Catch and Finall

            catch (SqlException sqlex)
            {
              

                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
              
                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }
            finally
            {
                //if (currConn == null)
                //{
                if (currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
                //}
            }

            #endregion

            #region Results

            return retResults;

            #endregion

        }

        public DataSet CompanyList(string ActiveStatus, SysDBInfoVMTemp connVM = null, string CompanyList = "")
        {

            #region Initializ

            string SysVersion = "1991";

            if (PFServer.Ordinary.DBConstant.IsProjectVersion2012)
            {
                SysVersion = "2012";
            }

            SqlConnection currConn = null;
            string sqlText = "";
            DataSet dataTable = new DataSet();

            #endregion

            #region try

            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnectionSys(connVM);//

                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                sqlText = @"

    SELECT 
    CompanySl,
    CompanyID,
    CompanyName,
    DatabaseName,
    ActiveStatus,
    Serial
    FROM CompanyInformations
    where (ActiveStatus = @ActiveStatus)	
     and  (SysVersion = @SysVersion)	
    and (CompanyName<>'NA')

";

                if (CompanyList.ToLower() != "all")
                {
                    List<string> result = CompanyList.Split(',').ToList();

                    sqlText += " and DatabaseName in (";

                    foreach (string Company in result)
                    {
                        sqlText += "'" + Converter.DESEncrypt(DBConstant.PassPhrase, DBConstant.EnKey, Company) + "',";
                    }

                    sqlText = sqlText.TrimEnd(',') + ")";

                }

                sqlText += @" 
    ORDER BY ISNULl(serial,CompanySL) asc;

";

                //SysVersion = "2020";

                SqlCommand objCommCompanyList = new SqlCommand();
                objCommCompanyList.Connection = currConn;
                objCommCompanyList.CommandText = sqlText;
                objCommCompanyList.CommandType = CommandType.Text;


                if (!objCommCompanyList.Parameters.Contains("@ActiveStatus"))
                { objCommCompanyList.Parameters.AddWithValue("@ActiveStatus", ActiveStatus); }
                else { objCommCompanyList.Parameters["@ActiveStatus"].Value = ActiveStatus; }

                if (!objCommCompanyList.Parameters.Contains("@SysVersion"))
                { objCommCompanyList.Parameters.AddWithValue("@SysVersion", SysVersion); }
                else { objCommCompanyList.Parameters["@SysVersion"].Value = SysVersion; }

                SqlDataAdapter dataAdapter = new SqlDataAdapter(objCommCompanyList);

                dataAdapter.Fill(dataTable);

            }
            #endregion

            #region Catch
            catch (SqlException sqlex)
            {
                

                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
                //////throw sqlex;
            }
            catch (Exception ex)
            {
             

                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
                //////throw ex;
            }
            #endregion

            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }
            #endregion

            return dataTable;
        }
     

        public DataTable SuperAdministrator(SysDBInfoVMTemp connVM = null)
        {

            #region Objects & Variables

            string Description = "";

            SqlConnection currConn = null;
            int transResult = 0;
            int countId = 0;
            string sqlText = "";

            DataTable dataTable = new DataTable("SA");
            #endregion
            #region try
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnectionSys(connVM);//
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction
                #region sql statement
                sqlText = @"
                            SELECT miki as [user],mouse as [pwd]
                 FROM SuperAdministrator";

                SqlCommand objCommProductType = new SqlCommand();
                objCommProductType.Connection = currConn;
                objCommProductType.CommandText = sqlText;
                objCommProductType.CommandType = CommandType.Text;



                SqlDataAdapter dataAdapter = new SqlDataAdapter(objCommProductType);
                dataAdapter.Fill(dataTable);
                #endregion
            }
            #endregion
            #region catch

            catch (SqlException sqlex)
            {
               

                throw new ArgumentNullException("", sqlex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
                //////throw sqlex;
            }
            catch (Exception ex)
            {
               

                throw new ArgumentNullException("", ex.Message.ToString());

                ////throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
                //////throw ex;
            }
            #endregion
            #region finally
            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }
            #endregion
            return dataTable;
        }


        public string[] DefaultDataSave(SysDBInfoVMTemp connVM = null)
        {
            #region Initializ

            string[] retResults = new string[3];
            retResults[0] = "Fail";
            retResults[1] = "Fail";
            retResults[2] = "";

            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            int countId = 0;
            string sqlText = "";
            string nextId = "";
            string newID = "";

            #endregion Initializ

            #region Try
            try
            {

                #region open connection and transaction sys / Master

                SysDBInfoVM.SysDatabaseName = "";

                currConn = _dbsqlConnection.GetConnectionSys(connVM);//start
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region Change Database for New DB

                transaction = currConn.BeginTransaction(MessageVM.dbMsgMethodName);
                #endregion open connection and transaction


                #region CreateTable Back
                sqlText = @"
INSERT [dbo].[Department] ([Id], [BranchId], [Code], [Name], [OrderNo], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom]) VALUES (N'1_1', 1, N'DEV', N'Development', 7, N'Test', 1, 0, N'Admin', N'20250320095411', N'', NULL, NULL, NULL)

INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_1', 1, N'HoPS', N'Head of Priority Services', NULL, 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_10', 1, N'', N'In-Charge, Aparajita Female Trading Booth-Gulshan', N'', 1, 1, N'Admin', N'19000101', N'local', N'ADMIN', N'20210228114849', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_100', 1, N'IMP-E', N'Executive -VAT & ERP', NULL, 1, 0, N'ADMIN', N'20210228121138', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_101', 1, N'IT-E', N'Executive IT & Networking', NULL, 1, 0, N'ADMIN', N'20210228121221', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_102', 1, N'DV-S', N'Sr. Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121254', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_103', 1, N'DV-D', N'Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121334', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_104', 1, N'A&A', N'Jr. Executive Admin & Accounts', NULL, 1, 0, N'ADMIN', N'20210228121409', N'', N'ADMIN', N'20210228130006', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_105', 1, N'GD', N'Graphics designer', NULL, 1, 0, N'ADMIN', N'20210228121437', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_106', 1, N'DV-Jr', N'Jr. Software Developer', NULL, 1, 0, N'ADMIN', N'20210228121625', N'', N'ADMIN', N'20230105123016', N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, 0, N'1_4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Designation] ([Id], [BranchId], [Code], [Name], [Remarks], [IsActive], [IsArchive], [CreatedBy], [CreatedAt], [CreatedFrom], [LastUpdateBy], [LastUpdateAt], [LastUpdateFrom], [AttendenceBonus], [EPZ], [Other], [DinnerAmount], [IfterAmount], [TiffinAmount], [ETiffinAmount], [OTAlloawance], [OTOrginal], [OTBayer], [ExtraOT], [PriorityLevel], [OrderNo], [DesignationGroupId], [GradeId], [HospitalPlanC1], [HospitalPlanC2], [HospitalPlanC3], [HospitalPlanC4], [HospitalPlanC5], [DeathCoveragePlanC6], [MaternityPlanC7], [MaternityPlanC8], [MaternityPlanC9], [EntitlementC1], [EntitlementC2], [EntitlementC3], [EntitlementC4], [EntitlementC5], [MobileExpenseC1], [MobileExpenseC2], [MobileExpenseC3], [MobileExpenseC4], [InternationalTravelC1], [InternationalTravelC2], [InternationalTravelC3], [DomesticlTravelC1], [DomesticTravelC2], [DomesticTravelC3], [DomesticTravelC4], [DomesticTravelC5]) VALUES (N'1_107', 1, N'MK-E', N'Executive -Marketing', NULL, 1, 0, N'ADMIN', N'20210228121727', N'', NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)

                ";
                SqlCommand cmdPrefetch = new SqlCommand(sqlText, currConn);
                cmdPrefetch.CommandTimeout = 500;
                cmdPrefetch.Transaction = transaction;
                transResult = (int)cmdPrefetch.ExecuteNonQuery();
                #endregion CreateTable



                #region Commit

                if (transaction != null)
                {
                    transaction.Commit();
                    #region SuccessResult

                    retResults[0] = "Success";
                    retResults[1] = "Requested Company Created successfully.";
                    retResults[2] = newID;
                    #endregion SuccessResult

                }

                #endregion Commit
            }
            #endregion Try

            #region Catch and Finall

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();

                    }
                }

            }
            #endregion Catch and Finall

            #region Result
            return retResults;
            #endregion Result
        }


    }
}
