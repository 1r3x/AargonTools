using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class UpdatingSchedulePaymentsResponses : IExamplesProvider<UpdatingSchedulePaymentsResponses>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public UpdatingSchedulePaymentsResponses GetExamples()
        {
            return new UpdatingSchedulePaymentsResponses()
            {
                Status = true,
                Data = "Data changed to 12.12.23"

            };
        }
    }

}
