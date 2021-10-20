using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetCcPaymnetResponse : IMultipleExamplesProvider<SetCcPaymnetResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetCcPaymnetResponse>> GetExamples()
        {
            return new SwaggerExample<SetCcPaymnetResponse>[]
            {
                new SwaggerExample<SetCcPaymnetResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetCcPaymnetResponse()
                    {
                        Data = "Successfully set CC payment",
                        Status = true
                    },
                    Summary = "Successful Response"
                },

                new SwaggerExample<SetCcPaymnetResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetCcPaymnetResponse()
                    {
                        Data = "Payment date won't be in future.",
                        Status = true
                    },
                    Summary = "Validation Error 1"
                },
                new SwaggerExample<SetCcPaymnetResponse>()
                {
                Name = "Error Example 2",
                Value =new SetCcPaymnetResponse()
                {
                    Data = "Please correct company name.",
                    Status = true
                },
                Summary = "Validation Error 2"
                },
                new SwaggerExample<SetCcPaymnetResponse>()
                {
                    Name = "Error Example 3",
                    Value =new SetCcPaymnetResponse()
                    {
                        Data = "Please correct approval status.",
                        Status = true
                    },
                    Summary = "Validation Error 3"
                },
                new SwaggerExample<SetCcPaymnetResponse>()
                {
                    Name = "Error Example 4",
                    Value =new SetCcPaymnetResponse()
                    {
                        Data = "Please correct reference number.",
                        Status = true
                    },
                    Summary = "Validation Error 4"
                },
                new SwaggerExample<SetCcPaymnetResponse>()
                {
                    Name = "Error Example 5",
                    Value =new SetCcPaymnetResponse()
                    {
                        Data = "SIF must be just 'Y' or 'N' ",
                        Status = true
                    },
                    Summary = "Validation Error 5"
                }
            };
        }
    }
}
