using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AargonTools.Manager;

namespace AargonTools.Interfaces
{
    public interface IGetAccountInformation
    {
        //get the account balance for a specific debtor account
        Task<decimal> GetAccountBalanceByDebtorAccount(string debtorAcct);
        //get the account validity by @"\d{4}-\d{6}" pattern
        string CheckAccountValidityByDebtorAccount(string debtorAcct);
        //get the account information like in which office the account belongs and is it available or not.
        Task<List<string>> CheckAccountExistenceByDebtorAccount(string debtorAcct);
        //get the account for any recent payment recent is like within 5 min
        Task<bool> GetRecentApprovalByDebtorAccount(string debtorAcct);
        //get the info of the related account and there balance ..
        Task<ResponseModel> GetMultiples(string debtorAcct);

    }
}
