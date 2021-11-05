using System;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetPostDateChecksRequest : IExamplesProvider<SetPostDateChecksRequestModel>
    {
        public SetPostDateChecksRequestModel GetExamples()
        {
            return new SetPostDateChecksRequestModel()
            {
                accountNumber = "02927898171918",
                amount = 10,
                debtorAcct = "0001-000001",
                totalPd = 5,
                sif = Convert.ToChar("Y"),
                postDate = DateTime.Now.Date,
                routingNumber = "7"
            };
        }
    }
}
