using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetUpdateAddressResponseModel : IMultipleExamplesProvider<SetUpdateAddressResponseModel>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetUpdateAddressResponseModel>> GetExamples()
        {
            return new SwaggerExample<SetUpdateAddressResponseModel>[]
            {
                new SwaggerExample<SetUpdateAddressResponseModel>()
                {
                    Name = "Successful Example",
                    Value =new SetUpdateAddressResponseModel()
                    {
                        Data = "Successfully update address response",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Successful Response."
                },
                
                new SwaggerExample<SetUpdateAddressResponseModel>()
                {
                    Name = "Error Example 1",
                    Value =new SetUpdateAddressResponseModel()
                    {
                        Data = "This is not a valid residenceType.",
                        Status = true,
                        TransactionStatus=false
                    },
                    Summary = "Validation Error"
                }
            };
        }
    }
}
