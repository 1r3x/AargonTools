using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface ISetCCPayment
    {
        Task<ResponseModel> SetCCPayment(CcPaymnetRequestModel request, string environment);
    }
}
