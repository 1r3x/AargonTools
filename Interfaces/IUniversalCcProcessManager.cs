using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IUniversalCcProcessManager
    {
        Task<ResponseModel> ForLcg(ProcessCcPaymentUniversalRequestModel request, string environment);
        Task<ResponseModel> ForLpbcg(ProcessCcPaymentUniversalRequestModel request, string environment);
        Task<ResponseModel> ForNtmc(ProcessCcPaymentUniversalRequestModel request, string environment);
        Task<ResponseModel> ForTcr(ProcessCcPaymentUniversalRequestModel request, string environment);
    }
}
