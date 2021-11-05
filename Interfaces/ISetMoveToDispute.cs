using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveToDispute
    {
        Task<ResponseModel> SetMoveToDispute(string debtorAcct, decimal amountDisputed, string environment);
    }
}
