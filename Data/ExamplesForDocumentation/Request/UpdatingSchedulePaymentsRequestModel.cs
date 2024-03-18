using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class UpdatingSchedulePaymentsRequestModel : IExamplesProvider<UpdatingSchedulePayments>
    {
        public UpdatingSchedulePayments GetExamples()
        {
            return new UpdatingSchedulePayments()
            {
                scheduleId=47,
                Updateddate = new DateTime(2024, 1, 10).Date
            };
        }
    }
}
