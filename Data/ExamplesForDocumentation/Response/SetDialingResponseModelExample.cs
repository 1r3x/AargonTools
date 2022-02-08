using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetDialingResponseModelExample : IMultipleExamplesProvider<SetDialingResponseModelExample>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public bool TransactionStatus { get; set; }

        public IEnumerable<SwaggerExample<SetDialingResponseModelExample>> GetExamples()
        {
            return new SwaggerExample<SetDialingResponseModelExample>[]
            {
                new SwaggerExample<SetDialingResponseModelExample>()
                {
                    Name = "Successful Example",
                    Value =new SetDialingResponseModelExample()
                    {
                        Data = "Successfully Set Dialing.",
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "When successfully Set Dialing."
                },
                new SwaggerExample<SetDialingResponseModelExample>()
                {
                    Name = "Error Example",
                    Value =new SetDialingResponseModelExample()
                    {
                        Data = "Oops something wen wrong , and you will get a custom error message.",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "When Something went wrong."
                }
            };
        }
    }
}
