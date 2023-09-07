using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IUniversalCcProcessApiService
    {




        //postPayment Later introduced 
        Task PostPaymentA(string debtorAccount, decimal paymentAmount, float balance,
                decimal interestAmount, decimal feePct, string sif, int qFrom, int qTo,
                string remit, string paymentType, string company, float adminAmount, string MainDA, string environment);
        //



        Task<string> InstaMedSale(SaleRequestModelForInstamed request);
        Task<string> InstaMedSalePro(SaleRequestModelForInstamed request);
        Task<string> InstaMedAuth(SaleRequestModelForInstamed request);
        Task<string> InstaMedTokenization(SaleRequestModelForInstamed request);
        Task<string> InstaMedTokenizationPro(SaleRequestModelForInstamed request);
        Task<string> IProGatewaySale(SaleRequestModelForInstamed request);
        Task<string> IProGatewaySalePro(SaleRequestModelForInstamed request);
        Task<string> IProGatewayAuth(SaleRequestModelForInstamed request);
        Task<string> ElavonSalePro(SaleRequestModelForInstamed request);
        Task<string> ElavonSaleTmcPro(SaleRequestModelForInstamed request);
        Task<string> ElavonSale(SaleRequestModelForInstamed request);
        Task<string> ElavonAuth(SaleRequestModelForInstamed request);
        Task<string> ElavonTokenization(SaleRequestModelForInstamed request);
        //


        Task<String> ReadHtmlPageAsync(string url, string post);

        Task SaveCardInfoAndScheduleData(ProcessCcPaymentUniversalRequestModel request, string environment);

        Task<ResponseModel> ProcessSaleTransForInstaMed(ProcessCcPaymentUniversalRequestModel request,
            string environment);
        Task<ResponseModel> ProcessSaleTransForInstaMedQA(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessOnFileSaleTransForInstaMed(AutoProcessCcUniversalViewModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForInstaMed(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task SaveCardInfoAndScheduleDataForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);


        Task<ResponseModel> ProcessSaleTransForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessSaleTransForIProGatewayQA(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessOnfileSaleTransForIProGateway(AutoProcessCcUniversalViewModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForIProGateway(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessSaleTransForElavon(ProcessCcPaymentUniversalRequestModel request,
            string environment);
        Task<ResponseModel> ProcessSaleTransForTmcElavon(ProcessCcPaymentUniversalRequestModel request,
            string environment);
        
        Task<ResponseModel> ProcessSaleTransForElavonQA(ProcessCcPaymentUniversalRequestModel request,
            string environment);

        Task<ResponseModel> ProcessOnfileSaleTransForElavon(AutoProcessCcUniversalViewModel request,
            string environment);

        Task<ResponseModel> ProcessCardAuthorizationForElavon(ProcessCcPaymentUniversalRequestModel request,
            string environment);


    }
}
