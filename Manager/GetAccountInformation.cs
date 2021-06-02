using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class GetAccountInformation : IGetAccountInformation
    {
        private static ExistingDataDbContext _context;
        private static ResponseModel _response;
        private static DebtorAccountAreaManager _areaManager;

        public GetAccountInformation(ExistingDataDbContext context, ResponseModel response, DebtorAccountAreaManager areaManager)
        {
            _context = context;
            _response = response;
            _areaManager = areaManager;
        }
        async Task<ResponseModel> IGetAccountInformation.GetAccountBalanceByDebtorAccount(string debtorAcct)
        {
          
                var data = await _areaManager.GetTheProperTable(debtorAcct).Result.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
                return _response.Response(data);
        }

        ResponseModel IGetAccountInformation.CheckAccountValidityByDebtorAccount(string debtorAcct)
        {
            var rx = new Regex(@"\d{4}-\d{6}");
            return _response.Response(rx.Match(debtorAcct).Success ? "Its a Valid Account." : "Its not a Valid Account.");
        }

        async Task<ResponseModel> IGetAccountInformation.CheckAccountExistenceByDebtorAccount(string debtorAcct)
        {
            var account = debtorAcct.Substring(0, 4);
            var item = new List<string>(new string[] { "Not Found" });
            if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item.Remove("Not Found");
                item.Add("A");
                item.Add("True");
            }
            else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item.Remove("Not Found");
                item.Add("D");
                item.Add("True");
            }
            else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item.Remove("Not Found");
                item.Add("H");
                item.Add("True");
            }
            else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item.Remove("Not Found");
                item.Add("L");
                item.Add("True");
            }
            else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item.Remove("Not Found");
                item.Add("T");
                item.Add("True");
            }
            else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
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

        async Task<ResponseModel> IGetAccountInformation.GetRecentApprovalByDebtorAccount(string debtorAcct)
        {
            var approvalStatus = await _context.CcPayments.CountAsync(x =>
                x.DebtorAcct == debtorAcct && x.PaymentDate == DateTime.Now.AddMinutes(-5) && x.ApprovalStatus == "APPROVED");

            return _response.Response(approvalStatus > 0);
        }

        async Task<ResponseModel> IGetAccountInformation.GetMultiples(string debtorAcct)
        {
        
                var listOfData = await (_areaManager.GetTheProperTable(debtorAcct).Result
                    .Join(_context.DebtorMultiples, accountInfo => accountInfo.DebtorAcct,
                        debtorMultiples => debtorMultiples.DebtorAcct2,
                        (accountInfo, debtorMultiples) => new { accountInfo, debtorMultiples })
                    .Where(@t => @t.debtorMultiples.DebtorAcct == debtorAcct)
                    .Select(@t => new { @t.accountInfo.DebtorAcct, @t.accountInfo.Balance })).ToListAsync();
                return _response.Response(listOfData);
        }

        async Task<ResponseModel> IGetAccountInformation.GetAccountInfo(string debtorAcct)
        {
            var data =await _areaManager.GetTheProperTable(debtorAcct)
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
}
