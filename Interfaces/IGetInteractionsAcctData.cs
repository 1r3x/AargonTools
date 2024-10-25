using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface IGetInteractionsAcctData
    {
        Task<ResponseModel> GetInteractionsAcctData(GetInteractionAcctDateRequestModel request, string environment);
        Task<ResponseModel> GetInteractionsAcctDataSpeedRun(GetInteractionAcctDateRequestModel request, string environment);//for speeding .
    }
}
