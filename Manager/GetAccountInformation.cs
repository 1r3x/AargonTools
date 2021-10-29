using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
            return _response.Response(true,data);
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
            else if (environment=="PO")
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
            else if (environment=="PO")
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
            else if (environment=="PO")
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
            else if (environment=="PO")
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
    }
}
