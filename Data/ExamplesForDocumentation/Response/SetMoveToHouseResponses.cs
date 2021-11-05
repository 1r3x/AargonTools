using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetMoveToHouseResponses : IMultipleExamplesProvider<SetMoveToHouseResponses>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetMoveToHouseResponses>> GetExamples()
        {
            return new SwaggerExample<SetMoveToHouseResponses>[]
            {
                new SwaggerExample<SetMoveToHouseResponses>()
                {
                    Name = "Successful Example",
                    Value =new SetMoveToHouseResponses()
                    {
                        Data = "Successfully Move 0000-000001 to house.",
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "Successful Response"
                },
               
                new SwaggerExample<SetMoveToHouseResponses>()
                {
                    Name = "Error Example 1",
                    Value =new SetMoveToHouseResponses()
                    {
                        Data = "Setup employee is out of the range from current move to house setup.",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Validation Error"
                }
            };
        }
    }
}
