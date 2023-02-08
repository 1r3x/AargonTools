using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IUniversalCcProcessHelper
    {
        Task<String> ReadHtmlPageAsync(string url, string post);
        Task SaveCardInfoAndScheduleData(ProcessCcPaymentUniversalRequestModel request, string environment);

        Task<ResponseModel> ProcessSaleTransForInstaMed(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForInstaMed(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task SaveCardInfoAndScheduleDataForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessSaleTransForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessSaleTransForElavon(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForElavon(ProcessCcPaymentUniversalRequestModel request,
            string environment);
    }
}
