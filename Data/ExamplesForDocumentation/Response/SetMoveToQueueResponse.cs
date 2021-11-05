using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetMoveToQueueResponse : IMultipleExamplesProvider<SetMoveToQueueResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetMoveToQueueResponse>> GetExamples()
        {
            return new SwaggerExample<SetMoveToQueueResponse>[]
            {
                new SwaggerExample<SetMoveToQueueResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetMoveToQueueResponse()
                    {
                        Data = "Successfully Move 0000-000001  to dispute.",
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "Successful Response"
                },

                new SwaggerExample<SetMoveToQueueResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetMoveToQueueResponse()
                    {
                        Data = "Setup employee is out of the range from current move to dispute setup.",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Validation Error"
                }
            };
        }
    }
}
