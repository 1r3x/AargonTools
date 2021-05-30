﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class GetAccountInformation : IGetAccountInformation
    {
        private static ExistingDataDbContext _context;

        public GetAccountInformation(ExistingDataDbContext context)
        {
            _context = context;
        }


        async Task<decimal> IGetAccountInformation.GetAccountBalanceByDebtorAccount(string debtorAcct)
        {
            var account = debtorAcct.Substring(0, 4);
            decimal item = 0;

            if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfos.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfoDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfoHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfoLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                item = await _context.DebtorAcctInfoWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.Balance).SingleOrDefaultAsync();
            }
            else
            {
                item = 0;
            }

            return item;
        }

         string IGetAccountInformation.CheckAccountValidityByDebtorAccount(string debtorAcct)
        {
            var rx = new Regex(@"\d{4}-\d{6}");
            if (rx.Match(debtorAcct).Success)
            {
                return  "Its a Valid Account.";
            }
            else
            {
                return "Its not a Valid Account.";
            }
        }

        async Task<List<string>> IGetAccountInformation.CheckAccountExistenceByDebtorAccount(string debtorAcct)
        {
            var account = debtorAcct.Substring(0, 4);
            var item = new List<string>(new string[] {"Not Found"});
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
                return item;
            }
            return item;
        }

        async Task<bool> IGetAccountInformation.GetRecentApprovalByDebtorAccount(string debtorAcct)
        {
            var approvalStatus = await _context.CcPayments.CountAsync(x =>
                x.DebtorAcct == debtorAcct && x.PaymentDate == DateTime.Now.AddMinutes(-5) && x.ApprovalStatus == "APPROVED");
            return approvalStatus>0;
        }

    }
}