using AargonTools.Data.ADO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace AargonTools.Manager.GenericManager
{
    public class PostPaymentA
    {
        private readonly AdoDotNetConnection _adoConnection;
        public PostPaymentA(AdoDotNetConnection adoConnection)
        {
            _adoConnection = adoConnection;
        }

        public Task Post(string debtorAccount, decimal paymentAmount, float balance, decimal interestAmount,
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


    }
}
