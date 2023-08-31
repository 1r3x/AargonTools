using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace AargonTools.Manager
{
    public class UniversalCcProcessApiService : IUniversalCcProcessApiService
    {
        private static HttpClient _clientForInstaMed = new();

        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        //private readonly IUniversalCcProcessHelper _ccProcessHelper;
        //



        private static ResponseModel _response;
        private static IAddNotesV3 _addNotes;
        private static IAddCcPaymentV2 _addCcPayment;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;
        private static ICryptoGraphy _crypto;
        private static GetTheCompanyFlag _getTheCompanyFlag;

        private readonly AdoDotNetConnection _adoConnection;
        //
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;

        public UniversalCcProcessApiService(HttpClient clientForInstaMed,
            IOptions<CentralizeVariablesModel> centralizeVariablesModel, IAddNotesV3 addNotes,
            IAddCcPaymentV2 addCcPayment,
            ICardTokenizationDataHelper cardTokenizationHelper, ICryptoGraphy crypto, ResponseModel response,
            AdoDotNetConnection adoConnection, GetTheCompanyFlag getTheCompanyFlag, ExistingDataDbContext context, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            CurrentBackupTestEnvironmentDbContext currentTestEnvironment)
        {
            _addCcPayment = addCcPayment;
            _addNotes = addNotes;
            _cardTokenizationHelper = cardTokenizationHelper;
            _crypto = crypto;
            _response = response;
            _adoConnection = adoConnection;
            _getTheCompanyFlag = getTheCompanyFlag;
            //
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _currentTestEnvironment = currentTestEnvironment;
            //
            _centralizeVariablesModel = centralizeVariablesModel;
            _clientForInstaMed = clientForInstaMed;
            _clientForInstaMed.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-key",
                _centralizeVariablesModel.Value.InstaMedCredentials.APIkey);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-secret",
                _centralizeVariablesModel.Value.InstaMedCredentials.APIsecret);
            _clientForInstaMed.DefaultRequestHeaders.Accept.Clear();
            _clientForInstaMed.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task PostPaymentA(string debtorAccount, decimal paymentAmount, float balance, decimal interestAmount,
            decimal feePct,
            string sif, int qFrom, int qTo, string remit, string paymentType, string company, float adminAmount,
            string MainDA,
            string environment)
        {
            var paymentCode = 0;
            bool removeMults = false;
            string payCode = "";
            string COMMENT;
            string intDescription;
            string acct_status;
            string Debtor_Acct2 = "";
            var tmpAmt = new float[10];
            decimal tot;
            float feeAdmin = 0f;
            float feeCosts = 0f;
            float feeAttorney = 0f;
            float feeDamages = 0f;
            float feeReturnCheck = 0f;
            float feeCollection = 0f;
            bool adminFee = true;
            float adminFee1 = 0f;
            float adminFee2 = 0f;
            if (Strings.Left(debtorAccount, 4) == "4526")
                adminFee = false;
            if (Strings.Left(debtorAccount, 4) == "4528")
                adminFee = false;
            paymentAmount = Conversions.ToDecimal(Strings.Format(paymentAmount, "####0.#0"));
            balance = Conversions.ToSingle(Strings.Format(balance, "####0.#0"));
            interestAmount = Conversions.ToDecimal(Strings.Format(interestAmount, "####0.#0"));
            adminAmount = Conversions.ToSingle(Strings.Format(adminAmount, "####0.#0"));
            if ((float)paymentAmount >= balance - (float)interestAmount - adminAmount)
            {
                if (adminAmount > 0f)
                {

                    var feeData = _adoConnection.GetData(
                        "Select admin_fees_balance,costs_balance,attorney_fees_balance,damages_balance,return_check_fees_balance,collection_fees_balance from debtor_acct_info" +
                        company + " where debtor_acct='" + debtorAccount + "'", environment);
                    if (feeData.Rows.Count > 0)
                    {
                        feeAdmin = Convert.ToSingle(feeData.Rows[0]["admin_fees_balance"]); // 4
                        feeCosts = Convert.ToSingle(feeData.Rows[0]["costs_balance"]); // 3
                        feeAttorney = Convert.ToSingle(feeData.Rows[0]["attorney_fees_balance"]); // 2
                        feeDamages = Convert.ToSingle(feeData.Rows[0]["damages_balance"]); // 5
                        feeReturnCheck = Convert.ToSingle(feeData.Rows[0]["return_check_fees_balance"]); // 6
                        feeCollection = Convert.ToSingle(feeData.Rows[0]["collection_fees_balance"]); // 1
                    }
                }

                payCode = "PIF";
                tot = paymentAmount;
                tmpAmt[1] = balance - (float)interestAmount - adminAmount;
                tmpAmt[3] = 0f;
                tmpAmt[2] = 0f;
                tmpAmt[4] = 0f;
                tmpAmt[5] = 0f;
                tmpAmt[6] = 0f;
                tmpAmt[7] = 0f;
                tmpAmt[8] = 0f;
                tmpAmt[9] = 0f;
                tot = (decimal)((float)tot - tmpAmt[1]);
                if ((float)tot >= feeCollection)
                {
                    tmpAmt[2] = feeCollection;
                    tot = (decimal)((float)tot - feeCollection);
                }
                else
                {
                    tmpAmt[2] = (float)tot;
                    tot = 0m;
                }

                if ((float)tot >= feeAttorney)
                {
                    tmpAmt[3] = feeAttorney;
                    tot = (decimal)((float)tot - feeAttorney);
                }
                else
                {
                    tmpAmt[3] = (float)tot;
                    tot = 0m;
                }

                if ((float)tot >= feeCosts)
                {
                    tmpAmt[4] = feeCosts;
                    tot = (decimal)((float)tot - feeCosts);
                }
                else
                {
                    tmpAmt[4] = (float)tot;
                    tot = 0m;
                }

                if ((float)tot >= feeAdmin)
                {
                    tmpAmt[5] = feeAdmin;
                    tot = (decimal)((float)tot - feeAdmin);
                }
                else
                {
                    tmpAmt[5] = (float)tot;
                    tot = 0m;
                }

                if ((float)tot >= feeDamages)
                {
                    tmpAmt[6] = feeDamages;
                    tot = (decimal)((float)tot - feeDamages);
                }
                else
                {
                    tmpAmt[6] = (float)tot;
                    tot = 0m;
                }

                if ((float)tot >= feeReturnCheck)
                {
                    tmpAmt[7] = feeReturnCheck;
                    tot = (decimal)((float)tot - feeReturnCheck);
                }
                else
                {
                    tmpAmt[7] = (float)tot;
                    tot = 0m;
                }

                tmpAmt[8] = (float)tot;
                paymentAmount = Conversions.ToDecimal(Strings.Format(tmpAmt[1], "####0.#0"));
                interestAmount = Conversions.ToDecimal(Strings.Format(tmpAmt[8], "####0.#0"));
                feeCollection = Conversions.ToSingle(Strings.Format(tmpAmt[2], "####0.#0"));
                feeAttorney = Conversions.ToSingle(Strings.Format(tmpAmt[3], "####0.#0"));
                feeCosts = Conversions.ToSingle(Strings.Format(tmpAmt[4], "####0.#0"));
                feeAdmin = Conversions.ToSingle(Strings.Format(tmpAmt[5], "####0.#0"));
                feeDamages = Conversions.ToSingle(Strings.Format(tmpAmt[6], "####0.#0"));
                feeReturnCheck = Conversions.ToSingle(Strings.Format(tmpAmt[7], "####0.#0"));
                adminAmount = feeCollection + feeAttorney + feeCosts + feeAdmin + feeDamages + feeReturnCheck;
            }
            else
            {
                if (sif == "Y")
                    payCode = "SIF";
                interestAmount = 0m;
                feeCollection = 0f;
                feeAttorney = 0f;
                feeCosts = 0f;
                feeAdmin = 0f;
                feeDamages = 0f;
                feeReturnCheck = 0f;
                adminAmount = feeCollection + feeAttorney + feeCosts + feeAdmin + feeDamages + feeReturnCheck;
            }

            var strQuery = "-- Create Date: 11/14/2016 12:26 PM" + Constants.vbCrLf;
            strQuery += "UPDATE code_master" + Constants.vbCrLf;
            strQuery += "SET code_value = code_value + 1" + Constants.vbCrLf;
            strQuery += "OUTPUT inserted.code_value" + Constants.vbCrLf;
            strQuery += "WHERE code_type = 'PAYMENT'" + Constants.vbCrLf;


            var payCodeDt = _adoConnection.GetData(strQuery, environment);

            paymentCode = Convert.ToInt32(payCodeDt.Rows[0]["code_value"]);


            using DataTable dt = _adoConnection.GetData("SELECT * FROM debtor_acct_info" + company +
                                                        " WHERE debtor_acct = '" +
                                                        debtorAccount + "'", environment);
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow acct in dt.Rows)
                {
                    var AgencyAmount = decimal.Round(paymentAmount * feePct * 0.01m, 2);
                    if (remit == "Y")
                        AgencyAmount = 0m;
                    if (adminFee == false)
                        AgencyAmount = 0m;
                    acct_status = acct["acct_status"].ToString();
                    strQuery = "INSERT INTO debtor_payment_master (" + Constants.vbCrLf;
                    strQuery += "    payment_code," + Constants.vbCrLf;
                    strQuery += "    debtor_acct," + Constants.vbCrLf;
                    strQuery += "    purge_flag," + Constants.vbCrLf;
                    strQuery += "    payment_type," + Constants.vbCrLf;
                    strQuery += "    payment_date," + Constants.vbCrLf;
                    strQuery += "    tran_date," + Constants.vbCrLf;
                    strQuery += "    total_payment_amt," + Constants.vbCrLf;
                    strQuery += "    client_amt," + Constants.vbCrLf;
                    strQuery += "    agency_amt_decl," + Constants.vbCrLf;
                    strQuery += "    agency_amt_not_decl," + Constants.vbCrLf;
                    strQuery += "    amt_decl_desc," + Constants.vbCrLf;
                    strQuery += "    interest_paid," + Constants.vbCrLf;
                    strQuery += "    admin_fees_paid," + Constants.vbCrLf;
                    strQuery += "    collection_fees_paid," + Constants.vbCrLf;
                    strQuery += "    costs_paid," + Constants.vbCrLf;
                    strQuery += "    attorney_fees_paid," + Constants.vbCrLf;
                    strQuery += "    damages_paid," + Constants.vbCrLf;
                    strQuery += "    return_check_fees_paid," + Constants.vbCrLf;
                    strQuery += "    balance," + Constants.vbCrLf;
                    strQuery += "    fee_pct," + Constants.vbCrLf;
                    strQuery += "    remit_full_pmt," + Constants.vbCrLf;
                    strQuery += "    check_num," + Constants.vbCrLf;
                    strQuery += "    sequence_num," + Constants.vbCrLf;
                    strQuery += "    employee," + Constants.vbCrLf;
                    strQuery += "    queue," + Constants.vbCrLf;
                    strQuery += "    status_code," + Constants.vbCrLf;
                    strQuery += "    show_on_invoice," + Constants.vbCrLf;
                    strQuery += "    rev_payment_code," + Constants.vbCrLf;
                    strQuery += "    vendor_code," + Constants.vbCrLf;
                    strQuery += "    coll_emp1," + Constants.vbCrLf;
                    strQuery += "    coll_pct1," + Constants.vbCrLf;
                    strQuery += "    coll_emp2," + Constants.vbCrLf;
                    strQuery += "    coll_pct2," + Constants.vbCrLf;
                    strQuery += "    comment" + Constants.vbCrLf;
                    strQuery += "    )" + Constants.vbCrLf;
                    strQuery += "SELECT " + paymentCode.ToString() + "," + Constants.vbCrLf;
                    strQuery += "    '" + debtorAccount + "'," + Constants.vbCrLf;
                    strQuery += "    'N'," + Constants.vbCrLf; // Purge Flag
                    strQuery += "    '" + paymentType + "'," + Constants.vbCrLf; // Payment Type
                    strQuery += "    '" + DateTime.Today.ToString("MM/dd/yyyy") + "'," +
                                Constants.vbCrLf; // Payment Date'
                    strQuery += "    GETDATE()," + Constants.vbCrLf; // Tran Date
                    strQuery += "    " +
                                Strings.FormatCurrency((float)(paymentAmount + interestAmount) + adminAmount, 2)
                                    .Replace("$", "").Replace(",", "") + "," +
                                Constants.vbCrLf; // Total Payment Amount
                    strQuery += "    " +
                                Strings.FormatCurrency(paymentAmount - AgencyAmount, 2).Replace("$", "")
                                    .Replace(",", "") + "," + Constants.vbCrLf; // Client Amount
                    strQuery += "    " + Strings.FormatCurrency(AgencyAmount, 2).Replace("$", "").Replace(",", "") +
                                "," + Constants.vbCrLf; // Agency Amount Declared
                    strQuery += "    " +
                                Strings.FormatCurrency((float)interestAmount + adminAmount, 2).Replace("$", "")
                                    .Replace(",", "") + "," + Constants.vbCrLf; // Agency Amount Not Declared
                    strQuery += "    'PAYMENT'," + Constants.vbCrLf; // Agency Declared Description
                    strQuery += "    " +
                                Strings.FormatCurrency(interestAmount, 2).Replace("$", "").Replace(",", "") + "," +
                                Constants.vbCrLf; // Interest Paid
                    strQuery += "    " + Strings.FormatCurrency(feeAdmin, 2).Replace("$", "").Replace(",", "") +
                                "," + Constants.vbCrLf; // Admin Fees Paid
                    strQuery += "    " +
                                Strings.FormatCurrency(feeCollection, 2).Replace("$", "").Replace(",", "") + "," +
                                Constants.vbCrLf; // Collection Fees Paid
                    strQuery += "    " + Strings.FormatCurrency(feeCosts, 2).Replace("$", "").Replace(",", "") +
                                "," + Constants.vbCrLf; // Costs Paid
                    strQuery += "    " + Strings.FormatCurrency(feeAttorney, 2).Replace("$", "").Replace(",", "") +
                                "," + Constants.vbCrLf; // Attorney Fees Paid
                    strQuery += "    " + Strings.FormatCurrency(feeDamages, 2).Replace("$", "").Replace(",", "") +
                                "," + Constants.vbCrLf; // Damaged Paid
                    strQuery += "    " +
                                Strings.FormatCurrency(feeReturnCheck, 2).Replace("$", "").Replace(",", "") + "," +
                                Constants.vbCrLf; // Return Check Fees Paid
                    if (payCode == "PIF" | payCode == "SIF")
                    {
                        strQuery += "    0," + Constants.vbCrLf; // Balance
                    }
                    else
                    {
                        //old 
                        //strQuery += "    " + (Convert.ToDecimal(acct["balance"]) - paymentAmount - interestAmount) +
                        //            "," + Constants.vbCrLf;


                        //new 
                        strQuery += "    " + (Convert.ToDecimal(acct["amount_placed"]) + (Convert.ToDecimal(acct["adjustments_life"])) - paymentAmount - (Convert.ToDecimal(acct["payment_amt_life"]))) +
                                    "," + Constants.vbCrLf;

                    } // Balance

                    strQuery += "    " + feePct.ToString() + "," + Constants.vbCrLf; // Fee Pct
                    strQuery += "    '" + remit + "'," + Constants.vbCrLf; // Remit Full Payment
                    if (paymentType == "CREDIT CARD")
                    {
                        strQuery += "    'CC'," + Constants.vbCrLf; // Check Num
                    }
                    else
                    {
                        strQuery += "    NULL," + Constants.vbCrLf;
                    } // Check Num

                    strQuery += "    NULL," + Constants.vbCrLf; // Sequence Num
                    strQuery += "    1993," + Constants.vbCrLf; // Employee
                    if (object.ReferenceEquals(acct["employee"], DBNull.Value))
                    {
                        strQuery += "    NULL," + Constants.vbCrLf; // Queue
                    }
                    else
                    {
                        strQuery += "    " + acct["employee"].ToString() + "," + Constants.vbCrLf;
                    } // Queue

                    if (payCode == "PIF" | payCode == "SIF")
                    {
                        strQuery += "    '" + payCode + "'," + Constants.vbCrLf; // Status Code
                    }
                    else
                    {
                        strQuery += "    '" + acct["status_code"].ToString() + "'," + Constants.vbCrLf;
                    } // Status Code

                    strQuery += "    'Y'," + Constants.vbCrLf; // Show On Invoice
                    strQuery += "    NULL," + Constants.vbCrLf; // Rev Payment Code
                    strQuery += "    'DEBTOR W/O PAYMENT ARRANGEMENTS'," + Constants.vbCrLf; // Vendor Code
                    strQuery += "    NULL," + Constants.vbCrLf; // Coll Emp 1
                    strQuery += "    NULL," + Constants.vbCrLf; // Coll Pct 1
                    strQuery += "    NULL," + Constants.vbCrLf; // Coll Emp 2
                    strQuery += "    NULL," + Constants.vbCrLf; // Coll Pct 2
                    if (qFrom == qTo)
                    {
                        COMMENT = Strings.Trim(Conversion.Str(qFrom));
                    }
                    else
                    {
                        COMMENT = Strings.Trim(Conversion.Str(qFrom)) + " -> " + Strings.Trim(Conversion.Str(qTo));
                    }

                    strQuery += "    '" + COMMENT + "'" + Constants.vbCrLf; // Comment

                    _adoConnection.GetData(strQuery, environment);

                    strQuery = "UPDATE debtor_acct_info" + company + Constants.vbCrLf;
                    strQuery += "SET begin_age_date = '" + DateTime.Today.ToString("MM/dd/yyyy") + "'," +
                                Constants.vbCrLf;
                    strQuery += "    date_last_accessed = GETDATE()," + Constants.vbCrLf;
                    strQuery += "    payment_amt_life = payment_amt_life + " + paymentAmount.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    interest_amt_life = interest_amt_life - " + interestAmount.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    total_fees_balance = total_fees_balance - " + adminAmount.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    admin_fees_paid = admin_fees_paid + " + feeAdmin.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    admin_fees_balance = admin_fees_balance - " + feeAdmin.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    costs_paid = costs_paid + " + feeCosts.ToString() + "," + Constants.vbCrLf;
                    strQuery += "    costs_balance = costs_balance - " + feeCosts.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    attorney_fees_paid = attorney_fees_paid + " + feeAttorney.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    attorney_fees_balance = attorney_fees_balance - " + feeAttorney.ToString() +
                                "," + Constants.vbCrLf;
                    strQuery += "    damages_paid = damages_paid + " + feeDamages.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    damages_balance = damages_balance - " + feeDamages.ToString() + "," +
                                Constants.vbCrLf;
                    strQuery += "    return_check_fees_paid = return_check_fees_paid + " +
                                feeReturnCheck.ToString(CultureInfo.InvariantCulture) + "," + Constants.vbCrLf;
                    strQuery += "    return_check_fees_balance = return_check_fees_balance - " +
                                feeReturnCheck.ToString(CultureInfo.InvariantCulture) + "," + Constants.vbCrLf;
                    strQuery += "    collection_fees_paid = collection_fees_paid + " + feeCollection.ToString() +
                                "," + Constants.vbCrLf;
                    strQuery += "    collection_fees_balance = collection_fees_balance - " +
                                feeCollection.ToString(CultureInfo.InvariantCulture) + "," + Constants.vbCrLf;
                    strQuery += "    last_payment_amt = " +
                                Strings.Trim(Conversion.Str(paymentAmount + interestAmount)) + "," +
                                Constants.vbCrLf;
                    if (payCode == "PIF" | payCode == "SIF")
                    {
                        strQuery += "    balance = 0," + Constants.vbCrLf;
                    }
                    else
                    {
                        strQuery += "    balance = balance - " +
                                    Strings.Trim(Conversion.Str(paymentAmount + interestAmount)) + "," +
                                    Constants.vbCrLf;
                    }

                    strQuery += "    out_of_statute = 'N'," + Constants.vbCrLf;
                    if (payCode == "PIF" | payCode == "SIF")
                    {
                        strQuery += "    status_code = '" + payCode + "'," + Constants.vbCrLf;
                        strQuery += "    acct_status = 'I'," + Constants.vbCrLf;
                        strQuery += "    date_inactivated = CONVERT(VARCHAR,GETDATE(),101)," + Constants.vbCrLf;
                        removeMults = true;
                    }

                    strQuery += "    activity_code = 'PM'" + Constants.vbCrLf;
                    strQuery += "WHERE debtor_acct = '" + debtorAccount + "'" + Constants.vbCrLf + Constants.vbCrLf;

                    _adoConnection.GetData(strQuery, environment);
                    strQuery = "UPDATE client_acct_info" + company + Constants.vbCrLf;
                    strQuery += "SET payment_amt_life = payment_amt_life + " + paymentAmount.ToString() +
                                Constants.vbCrLf;
                    strQuery += "WHERE client_acct = '" + debtorAccount.Substring(0, 4) + "'" + Constants.vbCrLf +
                                Constants.vbCrLf;
                    _adoConnection.GetData(strQuery, environment);
                    strQuery = "INSERT INTO note_master" + Constants.vbCrLf;
                    strQuery += "(debtor_acct,note_date,employee,activity_code,note_text)" + Constants.vbCrLf;
                    strQuery += "SELECT '" + debtorAccount + "'," + Constants.vbCrLf;
                    strQuery +=
                        "    CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),101) + ' ' + SUBSTRING(CONVERT(VARCHAR,GETDATE(),108),1,5))," +
                        Constants.vbCrLf;
                    strQuery += "    " + "0" + ",";//not valid for api
                    strQuery += "    'PM',";
                    intDescription = "";
                    if (interestAmount > 0m)
                        intDescription = "  IN: " + Strings.FormatCurrency(interestAmount, 2).Replace(",", "");
                    if (feeCollection > 0f)
                        intDescription = "  CF: " + Strings.FormatCurrency(feeCollection, 2).Replace(",", "");
                    if (feeAttorney > 0f)
                        intDescription = "  AT: " + Strings.FormatCurrency(feeAttorney, 2).Replace(",", "");
                    if (feeCosts > 0f)
                        intDescription = "  CO: " + Strings.FormatCurrency(feeCosts, 2).Replace(",", "");
                    if (feeAdmin > 0f)
                        intDescription = "  AD: " + Strings.FormatCurrency(feeAdmin, 2).Replace(",", "");
                    if (feeDamages > 0f)
                        intDescription = "  DA: " + Strings.FormatCurrency(feeDamages, 2).Replace(",", "");
                    if (feeReturnCheck > 0f)
                        intDescription = "  RC: " + Strings.FormatCurrency(feeReturnCheck, 2).Replace(",", "");
                    if (paymentType == "DIRECT")
                    {
                        strQuery += "    'CC DIRECT " + Strings.FormatCurrency(paymentAmount, 2).Replace(",", "") +
                                    intDescription + " {" + MainDA + "}{" + payCode + "}'" + Constants.vbCrLf +
                                    Constants.vbCrLf;
                    }
                    else
                    {
                        strQuery += "    'CC POSTED " + Strings.FormatCurrency(paymentAmount, 2).Replace(",", "") +
                                    intDescription + " {" + MainDA + "}{" + payCode + "}'" + Constants.vbCrLf +
                                    Constants.vbCrLf;
                    }

                    _adoConnection.GetData(strQuery, environment);
                    if (removeMults)
                    {
                        if (acct_status == "A")
                        {
                            {
                                strQuery = "select top 1 debtor_acct2 From debtor_multiples, debtor_acct_info" +
                                           company + " where debtor_multiples.debtor_acct = '" + debtorAccount +
                                           "' AND debtor_acct_info" + company +
                                           ".Debtor_acct = debtor_multiples.debtor_acct2 AND debtor_acct_info" +
                                           company + ".acct_status in ('A','M') " +
                                           "order by balance desc, date_placed";
                                using (var dTx = _adoConnection.GetData(strQuery, environment))
                                {
                                    if (dTx.Rows.Count > 0)
                                    {
                                        Debtor_Acct2 = (string)dTx.Rows[0]["debtor_acct2"];
                                    }
                                    else
                                    {
                                        Debtor_Acct2 = "";
                                    }
                                }

                                if (!string.IsNullOrEmpty(Debtor_Acct2))
                                {
                                    strQuery = "UPDATE debtor_acct_info" + company +
                                               " SET acct_status = 'A' WHERE debtor_acct = '" + Debtor_Acct2 + "'";

                                    _adoConnection.GetData(strQuery, environment);
                                }
                            }
                        }

                        strQuery = "DELETE FROM debtor_multiples where debtor_acct = '" + debtorAccount +
                                   "' OR debtor_acct2 = '" + debtorAccount + "'";


                        _adoConnection.GetData(strQuery, environment);
                    }
                }
            }

            return Task.CompletedTask;
        }




        public async Task<string> InstaMedSale(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/sale", request);
            var resultString = response.Content.ReadAsStringAsync();
            //var ensureSuccessStatusCode = response.EnsureSuccessStatusCode();
            return resultString.Result;
        }

        public async Task<string> InstaMedAuth(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/auth", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }

        public async Task<string> InstaMedTokenization(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/paymentplan", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }

        public Task<string> IProGatewaySale(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_key;
            //String firstname = request.Patient.FirstName;
            //String lastname = request.Patient.LastName;

            String firstname = "First Name";
            String lastname = "Last Name";



            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=sale"
                                             + "&amount=" + amount + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }

        public Task<string> IProGatewayAuth(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_key;
            String firstname = request.Patient.FirstName;
            String lastname = request.Patient.LastName;


            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=auth"
                                             + "&amount=" + 1 + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }

        public async Task<string> ElavonSale(SaleRequestModelForInstamed request)
        {
            var url = _centralizeVariablesModel.Value.ElavonCredentials.BaseAddress;
            var result = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";


            httpRequest.ContentType = "application/x-www-form-urlencoded ";
            httpRequest.Accept = "application/xml";

            var data = "xmldata=" +
                       @"<txn>" +
                       "<ssl_merchant_id>" + request.Outlet.MerchantID + "</ssl_merchant_id>" +//
                       "<ssl_user_id>" + request.Outlet.StoreID + "</ssl_user_id>" +//StoreID is user_id
                       "<ssl_pin>" + request.Outlet.TerminalID + "</ssl_pin>" +//TerminalID is pin
                       "<ssl_description>Test Authorization for 1.00</ssl_description>" +
                       "<ssl_transaction_type>ccsale</ssl_transaction_type>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2>" + request.Card.CVN + "</ssl_cvv2cvc2>" +
                       "<ssl_cardholder_ip>72.206.60.98</ssl_cardholder_ip>" +
                       "</txn>";


            //var data = "xmldata=" +
            //          @"<txn>" +
            //          "<ssl_merchant_id>807485</ssl_merchant_id>" +//
            //          "<ssl_user_id>8032017132</ssl_user_id>" +
            //          "<ssl_pin>59WAMNKEGMMZVZARP37FUEST83WIOZYU6AJXKOMLHAZIIDP4LKGIZ65DDSSBMJ9M</ssl_pin>" +
            //          "<ssl_transaction_type>CCSALE</ssl_transaction_type>" +
            //          "<ssl_card_number>4000000000000002</ssl_card_number>" +
            //          "<ssl_exp_date>1222</ssl_exp_date>" +
            //          "<ssl_amount>100.00</ssl_amount>" +
            //          "<ssl_cvv2cvc2_indicator>1</ssl_cvv2cvc2_indicator>" +
            //          "<ssl_cvv2cvc2>123</ssl_cvv2cvc2>" +
            //          "<ssl_healthcare_amount>32.00</ssl_healthcare_amount>" +
            //          "</txn>";






            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }
            return result;
        }

        public async Task<string> ElavonAuth(SaleRequestModelForInstamed request)
        {
            var url = _centralizeVariablesModel.Value.ElavonCredentials.BaseAddress;
            var result = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/x-www-form-urlencoded ";
            httpRequest.Accept = "application/xml";

            var data = "xmldata=" +
                       "<txn>" +
                       "<ssl_merchant_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id + "</ssl_merchant_id>" +
                       "<ssl_user_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_user_id + "</ssl_user_id>" +
                       "<ssl_pin>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_pin + " </ssl_pin>" +
                       "<ssl_transaction_type>ccauthonly</ssl_transaction_type>" +
                       "<ssl_description>Test Authorization for 1.00</ssl_description>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2 >" + 123 + "</ssl_cvv2cvc2>" +
                       "<ssl_cardholder_ip >103.112.55.88</ssl_cardholder_ip>" +
                       "</txn>";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }

            return result;
        }

        public Task<string> ElavonTokenization(SaleRequestModelForInstamed request)
        {
            throw new NotImplementedException();
        }



        //


        private SaleResponseModelForInstamed _responseModelForInstamed;
        private SaleResponseModelForIProGateway _responseModelForIProGateway;
        private txn _deserializeObjForElavon;
        private int _lcgPaymentScheduleId;
        private readonly DateTime _scheduleDateTime = DateTime.Now;






        public async Task<string> ReadHtmlPageAsync(string url, string post)
        {
            String result = "";
            String strPost = post;
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
                   new StreamReader(objResponse.GetResponseStream()))
            {
                result = await sr.ReadToEndAsync();

                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }

        public async Task SaveCardInfoAndScheduleData(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                PaymentPlanType = "SaveOnFile",
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }
            };
            try
            {
                var resultVerify = await InstaMedTokenization(saleRequestModel);

                var cardInfoData = new TokenizeResponseModelForInstaMed(resultVerify);


                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = cardInfoData.CardInfo.EntryMode,
                    BinNumber = cardInfoData.CardInfo.BinNumber,
                    ExpirationMonth = cardInfoData.CardInfo.ExpirationMonth,
                    ExpirationYear = cardInfoData.CardInfo.ExpirationYear,
                    LastFour = cardInfoData.CardInfo.LastFour,
                    PaymentMethodId = cardInfoData.CardInfo.PaymentMethodId,
                    Type = cardInfoData.CardInfo.Type,
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
                {
                    var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                    {
                        CardInfoId = paymentScheduleObj.CardInfoId,
                        EffectiveDate = paymentDate,
                        IsActive = true,
                        NumberOfPayments = i,
                        PatientAccount = paymentScheduleObj.PatientAccount,
                        Amount = paymentScheduleObj.Amount
                    };
                    await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                    if (i == 1)
                    {
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModelForInstamed.ResponseCode,
                    AuthorizationNumber = _responseModelForInstamed.AuthorizationNumber,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _responseModelForInstamed.ResponseMessage,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _responseModelForInstamed.TransactionId
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = "_username", //todo user name
                        UserName = "_username" + " -LCG", //todo user name
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = _scheduleDateTime,
                        ApprovalStatus = "APPROVED",
                        BillingName = "", //todo billing person name 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }


                //new implementation 
                var companyFlag = await _getTheCompanyFlag.GetStringFlagForAdoQuery(request.debtorAcc, environment);

                var balance = _adoConnection.GetData("SELECT CAST(balance as float) as balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment); ;

                var interestAmount = _adoConnection.GetData("SELECT interest_amt_life FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment); ;

                var placements = _adoConnection.GetData("SELECT ISNULL(placement,0) as placement FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);
                DataTable feePct = null;
                decimal feePctSimplified = 0;

                if (placements.Columns.Count > 1 && balance.Columns.Count > 1 && interestAmount.Columns.Count > 1)
                {

                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct1"];
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct2"];
                            break;
                    }


                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                    }


                    var qFrom = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);
                    //for now
                    var qTo = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    var remit = _adoConnection.GetData("SELECT remit_full_pmt FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                    var adminAmount = _adoConnection.GetData("SELECT CAST(total_fees_balance as float) as total_fees_balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    string paymentType;
                    if (companyFlag == "_t")
                    {
                        paymentType = "DIRECT";
                    }
                    else
                    {
                        paymentType = "CREDIT CARD";
                    }




                    // debugger
                    var a = request.debtorAcc;
                    var b = request.amount;
                    var balanceT = balance.Rows[0]["balance"];
                    var interestAmountT = interestAmount.Rows[0]["interest_amt_life"];
                    var e = feePctSimplified;
                    var qFromT = qFrom.Rows[0]["employee"];
                    var qToT = qTo.Rows[0]["employee"];
                    var remitT = remit.Rows[0]["remit_full_pmt"];
                    var m = paymentType;
                    var j = companyFlag;
                    var adminAmountT = adminAmount.Rows[0]["total_fees_balance"];
                    var l = request.debtorAcc;
                    //
                    var balanceC = Convert.ToSingle(balanceT);
                    var interestAmountC = Convert.ToDecimal(interestAmountT);
                    var qFromC = Convert.ToInt32(qFromT);
                    var qToC = Convert.ToInt32(qToT);
                    var remitC = Convert.ToString(remitT);
                    var adminAmountC = Convert.ToSingle(adminAmountT);




                    await PostPaymentA(request.debtorAcc, request.amount, balanceC,
                        interestAmountC, feePctSimplified, request.sif,
                        qFromC, qToC, remitC,
                        paymentType, companyFlag, adminAmountC, request.debtorAcc, environment);
                }

            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<ResponseModel> ProcessSaleTransForInstaMed(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await InstaMedSale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
                //have to check the field error 
                return _response.Response(true, false, resultVerify);

            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.debtorAcc,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.amount,
                    Subtotal = request.amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _responseModelForInstamed.ResponseCode,
                    OrderNumber = _responseModelForInstamed.TransactionId,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db



            return _response.Response(true, true, _responseModelForInstamed);
        }

        //qa
        public async Task<ResponseModel> ProcessSaleTransForInstaMedQA(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await InstaMedSale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
                //have to check the field error 
                return _response.Response(true, false, resultVerify);

            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
                //var ccPaymentObj = new CcPayment()
                //{
                //    DebtorAcct = request.debtorAcc,
                //    Company = "TOTAL CREDIT RECOVERY",
                //    //UserId = username,
                //    UserId = "_username", //todo   
                //    //UserName = username + " -LCG",
                //    UserName = "_username", //todo 
                //    ChargeTotal = request.amount,
                //    Subtotal = request.amount,
                //    PaymentDate = DateTime.Now,
                //    ApprovalStatus = "APPROVED",
                //    BillingName = "", // todo is it valid  for api transaction ? 
                //    ApprovalCode = _responseModelForInstamed.ResponseCode,
                //    OrderNumber = _responseModelForInstamed.TransactionId,
                //    RefNumber = "INSTAMEDLH",
                //    Sif = "N",
                //    VoidSale = "N"
                //};
                //await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            //this segment is for QA
            //cc_payment variables
            string idCc = "";
            string debtor_acctCc = "";
            string companyCc = "";
            string user_idCc = "";
            string user_nameCc = "";
            string subtotalCc = "";
            string charge_totalCc = "";
            string payment_dateCc = "";
            string approval_statusCc = "";
            string approval_codeCc = "";
            string error_codeCc = "";
            string order_numberCc = "";
            string ref_numberCc = "";
            string sifCc = "";







            var qaCC = _adoConnection.GetData("select * from cc_payment cc where cc.order_number='" + _responseModelForInstamed.TransactionId + "'", environment);
            if (qaCC.Rows.Count > 0)
            {
                idCc = Convert.ToString(qaCC.Rows[0]["id"]); // 4
                debtor_acctCc = Convert.ToString(qaCC.Rows[0]["debtor_acct"]); // 3
                companyCc = Convert.ToString(qaCC.Rows[0]["company"]); // 2
                user_idCc = Convert.ToString(qaCC.Rows[0]["user_id"]); // 5
                user_nameCc = Convert.ToString(qaCC.Rows[0]["user_name"]); // 6
                subtotalCc = Convert.ToString(qaCC.Rows[0]["subtotal"]); // 1
                charge_totalCc = Convert.ToString(qaCC.Rows[0]["charge_total"]); // 1
                payment_dateCc = Convert.ToString(qaCC.Rows[0]["payment_date"]); // 1
                approval_statusCc = Convert.ToString(qaCC.Rows[0]["approval_status"]); // 1
                approval_codeCc = Convert.ToString(qaCC.Rows[0]["approval_code"]); // 1
                error_codeCc = Convert.ToString(qaCC.Rows[0]["error_code"]); // 1
                order_numberCc = Convert.ToString(qaCC.Rows[0]["order_number"]); // 1
                ref_numberCc = Convert.ToString(qaCC.Rows[0]["ref_number"]); // 1
                sifCc = Convert.ToString(qaCC.Rows[0]["sif"]); // 1
            }

            var ccPaymnetTable = "cc_payment >>>>" + "--id-->" + idCc + "-- debtor_acct-->" + debtor_acctCc + "-- company -->" + companyCc + "-- user_id -->" +
                                 user_idCc + "-- user_name -->" + user_nameCc + "-- subtotal -->" + subtotalCc + "-- charge_total -->" + charge_totalCc
                                 + "-- payment_date -->" +
                                 payment_dateCc + "-- approval_status -->" + approval_statusCc + "-- approval_code -->" +
                                 approval_codeCc + "-- error_code -->" + error_codeCc + "-- order_number -->" + order_numberCc + "-- ref_number -->"
                                 + ref_numberCc + "--sif  -->" + sifCc;

            //[UCG_PaymentScheduleHistory] variables
            string Idhs = "";
            string PaymentScheduleIdhs = "";
            string TransactionIdhs = "";
            string ResponseCodehs = "";
            string ResponseMessagehs = "";
            string AuthorizationNumberhs = "";
            string AuthorizationTexths = "";
            string TimeLoghs = "";


            var qaHistory = _adoConnection.GetData("select * from UCG_PaymentScheduleHistory hs where hs.TransactionId='" + _responseModelForInstamed.TransactionId + "'", environment);

            if (qaHistory.Rows.Count > 0)
            {
                Idhs = Convert.ToString(qaHistory.Rows[0]["Id"]); // 4
                PaymentScheduleIdhs = Convert.ToString(qaHistory.Rows[0]["PaymentScheduleId"]); // 3
                TransactionIdhs = Convert.ToString(qaHistory.Rows[0]["TransactionId"]); // 2
                ResponseCodehs = Convert.ToString(qaHistory.Rows[0]["ResponseCode"]); // 5
                ResponseMessagehs = Convert.ToString(qaHistory.Rows[0]["ResponseMessage"]); // 6
                AuthorizationNumberhs = Convert.ToString(qaHistory.Rows[0]["AuthorizationNumber"]); // 1
                AuthorizationTexths = Convert.ToString(qaHistory.Rows[0]["AuthorizationText"]); // 1
                TimeLoghs = Convert.ToString(qaHistory.Rows[0]["TimeLog"]); // 1
            }
            var PaymentScheduleHistoryTable = "UCG_PaymentScheduleHistory >>>>" + "--id-->" + Idhs + "--id-->" + PaymentScheduleIdhs + "--TransactionIdhs-->" + TransactionIdhs
                                              + "--ResponseCode-->" + ResponseCodehs + "--ResponseMessage-->" + ResponseMessagehs + "--AuthorizationNumber-->" + AuthorizationNumberhs
                                              + "--AuthorizationText-->" + AuthorizationTexths + "--TimeLog-->" + TimeLoghs;

            //[UCG_PaymentSchedule] variables
            string IdPs = "";
            string PatientAccountPs = "";
            string EffectiveDatePs = "";
            string CardInfoIdPs = "";
            string NumberOfPaymentsPs = "";
            string AmountPs = "";
            string IsActivePs = "";


            var qaPaymentScheduleTEmp = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.Id='" + PaymentScheduleIdhs + "'", environment);
            var CardInfoIdTemp = Convert.ToString(qaPaymentScheduleTEmp.Rows[0]["CardInfoId"]); // 5

            var qaPaymentSchedule = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.CardInfoId='" + CardInfoIdTemp + "'", environment);
            var qaPaymentScheduleStringList = new List<string>
            {
                Capacity = 10
            };
            if (qaPaymentSchedule.Rows.Count > 0)
            {
                for (int i = 0; i < qaPaymentSchedule.Rows.Count; i++)
                {
                    IdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Id"]); // 4
                    PatientAccountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["PatientAccount"]); // 3
                    EffectiveDatePs = Convert.ToString(qaPaymentSchedule.Rows[i]["EffectiveDate"]); // 2
                    CardInfoIdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["CardInfoId"]); // 5
                    NumberOfPaymentsPs = Convert.ToString(qaPaymentSchedule.Rows[i]["NumberOfPayments"]); // 6
                    AmountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Amount"]); // 1
                    IsActivePs = Convert.ToString(qaPaymentSchedule.Rows[i]["IsActive"]); // 1


                    qaPaymentScheduleStringList.Add("UCG_PaymentSchedule >>>>" + "--id-->" + IdPs + "--PatientAccount-->" + PatientAccountPs +
                                                    "--EffectiveDate-->" + EffectiveDatePs + "--CardInfoId-->" + CardInfoIdPs
                                                    + "--NumberOfPayments-->" + NumberOfPaymentsPs + "--Amount-->" + AmountPs + "--IsActive-->" + IsActivePs);
                }
            }



            //[UCG_CardInfo] variables
            string Idci = "";
            string PaymentMethodID = "";
            string EntryMode = "";
            string Type = "";
            string BinNumber = "";
            string LastFour = "";
            string ExpirationMonth = "";
            string ExpirationYear = "";
            string CardHolderName = "";
            string AssociateDebtorAcct = "";
            string IsActiveci = "";


            var qaUCG_CardInfo = _adoConnection.GetData("select * from UCG_CardInfo hs where hs.Id='" + CardInfoIdPs + "'", environment);

            if (qaUCG_CardInfo.Rows.Count > 0)
            {
                Idci = Convert.ToString(qaUCG_CardInfo.Rows[0]["Id"]); // 4
                PaymentMethodID = Convert.ToString(qaUCG_CardInfo.Rows[0]["PaymentMethodID"]); // 3
                EntryMode = Convert.ToString(qaUCG_CardInfo.Rows[0]["EntryMode"]); // 2
                Type = Convert.ToString(qaUCG_CardInfo.Rows[0]["Type"]); // 5
                BinNumber = Convert.ToString(qaUCG_CardInfo.Rows[0]["BinNumber"]); // 6
                LastFour = Convert.ToString(qaUCG_CardInfo.Rows[0]["LastFour"]); // 1
                ExpirationMonth = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationMonth"]); // 1
                ExpirationYear = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationYear"]); // 1
                CardHolderName = Convert.ToString(qaUCG_CardInfo.Rows[0]["CardHolderName"]); // 1
                AssociateDebtorAcct = Convert.ToString(qaUCG_CardInfo.Rows[0]["AssociateDebtorAcct"]); // 1
                IsActiveci = Convert.ToString(qaUCG_CardInfo.Rows[0]["IsActive"]); // 1
            }

            var qaUCG_CardInfoString = "UCG_CardInfo >>>>" + "--Idci-->" + Idci + "--PaymentMethodID-->" + PaymentMethodID + "--EntryMode-->" + EntryMode
                                       + "--Type-->" + Type + "--BinNumber-->" + BinNumber + "--LastFour-->" + LastFour
                                       + "--ExpirationMonth-->" + ExpirationMonth + "--ExpirationYear-->" + ExpirationYear + "--CardHolderName-->" + CardHolderName
                                       + "--AssociateDebtorAcct-->" + AssociateDebtorAcct + "--CardHolderName-->" + CardHolderName + "--AssociateDebtorAcct-->" + AssociateDebtorAcct
                                       + "--IsActive-->" + IsActiveci;


            var dbQA = new List<string>
            {
                Capacity = 5
            };

            dbQA.Add(ccPaymnetTable);
            dbQA.Add(qaUCG_CardInfoString);
            dbQA.Add(PaymentScheduleHistoryTable);
            dbQA.AddRange(qaPaymentScheduleStringList);


            //

            return _response.Response(true, true, _responseModelForInstamed, dbQA);
        }

        //on file
        public async Task<ResponseModel> ProcessOnFileSaleTransForInstaMed(AutoProcessCcUniversalViewModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.Amount,
                PaymentMethod = "OnFile",
                PaymentMethodID = request.PaymentMethodId,
                Patient = new Patient()
                {
                    AccountNumber = request.PatientAccount
                }

            };

            var resultVerify = await InstaMedSale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {

                //todo  return the result of specific error
            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.Amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;


                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.AssociateDebtorAcct,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.Amount,
                    Subtotal = request.Amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _responseModelForInstamed.ResponseCode,
                    OrderNumber = _responseModelForInstamed.TransactionId,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.Amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.AssociateDebtorAcct,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.Amount,
                        Subtotal = request.Amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };

                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.AssociateDebtorAcct,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, true, _responseModelForInstamed);

        }

        public async Task<ResponseModel> ProcessCardAuthorizationForInstaMed(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await InstaMedAuth(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {

            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, true, resultVerify);
        }

        public async Task SaveCardInfoAndScheduleDataForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {

            try
            {
                var ccNUmber = request.ccNumber;

                var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();

                var encryptedCC = _crypto.Encrypt(ccNUmber, IVBase64, Key);

                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = "key",
                    BinNumber = request.cvv,
                    ExpirationMonth = 1,//todo 
                    ExpirationYear = 2,//todo
                    LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                    PaymentMethodId = encryptedCC,
                    Type = "VISA",
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
                {
                    var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                    {
                        CardInfoId = paymentScheduleObj.CardInfoId,
                        EffectiveDate = paymentDate,
                        IsActive = true,
                        NumberOfPayments = i,
                        PatientAccount = paymentScheduleObj.PatientAccount,
                        Amount = paymentScheduleObj.Amount
                    };
                    await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                    if (i == 1)
                    {
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModelForIProGateway.response_code,
                    AuthorizationNumber = _responseModelForIProGateway.authcode,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _responseModelForIProGateway.cvvresponse,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _responseModelForIProGateway.transactionid
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = "_username", //todo user name
                        UserName = "_username" + " -LCG", //todo user name
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = _scheduleDateTime,
                        ApprovalStatus = "APPROVED",
                        BillingName = "", //todo billing person name 
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "IPROGATEWAY",
                        Sif = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }

                //new implementation 
                var companyFlag = await _getTheCompanyFlag.GetStringFlagForAdoQuery(request.debtorAcc, environment);

                var balance = _adoConnection.GetData("SELECT CAST(balance as float) as balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment); ;

                var interestAmount = _adoConnection.GetData("SELECT interest_amt_life FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment); ;

                var placements = _adoConnection.GetData("SELECT ISNULL(placement,0) as placement FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);
                DataTable feePct = null;
                decimal feePctSimplified = 0;

                if (placements.Columns.Count > 1 && balance.Columns.Count > 1 && interestAmount.Columns.Count > 1)
                {

                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct1"];
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct2"];
                            break;
                    }


                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                    }


                    var qFrom = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);
                    //for now
                    var qTo = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    var remit = _adoConnection.GetData("SELECT remit_full_pmt FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                    var adminAmount = _adoConnection.GetData("SELECT CAST(total_fees_balance as float) as total_fees_balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    string paymentType;
                    if (companyFlag == "_t")
                    {
                        paymentType = "DIRECT";
                    }
                    else
                    {
                        paymentType = "CREDIT CARD";
                    }




                    // debugger
                    var a = request.debtorAcc;
                    var b = request.amount;
                    var balanceT = balance.Rows[0]["balance"];
                    var interestAmountT = interestAmount.Rows[0]["interest_amt_life"];
                    var e = feePctSimplified;
                    var qFromT = qFrom.Rows[0]["employee"];
                    var qToT = qTo.Rows[0]["employee"];
                    var remitT = remit.Rows[0]["remit_full_pmt"];
                    var m = paymentType;
                    var j = companyFlag;
                    var adminAmountT = adminAmount.Rows[0]["total_fees_balance"];
                    var l = request.debtorAcc;
                    //
                    var balanceC = Convert.ToSingle(balanceT);
                    var interestAmountC = Convert.ToDecimal(interestAmountT);
                    var qFromC = Convert.ToInt32(qFromT);
                    var qToC = Convert.ToInt32(qToT);
                    var remitC = Convert.ToString(remitT);
                    var adminAmountC = Convert.ToSingle(adminAmountT);




                    await PostPaymentA(request.debtorAcc, request.amount, balanceC,
                        interestAmountC, feePctSimplified, request.sif,
                        qFromC, qToC, remitC,
                        paymentType, companyFlag, adminAmountC, request.debtorAcc, environment);


                }
            }
            catch (Exception e)
            {

                throw e;
            }


        }



        public async Task<ResponseModel> ProcessSaleTransForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await IProGatewaySale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "100")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;

                //magic 
                await SaveCardInfoAndScheduleDataForIProGateway(request, environment);
                // for success
                //var ccPaymentObj = new CcPayment()
                //{
                //    DebtorAcct = request.debtorAcc,
                //    Company = "TOTAL CREDIT RECOVERY",
                //    //UserId = username,
                //    UserId = "_username", //todo   
                //    //UserName = username + " -LCG",
                //    UserName = "_username", //todo 
                //    ChargeTotal = request.amount,
                //    Subtotal = request.amount,
                //    PaymentDate = DateTime.Now,
                //    ApprovalStatus = "APPROVED",
                //    BillingName = "", // todo is it valid  for api transaction ? 
                //    ApprovalCode = _responseModelForIProGateway.response_code,
                //    OrderNumber = _responseModelForIProGateway.transactionid,
                //    RefNumber = "INSTAMEDLH",
                //    Sif = "N",
                //    VoidSale = "N"
                //};
                //await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
                if (_responseModelForIProGateway != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            if (_responseModelForIProGateway != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _responseModelForIProGateway.authcode,
                    ResponseCode = _responseModelForIProGateway.response_code,
                    ResponseMessage = _responseModelForIProGateway.responsetext,
                    TransactionId = _responseModelForIProGateway.transactionid
                };


                return _response.Response(true, true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        //QA
        public async Task<ResponseModel> ProcessSaleTransForIProGatewayQA(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await IProGatewaySale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "100")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;

                //magic 
                await SaveCardInfoAndScheduleDataForIProGateway(request, environment);
                // for success
                //var ccPaymentObj = new CcPayment()
                //{
                //    DebtorAcct = request.debtorAcc,
                //    Company = "TOTAL CREDIT RECOVERY",
                //    //UserId = username,
                //    UserId = "_username", //todo   
                //    //UserName = username + " -LCG",
                //    UserName = "_username", //todo 
                //    ChargeTotal = request.amount,
                //    Subtotal = request.amount,
                //    PaymentDate = DateTime.Now,
                //    ApprovalStatus = "APPROVED",
                //    BillingName = "", // todo is it valid  for api transaction ? 
                //    ApprovalCode = _responseModelForIProGateway.response_code,
                //    OrderNumber = _responseModelForIProGateway.transactionid,
                //    RefNumber = "INSTAMEDLH",
                //    Sif = "N",
                //    VoidSale = "N"
                //};
                //await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
                if (_responseModelForIProGateway != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            if (_responseModelForIProGateway != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _responseModelForIProGateway.authcode,
                    ResponseCode = _responseModelForIProGateway.response_code,
                    ResponseMessage = _responseModelForIProGateway.responsetext,
                    TransactionId = _responseModelForIProGateway.transactionid
                };


                //this segment is for QA
                //cc_payment variables
                string idCc = "";
                string debtor_acctCc = "";
                string companyCc = "";
                string user_idCc = "";
                string user_nameCc = "";
                string subtotalCc = "";
                string charge_totalCc = "";
                string payment_dateCc = "";
                string approval_statusCc = "";
                string approval_codeCc = "";
                string error_codeCc = "";
                string order_numberCc = "";
                string ref_numberCc = "";
                string sifCc = "";







                var qaCC = _adoConnection.GetData("select * from cc_payment cc where cc.order_number='" + _responseModelForIProGateway.transactionid + "'", environment);
                if (qaCC.Rows.Count > 0)
                {
                    idCc = Convert.ToString(qaCC.Rows[0]["id"]); // 4
                    debtor_acctCc = Convert.ToString(qaCC.Rows[0]["debtor_acct"]); // 3
                    companyCc = Convert.ToString(qaCC.Rows[0]["company"]); // 2
                    user_idCc = Convert.ToString(qaCC.Rows[0]["user_id"]); // 5
                    user_nameCc = Convert.ToString(qaCC.Rows[0]["user_name"]); // 6
                    subtotalCc = Convert.ToString(qaCC.Rows[0]["subtotal"]); // 1
                    charge_totalCc = Convert.ToString(qaCC.Rows[0]["charge_total"]); // 1
                    payment_dateCc = Convert.ToString(qaCC.Rows[0]["payment_date"]); // 1
                    approval_statusCc = Convert.ToString(qaCC.Rows[0]["approval_status"]); // 1
                    approval_codeCc = Convert.ToString(qaCC.Rows[0]["approval_code"]); // 1
                    error_codeCc = Convert.ToString(qaCC.Rows[0]["error_code"]); // 1
                    order_numberCc = Convert.ToString(qaCC.Rows[0]["order_number"]); // 1
                    ref_numberCc = Convert.ToString(qaCC.Rows[0]["ref_number"]); // 1
                    sifCc = Convert.ToString(qaCC.Rows[0]["sif"]); // 1
                }

                var ccPaymnetTable = "cc_payment >>>>" + "--id-->" + idCc + "-- debtor_acct-->" + debtor_acctCc + "-- company -->" + companyCc + "-- user_id -->" +
                                     user_idCc + "-- user_name -->" + user_nameCc + "-- subtotal -->" + subtotalCc + "-- charge_total -->" + charge_totalCc
                                     + "-- payment_date -->" +
                                     payment_dateCc + "-- approval_status -->" + approval_statusCc + "-- approval_code -->" +
                                     approval_codeCc + "-- error_code -->" + error_codeCc + "-- order_number -->" + order_numberCc + "-- ref_number -->"
                                     + ref_numberCc + "--sif  -->" + sifCc;

                //[UCG_PaymentScheduleHistory] variables
                string Idhs = "";
                string PaymentScheduleIdhs = "";
                string TransactionIdhs = "";
                string ResponseCodehs = "";
                string ResponseMessagehs = "";
                string AuthorizationNumberhs = "";
                string AuthorizationTexths = "";
                string TimeLoghs = "";


                var qaHistory = _adoConnection.GetData("select * from UCG_PaymentScheduleHistory hs where hs.TransactionId='" + _responseModelForIProGateway.transactionid + "'", environment);

                if (qaHistory.Rows.Count > 0)
                {
                    Idhs = Convert.ToString(qaHistory.Rows[0]["Id"]); // 4
                    PaymentScheduleIdhs = Convert.ToString(qaHistory.Rows[0]["PaymentScheduleId"]); // 3
                    TransactionIdhs = Convert.ToString(qaHistory.Rows[0]["TransactionId"]); // 2
                    ResponseCodehs = Convert.ToString(qaHistory.Rows[0]["ResponseCode"]); // 5
                    ResponseMessagehs = Convert.ToString(qaHistory.Rows[0]["ResponseMessage"]); // 6
                    AuthorizationNumberhs = Convert.ToString(qaHistory.Rows[0]["AuthorizationNumber"]); // 1
                    AuthorizationTexths = Convert.ToString(qaHistory.Rows[0]["AuthorizationText"]); // 1
                    TimeLoghs = Convert.ToString(qaHistory.Rows[0]["TimeLog"]); // 1
                }
                var PaymentScheduleHistoryTable = "UCG_PaymentScheduleHistory >>>>" + "--id-->" + Idhs + "--id-->" + PaymentScheduleIdhs + "--TransactionIdhs-->" + TransactionIdhs
                                                  + "--ResponseCode-->" + ResponseCodehs + "--ResponseMessage-->" + ResponseMessagehs + "--AuthorizationNumber-->" + AuthorizationNumberhs
                                                  + "--AuthorizationText-->" + AuthorizationTexths + "--TimeLog-->" + TimeLoghs;

                //[UCG_PaymentSchedule] variables
                string IdPs = "";
                string PatientAccountPs = "";
                string EffectiveDatePs = "";
                string CardInfoIdPs = "";
                string NumberOfPaymentsPs = "";
                string AmountPs = "";
                string IsActivePs = "";


                var qaPaymentScheduleTEmp = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.Id='" + PaymentScheduleIdhs + "'", environment);
                var CardInfoIdTemp = Convert.ToString(qaPaymentScheduleTEmp.Rows[0]["CardInfoId"]); // 5

                var qaPaymentSchedule = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.CardInfoId='" + CardInfoIdTemp + "'", environment);
                var qaPaymentScheduleStringList = new List<string>
                {
                    Capacity = 10
                };
                if (qaPaymentSchedule.Rows.Count > 0)
                {
                    for (int i = 0; i < qaPaymentSchedule.Rows.Count; i++)
                    {
                        IdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Id"]); // 4
                        PatientAccountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["PatientAccount"]); // 3
                        EffectiveDatePs = Convert.ToString(qaPaymentSchedule.Rows[i]["EffectiveDate"]); // 2
                        CardInfoIdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["CardInfoId"]); // 5
                        NumberOfPaymentsPs = Convert.ToString(qaPaymentSchedule.Rows[i]["NumberOfPayments"]); // 6
                        AmountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Amount"]); // 1
                        IsActivePs = Convert.ToString(qaPaymentSchedule.Rows[i]["IsActive"]); // 1


                        qaPaymentScheduleStringList.Add("UCG_PaymentSchedule >>>>" + "--id-->" + IdPs + "--PatientAccount-->" + PatientAccountPs +
                                                        "--EffectiveDate-->" + EffectiveDatePs + "--CardInfoId-->" + CardInfoIdPs
                                                        + "--NumberOfPayments-->" + NumberOfPaymentsPs + "--Amount-->" + AmountPs + "--IsActive-->" + IsActivePs);
                    }
                }



                //[UCG_CardInfo] variables
                string Idci = "";
                string PaymentMethodID = "";
                string EntryMode = "";
                string Type = "";
                string BinNumber = "";
                string LastFour = "";
                string ExpirationMonth = "";
                string ExpirationYear = "";
                string CardHolderName = "";
                string AssociateDebtorAcct = "";
                string IsActiveci = "";


                var qaUCG_CardInfo = _adoConnection.GetData("select * from UCG_CardInfo hs where hs.Id='" + CardInfoIdPs + "'", environment);

                if (qaUCG_CardInfo.Rows.Count > 0)
                {
                    Idci = Convert.ToString(qaUCG_CardInfo.Rows[0]["Id"]); // 4
                    PaymentMethodID = Convert.ToString(qaUCG_CardInfo.Rows[0]["PaymentMethodID"]); // 3
                    EntryMode = Convert.ToString(qaUCG_CardInfo.Rows[0]["EntryMode"]); // 2
                    Type = Convert.ToString(qaUCG_CardInfo.Rows[0]["Type"]); // 5
                    BinNumber = Convert.ToString(qaUCG_CardInfo.Rows[0]["BinNumber"]); // 6
                    LastFour = Convert.ToString(qaUCG_CardInfo.Rows[0]["LastFour"]); // 1
                    ExpirationMonth = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationMonth"]); // 1
                    ExpirationYear = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationYear"]); // 1
                    CardHolderName = Convert.ToString(qaUCG_CardInfo.Rows[0]["CardHolderName"]); // 1
                    AssociateDebtorAcct = Convert.ToString(qaUCG_CardInfo.Rows[0]["AssociateDebtorAcct"]); // 1
                    IsActiveci = Convert.ToString(qaUCG_CardInfo.Rows[0]["IsActive"]); // 1
                }

                var qaUCG_CardInfoString = "UCG_CardInfo >>>>" + "--Idci-->" + Idci + "--PaymentMethodID-->" + PaymentMethodID + "--EntryMode-->" + EntryMode
                                           + "--Type-->" + Type + "--BinNumber-->" + BinNumber + "--LastFour-->" + LastFour
                                           + "--ExpirationMonth-->" + ExpirationMonth + "--ExpirationYear-->" + ExpirationYear + "--CardHolderName-->" + CardHolderName
                                           + "--AssociateDebtorAcct-->" + AssociateDebtorAcct + "--CardHolderName-->" + CardHolderName + "--AssociateDebtorAcct-->" + AssociateDebtorAcct
                                           + "--IsActive-->" + IsActiveci;


                var dbQA = new List<string>
                {
                    Capacity = 5
                };

                dbQA.Add(ccPaymnetTable);
                dbQA.Add(qaUCG_CardInfoString);
                dbQA.Add(PaymentScheduleHistoryTable);
                dbQA.AddRange(qaPaymentScheduleStringList);


                //

                return _response.Response(true, true, response, dbQA);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        //on file
        public async Task<ResponseModel> ProcessOnfileSaleTransForIProGateway(AutoProcessCcUniversalViewModel request, string environment)
        {


            var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();
            var decryptedCC = _crypto.Decrypt(request.PaymentMethodId, IVBase64, Key);
            var saleRequestModel = new SaleRequestModelForInstamed()
            {

                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.Amount,
                PaymentMethod = "OnFile",
                Card = new Card()
                {
                    CVN = request.BinNumber,

                    CardNumber = decryptedCC,//
                    EntryMode = "key",
                    Expiration = request.ExpirationMonth + "" + request.ExpirationYear,

                },
                PaymentMethodID = request.PaymentMethodId,
                Patient = new Patient()
                {
                    AccountNumber = request.PatientAccount
                }

            };
            var resultVerify = await InstaMedSale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
                //todo  return the result of specific error
            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.Amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.AssociateDebtorAcct,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.Amount,
                    Subtotal = request.Amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _responseModelForInstamed.ResponseCode,
                    OrderNumber = _responseModelForInstamed.TransactionId,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.Amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.AssociateDebtorAcct,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.Amount,
                        Subtotal = request.Amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };

                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.AssociateDebtorAcct,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, true, _responseModelForInstamed);
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await IProGatewayAuth(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, resultVerify);
        }

        public async Task SaveCardInfoAndScheduleDataForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {

            try
            {
                var ccNUmber = request.ccNumber;

                var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();

                var encryptedCC = _crypto.Encrypt(ccNUmber, IVBase64, Key);

                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = "key",
                    BinNumber = request.cvv,
                    ExpirationMonth = 1,//todo 
                    ExpirationYear = 2,//todo
                    LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                    PaymentMethodId = encryptedCC,
                    Type = "VISA",
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
                {
                    var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                    {
                        CardInfoId = paymentScheduleObj.CardInfoId,
                        EffectiveDate = paymentDate,
                        IsActive = true,
                        NumberOfPayments = i,
                        PatientAccount = paymentScheduleObj.PatientAccount,
                        Amount = paymentScheduleObj.Amount
                    };
                    await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                    if (i == 1)
                    {
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    //AuthorizationNumber = _deserializeObjForElavon.,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                //_adoConnection.GetData("INSERT INTO UCG_PaymentScheduleHistory(PaymentScheduleId, TransactionId, ResponseCode, ResponseMessage," +
                //    " AuthorizationNumber, AuthorizationText) " +
                //    "VALUES("+ _lcgPaymentScheduleId + ", '"+ _deserializeObjForElavon.ssl_txn_id + "'," +
                //    " '"+ _deserializeObjForElavon.ssl_approval_code + "', '"+ _deserializeObjForElavon.ssl_result_message + "'," +
                //    " '"+ _deserializeObjForElavon.ssl_oar_data + "','_username'); ", environment);




                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    //await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);


                    //var ccPaymentObj = new CcPayment()
                    //{
                    //    DebtorAcct = request.debtorAcc,
                    //    Company = "TOTAL CREDIT RECOVERY",
                    //    UserId = "_username", //todo user name
                    //    UserName = "_username" + " -LCG", //todo user name
                    //    ChargeTotal = request.amount,
                    //    Subtotal = request.amount,
                    //    PaymentDate = _scheduleDateTime,
                    //    ApprovalStatus = "APPROVED",
                    //    BillingName = "", //todo billing person name 
                    //    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                    //    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                    //    RefNumber = "INSTAMEDLH",
                    //    Sif = "N"
                    //};
                    //await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                    _adoConnection.GetData("INSERT INTO cc_payment " +
                        "(debtor_acct,company, user_id,user_name,charge_total,subtotal,payment_date," +
                        "approval_status,approval_code, order_number,billing_name, ref_number, sif)" +
                        "VALUES('" + request.debtorAcc + "', 'TOTAL CREDIT RECOVERY', '_username', '_username elavon -API'," +
                        " " + request.amount + "," + request.amount + ", '" + _scheduleDateTime + "', 'APPROVED', '" + _deserializeObjForElavon.ssl_approval_code + "'," +
                        " '" + _deserializeObjForElavon.ssl_txn_id + "', '', 'INSTAMEDLH', 'N'); ", environment);
                }
                //new implementation 
                var companyFlag = await _getTheCompanyFlag.GetStringFlagForAdoQuery(request.debtorAcc, environment);

                var balance = _adoConnection.GetData("SELECT CAST(balance as float) as balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                var interestAmount = _adoConnection.GetData("SELECT interest_amt_life FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                var placements = _adoConnection.GetData("SELECT ISNULL((SELECT  placement FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'),0) as placement", environment);
                DataTable feePct = null;
                decimal feePctSimplified = 0;
                if (placements.Columns.Count > 1 && balance.Columns.Count > 1 && interestAmount.Columns.Count > 1)
                {

                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct1"];
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct2"];
                            break;
                    }


                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                            break;
                    }






                    var qFrom = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);
                    //for now
                    var qTo = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    var remit = _adoConnection.GetData("SELECT remit_full_pmt FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcc, 4) + "'", environment);
                    var adminAmount = _adoConnection.GetData("SELECT CAST(total_fees_balance as float) as total_fees_balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcc + "'", environment);

                    string paymentType = "DIRECT";//for elavon it's valid





                    // debugger
                    var a = request.debtorAcc;
                    var b = request.amount;
                    var balanceT = balance.Rows[0]["balance"];
                    var interestAmountT = interestAmount.Rows[0]["interest_amt_life"];
                    var e = feePctSimplified;
                    var qFromT = qFrom.Rows[0]["employee"];
                    var qToT = qTo.Rows[0]["employee"];
                    var remitT = remit.Rows[0]["remit_full_pmt"];
                    var m = paymentType;
                    var j = companyFlag;
                    var adminAmountT = adminAmount.Rows[0]["total_fees_balance"];
                    var l = request.debtorAcc;
                    //
                    var balanceC = Convert.ToSingle(balanceT);
                    var interestAmountC = Convert.ToDecimal(interestAmountT);
                    var qFromC = Convert.ToInt32(qFromT);
                    var qToC = Convert.ToInt32(qToT);
                    var remitC = Convert.ToString(remitT);
                    var adminAmountC = Convert.ToSingle(adminAmountT);




                    await PostPaymentA(request.debtorAcc, request.amount, balanceC,
                        interestAmountC, feePctSimplified, request.sif,
                        qFromC, qToC, remitC,
                        paymentType, companyFlag, adminAmountC, request.debtorAcc, environment);
                }


            }
            catch (Exception e)
            {
                throw;
            }


        }

        public async Task<ResponseModel> ProcessSaleTransForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed();
            if (environment == "P")
            {
                string ElavonclientUser= "8032017132";
                string ElavonclientPass="59WAMNKEGMMZVZARP37FUEST83WIOZYU6AJXKOMLHAZIIDP4LKGIZ65DDSSBMJ9M";
                if (request.debtorAcc.Substring(0, 4) == "1902")
                {
                    ElavonclientUser = "8032017132";
                    ElavonclientPass = "59WAMNKEGMMZVZARP37FUEST83WIOZYU6AJXKOMLHAZIIDP4LKGIZ65DDSSBMJ9M";
                }
                else
                {
                    ElavonclientUser = (from r in _context.LarryCcIndex2s
                                        where (r.AcctStatus == "A" && r.ClientAcct == request.debtorAcc.Substring(0, 4))
                                        select r.ClientUser).ToString();
                    ElavonclientPass = (from r in _context.LarryCcIndex2s
                                        where (r.AcctStatus == "A" && r.ClientAcct == request.debtorAcc.Substring(0, 4))
                                        select r.ClientPass).ToString();
                }
              




                saleRequestModel = new SaleRequestModelForInstamed()
                {
                    Outlet = new InstaMedOutlet()
                    {
                        MerchantID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id,
                        StoreID = ElavonclientUser,
                        TerminalID = ElavonclientPass

                    },
                    Amount = request.amount,
                    PaymentMethod = "Card",
                    Card = new Card()
                    {
                        CVN = request.cvv,
                        CardNumber = request.ccNumber,
                        EntryMode = "key",
                        Expiration = request.expiredDate,
                        IsCardDataEncrypted = false,
                        IsEMVCapableDevice = false,
                    }

                };
            }
            else
            {
                saleRequestModel = new SaleRequestModelForInstamed()
                {
                    Outlet = new InstaMedOutlet()
                    {
                        MerchantID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id,
                        StoreID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_user_id,
                        TerminalID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_pin
                    },
                    Amount = request.amount,
                    PaymentMethod = "Card",
                    Card = new Card()
                    {
                        CVN = request.cvv,
                        CardNumber = request.ccNumber,
                        EntryMode = "key",
                        Expiration = request.expiredDate,
                        IsCardDataEncrypted = false,
                        IsEMVCapableDevice = false,
                    }

                };
            }

            var resultVerify = await ElavonSale(saleRequestModel);
            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                _deserializeObjForElavon = (txn)ser.Deserialize(sr);

            }

            string noteText = null;
            if (_deserializeObjForElavon != null && _deserializeObjForElavon.ssl_issuer_response == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _deserializeObjForElavon.ssl_result_message.ToUpper() +
                           " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;

                //magic
                await SaveCardInfoAndScheduleDataForElavon(request, environment);
                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.debtorAcc,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.amount,
                    Subtotal = request.amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                    RefNumber = "ELAVON",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _deserializeObjForElavon.ssl_result_message.ToUpper() +
                               " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                        RefNumber = "ELAVON",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db




            if (_deserializeObjForElavon != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };


                return _response.Response(true, true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }
        //qa
        public async Task<ResponseModel> ProcessSaleTransForElavonQA(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await ElavonSale(saleRequestModel);
            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                _deserializeObjForElavon = (txn)ser.Deserialize(sr);

            }

            string noteText = null;
            if (_deserializeObjForElavon != null && _deserializeObjForElavon.ssl_issuer_response == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _deserializeObjForElavon.ssl_result_message.ToUpper() +
                           " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;

                //magic
                await SaveCardInfoAndScheduleDataForElavon(request, environment);
                //// for success
                //var ccPaymentObj = new CcPayment()
                //{
                //    DebtorAcct = request.debtorAcc,
                //    Company = "TOTAL CREDIT RECOVERY",
                //    //UserId = username,
                //    UserId = "_username", //todo   
                //    //UserName = username + " -LCG",
                //    UserName = "_username", //todo 
                //    ChargeTotal = request.amount,
                //    Subtotal = request.amount,
                //    PaymentDate = DateTime.Now,
                //    ApprovalStatus = "APPROVED",
                //    BillingName = "", // todo is it valid  for api transaction ? 
                //    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                //    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                //    RefNumber = "INSTAMEDLH",
                //    Sif = "N",
                //    VoidSale = "N"
                //};
                //await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_deserializeObjForElavon != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _deserializeObjForElavon.ssl_result_message.ToUpper() +
                               " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;
                // for DECLINED
                if (_deserializeObjForElavon != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_approval_code,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db




            if (_deserializeObjForElavon != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };


                //this segment is for QA
                //cc_payment variables
                string idCc = "";
                string debtor_acctCc = "";
                string companyCc = "";
                string user_idCc = "";
                string user_nameCc = "";
                string subtotalCc = "";
                string charge_totalCc = "";
                string payment_dateCc = "";
                string approval_statusCc = "";
                string approval_codeCc = "";
                string error_codeCc = "";
                string order_numberCc = "";
                string ref_numberCc = "";
                string sifCc = "";





                //todo

                var qaCC = _adoConnection.GetData("select * from cc_payment cc where cc.order_number='" + _deserializeObjForElavon.ssl_txn_id + "'", environment);
                if (qaCC.Rows.Count > 0)
                {
                    idCc = Convert.ToString(qaCC.Rows[0]["id"]); // 4
                    debtor_acctCc = Convert.ToString(qaCC.Rows[0]["debtor_acct"]); // 3
                    companyCc = Convert.ToString(qaCC.Rows[0]["company"]); // 2
                    user_idCc = Convert.ToString(qaCC.Rows[0]["user_id"]); // 5
                    user_nameCc = Convert.ToString(qaCC.Rows[0]["user_name"]); // 6
                    subtotalCc = Convert.ToString(qaCC.Rows[0]["subtotal"]); // 1
                    charge_totalCc = Convert.ToString(qaCC.Rows[0]["charge_total"]); // 1
                    payment_dateCc = Convert.ToString(qaCC.Rows[0]["payment_date"]); // 1
                    approval_statusCc = Convert.ToString(qaCC.Rows[0]["approval_status"]); // 1
                    approval_codeCc = Convert.ToString(qaCC.Rows[0]["approval_code"]); // 1
                    error_codeCc = Convert.ToString(qaCC.Rows[0]["error_code"]); // 1
                    order_numberCc = Convert.ToString(qaCC.Rows[0]["order_number"]); // 1
                    ref_numberCc = Convert.ToString(qaCC.Rows[0]["ref_number"]); // 1
                    sifCc = Convert.ToString(qaCC.Rows[0]["sif"]); // 1
                }

                var ccPaymnetTable = "cc_payment >>>>" + "--id-->" + idCc + "-- debtor_acct-->" + debtor_acctCc + "-- company -->" + companyCc + "-- user_id -->" +
                                     user_idCc + "-- user_name -->" + user_nameCc + "-- subtotal -->" + subtotalCc + "-- charge_total -->" + charge_totalCc
                                     + "-- payment_date -->" +
                                     payment_dateCc + "-- approval_status -->" + approval_statusCc + "-- approval_code -->" +
                                     approval_codeCc + "-- error_code -->" + error_codeCc + "-- order_number -->" + order_numberCc + "-- ref_number -->"
                                     + ref_numberCc + "--sif  -->" + sifCc;

                //[UCG_PaymentScheduleHistory] variables
                string Idhs = "";
                string PaymentScheduleIdhs = "";
                string TransactionIdhs = "";
                string ResponseCodehs = "";
                string ResponseMessagehs = "";
                string AuthorizationNumberhs = "";
                string AuthorizationTexths = "";
                string TimeLoghs = "";


                var qaHistory = _adoConnection.GetData("select * from UCG_PaymentScheduleHistory hs where hs.TransactionId='" + _deserializeObjForElavon.ssl_txn_id + "'", environment);

                if (qaHistory.Rows.Count > 0)
                {
                    Idhs = Convert.ToString(qaHistory.Rows[0]["Id"]); // 4
                    PaymentScheduleIdhs = Convert.ToString(qaHistory.Rows[0]["PaymentScheduleId"]); // 3
                    TransactionIdhs = Convert.ToString(qaHistory.Rows[0]["TransactionId"]); // 2
                    ResponseCodehs = Convert.ToString(qaHistory.Rows[0]["ResponseCode"]); // 5
                    ResponseMessagehs = Convert.ToString(qaHistory.Rows[0]["ResponseMessage"]); // 6
                    AuthorizationNumberhs = Convert.ToString(qaHistory.Rows[0]["AuthorizationNumber"]); // 1
                    AuthorizationTexths = Convert.ToString(qaHistory.Rows[0]["AuthorizationText"]); // 1
                    TimeLoghs = Convert.ToString(qaHistory.Rows[0]["TimeLog"]); // 1
                }
                var PaymentScheduleHistoryTable = "UCG_PaymentScheduleHistory >>>>" + "--id-->" + Idhs + "--id-->" + PaymentScheduleIdhs + "--TransactionIdhs-->" + TransactionIdhs
                                                  + "--ResponseCode-->" + ResponseCodehs + "--ResponseMessage-->" + ResponseMessagehs + "--AuthorizationNumber-->" + AuthorizationNumberhs
                                                  + "--AuthorizationText-->" + AuthorizationTexths + "--TimeLog-->" + TimeLoghs;

                //[UCG_PaymentSchedule] variables
                string IdPs = "";
                string PatientAccountPs = "";
                string EffectiveDatePs = "";
                string CardInfoIdPs = "";
                string NumberOfPaymentsPs = "";
                string AmountPs = "";
                string IsActivePs = "";


                var qaPaymentScheduleTEmp = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.Id='" + PaymentScheduleIdhs + "'", environment);

                //check if null
                string CardInfoIdTemp = "";
                if (qaPaymentScheduleTEmp.Rows.Count > 0)
                {
                    CardInfoIdTemp = Convert.ToString(qaPaymentScheduleTEmp.Rows[0]["CardInfoId"]); // 5
                }


                var qaPaymentSchedule = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.CardInfoId='" + CardInfoIdTemp + "'", environment);
                var qaPaymentScheduleStringList = new List<string>
                {
                    Capacity = 10
                };
                if (qaPaymentSchedule.Rows.Count > 0)
                {
                    for (int i = 0; i < qaPaymentSchedule.Rows.Count; i++)
                    {
                        IdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Id"]); // 4
                        PatientAccountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["PatientAccount"]); // 3
                        EffectiveDatePs = Convert.ToString(qaPaymentSchedule.Rows[i]["EffectiveDate"]); // 2
                        CardInfoIdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["CardInfoId"]); // 5
                        NumberOfPaymentsPs = Convert.ToString(qaPaymentSchedule.Rows[i]["NumberOfPayments"]); // 6
                        AmountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Amount"]); // 1
                        IsActivePs = Convert.ToString(qaPaymentSchedule.Rows[i]["IsActive"]); // 1


                        qaPaymentScheduleStringList.Add("UCG_PaymentSchedule >>>>" + "--id-->" + IdPs + "--PatientAccount-->" + PatientAccountPs +
                                                        "--EffectiveDate-->" + EffectiveDatePs + "--CardInfoId-->" + CardInfoIdPs
                                                        + "--NumberOfPayments-->" + NumberOfPaymentsPs + "--Amount-->" + AmountPs + "--IsActive-->" + IsActivePs);
                    }
                }



                //[UCG_CardInfo] variables
                string Idci = "";
                string PaymentMethodID = "";
                string EntryMode = "";
                string Type = "";
                string BinNumber = "";
                string LastFour = "";
                string ExpirationMonth = "";
                string ExpirationYear = "";
                string CardHolderName = "";
                string AssociateDebtorAcct = "";
                string IsActiveci = "";


                var qaUCG_CardInfo = _adoConnection.GetData("select * from UCG_CardInfo hs where hs.Id='" + CardInfoIdPs + "'", environment);

                if (qaUCG_CardInfo.Rows.Count > 0)
                {
                    Idci = Convert.ToString(qaUCG_CardInfo.Rows[0]["Id"]); // 4
                    PaymentMethodID = Convert.ToString(qaUCG_CardInfo.Rows[0]["PaymentMethodID"]); // 3
                    EntryMode = Convert.ToString(qaUCG_CardInfo.Rows[0]["EntryMode"]); // 2
                    Type = Convert.ToString(qaUCG_CardInfo.Rows[0]["Type"]); // 5
                    BinNumber = Convert.ToString(qaUCG_CardInfo.Rows[0]["BinNumber"]); // 6
                    LastFour = Convert.ToString(qaUCG_CardInfo.Rows[0]["LastFour"]); // 1
                    ExpirationMonth = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationMonth"]); // 1
                    ExpirationYear = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationYear"]); // 1
                    CardHolderName = Convert.ToString(qaUCG_CardInfo.Rows[0]["CardHolderName"]); // 1
                    AssociateDebtorAcct = Convert.ToString(qaUCG_CardInfo.Rows[0]["AssociateDebtorAcct"]); // 1
                    IsActiveci = Convert.ToString(qaUCG_CardInfo.Rows[0]["IsActive"]); // 1
                }

                var qaUCG_CardInfoString = "UCG_CardInfo >>>>" + "--Idci-->" + Idci + "--PaymentMethodID-->" + PaymentMethodID + "--EntryMode-->" + EntryMode
                                           + "--Type-->" + Type + "--BinNumber-->" + BinNumber + "--LastFour-->" + LastFour
                                           + "--ExpirationMonth-->" + ExpirationMonth + "--ExpirationYear-->" + ExpirationYear + "--CardHolderName-->" + CardHolderName
                                           + "--AssociateDebtorAcct-->" + AssociateDebtorAcct + "--CardHolderName-->" + CardHolderName + "--AssociateDebtorAcct-->" + AssociateDebtorAcct
                                           + "--IsActive-->" + IsActiveci;


                var dbQA = new List<string>
                {
                    Capacity = 5
                };

                dbQA.Add(ccPaymnetTable);
                dbQA.Add(qaUCG_CardInfoString);
                dbQA.Add(PaymentScheduleHistoryTable);
                dbQA.AddRange(qaPaymentScheduleStringList);


                //

                return _response.Response(true, true, response, dbQA);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        //on file
        public async Task<ResponseModel> ProcessOnfileSaleTransForElavon(AutoProcessCcUniversalViewModel request, string environment)
        {
            var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();
            var decryptedCC = _crypto.Decrypt(request.PaymentMethodId, IVBase64, Key);

            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.Amount,
                PaymentMethod = "OnFile",
                Card = new Card()
                {
                    CVN = request.BinNumber,

                    CardNumber = decryptedCC,//
                    EntryMode = "key",
                    Expiration = request.ExpirationMonth + "" + request.ExpirationYear,

                },
                PaymentMethodID = request.PaymentMethodId,
                Patient = new Patient()
                {
                    AccountNumber = request.PatientAccount
                }
            };
            var resultVerify = await ElavonSale(saleRequestModel);
            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                _deserializeObjForElavon = (txn)ser.Deserialize(sr);

            }

            string noteText = null;
            if (_deserializeObjForElavon != null && _deserializeObjForElavon.ssl_issuer_response == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.Amount + " " +
                           _deserializeObjForElavon.ssl_result_message.ToUpper() +
                           " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;


                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.AssociateDebtorAcct,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.Amount,
                    Subtotal = request.Amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.Amount + " " +
                               _deserializeObjForElavon.ssl_result_message.ToUpper() +
                               " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.AssociateDebtorAcct,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.Amount,
                        Subtotal = request.Amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.AssociateDebtorAcct,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db




            if (_deserializeObjForElavon != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };


                return _response.Response(true, true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await ElavonAuth(saleRequestModel);
            var objs = new txn();
            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                objs = (txn)ser.Deserialize(sr);
            }

            string noteText = null;
            if (objs != null && _responseModelForInstamed.ResponseCode == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           objs.ssl_result_message.ToUpper() +
                           " AUTH #:" + objs.ssl_oar_data;

                //magic 
                await SaveCardInfoAndScheduleDataForElavon(request, environment);
                // for success
            }
            else
            {
                if (objs != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               objs.ssl_result_message.ToUpper().ToUpper() +
                               " AUTH #:" + objs.ssl_oar_data;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, true, resultVerify);
        }


    }
}
