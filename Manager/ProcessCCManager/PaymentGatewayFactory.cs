using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AargonTools.Manager.ProcessCCManager
{
    public class PaymentGatewayFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly GatewaySelectionHelper _gatewaySelectionHelper;

        public PaymentGatewayFactory(IServiceProvider serviceProvider, GatewaySelectionHelper gatewaySelectionHelper)
        {
            _serviceProvider = serviceProvider;
            _gatewaySelectionHelper = gatewaySelectionHelper;
        }

        public IPaymentGateway GetPaymentGateway(string accountNumber, string environmnet)
        {
            var scheduleDateTime = DateTime.Now;//todo 
            var acctLimitTemp = accountNumber.Split('-');
            var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);

            if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999 || acctLimitCheck >= 4984000001 && acctLimitCheck < 4984999999
                || acctLimitCheck >= 4953000001 && acctLimitCheck < 4953999999 || acctLimitCheck >= 4985000001 && acctLimitCheck < 4985999999)
            {
                Serilog.Log.Information("This accout belongs to InstaMed");
                return _serviceProvider.GetService<InstaMedManager>();
            }
            else if (acctLimitCheck >= 4514000001 && acctLimitCheck < 4514999999)
            {
                Serilog.Log.Information("This accout belongs to IProClass");
                return _serviceProvider.GetService<IProClassManager>();
            }
            else
            {
                var gatewaySelect = _gatewaySelectionHelper.UniversalCcProcessGatewaySelectionHelper(accountNumber, environmnet);
                if (gatewaySelect.Result == "ELAVON" || acctLimitCheck >= 1902000001 && acctLimitCheck < 1902999999)//for staging
                {
                    Serilog.Log.Information("This accout belongs to Elavon");
                    return _serviceProvider.GetService<ElavonManager>();
                }
                else if (gatewaySelect.Result == "TMCBONHAMELAVON")
                {
                    Serilog.Log.Information("This accout belongs to TmcElavon");
                    return _serviceProvider.GetService<TmcElavonManager>();
                }
                else if (gatewaySelect.Result == "")
                {
                    Serilog.Log.Information("This accout belongs to USAePay");
                    return _serviceProvider.GetService<UsaEPayManager>();
                }
            }


            // Add more conditions as needed
            throw new Exception("Unsupported account number format");
        }

    }
}
