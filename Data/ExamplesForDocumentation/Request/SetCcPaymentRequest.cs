using System;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetCcPaymentRequest:IExamplesProvider<CcPaymnetRequestModel>
    {
        public CcPaymnetRequestModel GetExamples()
        {
            return new CcPaymnetRequestModel()
            {
                debtorAcc = "0001-000001",
                company = "AARGON AGENCY",
                userId = "12",
                chargeTotal = (decimal) 9.99,
                paymentDate =DateTime.Now,
                approvalStatus = "APPROVED",
                 approvalCode = "019373",
                 orderNumber = "0978",
                 refNo = "USAEPAY2",
                 sif = "Y"
            };
        }
    }
}
