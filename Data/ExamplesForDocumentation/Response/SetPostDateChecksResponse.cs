using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetPostDateChecksResponse : IMultipleExamplesProvider<SetPostDateChecksResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetPostDateChecksResponse>> GetExamples()
        {
            return new SwaggerExample<SetPostDateChecksResponse>[]
            {
                new SwaggerExample<SetPostDateChecksResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetPostDateChecksResponse()
                    {
                        Data = "Successfully Set Post Date Checks",
                        Status = true
                    },
                    Summary = "Successful Response"
                },

                new SwaggerExample<SetPostDateChecksResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetPostDateChecksResponse()
                    {
                        Data = "Oops Something went wrong.",
                        Status = true
                    },
                    Summary = "Error Response"
                }
            };
        }
    }
}
