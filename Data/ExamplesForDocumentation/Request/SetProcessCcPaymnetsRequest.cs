using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetProcessCcPaymentsRequest:IExamplesProvider<ProcessCcPaymentRequestModel>
    {
        public ProcessCcPaymentRequestModel GetExamples()
        {
            return new ProcessCcPaymentRequestModel()
            {
                debtorAcc = "0001-000001",
                ccNumber = "378282246310005",
                expiredDate = "12-23",
                cvv = "123",
                numberOfPayments = 1,
                amount = (decimal) 9.9
            };
        }
    }
}
