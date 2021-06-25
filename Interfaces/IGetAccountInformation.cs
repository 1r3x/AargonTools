using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AargonTools.Manager;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface IGetAccountInformation
    {
        //get the account balance for a specific debtor account
        Task<ResponseModel> GetAccountBalanceByDebtorAccount(string debtorAcct,string environment);
        //get the account validity by @"\d{4}-\d{6}" pattern
        ResponseModel CheckAccountValidityByDebtorAccount(string debtorAcct, string environment);
        //get the account information like in which office the account belongs and is it available or not.
        Task<ResponseModel> CheckAccountExistenceByDebtorAccount(string debtorAcct, string environment);
        //get the account for any recent payment recent is like within 5 min
        Task<ResponseModel> GetRecentApprovalByDebtorAccount(string debtorAcct, string environment);
        //get the info of the related account and there balance ..
        Task<ResponseModel> GetMultiples(string debtorAcct, string environment);
        //get some account infos with some logic applied 
        Task<ResponseModel> GetAccountInfo(string debtorAcct, string environment);
        Task<ResponseModel> GetSIF(string debtorAcct, string environment);

    }
}
