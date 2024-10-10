using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;
using System.Threading.Tasks;

namespace AargonTools.Interfaces.ProcessCC
{
    public interface IPaymentGateway
    {
        Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment);
        Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment);
    }
}
