using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class GetAccountInformation : IGetAccountInformation
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private readonly AdoDotNetConnection _adoConnection;


        public GetAccountInformation(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag,
            TestEnvironmentDbContext contextTest, AdoDotNetConnection adoConnection, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
            _adoConnection = adoConnection;
            _contextProdOld = contextProdOld;

        }
        async Task<ResponseModel> IGetAccountInformation.GetAccountBalanceByDebtorAccount(string debtorAcct, string environment)
        {

            var data = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            return _response.Response(true, data);
        }

        ResponseModel IGetAccountInformation.CheckAccountValidityByDebtorAccount(string debtorAcct, string environment)
        {
            //no implementation for environment
            var rx = new Regex(@"\d{4}-\d{6}");
            return _response.Response(rx.Match(debtorAcct).Success ? "Its a Valid Account." : "Its not a Valid Account.");
        }

        async Task<ResponseModel> IGetAccountInformation.CheckAccountExistenceByDebtorAccount(string debtorAcct, string environment)
        {
            var item = new List<string>(new[] { "Not Found" });

            //P for prod.
            if (environment == "P")
            {
                item.Clear();
                if (await _context.DebtorAcctInfos.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("A");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("D");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("H");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("L");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("T");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("W");
                    item.Add("True");
                }
                else
                {
                    item.Add("Not Found");
                    return _response.Response(item);
                }
                return _response.Response(item);
            }
            else if (environment == "PO")
            {
                item.Clear();
                if (await _contextProdOld.DebtorAcctInfos.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("A");
                    item.Add("True");
                }
                else if (await _contextProdOld.DebtorAcctInfoDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("D");
                    item.Add("True");
                }
                else if (await _contextProdOld.DebtorAcctInfoHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("H");
                    item.Add("True");
                }
                else if (await _context.DebtorAcctInfoLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("L");
                    item.Add("True");
                }
                else if (await _contextProdOld.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("T");
                    item.Add("True");
                }
                else if (await _contextProdOld.DebtorAcctInfoWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Add("W");
                    item.Add("True");
                }
                else
                {
                    item.Add("Not Found");
                    return _response.Response(item);
                }
                return _response.Response(item);
            }
            else
            {
                if (await _contextTest.DebtorAcctInfos.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("A");
                    item.Add("True");
                }
                else if (await _contextTest.DebtorAcctInfoDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("D");
                    item.Add("True");
                }
                else if (await _contextTest.DebtorAcctInfoHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("H");
                    item.Add("True");
                }
                else if (await _contextTest.DebtorAcctInfoLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("L");
                    item.Add("True");
                }
                else if (await _contextTest.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("T");
                    item.Add("True");
                }
                else if (await _contextTest.DebtorAcctInfoWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    item.Remove("Not Found");
                    item.Add("W");
                    item.Add("True");
                }
                else
                {
                    return _response.Response(item);
                }
                return _response.Response(item);
            }


        }

        async Task<ResponseModel> IGetAccountInformation.GetRecentApprovalByDebtorAccount(string debtorAcct, string environment)
        {
            //P for prod.
            if (environment == "P")
            {
                var approvalStatus = await _context.CcPayments.CountAsync(x =>
                    x.DebtorAcct == debtorAcct && x.PaymentDate == DateTime.Now.AddMinutes(-5) && x.ApprovalStatus == "APPROVED");

                return _response.Response(approvalStatus > 0);
            }
            else if (environment == "PO")
            {
                var approvalStatus = await _contextProdOld.CcPayments.CountAsync(x =>
                    x.DebtorAcct == debtorAcct && x.PaymentDate == DateTime.Now.AddMinutes(-5) && x.ApprovalStatus == "APPROVED");

                return _response.Response(approvalStatus > 0);
            }
            else
            {
                var approvalStatus = await _contextTest.CcPayments.CountAsync(x =>
                    x.DebtorAcct == debtorAcct && x.PaymentDate == DateTime.Now.AddMinutes(-5) && x.ApprovalStatus == "APPROVED");

                return _response.Response(approvalStatus > 0);
            }

        }

        async Task<ResponseModel> IGetAccountInformation.GetMultiples(string debtorAcct, string environment)
        {
            //P for prod.
            if (environment == "P")
            {
                var listOfData = await (_companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result
                    .Join(_context.DebtorMultiples, accountInfo => accountInfo.DebtorAcct,
                        debtorMultiples => debtorMultiples.DebtorAcct2,
                        (accountInfo, debtorMultiples) => new { accountInfo, debtorMultiples })
                    .Where(@t => @t.debtorMultiples.DebtorAcct == debtorAcct)
                    .Select(@t => new { @t.accountInfo.DebtorAcct, @t.accountInfo.Balance })).ToListAsync();
                return _response.Response(listOfData);
            }
            else if (environment == "PO")
            {
                var listOfData = await (_companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result
                    .Join(_contextProdOld.DebtorMultiples, accountInfo => accountInfo.DebtorAcct,
                        debtorMultiples => debtorMultiples.DebtorAcct2,
                        (accountInfo, debtorMultiples) => new { accountInfo, debtorMultiples })
                    .Where(@t => @t.debtorMultiples.DebtorAcct == debtorAcct)
                    .Select(@t => new { @t.accountInfo.DebtorAcct, @t.accountInfo.Balance })).ToListAsync();
                return _response.Response(listOfData);
            }
            else
            {
                var listOfData = await (_companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result
                    .Join(_contextTest.DebtorMultiples, accountInfo => accountInfo.DebtorAcct,
                        debtorMultiples => debtorMultiples.DebtorAcct2,
                        (accountInfo, debtorMultiples) => new { accountInfo, debtorMultiples })
                    .Where(@t => @t.debtorMultiples.DebtorAcct == debtorAcct)
                    .Select(@t => new { @t.accountInfo.DebtorAcct, @t.accountInfo.Balance })).ToListAsync();
                return _response.Response(listOfData);
            }

        }

        async Task<ResponseModel> IGetAccountInformation.GetAccountInfo(string debtorAcct, string environment)
        {
            //P for prod.
            if (environment == "P")
            {
                var data = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment)
                   .Result.Where(a => a.DebtorAcct == debtorAcct)
                   .Select(a => new
                   {
                       a.DebtorAcct,
                       a.SuppliedAcct,
                       AcountStatus =
                           a.AcctStatus == Convert.ToString('A') ? "ACTIVE" :
                           a.AcctStatus == Convert.ToString('M') ? "ACTIVE" : "INACTIVE",
                       a.Balance,
                       a.MailReturn,
                       a.Employee,
                       a.DateOfService,
                       a.DatePlaced,
                       a.LastPaymentAmt,
                       TotalPayments = a.PaymentAmtLife,
                       LastPayDate = a.LastPaymentAmt != null
                           ? Convert.ToString(a.BeginAgeDate, CultureInfo.InvariantCulture)
                           : null
                   }).ToListAsync();
                return _response.Response(data);
            }
            else if (environment == "PO")
            {
                var data = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment)
                    .Result.Where(a => a.DebtorAcct == debtorAcct)
                    .Select(a => new
                    {
                        a.DebtorAcct,
                        a.SuppliedAcct,
                        AcountStatus =
                            a.AcctStatus == Convert.ToString('A') ? "ACTIVE" :
                            a.AcctStatus == Convert.ToString('M') ? "ACTIVE" : "INACTIVE",
                        a.Balance,
                        a.MailReturn,
                        a.Employee,
                        a.DateOfService,
                        a.DatePlaced,
                        a.LastPaymentAmt,
                        TotalPayments = a.PaymentAmtLife,
                        LastPayDate = a.LastPaymentAmt != null
                            ? Convert.ToString(a.BeginAgeDate, CultureInfo.InvariantCulture)
                            : null
                    }).ToListAsync();
                return _response.Response(data);
            }
            else
            {
                var data = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment)
                    .Result.Where(a => a.DebtorAcct == debtorAcct)
                    .Select(a => new
                    {
                        a.DebtorAcct,
                        a.SuppliedAcct,
                        AcountStatus =
                            a.AcctStatus == Convert.ToString('A') ? "ACTIVE" :
                            a.AcctStatus == Convert.ToString('M') ? "ACTIVE" : "INACTIVE",
                        a.Balance,
                        a.MailReturn,
                        a.Employee,
                        a.DateOfService,
                        a.DatePlaced,
                        a.LastPaymentAmt,
                        TotalPayments = a.PaymentAmtLife,
                        LastPayDate = a.LastPaymentAmt != null
                            ? Convert.ToString(a.BeginAgeDate, CultureInfo.InvariantCulture)
                            : null
                    }).ToListAsync();
                return _response.Response(data);
            }


        }

        async Task<ResponseModel> IGetAccountInformation.GetSIF(string debtorAcct, string environment)
        {
            //P for prod.
            if (environment == "P")
            {
                var flag = await _companyFlag.GetStringFlag(debtorAcct, environment);

                var rowAdo = _adoConnection.GetData("SELECT * FROM func_sif('" + debtorAcct + "', '" + flag + "')", environment);
                var listOfItems = new List<SIFViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new SIFViewModel
                    {
                        AccountBalance = Convert.ToDecimal(rowAdo.Rows[i]["account_balance"]),
                        SifDiscount = Convert.ToDecimal(rowAdo.Rows[i]["sif_discount"]),
                        SifPayNow = Convert.ToDecimal(rowAdo.Rows[i]["sif_pay_now"]),
                        SifPct = Convert.ToDecimal(rowAdo.Rows[i]["sif_pct"])
                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            else if (environment == "PO")
            {
                var flag = await _companyFlag.GetStringFlag(debtorAcct, environment);

                var rowAdo = _adoConnection.GetData("SELECT * FROM func_sif('" + debtorAcct + "', '" + flag + "')", environment);
                var listOfItems = new List<SIFViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new SIFViewModel
                    {
                        AccountBalance = Convert.ToDecimal(rowAdo.Rows[i]["account_balance"]),
                        SifDiscount = Convert.ToDecimal(rowAdo.Rows[i]["sif_discount"]),
                        SifPayNow = Convert.ToDecimal(rowAdo.Rows[i]["sif_pay_now"]),
                        SifPct = Convert.ToDecimal(rowAdo.Rows[i]["sif_pct"])
                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            else
            {
                var flag = await _companyFlag.GetStringFlag(debtorAcct, environment);

                var rowAdo = _adoConnection.GetData("SELECT * FROM func_sif('" + debtorAcct + "', '" + flag + "')", environment);
                var listOfItems = new List<SIFViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new SIFViewModel
                    {
                        AccountBalance = Convert.ToDecimal(rowAdo.Rows[i]["account_balance"]),
                        SifDiscount = Convert.ToDecimal(rowAdo.Rows[i]["sif_discount"]),
                        SifPayNow = Convert.ToDecimal(rowAdo.Rows[i]["sif_pay_now"]),
                        SifPct = Convert.ToDecimal(rowAdo.Rows[i]["sif_pct"])
                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
        }

        public async Task<ResponseModel> GetInteractionsAcctData(GetInteractionAcctDateRequestModel request, string environment)
        {
            try
            {
                switch (environment)
                {
                    case "PO":
                        {
                            if (request.debtorAcct != "" || request.phone != "" || request.ssn != "")
                            {
                                string phoneAreaCode;
                                string phoneNo;
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
                                    var phoneInfoData = from phn in _contextProdOld.DebtorPhoneInfos
                                                        where phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode
                                                        select new
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
                                                        };
                                    var debtorAccountCheckFromPhone = await (phoneInfoData.Select(a => a.DebtorAcct)).SingleOrDefaultAsync();
                                    if (debtorAccountCheckFromPhone.Any())
                                    {
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountCheckFromPhone, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {
                                        var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                        for (var i = 0; i < phoneInfoData.Count(); i++)
                                        {
                                            var itemData = new GetInteractionAcctDataViewModel
                                            {
                                                debtorAcct = null,
                                                date = null,
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
                                                relatiovePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
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


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));

                                }
                                else if (request.debtorAcct == "")
                                {
                                    var debtorAccountNumberFromSsnAsync = await _companyFlag
                                        .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                        .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                        .ToListAsync();


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));
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
                                    var phoneInfoData = from phn in _context.DebtorPhoneInfos
                                                        where phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode
                                                        select new
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
                                                        };
                                    var debtorAccountCheckFromPhone = await (phoneInfoData.Select(a => a.DebtorAcct)).SingleOrDefaultAsync();
                                    if (debtorAccountCheckFromPhone.Any())
                                    {
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountCheckFromPhone, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {
                                        var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                        for (var i = 0; i < phoneInfoData.Count(); i++)
                                        {
                                            var itemData = new GetInteractionAcctDataViewModel
                                            {
                                                debtorAcct = null,
                                                date = null,
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
                                                relatiovePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
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


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));

                                }
                                else if (request.debtorAcct == "")
                                {
                                    var debtorAccountNumberFromSsnAsync = await _companyFlag
                                        .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                        .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                        .ToListAsync();


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));
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
                                    var phoneInfoData = from phn in _contextTest.DebtorPhoneInfos
                                                        where phn.HomePhone == phoneNo && phn.HomeAreaCode == phoneAreaCode
                                                        select new
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
                                                        };
                                    var debtorAccountCheckFromPhone = await (phoneInfoData.Select(a => a.DebtorAcct)).SingleOrDefaultAsync();
                                    if (debtorAccountCheckFromPhone.Any())
                                    {
                                        return _response.Response(await GetInteractionsAcctDataHelper(debtorAccountCheckFromPhone, request.ssn, phoneNo, phoneAreaCode, environment));
                                    }
                                    else
                                    {
                                        var listOfItems = new List<GetInteractionAcctDataViewModel>();
                                        for (var i = 0; i < phoneInfoData.Count(); i++)
                                        {
                                            var itemData = new GetInteractionAcctDataViewModel
                                            {
                                                debtorAcct = null,
                                                date = null,
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
                                                relatiovePhoneNumber = (phoneInfoData.Select(a => a.RelativeAreaCode)) + "-" + (phoneInfoData.Select(a => a.RelativePhone)),
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


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));

                                }
                                else if (request.debtorAcct == "")
                                {
                                    var debtorAccountNumberFromSsnAsync = await _companyFlag
                                        .GetFlagForDebtorMasterBySsn(request.ssn, environment).Result
                                        .Where(x => x.Ssn == request.ssn).Select(x => x.DebtorAcct)
                                        .ToListAsync();


                                    return _response.Response(await GetInteractionsAcctDataHelperWithoutDebtorAcct(debtorAccountNumberFromSsnAsync[0], request.ssn, phoneNo, phoneAreaCode, environment));
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
            catch (Exception e)
            {
                return _response.Response(e.Message);

            }

            return _response.Response("Oops... must be provide one of the parameter");
        }

        public async Task<ResponseModel> GetClientInvoiceHeader(string clientAcct, string company, string environment)
        {
            var flag = company switch
            {
                "A" => "",
                "T" => "_t",
                "D" => "_d",
                "H" => "_h",
                "L" => "_l",
                "W" => "_w",
                _ => ""
            };

            if (environment == "P")
            {
                var rowAdo = _adoConnection.GetData("SELECT client_acct_info.client_acct," +
                                                    "client_master.orig_creditor," +
                                                    "remit_full_pmt," +
                                                    "address12," +
                                                    "address22," +
                                                    "city2," +
                                                    "state_code2," +
                                                    "zip2 " +
                                                    "FROM client_acct_info" + flag + ",client_master" + flag + " " +
                                                    "WHERE client_master.client_acct = client_acct_info.client_acct" +
                                                    " AND client_acct_info.client_acct = '" + clientAcct + "'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoiceHeaderViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoiceHeaderViewModel
                    {

                        client_acct = Convert.ToString(rowAdo.Rows[i]["client_acct"]),
                        address12 = Convert.ToString(rowAdo.Rows[i]["address12"]),
                        address22 = Convert.ToString(rowAdo.Rows[i]["address22"]),
                        city2 = Convert.ToString(rowAdo.Rows[i]["city2"]),
                        orig_creditor = Convert.ToString(rowAdo.Rows[i]["orig_creditor"]),
                        remit_full_pmt = Convert.ToString(rowAdo.Rows[i]["remit_full_pmt"]),
                        state_code2 = Convert.ToString(rowAdo.Rows[i]["state_code2"]),
                        zip2 = Convert.ToString(rowAdo.Rows[i]["zip2"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }


            if (environment == "PO")
            {
                var rowAdo = _adoConnection.GetData("SELECT client_acct_info.client_acct," +
                                                    "client_master.orig_creditor," +
                                                    "remit_full_pmt," +
                                                    "address12," +
                                                    "address22," +
                                                    "city2," +
                                                    "state_code2," +
                                                    "zip2 " +
                                                    "FROM client_acct_info" + flag + ",client_master" + flag + " " +
                                                    "WHERE client_master.client_acct = client_acct_info.client_acct" +
                                                    " AND client_acct_info.client_acct = '" + clientAcct + "'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoiceHeaderViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoiceHeaderViewModel
                    {

                        client_acct = Convert.ToString(rowAdo.Rows[i]["client_acct"]),
                        address12 = Convert.ToString(rowAdo.Rows[i]["address12"]),
                        address22 = Convert.ToString(rowAdo.Rows[i]["address22"]),
                        city2 = Convert.ToString(rowAdo.Rows[i]["city2"]),
                        orig_creditor = Convert.ToString(rowAdo.Rows[i]["orig_creditor"]),
                        remit_full_pmt = Convert.ToString(rowAdo.Rows[i]["remit_full_pmt"]),
                        state_code2 = Convert.ToString(rowAdo.Rows[i]["state_code2"]),
                        zip2 = Convert.ToString(rowAdo.Rows[i]["zip2"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }


            if (environment == "T")
            {
                var rowAdo = _adoConnection.GetData("SELECT client_acct_info.client_acct," +
                                                    "client_master.orig_creditor," +
                                                    "remit_full_pmt," +
                                                    "address12," +
                                                    "address22," +
                                                    "city2," +
                                                    "state_code2," +
                                                    "zip2 " +
                                                    "FROM client_acct_info" + flag + ",client_master" + flag + " " +
                                                    "WHERE client_master.client_acct = client_acct_info.client_acct" +
                                                    " AND client_acct_info.client_acct = '" + clientAcct + "'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoiceHeaderViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoiceHeaderViewModel
                    {

                        client_acct = Convert.ToString(rowAdo.Rows[i]["client_acct"]),
                        address12 = Convert.ToString(rowAdo.Rows[i]["address12"]),
                        address22 = Convert.ToString(rowAdo.Rows[i]["address22"]),
                        city2 = Convert.ToString(rowAdo.Rows[i]["city2"]),
                        orig_creditor = Convert.ToString(rowAdo.Rows[i]["orig_creditor"]),
                        remit_full_pmt = Convert.ToString(rowAdo.Rows[i]["remit_full_pmt"]),
                        state_code2 = Convert.ToString(rowAdo.Rows[i]["state_code2"]),
                        zip2 = Convert.ToString(rowAdo.Rows[i]["zip2"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            return _response.Response("");
        }

        public async Task<ResponseModel> GetClientPrimaryContact(string clientAcct, string company, string environment)
        {
            var flag = company switch
            {
                "A" => "",
                "T" => "_t",
                "D" => "_d",
                "H" => "_h",
                "L" => "_l",
                "W" => "_w",
                _ => ""
            };

            if (environment == "P")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    "first_name," +
                                                    "last_name " +
                                                    "FROM contact_master" + flag + " " +
                                                    "WHERE client_acct = '" + clientAcct + "' " +
                                                    "AND primary_contact = 'Y'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientPrimaryContactViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientPrimaryContactViewModel
                    {

                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),


                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }

            if (environment == "PO")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    "first_name," +
                                                    "last_name " +
                                                    "FROM contact_master" + flag + " " +
                                                    "WHERE client_acct = '" + clientAcct + "' " +
                                                    "AND primary_contact = 'Y'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientPrimaryContactViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientPrimaryContactViewModel
                    {

                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),


                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            if (environment == "T")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    "first_name," +
                                                    "last_name " +
                                                    "FROM contact_master" + flag + " " +
                                                    "WHERE client_acct = '" + clientAcct + "' " +
                                                    "AND primary_contact = 'Y'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientPrimaryContactViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientPrimaryContactViewModel
                    {

                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),


                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            return _response.Response("");
        }

        public async Task<ResponseModel> GetClientInvoicePayments(GetClientInvoiceRequestModel request, string environment)
        {
            var flag = request.Company switch
            {
                "A" => "",
                "T" => "_t",
                "D" => "_d",
                "H" => "_h",
                "L" => "_l",
                "W" => "_w",
                _ => ""
            };

            if (environment == "P")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    " debtor_acct_info.debtor_acct," +
                                                    " supplied_acct," +
                                                    " date_of_service," +
                                                    " date_placed," +
                                                    " first_name," +
                                                    " last_name," +
                                                    " client_amt," +
                                                    " agency_amt_decl," +
                                                    " fee_pct," +
                                                    " tran_date," +
                                                    " debtor_payment_master.balance," +
                                                    " CASE WHEN debtor_payment_master.status_code in ('PIF', 'SIF') " +
                                                    " THEN debtor_payment_master.status_code " +
                                                    " ELSE 'BAL' " +
                                                    " END as status_code," +
                                                    " payment_type," +
                                                    " agency_amt_decl + client_amt as total_payments_amt," +
                                                    " CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN ROUND(client_amt * (fee_pct / 100), 2)" +
                                                    " ELSE 0" +
                                                    " END as amount_due_agency," +
                                                    " CASE" +
                                                    " WHEN payment_type <> 'DIRECT'" +
                                                    " THEN CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN client_amt" +
                                                    " ELSE client_amt" +
                                                    " END" +
                                                    " ELSE 0" +
                                                    " END as amount_due_client," +
                                                    " debtor_acct_info.cosigner_last_name" +
                                                    " FROM debtor_acct_info," +
                                                    " debtor_master," +
                                                    " debtor_payment_master" +
                                                    " WHERE debtor_payment_master.debtor_acct = debtor_master.debtor_acct" +
                                                    " AND debtor_payment_master.debtor_acct = debtor_acct_info.debtor_acct" +
                                                    " AND payment_date >= '" + request.StartDate + "'" +
                                                    " AND payment_date <= '" + request.EndDate + "'" +
                                                    " AND show_on_invoice = 'Y'" +
                                                    " AND debtor_payment_master.debtor_acct >= '" + request.ClientAccount + "-000001'" +
                                                    " AND debtor_payment_master.debtor_acct <= '" + request.ClientAccount + "-999999'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoicePaymentsViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoicePaymentsViewModel
                    {
                        debtor_acct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                        supplied_acct = Convert.ToString(rowAdo.Rows[i]["supplied_acct"]),
                        date_of_service = Convert.ToString(rowAdo.Rows[i]["date_of_service"]),
                        date_placed = Convert.ToString(rowAdo.Rows[i]["date_placed"]),
                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                        client_amt = Convert.ToString(rowAdo.Rows[i]["client_amt"]),
                        agency_amt_decl = Convert.ToString(rowAdo.Rows[i]["agency_amt_decl"]),
                        fee_pct = Convert.ToString(rowAdo.Rows[i]["fee_pct"]),
                        tran_date = Convert.ToString(rowAdo.Rows[i]["tran_date"]),
                        balance = Convert.ToString(rowAdo.Rows[i]["balance"]),
                        status_code = Convert.ToString(rowAdo.Rows[i]["status_code"]),
                        payment_type = Convert.ToString(rowAdo.Rows[i]["payment_type"]),
                        total_payments_amt = Convert.ToString(rowAdo.Rows[i]["total_payments_amt"]),
                        amount_due_agency = Convert.ToString(rowAdo.Rows[i]["amount_due_agency"]),
                        amount_due_client = Convert.ToString(rowAdo.Rows[i]["amount_due_client"]),
                        cosigner_last_name = Convert.ToString(rowAdo.Rows[i]["cosigner_last_name"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }

            else if (environment == "PO")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    " debtor_acct_info.debtor_acct," +
                                                    " supplied_acct," +
                                                    " date_of_service," +
                                                    " date_placed," +
                                                    " first_name," +
                                                    " last_name," +
                                                    " client_amt," +
                                                    " agency_amt_decl," +
                                                    " fee_pct," +
                                                    " tran_date," +
                                                    " debtor_payment_master.balance," +
                                                    " CASE WHEN debtor_payment_master.status_code in ('PIF', 'SIF') " +
                                                    " THEN debtor_payment_master.status_code " +
                                                    " ELSE 'BAL' " +
                                                    " END as status_code," +
                                                    " payment_type," +
                                                    " agency_amt_decl + client_amt as total_payments_amt," +
                                                    " CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN ROUND(client_amt * (fee_pct / 100), 2)" +
                                                    " ELSE 0" +
                                                    " END as amount_due_agency," +
                                                    " CASE" +
                                                    " WHEN payment_type <> 'DIRECT'" +
                                                    " THEN CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN client_amt" +
                                                    " ELSE client_amt" +
                                                    " END" +
                                                    " ELSE 0" +
                                                    " END as amount_due_client," +
                                                    " debtor_acct_info.cosigner_last_name" +
                                                    " FROM debtor_acct_info," +
                                                    " debtor_master," +
                                                    " debtor_payment_master" +
                                                    " WHERE debtor_payment_master.debtor_acct = debtor_master.debtor_acct" +
                                                    " AND debtor_payment_master.debtor_acct = debtor_acct_info.debtor_acct" +
                                                    " AND payment_date >= '" + request.StartDate + "'" +
                                                    " AND payment_date <= '" + request.EndDate + "'" +
                                                    " AND show_on_invoice = 'Y'" +
                                                    " AND debtor_payment_master.debtor_acct >= '" + request.ClientAccount + "-000001'" +
                                                    " AND debtor_payment_master.debtor_acct <= '" + request.ClientAccount + "-999999'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoicePaymentsViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoicePaymentsViewModel
                    {
                        debtor_acct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                        supplied_acct = Convert.ToString(rowAdo.Rows[i]["supplied_acct"]),
                        date_of_service = Convert.ToString(rowAdo.Rows[i]["date_of_service"]),
                        date_placed = Convert.ToString(rowAdo.Rows[i]["date_placed"]),
                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                        client_amt = Convert.ToString(rowAdo.Rows[i]["client_amt"]),
                        agency_amt_decl = Convert.ToString(rowAdo.Rows[i]["agency_amt_decl"]),
                        fee_pct = Convert.ToString(rowAdo.Rows[i]["fee_pct"]),
                        tran_date = Convert.ToString(rowAdo.Rows[i]["tran_date"]),
                        balance = Convert.ToString(rowAdo.Rows[i]["balance"]),
                        status_code = Convert.ToString(rowAdo.Rows[i]["status_code"]),
                        payment_type = Convert.ToString(rowAdo.Rows[i]["payment_type"]),
                        total_payments_amt = Convert.ToString(rowAdo.Rows[i]["total_payments_amt"]),
                        amount_due_agency = Convert.ToString(rowAdo.Rows[i]["amount_due_agency"]),
                        amount_due_client = Convert.ToString(rowAdo.Rows[i]["amount_due_client"]),
                        cosigner_last_name = Convert.ToString(rowAdo.Rows[i]["cosigner_last_name"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            else if (environment == "T")
            {
                var rowAdo = _adoConnection.GetData("SELECT " +
                                                    " debtor_acct_info.debtor_acct," +
                                                    " supplied_acct," +
                                                    " date_of_service," +
                                                    " date_placed," +
                                                    " first_name," +
                                                    " last_name," +
                                                    " client_amt," +
                                                    " agency_amt_decl," +
                                                    " fee_pct," +
                                                    " tran_date," +
                                                    " debtor_payment_master.balance," +
                                                    " CASE WHEN debtor_payment_master.status_code in ('PIF', 'SIF') " +
                                                    " THEN debtor_payment_master.status_code " +
                                                    " ELSE 'BAL' " +
                                                    " END as status_code," +
                                                    " payment_type," +
                                                    " agency_amt_decl + client_amt as total_payments_amt," +
                                                    " CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN ROUND(client_amt * (fee_pct / 100), 2)" +
                                                    " ELSE 0" +
                                                    " END as amount_due_agency," +
                                                    " CASE" +
                                                    " WHEN payment_type <> 'DIRECT'" +
                                                    " THEN CASE" +
                                                    " WHEN remit_full_pmt = 'Y'" +
                                                    " THEN client_amt" +
                                                    " ELSE client_amt" +
                                                    " END" +
                                                    " ELSE 0" +
                                                    " END as amount_due_client," +
                                                    " debtor_acct_info.cosigner_last_name" +
                                                    " FROM debtor_acct_info," +
                                                    " debtor_master," +
                                                    " debtor_payment_master" +
                                                    " WHERE debtor_payment_master.debtor_acct = debtor_master.debtor_acct" +
                                                    " AND debtor_payment_master.debtor_acct = debtor_acct_info.debtor_acct" +
                                                    " AND payment_date >= '" + request.StartDate + "'" +
                                                    " AND payment_date <= '" + request.EndDate + "'" +
                                                    " AND show_on_invoice = 'Y'" +
                                                    " AND debtor_payment_master.debtor_acct >= '" + request.ClientAccount + "-000001'" +
                                                    " AND debtor_payment_master.debtor_acct <= '" + request.ClientAccount + "-999999'", environment);
                await Task.CompletedTask;
                var listOfItems = new List<GetClientInvoicePaymentsViewModel>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new GetClientInvoicePaymentsViewModel
                    {
                        debtor_acct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                        supplied_acct = Convert.ToString(rowAdo.Rows[i]["supplied_acct"]),
                        date_of_service = Convert.ToString(rowAdo.Rows[i]["date_of_service"]),
                        date_placed = Convert.ToString(rowAdo.Rows[i]["date_placed"]),
                        first_name = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                        last_name = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                        client_amt = Convert.ToString(rowAdo.Rows[i]["client_amt"]),
                        agency_amt_decl = Convert.ToString(rowAdo.Rows[i]["agency_amt_decl"]),
                        fee_pct = Convert.ToString(rowAdo.Rows[i]["fee_pct"]),
                        tran_date = Convert.ToString(rowAdo.Rows[i]["tran_date"]),
                        balance = Convert.ToString(rowAdo.Rows[i]["balance"]),
                        status_code = Convert.ToString(rowAdo.Rows[i]["status_code"]),
                        payment_type = Convert.ToString(rowAdo.Rows[i]["payment_type"]),
                        total_payments_amt = Convert.ToString(rowAdo.Rows[i]["total_payments_amt"]),
                        amount_due_agency = Convert.ToString(rowAdo.Rows[i]["amount_due_agency"]),
                        amount_due_client = Convert.ToString(rowAdo.Rows[i]["amount_due_client"]),
                        cosigner_last_name = Convert.ToString(rowAdo.Rows[i]["cosigner_last_name"])

                    };
                    listOfItems.Add(itemData);
                }

                return _response.Response(listOfItems);
            }
            return _response.Response("");
        }

        private async Task<List<GetInteractionAcctDataViewModel>> GetInteractionsAcctDataHelper(string debtorAcct, string ssn, string phoneNo,
            string phoneAreaCode, string environment)
        {

            DateTime? latestDate;
            switch (environment)
            {
                case "p":
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




            var rowAdo = _adoConnection.GetData("DECLARE @acctNoVar NVARCHAR(15)='" + debtorAcct + "'" +
                                                " IF(@acctNoVar='')" +
                                                "SET @acctNoVar=NULL" +
                                                " DECLARE @ssnVar NVARCHAR(15)='" + ssn + "'" +
                                                " IF(@ssnVar='') " +
                                                "SET @ssnVar=NULL " +
                                                "DECLARE @areaCode NVARCHAR(15)='" + phoneAreaCode + "'" +
                                                "IF(@areaCode='') " +
                                                "SET @areaCode=NULL " +
                                                "DECLARE @phoneNo NVARCHAR(15)='" + phoneNo + "' " +
                                                "IF(@phoneNo='') SET @phoneNo=NULL" +
                                                " SELECT distinct dai.debtor_acct," +
                                                "dai.balance," +
                                                "dai.email_address," +
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
                                                "FROM debtor_acct_info" + flag + " dai" +
                                                " LEFT JOIN debtor_master" + flag + " dm on dm.debtor_acct=dai.debtor_acct " +
                                                "LEFT JOIN debtor_phone_info dpi on dpi.debtor_acct=dai.debtor_acct " +
                                                "LEFT OUTER JOIN client_master" + flag + " cm on cm.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "LEFT OUTER JOIN client_acct_info" + flag + " cai on cai.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "WHERE (@acctNoVar IS NULL OR  dai.debtor_acct=@acctNoVar)" +
                                                " AND(@ssnVar IS NULL OR dm.ssn=@ssnVar) " +
                                                "AND (@areaCode IS NULL OR dpi.home_area_code=@areaCode) " +
                                                "AND (@phoneNo IS NULL OR dpi.home_phone=@phoneNo)", environment);


            var listOfItems = new List<GetInteractionAcctDataViewModel>();
            for (var i = 0; i < rowAdo.Rows.Count; i++)
            {
                var birthDateProcess = Convert.ToDateTime(rowAdo.Rows[i]["birth_date"]).ToString("d/M/yyyy");
                var balanceProcess = Math.Round(Convert.ToDouble(rowAdo.Rows[i]["balance"]), 2);
                var itemData = new GetInteractionAcctDataViewModel
                {
                    debtorAcct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                    date = latestDate,
                    ssn = Convert.ToString(rowAdo.Rows[i]["ssn"]),
                    balance = balanceProcess,
                    address1 = Convert.ToString(rowAdo.Rows[i]["address1"]),
                    address2 = Convert.ToString(rowAdo.Rows[i]["address2"]),
                    birthDate = birthDateProcess,
                    cellPhoneNumber = Convert.ToString(rowAdo.Rows[i]["cell_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["cell_phone"]),
                    city = Convert.ToString(rowAdo.Rows[i]["city"]),
                    clientName = Convert.ToString(rowAdo.Rows[i]["client_name"]),
                    debtType = Convert.ToString(rowAdo.Rows[i]["acct_type"]),
                    emailAddress = Convert.ToString(rowAdo.Rows[i]["email_address"]),
                    firstName = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                    homePhoneNumber = Convert.ToString(rowAdo.Rows[i]["home_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["home_phone"]),
                    lastName = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                    otherPhoneNumer = Convert.ToString(rowAdo.Rows[i]["other_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["other_phone"]),
                    relatiovePhoneNumber = Convert.ToString(rowAdo.Rows[i]["relative_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["relative_phone"]),
                    stateCode = Convert.ToString(rowAdo.Rows[i]["state_code"]),
                    workPhoneNumber = Convert.ToString(rowAdo.Rows[i]["work_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["work_phone"]),
                    zip = Convert.ToString(rowAdo.Rows[i]["zip"])
                };
                listOfItems.Add(itemData);
            }

            return listOfItems;
        }

        private async Task<List<GetInteractionAcctDataViewModel>> GetInteractionsAcctDataHelperWithoutDebtorAcct(string debtorAcct, string ssn, string phoneNo,
           string phoneAreaCode, string environment)
        {






            var flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAcct, environment);




            var rowAdo = _adoConnection.GetData("DECLARE @acctNoVar NVARCHAR(15)=''" +
                                                " IF(@acctNoVar='')" +
                                                "SET @acctNoVar=NULL" +
                                                " DECLARE @ssnVar NVARCHAR(15)='" + ssn + "'" +
                                                " IF(@ssnVar='') " +
                                                "SET @ssnVar=NULL " +
                                                "DECLARE @areaCode NVARCHAR(15)='" + phoneAreaCode + "'" +
                                                "IF(@areaCode='') " +
                                                "SET @areaCode=NULL " +
                                                "DECLARE @phoneNo NVARCHAR(15)='" + phoneNo + "' " +
                                                "IF(@phoneNo='') SET @phoneNo=NULL" +
                                                " SELECT distinct dai.debtor_acct," +
                                                "dai.balance," +
                                                "dai.email_address," +
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
                                                "FROM debtor_acct_info" + flag + " dai" +
                                                " LEFT JOIN debtor_master" + flag + " dm on dm.debtor_acct=dai.debtor_acct " +
                                                "LEFT JOIN debtor_phone_info dpi on dpi.debtor_acct=dai.debtor_acct " +
                                                "LEFT OUTER JOIN client_master" + flag + " cm on cm.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "LEFT OUTER JOIN client_acct_info" + flag + " cai on cai.client_acct=SUBSTRING(dai.debtor_acct, 1, 4) " +
                                                "WHERE (@acctNoVar IS NULL OR  dai.debtor_acct=@acctNoVar)" +
                                                " AND(@ssnVar IS NULL OR dm.ssn=@ssnVar) " +
                                                "AND (@areaCode IS NULL OR dpi.home_area_code=@areaCode) " +
                                                "AND (@phoneNo IS NULL OR dpi.home_phone=@phoneNo)", environment);


            var listOfItems = new List<GetInteractionAcctDataViewModel>();
            for (var i = 0; i < rowAdo.Rows.Count; i++)
            {
                var birthDateProcess = Convert.ToDateTime(rowAdo.Rows[i]["birth_date"]).ToString("d/M/yyyy");
                var balanceProcess = Math.Round(Convert.ToDouble(rowAdo.Rows[i]["balance"]), 2);
                var itemData = new GetInteractionAcctDataViewModel
                {
                    debtorAcct = Convert.ToString(rowAdo.Rows[i]["debtor_acct"]),
                    date = null,
                    ssn = Convert.ToString(rowAdo.Rows[i]["ssn"]),
                    balance = balanceProcess,
                    address1 = Convert.ToString(rowAdo.Rows[i]["address1"]),
                    address2 = Convert.ToString(rowAdo.Rows[i]["address2"]),
                    birthDate = birthDateProcess,
                    cellPhoneNumber = Convert.ToString(rowAdo.Rows[i]["cell_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["cell_phone"]),
                    city = Convert.ToString(rowAdo.Rows[i]["city"]),
                    clientName = Convert.ToString(rowAdo.Rows[i]["client_name"]),
                    debtType = Convert.ToString(rowAdo.Rows[i]["acct_type"]),
                    emailAddress = Convert.ToString(rowAdo.Rows[i]["email_address"]),
                    firstName = Convert.ToString(rowAdo.Rows[i]["first_name"]),
                    homePhoneNumber = Convert.ToString(rowAdo.Rows[i]["home_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["home_phone"]),
                    lastName = Convert.ToString(rowAdo.Rows[i]["last_name"]),
                    otherPhoneNumer = Convert.ToString(rowAdo.Rows[i]["other_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["other_phone"]),
                    relatiovePhoneNumber = Convert.ToString(rowAdo.Rows[i]["relative_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["relative_phone"]),
                    stateCode = Convert.ToString(rowAdo.Rows[i]["state_code"]),
                    workPhoneNumber = Convert.ToString(rowAdo.Rows[i]["work_area_code"]) + "-" + Convert.ToString(rowAdo.Rows[i]["work_phone"]),
                    zip = Convert.ToString(rowAdo.Rows[i]["zip"])
                };
                listOfItems.Add(itemData);
            }

            return listOfItems;
        }

        public async Task<ResponseModel> GetNextPaymentInfo(string debtorAcct, string environment)
        {
            //this function automatically works for all environments cause every things is submitted  from API controller  
            var flag = await _companyFlag.GetStringFlagForAdoQuery(debtorAcct, environment);

            var rowAdo = _adoConnection.GetData("SELECT DM.first_name+' '+DM.last_name as name_first_last," +
                                                "DAI.balance as balance," +
                                                "DAI.payment_amt_life as amount_paid_life," +
                                                "DAI.date_of_service as date_of_service," +
                                                "DM.ssn as ssn9," +
                                                "substring(DM.address1,1,patindex('% %',DM.address1)-1) as street_number," +
                                                "substring(DM.address1,patindex('% %',DM.address1)+1,99)+' '+DM.address2 as street_name," +
                                                "DM.city as city," +
                                                "DM.state_code as state," +
                                                "DM.zip as zip_code," +
                                                "CM.client_name as client_name," +
                                                "CM.client_desc as client_description," +
                                                "CAI.charge_interest as client_interst_bearingB," +
                                                "CAI.report_to_bureau as client_credit_reportableB," +
                                                "DPI.home_area_code+DPI.home_phone as home_phone_number," +
                                                "DPI.home_phone_ver as home_phone_verifiedB," +
                                                "DPI.home_phone_dont_call as home_phone_ponB," +
                                                "DPI.cell_area_code+DPI.cell_phone as cell_phone_number," +
                                                "DPI.cell_phone_ver as cell_phone_verifiedB," +
                                                "DPI.cell_phone_dont_call as cell_phone_ponB," +
                                                "DAI.last_payment_amt as last_payment_amount," +
                                                "DAI.begin_age_date as last_payment_date," +
                                                "DM.birth_date as birth_date ," +
                                                "isnull(DPPI.pp_amount1,0) as promise_amount," +
                                                "isnull(DPPI.pp_date1,'2000-01-01') as promise_date," +
                                                "isnull((select check_amt from check_detail where debtor_acct='" + debtorAcct + "'),0) as check_amt," +
                                                "isnull((select check_date from check_detail where debtor_acct='" + debtorAcct + "'),'2000-01-01') as check_date," +
                                                "isnull((select top 1 total from larry_cc_payments where debtor_acct='" + debtorAcct + "' and processed='N' order by date_process asc),0) as cc_amt," +
                                                "isnull((select top 1 date_process from larry_cc_payments where debtor_acct='" + debtorAcct + "' and processed='N' order by date_process asc),'2000-01-01') as cc_date " +
                                                "FROM debtor_master" + flag + " DM," +
                                                "debtor_acct_info" + flag + " DAI," +
                                                "client_master" + flag + " CM," +
                                                "client_acct_info" + flag + " CAI," +
                                                "debtor_phone_info DPI," +
                                                "debtor_pp_info DPPI" +
                                                " WHERE " +
                                                "DM.debtor_acct=DAI.debtor_acct " +
                                                "AND LEFT(DAI.debtor_acct,4)=CM.client_acct " +
                                                "AND CM.client_acct =CAI.client_acct " +
                                                "AND DAI.debtor_acct=DPI.debtor_acct " +
                                                "AND DAI.debtor_acct=DPPI.debtor_acct " +
                                                "AND DAI.debtor_acct='" + debtorAcct + "'", environment);





            var listOfItems = new List<GetNextPaymentInfoViewModel>();
            for (var i = 0; i < rowAdo.Rows.Count; i++)
            {
                var itemData = new GetNextPaymentInfoViewModel
                {
                    name_first_last = Convert.ToString(rowAdo.Rows[i]["name_first_last"]),
                    balance = rowAdo.Rows[i]["balance"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["balance"]),
                    amount_paid_life = rowAdo.Rows[i]["amount_paid_life"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["amount_paid_life"]),
                    date_of_service= Convert.ToDateTime(rowAdo.Rows[i]["date_of_service"]),
                    ssn9 = rowAdo.Rows[i]["ssn9"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["ssn9"]),
                    street_number = Convert.ToString(rowAdo.Rows[i]["street_number"]),
                    street_name = Convert.ToString(rowAdo.Rows[i]["street_name"]),
                    city = Convert.ToString(rowAdo.Rows[i]["city"]),
                    state = Convert.ToString(rowAdo.Rows[i]["state"]),
                    zip_code = Convert.ToString(rowAdo.Rows[i]["zip_code"]),
                    client_name = Convert.ToString(rowAdo.Rows[i]["client_name"]),
                    client_description = Convert.ToString(rowAdo.Rows[i]["client_description"]),
                    client_interst_bearingB = Convert.ToString(rowAdo.Rows[i]["client_interst_bearingB"]),
                    client_credit_reportableB = Convert.ToString(rowAdo.Rows[i]["client_credit_reportableB"]),
                    home_phone_number = Convert.ToString(rowAdo.Rows[i]["home_phone_number"]),
                    home_phone_verifiedB = Convert.ToString(rowAdo.Rows[i]["home_phone_verifiedB"]),
                    home_phone_ponB = Convert.ToString(rowAdo.Rows[i]["home_phone_ponB"]),
                    cell_phone_number = Convert.ToString(rowAdo.Rows[i]["cell_phone_number"]),
                    cell_phone_verifiedB = Convert.ToString(rowAdo.Rows[i]["cell_phone_verifiedB"]),
                    cell_phone_ponB = Convert.ToString(rowAdo.Rows[i]["cell_phone_ponB"]),
                    last_payment_amount = rowAdo.Rows[i]["last_payment_amount"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["last_payment_amount"]),
                    last_payment_date = Convert.ToDateTime(rowAdo.Rows[i]["last_payment_date"]),
                    birth_date = Convert.ToDateTime(rowAdo.Rows[i]["birth_date"]),
                    promise_amount = rowAdo.Rows[i]["promise_amount"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["promise_amount"]),
                    promise_date = Convert.ToDateTime(rowAdo.Rows[i]["promise_date"]),
                    check_amt = rowAdo.Rows[i]["check_amt"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["check_amt"]),
                    check_date = Convert.ToDateTime(rowAdo.Rows[i]["check_date"]),
                    cc_amt = rowAdo.Rows[i]["cc_amt"] is DBNull ? 0 : Convert.ToDecimal(rowAdo.Rows[i]["cc_amt"]),
                    cc_date = Convert.ToDateTime(rowAdo.Rows[i]["cc_date"])
                };
                listOfItems.Add(itemData);
            }

            return _response.Response(listOfItems);

        }
    }
}
