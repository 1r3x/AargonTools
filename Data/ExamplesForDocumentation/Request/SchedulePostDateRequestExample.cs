using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SchedulePostDateRequestExample : IExamplesProvider<SchedulePostDateRequest>
    {
        public SchedulePostDateRequest GetExamples()
        {
            return new SchedulePostDateRequest()
            {
                debtorAcct = "",
                postDate = DateTime.Now.Date,
                amount = 10,
                numberOfPayments = 1,
                cardNumber = "378282246310005",
                expMonth = "1",
                expYear = "2023"
            };
        }
    }
}
