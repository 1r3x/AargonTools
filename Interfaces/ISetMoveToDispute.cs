using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveToDispute
    {
        Task<ResponseModel> SetMoveToDispute(string debtorAcct, string environment);// decimal amountDisputed, removed 26 oct 23
    }
}
