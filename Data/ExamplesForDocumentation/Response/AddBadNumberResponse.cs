using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class AddBadNumberResponse : IExamplesProvider<AddBadNumberResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public AddBadNumberResponse GetExamples()
        {
            return new AddBadNumberResponse()
            {
                Status = true,
                Data = "Successfully enlisted a bad number."
            };

        }
    }
}
