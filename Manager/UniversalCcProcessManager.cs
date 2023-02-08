using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AargonTools.Manager
{
    public class UniversalCcProcessManager : IUniversalCcProcessManager
    {
        private static ResponseModel _response;
        private static GatewaySelectionHelper _gatewaySelectionHelper;
        private static UniversalCcProcessApiService _ccProcessApiService;


        public UniversalCcProcessManager(ResponseModel response, GatewaySelectionHelper gatewaySelectionHelper, UniversalCcProcessApiService ccProcessApiService)
        {
            _response = response;
            _gatewaySelectionHelper = gatewaySelectionHelper;
            _ccProcessApiService = ccProcessApiService;
        }

        //instaMed api gateway essentials  
        private readonly DateTime _scheduleDateTime = DateTime.Now;



        public async Task<ResponseModel> ForLcg(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ResponseModel response;
            if (_scheduleDateTime.Date == DateTime.Now.Date)
            {
                response = await _ccProcessApiService.ProcessSaleTransForInstaMed(request, environment);
            }
            else
            {
                response = await _ccProcessApiService.ProcessCardAuthorizationForInstaMed(request, environment);
            }

            return _response.Response(true, response.Data);
        }

        public async Task<ResponseModel> ForLpbcg(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ResponseModel response;
            if (_scheduleDateTime.Date == DateTime.Now.Date)
            {
                response = await _ccProcessApiService.ProcessSaleTransForInstaMed(request, environment);
            }
            else
            {
                response = await _ccProcessApiService.ProcessCardAuthorizationForInstaMed(request, environment);
            }

            return _response.Response(true, response.Data);
        }


        public async Task<ResponseModel> ForNtmc(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ResponseModel response;
            if (_scheduleDateTime.Date == DateTime.Now.Date)
            {
                response = await _ccProcessApiService.ProcessSaleTransForIProGateway(request, environment);
            }
            else
            {
                response = await _ccProcessApiService.ProcessCardAuthorizationForIProGateway(request, environment);
            }

            return _response.Response(true, response.Data);
        }


        public async Task<ResponseModel> ForTcr(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var gatewaySelect = _gatewaySelectionHelper.UniversalCcProcessGatewaySelectionHelper(request.debtorAcc, "T");
            if (gatewaySelect.Result == "ELAVON")
            {
                ResponseModel response;
                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    response = await _ccProcessApiService.ProcessSaleTransForElavon(request, environment);
                }
                else
                {
                    response = await _ccProcessApiService.ProcessCardAuthorizationForIProGateway(request, environment);
                }

                return _response.Response(true, response.Data);
            }

            //if (gatewaySelct=="")
            //{

            //}
            return _response.Response(true, "");
        }



    }

}
