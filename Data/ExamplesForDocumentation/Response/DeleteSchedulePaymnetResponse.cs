using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class DeleteSchedulePaymnetResponse : IExamplesProvider<DeleteSchedulePaymnetResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public DeleteSchedulePaymnetResponse GetExamples()
        {
            return new DeleteSchedulePaymnetResponse()
            {
                Status = true,
                Data = "Inactiveted"

            };
        }
    }

}
