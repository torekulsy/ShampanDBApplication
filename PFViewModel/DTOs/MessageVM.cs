﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFViewModel.DTOs
{
   public class MessageVM
    {
        #region Customer Item

        public static string CustomerItemMsgMethodNameInsert = "CustomerItemInsert";
        public static string CustomerItemMsgMethodNameUpdate = "CustomerItemUpdate";
        public static string CustomerItemMsgMethodNamePost = "CustomerItemPost";

        public static string customerItemMsgFindExistID = "Requested customer Information already Added";

        public static string MsgFindExistID = "Requested Information already Added";

        #endregion

        public static string msgRestQnotavailable = "Quantity is more then Original Quantity";

        public static string msgExcelTableNotFound = "SaleM sheet not Found in the Excel !";

        public static string msgSelectPostedtransaction = "Please Select Posted transaction ";

        public static string BENumberAlreadyExit = "BENumber/VAT 6.3 Number already exits";

        public static string purchaseMsgMethodNameBENumbercheck = "BENumbercheck";

        public static string msgDelete = "Delete Successful.";

        public static string MsgMultipleHscode = "There Are Multiple Hscode For This Product , Please Check Properly.";


        #region BankChannel Payment

        public static string BankChannelmsgMethodNameInsert = "InsertBankChannelPayment";
        public static string BankChannelmsgMethodNameUpdate = "UpdateBankChannelPayment";
        public static string BankChannelmsgMethodNameDelete = "DeleteBankChannelPayment";
        public static string BankChannelmsgMethodNameSearch = "SearchBankChannelPayment";
        public static string BankChannelmsgMethodNamePost = "PostBankChannelPayment";

        public static string BankChannelmsgPleaseEnterBankInformation = "Please Enter Bank Information";
        public static string BankChannelmsgPleaseSelectPurchaseInvoiceNo = "Please Select Purchase InvoiceNo";
        public static string BankChannelmsgPleaseEnterPaymentAmount = "Please Enter Payment Amount";
        public static string BankChannelmsgPleaseEnterVATAmount = "Please Enter VAT Payment";
        public static string BankChannelmsgPleaseSelectPaymentType = "Please Select Payment Type";

        #endregion

        public static string CodeNotFound = "Code Not Found";
        public static string msgDataAlreadyUsed = "The Information you want to delete already user in";
        public static string msgDeleteOperationterminated = "\nDelete Operation is terminated";

        public static string msgNegPrice = "Price can not be negative or zero";

        public static string msgSettingValueNotSave = "Settings Value Not Save, Please contact with Administrator";

        public static string msgNotPost = "Transaction not Saved, Please Save first";

        public static string msgWantToPost = "Do you want to Post this transaction?";
        public static string msgWantToPost1 = "After posted no changes will be performed";
        public static string msgNotPostAccess = "You do not have the post permission, please contact with administrator";

        public static string ThisTransactionWasPosted = "This transaction was posted";
        public static string UnableUpdate = "Unable to Update requested information";

        public static string msgWantToRemoveRow = "Do you want to Remove this transaction?";

        public static string msgNotAddAccess = "You do not have the add permission, please contact with administrator";
        public static string msgNotEditAccess = "You do not have the update permission, please contact with administrator";
        public static string SelfBranchNotMatch = "This Information not for this Branch";

        public static string msgWantToRebate = "Do you want to Rebate this transaction?";

        public static string msgWantToSave = "Do you want to Save this transaction?";


        #region AdjustmentName

        public static string AdjmsgMethodNameInsert = "InsertAdjustmentName";

        public static string AdjmsgMethodNameUpdate = "UpdateAdjustmentName";
        public static string AdjmsgMethodNameDelete = "DeleteAdjustmentName";
        public static string AdjmsgMethodNameSearch = "SearchAdjustmentName";

        #endregion AdjustmentName

        #region BranchName
        public static string BranchMsgMethodNameInsert = "InsertBranchName";

        public static string BranchMsgMethodNameUpdate = "UpdateBranchName";
        public static string BranchMsgMethodNameDelete = "Delete";
        public static string BranchMsgMethodNameSearch = "SearchBranchName";
        #endregion BranchName

        #region BranchName
        public static string BranchReportMsgMethodNameInsert = "InsertBranchName";

        public static string BranchReportMsgMethodNameUpdate = "UpdateBranchName";
        public static string BranchReportMsgMethodNameDelete = "Delete";
        public static string BranchReportMsgMethodNameSearch = "SearchBranchName";
        #endregion BranchName

        #region New DatabaseCreate
        public static string dbMsgMethodName = "NewDBCreate";

        public static string dbMsgNoCompanyName = "Unable to find Company Name, Please input Name";
        public static string dbMsgNoFiscalYear = "Unable to find Fiscal Year, Please input Fiscal year";
        public static string dbMsgNoCompanyInformation = "Unable to find Company Information, Please input Company Detail";
        public static string dbMsgCompanyInformationNotSave = "Unable to find Company Information";
        public static string dbMsgCFiscalYearNotSave = "Unable to find Fiscal Year Information";
        public static string dbMsgDBExist = "Database Already Exist";
        public static string dbMsgDBInfoInsert = "Database Information not save";
        public static string dbMsgDBNotCreate = "Database not create";
        public static string dbMsgTableNotCreate = "Table not create";
        public static string dbMsgTableDefaultData = "Default Data not saved";

        #endregion New DatabaseCreate

        public static string msgTransactionNotDeclared = "Transaction Not Declared ";
        public static string msgCPCNameNotAdd = " CPC Name is Mandatory, Please add CPC Name First ";
        public static string msgQuantitycheck = " For Credit Note Quatity Exceded";


        #region Purchase

        public static string PurchasemsgNoDataToSave = "There is no data to save";
        public static string PurchasemsgNoDataPostToPost = "There is no data to Post";
        public static string PurchasemsgNoDataToSaveImportDuties = "There is no data to save the Import Duties";
        public static string PurchasemsgMethodNameInsert = "PurchaseInsert";
        public static string PurchasemsgMethodNameUpdate = "PurchaseUpdate";
        public static string PurchasemsgUnableCreatID = "Unable to create new ID";
        public static string PurchasemsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string PurchasemsgUnableToSaveIssue = "Requested Information not added, transaction not complete(1)";
        public static string PurchasemsgUnableToSaveReceive = "Requested Information not added, transaction not complete(2)";
        public static string PurchasemsgSaveSuccessfully = "Requested Information successfully Added";
        public static string PurchasemsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string PurchasemsgSaveNotSuccessfully = "Requested Information not Added";
        public static string PurchasemsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string PurchasemsgFindExistID = "Requested Information already Added";
        public static string PurchasemsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string PurchasemsgNoDataToUpdateImportDyties = "There is no data to Update, Please Save first.";
        public static string PurchasemsgUnableToUpdateIssue = "Requested Information not Update, transaction not complete(1)";
        public static string PurchasemsgUnableToUpdateReceive = "Requested Information not Update, transaction not complete(2)";
        public static string PurchasemsgDutyIdNotCreate = "Unable to create ID";


        #region Post

        public static string purchaseMsgMethodNamePost = "PurchasePost";
        public static string purchaseMsgNoDataToPost = "There is no data to Post";
        public static string purchaseMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string purchaseMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string purchaseMsgPostNotSuccessfully = "Requested Information not Post";
        public static string purchaseMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string purchaseMsgPostNotSelect = "Requested Information not Post";
        public static string purchaseMsgSuccessfullyPost = "Requested Information successfully Posted";
        public static string purchaseMsgUnableToPostIssue = "Requested Information not Post, transaction not complete(1)";
        public static string purchaseMsgUnableToPostReceive = "Requested Information not Post, transaction not complete(2)";

        #endregion Post

        #endregion Purchase
        public static string CPCmsgMethodNameInsert = "CPCDetailsInsert";

        #region Dispose
        public static string disposeMsgNoDataToSave = "There is no data to save";
        public static string disposeMsgMethodNameInsert = "DisposeInsert";
        public static string disposeMsgMethodNameUpdate = "DisposeUpdate";
        public static string disposeMsgUnableCreatID = "Unable to create new ID";
        public static string disposeMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string disposeMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string disposeMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string disposeMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string disposeMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string disposeMsgFindExistID = "Purchase Requested Information already Added";
        public static string disposeMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string disposeMsgCheckDate = "Please Check Transaction Date";


        #region Post
        public static string disposeMsgMethodNamePost = "DisposePost";
        public static string disposeMsgNoDataToPost = "There is no data to Post";
        public static string disposeMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string disposeMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string disposeMsgPostNotSuccessfully = "Requested Information not Post";
        public static string disposeMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string disposeMsgPostNotSelect = "Requested Information not Post";
        public static string disposeMsgSuccessfullyPost = "Requested Information successfully Posted";
        public static string disposeMsgUnableToPostIssue = "Requested Information not Post, transaction not complete(1)";
        public static string disposeMsgUnableToPostReceive = "Requested Information not Post, transaction not complete(2)";

        #endregion Post

        #endregion Dispose

        #region Tender Price
        //Complete
        public static string tpMsgMethodNameInsert = "TenderPriceInsert";
        public static string tpMsgMethodNameUpdate = "TenderPriceUpdate";
        public static string tpMsgFindExistID = "Tender Requested Information already Added";
        public static string tpMsgIDNotCreate = "ID can't generated, Transaction not complete";
        public static string tpMsgFindRef = "Requested Reference Number already Added";
        public static string tpMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string tpMsgNoDataToSave = "There is no data to save";
        public static string tpMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string tpMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string tpMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string tpMsgUpdateSuccessfully = "Requested Information successfully Updated";

        #endregion Tender Price

        //Toll
        public static string tollMsgMethodNameUpdate = "Data Update Successfully";
        public static string tollMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string tollMsgSuccessfullyPost = "Requested Information successfully Posted";
        public static string tollMsgSuccessfullyPostFail = "Data Already Posted";
        public static string tollMsgNoDataToSave = "There is no data to save";
        public static string tollMsgMethodNameInsert = "Toll Insert";
        public static string tollMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string tollMsgMethodNameUpdateData = "TollUpdate";
        public static string tollMsgMethodNamePost = "TollPost";
        public static string tollMsgNoDataToPost = "There is no data to Post";
        public static string tollMsgCheckDatePost = "Please Check Transaction Date for Post";

        //TollOut
        public static string tollOutMsgMethodNameInsert = "TollOutInsert";
        public static string tollOutMsgNoDataToSave = "There is no data to save";
        public static string tollOutMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string tollOutMethodNameUpdate = "Data Update Successfully";
        public static string tollOutNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string tollOutMsgMethodNameUpdate = "TollOut Update";
        public static string tollOutMsgPostFail = "Data Already Posted";
        public static string tollOutMsgMethodNamePost = "TollOut Post";
        public static string tollOutMsgNoDataToPost = "There is no data to Post";


        //TollClint
        public static string tollClientMsgMethodNameInsert = "TollClient Insert";
        public static string tollClientMsgNoDataToSave = "There is no data to save";
        public static string tollClientMsgMethodNameUpdate = "Data Update Successfully";
        public static string tollClientMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string tollClientMsgNoDataToPost = "There is no data to Post";
        public static string tollClientMsgSuccessfullyPostFail = "Data Already Posted";

        #region Issue

        public static string issueMsgNoDataToSave = "There is no data to save";
        public static string issueMsgMethodNameInsert = "IssueInsert";
        public static string issueMsgMethodNameUpdate = "IssueUpdate";

        public static string issueMsgUnableCreatID = "Unable to create new ID";
        public static string issueMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string issueMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string issueMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string issueMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string issueMsgUpdateNotSuccessfully = "Requested Information not Update";

        public static string issueMsgFindExistID = "Issue Requested Information already Added";
        public static string issueMsgNoDataToUpdate = "There is no data to Update, Please Save first.";


        #region Post
        public static string issueMsgMethodNamePost = "IssuePost";
        public static string issueMsgNoDataToPost = "There is no data to Post";
        public static string issueMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string issueMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string issueMsgPostNotSuccessfully = "Requested Information not Post";
        public static string issueMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string issueMsgPostNotSelect = "Requested Information not Post";
        public static string issueMsgSuccessfullyPost = "Requested Information successfully Posted";


        #endregion Post

        #endregion Issue

        #region Deposit and VDS

        public static string depMsgNoDataToSave = "There is no data to save";

        public static string depMsgMethodNameInsert = "Insert";
        public static string depMsgMethodNameUpdate = "Update";
        public static string dpMsgFindExistID = "Deposit or VDS Requested Information already Added";
        public static string dpMsgFindExistIDP = "Requested VDS Information already Added form this Purchase";
        public static string dpMsgUnableCreatID = "Unable to create new ID";
        public static string dpMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string dpMsgSaveSuccessfully = "Requested Information successfully Added";

        public static string dpMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string dpMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string dpMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string dpMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string dpMsgPostSuccessfully = "Requested Information successfully Post";



        #endregion Deposit and VDS


        #region Sale
        public static string saleMsgNoDataToSave = "There is no data to save";
        public static string saleMsgMethodNameInsert = "SaleInsert";
        public static string insert = "Insert";
        public static string saleDeliveryMsgMethodNameInsert = "DeliverySaleInsert";
        public static string saleMsgMethodNameUpdate = "SaleInsert";

        public static string saleMsgUnableCreatID = "Unable to create new ID";
        public static string saleMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string saleMsgUnableToSaveIssue = "Requested Information not added, transaction not complete(1)";
        public static string saleMsgUnableToSaveReceive = "Requested Information not added, transaction not complete(2)";
        public static string saleMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string saleMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string saleMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string saleMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string mailSend = "Mail Send Successful.";

        public static string saleMsgFindExistID = "Sales Requested Information already Added";
        public static string msgFindExistID = "Requested Code already Exist!";
        public static string msgFindDayEndExistSameDate = "Already Data Exist for This Day!";
        public static string msgFindExistSame = "Already Data Exist!";

        public static string saleDeliveryMsgFindExistID = "Delivery Challan Sales Requested Information already Added";


        public static string saleMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string saleMsgUnableToUpdateIssue = "Requested Information not Update, transaction not complete(1)";
        public static string saleMsgUnableToUpdateReceive = "Requested Information not Update, transaction not complete(2)";

        #region Post
        public static string saleMsgMethodNamePost = "SalePost";
        public static string saleMsgNoDataToPost = "There is no data to Post";
        public static string saleMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string saleMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string saleMsgPostNotSuccessfully = "Requested Information not Post";
        public static string saleMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string saleMsgPostNotSelect = "Requested Information not Post";
        public static string saleMsgSuccessfullyPost = "Requested Information successfully Posted";

        public static string saleMsgUnableToPostIssue = "Requested Information not Post, transaction not complete(1)";
        public static string saleMsgUnableToPostReceive = "Requested Information not Post, transaction not complete(2)";
        public static string msgprevoisUnPostTransaction = "Please Post Prevoious Date Transaction First!";

        #endregion Post

        #endregion Sale
        #region Service Price Decleration
        //Complete
        public static string spMsgMethodNameInsert = "ServiceInsert";
        public static string spMsgMethodNameUpdate = "ServiceUpdate";
        public static string spMsgNoDataToSave = "There is no data to save";
        public static string spMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string spMsgSaveSuccessfully = "Requested Information successfully Added";

        public static string spMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string spMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string spMsgUpdateSuccessfully = "Requested Information successfully Update";


        #endregion Service Price Decleration

        #region Receive
        public static string receiveMsgNoDataToSave = "There is no data to save";
        public static string receiveMsgMethodNameInsert = "ReceiveInsert";
        public static string receiveMsgMethodNameUpdate = "ReceiveUpdate";

        public static string receiveMsgUnableCreatID = "Unable to create new ID";
        public static string receiveMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string receiveMsgUnableToSaveIssue = "Requested Information not added, transaction not complete(1)";
        public static string receiveMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string receiveMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string receiveMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string receiveMsgUpdateNotSuccessfully = "Requested Information not Update";

        public static string receiveMsgFindExistID = "Receive Requested Information already Added";


        public static string receiveMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string receiveMsgUnableToUpdateIssue = "Requested Information not Update, transaction not complete(1)";
        public static string receiveMsgUnableToUpdateReceive = "Requested Information not Update, transaction not complete(2)";


        #region Post
        public static string receiveMsgMethodNamePost = "ReceivePost";
        public static string receiveMsgNoDataToPost = "There is no data to Post";
        public static string receiveMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string receiveMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string receiveMsgPostNotSuccessfully = "Requested Information not Post";
        public static string receiveMsgUnableToIssuePost = "Requested Information not Post, transaction not complete(1)";
        public static string receiveMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string receiveMsgPostNotSelect = "Requested Information not Post";
        public static string receiveMsgSuccessfullyPost = "Requested Information successfully Posted";

        #endregion Post
        #endregion Receive
        #region BOM
        public static string msgVatNameNotFound = "Unable to find VAT Name, Please Provide VAT Name";
        public static string msgUpdateNBRPrice = "Unable to Save BOM";
        public static string bomMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string bomMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string bomMsgMethodNameInsert = "BOMInsert";


        #endregion BOM

        #region FiscalYear

        // Complete
        public static string FYMsgMethodNameInsert = "Fiscal Year Insert";
        public static string FYMsgMethodNameUpdate = "Fiscal Year Update";

        public static string FYMsgMethodNameProcess = "ExecuteYearProcess";

        public static string FYMsgAlreadyExist = "Fiscal Year Requested Information already Added, Transaction not complete";
        public static string FYMsgPreviouseYearNotLock = " Please Lock previous year first, Transaction not complete";
        public static string FYMsgNoDataToSave = "There is no data to save";
        public static string FYMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string FYMsgNotExist = "Requested Information not find, Transaction not complete";
        public static string FYMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string FYMsgNoDataToUpdate = "There is no data to Update, Please Save first.";
        public static string FYMsgUpdateNotSuccessfully = "Requested Information not Update";
        public static string FYMsgUpdateSuccessfully = "Requested Information successfully Update";

        public static string FYMsgIDNotExist = "Requested Information not find, Transaction not complete";
        public static string FYMsgIDUpdateNotSuccessfully = "Requested Information not Update";
        public static string FYMsgIDUpdateSuccessfully = "Requested Information successfully Update";


        public static string msgFiscalYearisLock = "Fiscal calender is locked, transaction not complete";
        public static string msgFiscalYearNotExist = "Fiscal calender not created, transaction not complete";

        public static string msgFiscalYearNotGenerate = "Fiscal Year Not Generate ";



        #endregion FiscalYear

        #region FiscalPeriod

        public static string msgFiscalPeriodNotGenerate = "Fiscal Period Not Generate ";
        public static string msgFiscalPeriodisLock = "Fiscal Period is locked";


        #endregion FiscalPeriod

        #region Release Note

        public static string msgAlpha =
            @"The alpha phase of the release life cycle is the first phase to begin software testing.
In this phase, developers generally test the software. It may contain known bugs, and almost certainly contains
unknown bugs. Features intended for the final release may be incomplete or missing.
Alpha software can be unstable and could cause crashes or data loss.";

        public static string msgBeta =
            @"Beta is the software development phase following alpha.
It generally begins when the software is feature complete. 
Software in the beta phase will generally have many more bugs in it than completed software, 
as well as speed/performance issues and may still cause crashes or data loss.
The focus of beta testing is reducing impacts to users, often incorporating usability testing. 
The process of delivering a beta version to the users is called beta release and this is typically 
the first time that the software is available outside of the development team.";

        public static string msgAccessPermision =
            "You do not have to access this form. Please contact with administrator";


        #endregion

        #region Demand

        public static string demandMsgNoDataToSave = "There is no data to save";
        public static string demandMsgMethodNameInsert = "DemandInsert";
        public static string demandMsgMethodNameUpdate = "DemandUpdate";

        public static string demandMsgUnableCreatID = "Unable to create new ID";
        public static string demandMsgUnableFindExistID = "Unable to find requested information, Please Save first.";
        public static string demandMsgSaveSuccessfully = "Requested Information successfully Added";
        public static string demandMsgUpdateSuccessfully = "Requested Information successfully Update";
        public static string demandMsgSaveNotSuccessfully = "Requested Information not Added";
        public static string demandMsgUpdateNotSuccessfully = "Requested Information not Update";

        public static string demandMsgFindExistID = " Demand Note Requested Information already Added";
        public static string demandMsgNoDataToUpdate = "There is no data to Update, Please Save first.";


        #region Post
        public static string demandMsgMethodNamePost = "DemandPost";
        public static string demandMsgNoDataToPost = "There is no data to Post";
        public static string demandMsgCheckDatePost = "Please Check Transaction Date for Post";
        public static string demandMsgUnableFindExistIDPost = "Unable to find requested information for post, Please Save first.";
        public static string demandMsgPostNotSuccessfully = "Requested Information not Post";
        public static string demandMsgStockNotAvailablePost = "Stock Not Available, Transaction Not Posted(1)";
        public static string demandMsgPostNotSelect = "Requested Information not Post";
        public static string demandMsgSuccessfullyPost = "Requested Information successfully Posted";

        #endregion Post

        #endregion Demand

        #region
        public static string EngineMsgMethodNameInsert = "Engine/ChassisInsert";
        public static string EngineMsgFindExistID = "This Engine No already Used";
        public static string ChassisMsgFindExistID = "This Chassis No already Used";


        #endregion
        #region Customer

        public static string CustomerMsgSpecialRateCustomer = "This Customer is a Special Rate Customer";

        #endregion

        #region DisposeFinish

        public static string disposeFinishMsgselectItem = "Please select a Item";
        public static string disposeFinishMsgselectPurchaseNo = "Please select Purchase No ";
        public static string disposeFinishMsgInputQuantity = "Please input quantity";
        public static string disposeFinishMsgDeclarePriceFirst = "Please declare price first";
        public static string disposeFinishMsgSameProductAlreadyExist = "Same Product already exist.";
        public static string disposeFinishMsgNoItemsFound = "No Items Found in Remove.";

        public static string disposeFinishMsgNoDataTransaction = "There is no data for transaction.";
        public static string disposeFinishMsgDesiredItemRow = "Please Select Desired Item Row Appropriately Using Double Click!";
        public static string disposeFinishMsgZeroQuantityIsnotAllowed = "Zero Quantity Is not Allowed  for Change Button!\nPlease Provide Quantity.";
        //public static string disposeFinishMsg = "No";
        //public static string disposeFinishMsg = "No";
        //public static string disposeFinishMsg = "No";


        #endregion

        #region BillInvoice
        public static string billinvoiceMsgMethodNameUpdate = "BillInvoice Insert";

        #endregion
    }
}
