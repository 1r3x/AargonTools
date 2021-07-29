using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetMoveToDisputeResponses : IMultipleExamplesProvider<SetMoveToDisputeResponses>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetMoveToDisputeResponses>> GetExamples()
        {
            return new SwaggerExample<SetMoveToDisputeResponses>[]
            {
                new SwaggerExample<SetMoveToDisputeResponses>()
                {
                    Name = "Successful Example",
                    Value =new SetMoveToDisputeResponses()
                    {
                        Data = "Successfully Move 0000-000001  to dispute.",
                        Status = true
                    },
                    Summary = "Successful Response"
                },

                new SwaggerExample<SetMoveToDisputeResponses>()
                {
                    Name = "Error Example 1",
                    Value =new SetMoveToDisputeResponses()
                    {
                        Data = "Setup employee is out of the range from current move to dispute setup.",
                        Status = true
                    },
                    Summary = "Validation Error"
                }
            };
        }
    }
}
