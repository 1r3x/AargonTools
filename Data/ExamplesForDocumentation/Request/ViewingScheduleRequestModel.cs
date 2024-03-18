using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class ViewingScheduleRequestModel : IExamplesProvider<ViewingSchedulePaymentsRequestModel>
    {
        public ViewingSchedulePaymentsRequestModel GetExamples()
        {
            return new ViewingSchedulePaymentsRequestModel()
            {
                date = new DateTime(2024, 1, 10).Date
            };
        }
    }
}
