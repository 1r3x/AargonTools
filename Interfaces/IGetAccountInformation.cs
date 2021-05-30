using System.Collections.Generic;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface IGetAccountInformation
    {
        Task<decimal> GetAccountBalanceByDebtorAccount(string debtorAcct);
        string CheckAccountValidityByDebtorAccount(string debtorAcct);
        Task<List<string>> CheckAccountExistenceByDebtorAccount(string debtorAcct);
        Task<bool> GetRecentApprovalByDebtorAccount(string debtorAcct);

    }
}
