using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class AddBadNumberResponse : IMultipleExamplesProvider<AddBadNumberResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }

        public IEnumerable<SwaggerExample<AddBadNumberResponse>> GetExamples()
        {
            return new SwaggerExample<AddBadNumberResponse>[]
            {
                new SwaggerExample<AddBadNumberResponse>()
                {
                    Name = "Successful Example",
                    Value =new AddBadNumberResponse()
                    {
                        Data = "Successfully enlisted a bad number.",
                        Status = true
                    },
                    Summary = "When successfully added a bad number."
                },
                new SwaggerExample<AddBadNumberResponse>()
                {
                    Name = "Error Example",
                    Value =new AddBadNumberResponse()
                    {
                        Data = "Oops something wen wrong , and you will get a custom error message.",
                        Status = true
                    },
                    Summary = "When Something went wrong."
                }
            };
        }
    }
}
