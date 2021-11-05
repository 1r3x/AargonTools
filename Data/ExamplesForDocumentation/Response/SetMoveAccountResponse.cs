using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetMoveAccountResponse : IMultipleExamplesProvider<SetMoveAccountResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetMoveAccountResponse>> GetExamples()
        {
            return new SwaggerExample<SetMoveAccountResponse>[]
            {
                new SwaggerExample<SetMoveAccountResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetMoveAccountResponse()
                    {
                        Data = "Account moved successfully.",
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "Successful Move."
                },
                new SwaggerExample<SetMoveAccountResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetMoveAccountResponse()
                    {
                        Data = "Invalid Request[This account is inactive].",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Inactive Account"
                },
                new SwaggerExample<SetMoveAccountResponse>()
                {
                    Name = "Error Example 2",
                    Value =new SetMoveAccountResponse()
                    {
                        Data = "Invalid Request[Request queue not available].",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Queue Unavailable"
                },
                new SwaggerExample<SetMoveAccountResponse>()
                {
                Name = "Error Example 3",
                Value =new SetMoveAccountResponse()
                {
                    Data = "Invalid Request.[By any how data corrupted for 0001-000001 its not in the any queue master tables].",
                    Status = true,
                    TransactionStatus=false
                },
                Summary = "Data Corrupted"
                }
            };
        }
    }
}
