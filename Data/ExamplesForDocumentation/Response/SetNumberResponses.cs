using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetNumberResponses : IMultipleExamplesProvider<SetNumberResponses>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetNumberResponses>> GetExamples()
        {
            return new SwaggerExample<SetNumberResponses>[]
            {
                new SwaggerExample<SetNumberResponses>()
                {
                    Name = "Successful Example",
                    Value =new SetNumberResponses()
                    {
                        Data = "Successfully set a new number on new phone number directory " +
                               "for debtor account 0001-000001",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Successful Response(debtor cell phone Not available)."
                },
                new SwaggerExample<SetNumberResponses>()
                {
                    Name = "Successful Example 1",
                    Value =new SetNumberResponses()
                    {
                        Data = "Successfully set the number for debtor account 0001-000001",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Successful Response(debtor cell phone available and replaced)."
                },
                new SwaggerExample<SetNumberResponses>()
                {
                    Name = "Error Example 1",
                    Value =new SetNumberResponses()
                    {
                        Data = "This is not a valid US cell number.",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Validation Error"
                }
            };
        }
    }
}
