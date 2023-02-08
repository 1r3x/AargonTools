using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IUniversalCcProcessApiService
    {
        Task<string> InstaMedSale(SaleRequestModelForInstamed request);
        Task<string> InstaMedAuth(SaleRequestModelForInstamed request);
        Task<string> InstaMedTokenization(SaleRequestModelForInstamed request);
        Task<string> IProGatewaySale(SaleRequestModelForInstamed request);
        Task<string> IProGatewayAuth(SaleRequestModelForInstamed request);
        Task<string> ElavonSale(SaleRequestModelForInstamed request);
        Task<string> ElavonAuth(SaleRequestModelForInstamed request);
        Task<string> ElavonTokenization(SaleRequestModelForInstamed request);
        //


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
