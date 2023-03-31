using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class ProcessCCRequest : IExamplesProvider<ProcessCcPaymentUniversalRequestModel>
    {
        public ProcessCcPaymentUniversalRequestModel GetExamples()
        {
            return new ProcessCcPaymentUniversalRequestModel()
            {
                debtorAcc = "4950-000001",
                ccNumber = "378282246310005",
                expiredDate = "12/24",
                cvv = "1234",
                numberOfPayments = 1,
                amount = 9,
                sif = "Y"
            };
        }
    }
}
