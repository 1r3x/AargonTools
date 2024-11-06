using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Models;

namespace AargonTools.Manager
{
    public class GetInteractionsAcctDataManager : IGetInteractionsAcctData
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private static AdoDotNetConnection _adoConnection;


        public GetInteractionsAcctDataManager(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag,
            TestEnvironmentDbContext contextTest, AdoDotNetConnection adoConnection, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
            _adoConnection = adoConnection;
            _contextProdOld = contextProdOld;

        }




        private async Task<List<GetInteractionAcctDataViewModel>> GetInteractionsAcctDataHelperSpeedRun(string debtorAcct, string flag, string environment)
        {
            DateTime? latestDate;
            switch (environment)
            {
                case "P":
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _context.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _context.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _context.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }

                case "PO":
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _contextProdOld.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _contextProdOld.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _contextProdOld.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }
                default:
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _contextTest.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _contextTest.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _contextTest.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }
            }



            var CompanyflagString = "";
            if (flag == "")
            {
                CompanyflagString = "A";
            }
            else if (flag == "_d")
            {
                CompanyflagString = "D";
            }
            else if (flag == "_h")
            {
                CompanyflagString = "H";
            }

            else if (flag == "_l")
            {
                CompanyflagString = "L";
            }
            else if (flag == "_t")
            {
                CompanyflagString = "T";
            }
            else if (flag == "_w")
            {
                CompanyflagString = "W";
            }
            else if (flag == "_p")
            {
                CompanyflagString = "P";
            }


            var rowAdo = await _adoConnection.GetDataAsync("DECLARE @acctNoVar NVARCHAR(15)='" + debtorAcct + "'" +
                                                " IF(@acctNoVar='')" +
                                                "SET @acctNoVar=NULL " +
                                                "SELECT dai.debtor_acct," +
                                                "dai.balance," +
                                                "dai.email_address," +
                                                "dai.acct_status," +
                                                "dai.date_of_Service," +
                                                "dai.employee," +
                                                "dm.ssn," +
                                                "dm.address1," +
                                                "dm.address2," +
                                                "dm.city," +
                                                "dm.state_code," +
                                                "dm.zip," +
                                                "dm.birth_date," +
                                                "dm.first_name," +
                                                "dm.last_name," +
                                                "cm.client_name," +
                                                "cai.acct_type," +
                                                "dpi.home_area_code, " +
                                                "dpi.home_phone, " +
                                                "dpi.cell_area_code, " +
                                                "dpi.cell_phone, " +
                                                "dpi.work_area_code, " +
                                                "dpi.work_phone, " +
                                                "dpi.relative_area_code, " +
                                                "dpi.relative_phone, " +
                                                "dpi.other_area_code, " +
                                                "dpi.other_phone " +
                                                "FROM debtor_acct_info" + flag + " dai " +
                                                "INNER JOIN debtor_master" + flag + " dm on dm.debtor_acct=dai.debtor_acct " +
                                                "INNER JOIN debtor_phone_info dpi on dpi.debtor_acct=dai.debtor_acct " +
                                                "INNER JOIN client_master" + flag + " cm on cm.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "INNER JOIN client_acct_info" + flag + " cai on cai.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "WHERE dai.debtor_acct=@acctNoVar" +
                                                "", environment);


            var listOfItems = new List<GetInteractionAcctDataViewModel>();
            for (var i = 0; i < rowAdo.Rows.Count; i++)
            {
                var birthDateProcess = Convert.ToDateTime(rowAdo.Rows[i]["birth_date"]).ToString("d/M/yyyy");
                var date_of_ServiceProcess = Convert.ToDateTime(rowAdo.Rows[i]["date_of_Service"]).ToString("d/M/yyyy");
                var balanceProcess = Math.Round(Convert.ToDouble(rowAdo.Rows[i]["balance"]), 2);
                //

                var ssnExposed = Convert.ToString(rowAdo.Rows[i]["ssn"]);

                var sslMaskLast4 = "";

                for (int a = 0; a < ssnExposed.Length - 4; a++)
                {
                    sslMaskLast4 += "*";
                }

                sslMaskLast4 += ssnExposed.Substring(ssnExposed.Length - 4);

                //

                var itemData = new GetInteractionAcctDataViewModel
                {
                    debtorAcct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                    promiseDate = latestDate,
                    ssn = sslMaskLast4,
                    balance = balanceProcess,
                    address1 = Convert.ToString(rowAdo.Rows[i]["address1"]),
                    address2 = Convert.ToString(rowAdo.Rows[i]["address2"]),
                    birthDate = birthDateProcess,
                    cellPhoneNumber = Convert.ToString(rowAdo.Rows[i]["cell_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["cell_phone"]),
                    city = Convert.ToString(rowAdo.Rows[i]["city"]),
                    clientName = Convert.ToString(rowAdo.Rows[i]["client_name"]),
                    debtType = Convert.ToString(rowAdo.Rows[i]["acct_type"]),
                    emailAddress = Convert.ToString(rowAdo.Rows[i]["email_address"]),
                    firstName = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                    homePhoneNumber = Convert.ToString(rowAdo.Rows[i]["home_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["home_phone"]),
                    lastName = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                    otherPhoneNumer = Convert.ToString(rowAdo.Rows[i]["other_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["other_phone"]),
                    relativePhoneNumber = Convert.ToString(rowAdo.Rows[i]["relative_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["relative_phone"]),
                    stateCode = Convert.ToString(rowAdo.Rows[i]["state_code"]),
                    workPhoneNumber = Convert.ToString(rowAdo.Rows[i]["work_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["work_phone"]),
                    zip = Convert.ToString(rowAdo.Rows[i]["zip"]),
                    accountStatus = Convert.ToString(rowAdo.Rows[i]["acct_status"]),
                    date_of_Service = date_of_ServiceProcess,
                    employee = Convert.ToString(rowAdo.Rows[i]["employee"]),
                    companyFlag = CompanyflagString

                };
                Serilog.Log.Debug("Successfully retrieved interactions account: {debtorAcct} company flag: {companyFlag}", itemData.debtorAcct, itemData.companyFlag); //debug log
                listOfItems.Add(itemData);
            }

            return listOfItems;
        }



        public async Task<ResponseModel> GetInteractionsAcctDataSpeedRun(GetInteractionAcctDateRequestModel request, string environment)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.debtorAcct) || !string.IsNullOrEmpty(request.phone) || !string.IsNullOrEmpty(request.ssn))
                {

                    switch (environment)
                    {

                        case "PO":
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoData = await _contextProdOld.DebtorPhoneInfos
                                            .Where(phn =>
                                                (phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode) ||
                                                (phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode) ||
                                                (phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode))
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();

                                        if (phoneInfoData.Any())
                                        {
                                            foreach (var item in phoneInfoData)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }
                                            }
                                        }



                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoData.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoData.Select(a => a.CellAreaCode)) + "-" + (phoneInfoData.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoData.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoData.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoData.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoData.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoData.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoData.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }

                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }
                                            else
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));

                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }
                                            else
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {

                                        return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }
                                break;
                            }
                        case "P":
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoData = await _context.DebtorPhoneInfos
                                            .Where(phn =>
                                                (phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode) ||
                                                (phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode) ||
                                                (phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode))
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();
                                        var flag = "";
                                        if (phoneInfoData.Any())
                                        {
                                            foreach (var item in phoneInfoData)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }
                                            }
                                        }

                                        flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);

                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(debtorAccountActiveOne, flag, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoData.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoData.Select(a => a.CellAreaCode)) + "-" + (phoneInfoData.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoData.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoData.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoData.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoData.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoData.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoData.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }
                                            Serilog.Log.Debug("Successfully retrieved interactions account but with default data"); //debug log
                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorFlags = await _companyFlag.GetFlagForDebtorMasterBySsn(request.ssn, environment);
                                        var debtorAccountNumberFromSsnAsync = await debtorFlags
                                            .Where(x => x.Ssn == request.ssn)
                                            .Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        var flag = "";

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }
                                            else
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();

                                            }

                                        }
                                        flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(debtorAccountActiveOne, flag, environment));
                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();
                                        var flag = "";
                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(debtorAccountActiveOne, flag, environment));

                                    }
                                    else
                                    {

                                        var flag = await _companyFlag.GetStringFlagForAdoQuery(request.debtorAcct, environment);

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(request.debtorAcct, flag, environment));
                                        //return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }

                                break;
                            }
                        default:
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoData = await _contextTest.DebtorPhoneInfos
                                            .Where(phn =>
                                                (phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode) ||
                                                (phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode) ||
                                                (phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode))
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();
                                        var flag = "";
                                        if (phoneInfoData.Any())
                                        {
                                            foreach (var item in phoneInfoData)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }
                                            }
                                        }

                                        flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);

                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(debtorAccountActiveOne, flag, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoData.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoData.Select(a => a.CellAreaCode)) + "-" + (phoneInfoData.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoData.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoData.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoData.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoData.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoData.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoData.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }

                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorFlags = await _companyFlag.GetFlagForDebtorMasterBySsn(request.ssn, environment);
                                        var debtorAccountNumberFromSsnAsync = await debtorFlags
                                            .Where(x => x.Ssn == request.ssn)
                                            .Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        var flag = "";

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }
                                            else
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();

                                            }

                                        }
                                        flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(debtorAccountActiveOne, flag, environment));
                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        var flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAccountActiveOne, environment);
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {

                                        var flag = await _companyFlag.GetStringFlagForAdoQuery(request.debtorAcct, environment);

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelperSpeedRun(request.debtorAcct, flag, environment));
                                        //return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }

                                break;
                            }
                    }
                }
                else
                {
                    // Handle the case when none of the variables are provided
                    return _response.Response("You must provide one of the variables, otherwise the endpoint gets overwhelmed and loads unnecessary data. It's a waste of energy.");
                }
            }
            catch (Exception e)
            {
                return _response.Response(e.Message);

            }

            return _response.Response("Oops... must be provide one of the parameter");
        }





        private async Task<List<GetInteractionAcctDataViewModel>> GetInteractionsAcctDataHelper(string debtorAcct, string ssn, string phoneNo,
           string phoneAreaCode, string environment)
        {

            DateTime? latestDate;
            switch (environment)
            {
                case "P":
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _context.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _context.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _context.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }
                case "PO":
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _contextProdOld.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _contextProdOld.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _contextProdOld.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }
                default:
                    {
                        var dateTimeList = new List<DateTime>();
                        var dateFromCheckDetails = await (from checkDate in _contextTest.CheckDetails
                                                          where checkDate.DebtorAcct == debtorAcct
                                                          select checkDate.CheckDate).SingleOrDefaultAsync();
                        var dateFromLarryCcPayment = await (from checkDate in _contextTest.LarryCcPayments
                                                            where checkDate.DebtorAcct == debtorAcct && checkDate.Processed == "N"
                                                            select checkDate.DateProcess).SingleOrDefaultAsync();
                        var dateFromDebtorPpInfo = await (from checkDate in _contextTest.DebtorPpInfos
                                                          where checkDate.DebtorAcct == debtorAcct && checkDate.PromiseType != null
                                                          select checkDate.PpDate1).SingleOrDefaultAsync();

                        dateTimeList.Add(dateFromCheckDetails);
                        if (dateFromLarryCcPayment != null) dateTimeList.Add((DateTime)dateFromLarryCcPayment);
                        if (dateFromDebtorPpInfo != null) dateTimeList.Add((DateTime)dateFromDebtorPpInfo);


                        latestDate = dateTimeList.Max();
                        break;
                    }
            }



            var flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAcct, environment);
            var CompanyflagString = await _companyFlag.GetStringFlag(debtorAcct, environment);



            var rowAdo = await _adoConnection.GetDataAsync("DECLARE @acctNoVar NVARCHAR(15)='" + debtorAcct + "'" +
                                                " IF(@acctNoVar='')" +
                                                "SET @acctNoVar=NULL " +
                                                "SELECT distinct dai.debtor_acct," +
                                                "dai.balance," +
                                                "dai.email_address," +
                                                "dai.acct_status," +
                                                "dai.date_of_Service," +
                                                "dai.employee," +
                                                "dm.ssn," +
                                                "dm.address1," +
                                                "dm.address2," +
                                                "dm.city," +
                                                "dm.state_code," +
                                                "dm.zip," +
                                                "dm.birth_date," +
                                                "dm.first_name," +
                                                "dm.last_name," +
                                                "cm.client_name," +
                                                "cai.acct_type," +
                                                "dpi.home_area_code, " +
                                                "dpi.home_phone, " +
                                                "dpi.cell_area_code, " +
                                                "dpi.cell_phone, " +
                                                "dpi.work_area_code, " +
                                                "dpi.work_phone, " +
                                                "dpi.relative_area_code, " +
                                                "dpi.relative_phone, " +
                                                "dpi.other_area_code, " +
                                                "dpi.other_phone " +
                                                "FROM debtor_acct_info" + flag + " dai " +
                                                "LEFT JOIN debtor_master" + flag + " dm on dm.debtor_acct=dai.debtor_acct " +
                                                "LEFT JOIN debtor_phone_info dpi on dpi.debtor_acct=dai.debtor_acct " +
                                                "LEFT OUTER JOIN client_master" + flag + " cm on cm.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "LEFT OUTER JOIN client_acct_info" + flag + " cai on cai.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "WHERE dai.debtor_acct=@acctNoVar" +
                                                "", environment);


            var listOfItems = new List<GetInteractionAcctDataViewModel>();
            for (var i = 0; i < rowAdo.Rows.Count; i++)
            {
                var birthDateProcess = Convert.ToDateTime(rowAdo.Rows[i]["birth_date"]).ToString("d/M/yyyy");
                var date_of_ServiceProcess = Convert.ToDateTime(rowAdo.Rows[i]["date_of_Service"]).ToString("d/M/yyyy");
                var balanceProcess = Math.Round(Convert.ToDouble(rowAdo.Rows[i]["balance"]), 2);
                //

                var ssnExposed = Convert.ToString(rowAdo.Rows[i]["ssn"]);

                var sslMaskLast4 = "";

                for (int a = 0; a < ssnExposed.Length - 4; a++)
                {
                    sslMaskLast4 += "*";
                }

                sslMaskLast4 += ssnExposed.Substring(ssnExposed.Length - 4);

                //

                var itemData = new GetInteractionAcctDataViewModel
                {
                    debtorAcct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                    promiseDate = latestDate,
                    ssn = sslMaskLast4,
                    balance = balanceProcess,
                    address1 = Convert.ToString(rowAdo.Rows[i]["address1"]),
                    address2 = Convert.ToString(rowAdo.Rows[i]["address2"]),
                    birthDate = birthDateProcess,
                    cellPhoneNumber = Convert.ToString(rowAdo.Rows[i]["cell_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["cell_phone"]),
                    city = Convert.ToString(rowAdo.Rows[i]["city"]),
                    clientName = Convert.ToString(rowAdo.Rows[i]["client_name"]),
                    debtType = Convert.ToString(rowAdo.Rows[i]["acct_type"]),
                    emailAddress = Convert.ToString(rowAdo.Rows[i]["email_address"]),
                    firstName = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                    homePhoneNumber = Convert.ToString(rowAdo.Rows[i]["home_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["home_phone"]),
                    lastName = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                    otherPhoneNumer = Convert.ToString(rowAdo.Rows[i]["other_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["other_phone"]),
                    relativePhoneNumber = Convert.ToString(rowAdo.Rows[i]["relative_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["relative_phone"]),
                    stateCode = Convert.ToString(rowAdo.Rows[i]["state_code"]),
                    workPhoneNumber = Convert.ToString(rowAdo.Rows[i]["work_area_code"]) + "" + Convert.ToString(rowAdo.Rows[i]["work_phone"]),
                    zip = Convert.ToString(rowAdo.Rows[i]["zip"]),
                    accountStatus = Convert.ToString(rowAdo.Rows[i]["acct_status"]),
                    date_of_Service = date_of_ServiceProcess,
                    employee = Convert.ToString(rowAdo.Rows[i]["employee"]),
                    companyFlag = CompanyflagString

                };
                listOfItems.Add(itemData);
            }

            return listOfItems;
        }

        public async Task<ResponseModel> GetInteractionsAcctData(GetInteractionAcctDateRequestModel request, string environment)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.debtorAcct) || !string.IsNullOrEmpty(request.phone) || !string.IsNullOrEmpty(request.ssn))
                {

                    switch (environment)
                    {

                        case "PO":
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoDataFromHome = await _contextProdOld.DebtorPhoneInfos
                                            .Where(phn => phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        var phoneInfoDataFromCell = await _contextProdOld.DebtorPhoneInfos
                                            .Where(phn => phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();






                                        var phoneInfoDataFromOther = await _contextProdOld.DebtorPhoneInfos
                                            .Where(phn => phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        if (phoneInfoDataFromHome.Any())
                                        {

                                            foreach (var item in phoneInfoDataFromHome)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }

                                        }


                                        else if (phoneInfoDataFromCell.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromCell)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }

                                        }





                                        else if (phoneInfoDataFromOther.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromOther)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }
                                        }


                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoDataFromHome.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoDataFromHome.Select(a => a.CellAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoDataFromHome.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoDataFromHome.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoDataFromHome.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoDataFromHome.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }

                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));

                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {

                                        return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }

                                break;
                            }
                        case "P":
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoDataFromHome = await _context.DebtorPhoneInfos
                                            .Where(phn => phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        var phoneInfoDataFromCell = await _context.DebtorPhoneInfos
                                            .Where(phn => phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();






                                        var phoneInfoDataFromOther = await _context.DebtorPhoneInfos
                                            .Where(phn => phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        if (phoneInfoDataFromHome.Any())
                                        {

                                            foreach (var item in phoneInfoDataFromHome)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }

                                            }

                                        }


                                        else if (phoneInfoDataFromCell.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromCell)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }

                                            }

                                        }





                                        else if (phoneInfoDataFromOther.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromOther)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }
                                                else
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                }


                                            }
                                        }


                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoDataFromHome.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoDataFromHome.Select(a => a.CellAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoDataFromHome.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoDataFromHome.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoDataFromHome.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoDataFromHome.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }

                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();




                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }
                                            else
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                            }

                                        }

                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                        }


                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {

                                        return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }

                                break;
                            }
                        default:
                            {
                                if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                                {
                                    string phoneAreaCode;
                                    string phoneNo;
                                    string debtorAccountActiveOne = "";
                                    if (request.phone == "")
                                    {
                                        phoneAreaCode = "";
                                        phoneNo = "";
                                    }
                                    else
                                    {
                                        phoneAreaCode = request.phone.Substring(0, 3);
                                        phoneNo = request.phone.Substring(3, 7);
                                    }

                                    if (request.debtorAcct == "" && request.ssn == "")
                                    {
                                        var phoneInfoDataFromHome = await _contextTest.DebtorPhoneInfos
                                            .Where(phn => phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        var phoneInfoDataFromCell = await _contextTest.DebtorPhoneInfos
                                            .Where(phn => phn.CellPhone == phoneNo && phn.CellAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();






                                        var phoneInfoDataFromOther = await _contextTest.DebtorPhoneInfos
                                            .Where(phn => phn.OtherPhone == phoneNo && phn.OtherAreaCode == phoneAreaCode)
                                            .Select(phn => new
                                            {
                                                phn.HomePhone,
                                                phn.HomeAreaCode,
                                                phn.CellPhone,
                                                phn.CellAreaCode,
                                                phn.RelativeAreaCode,
                                                phn.RelativePhone,
                                                phn.OtherAreaCode,
                                                phn.OtherPhone,
                                                phn.DebtorAcct,
                                                phn.WorkAreaCode,
                                                phn.WorkPhone
                                            }).ToListAsync();




                                        if (phoneInfoDataFromHome.Any())
                                        {

                                            foreach (var item in phoneInfoDataFromHome)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }

                                        }


                                        else if (phoneInfoDataFromCell.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromCell)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }

                                        }





                                        else if (phoneInfoDataFromOther.Any())
                                        {
                                            foreach (var item in phoneInfoDataFromOther)
                                            {
                                                var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item.DebtorAcct, environment))
                                                    .Where(x => x.DebtorAcct == item.DebtorAcct);
                                                if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                                {
                                                    debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                    break;
                                                }

                                            }
                                        }


                                        if (debtorAccountActiveOne != null && debtorAccountActiveOne.Any())
                                        {
                                            return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                        }
                                        else
                                        {
                                            var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                            for (var i = 0; i < phoneInfoDataFromHome.Count(); i++)
                                            {
                                                var itemData = new GetInteractionAcctDataViewModel
                                                {
                                                    debtorAcct = null,
                                                    promiseDate = null,
                                                    ssn = null,
                                                    balance = 0,
                                                    address1 = null,
                                                    address2 = null,
                                                    birthDate = null,
                                                    cellPhoneNumber = (phoneInfoDataFromHome.Select(a => a.CellAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.CellPhone)),
                                                    city = null,
                                                    clientName = null,
                                                    debtType = null,
                                                    emailAddress = null,
                                                    firstName = null,
                                                    homePhoneNumber = (phoneInfoDataFromHome.Select(a => a.HomeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.HomePhone)),
                                                    lastName = null,
                                                    otherPhoneNumer = (phoneInfoDataFromHome.Select(a => a.OtherAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.OtherPhone)),
                                                    relativePhoneNumber = (phoneInfoDataFromHome.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.RelativePhone)),
                                                    stateCode = null,
                                                    workPhoneNumber = (phoneInfoDataFromHome.Select(a => a.WorkAreaCode)) + "-" + (phoneInfoDataFromHome.Select(a => a.WorkPhone)),
                                                    zip = null
                                                };
                                                listOfItems.Add(itemData);
                                            }

                                            return _response.Response(listOfItems);
                                        }

                                    }
                                    else if (request.debtorAcct == "" && request.phone == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));

                                    }
                                    else if (request.debtorAcct == "")
                                    {
                                        var debtorAccountNumberFromSsnAsync = await _companyFlag
                                            .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                            .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                            .ToListAsync();

                                        foreach (var item in debtorAccountNumberFromSsnAsync)
                                        {
                                            var debtorInfoFlag = (await _companyFlag.GetFlagForDebtorAccount(item, environment))
                                                .Where(x => x.DebtorAcct == item);
                                            if (debtorInfoFlag.Any(x => x.AcctStatus == "A"))
                                            {
                                                debtorAccountActiveOne = await debtorInfoFlag.Select(x => x.DebtorAcct).FirstOrDefaultAsync();
                                                break;
                                            }

                                        }
                                        //changed to GetInteractionsAcctDataHelperWithoutDebtorAcct to GetInteractionsAcctDataHelper
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountActiveOne, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {

                                        return _response.Response(await GetInteractionsAcctDataHelper(request.debtorAcct, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }




                                }

                                break;
                            }
                    }
                }
                else
                {
                    // Handle the case when none of the variables are provided
                    return _response.Response("You must provide one of the variables, otherwise the endpoint gets overwhelmed and loads unnecessary data. It's a waste of energy.");
                }
            }
            catch (Exception e)
            {
                return _response.Response(e.Message);

            }

            return _response.Response("Oops... must be provide one of the parameter");
        }
    }
}
