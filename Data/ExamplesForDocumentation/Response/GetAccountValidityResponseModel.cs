using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetAccountValidityResponseModel : IMultipleExamplesProvider<GetAccountValidityResponseModel>
    {
        public bool Status { get; set; }
        public string Data { get; set; }


        IEnumerable<SwaggerExample<GetAccountValidityResponseModel>> IMultipleExamplesProvider<GetAccountValidityResponseModel>.GetExamples()
        {
            return new List<SwaggerExample<GetAccountValidityResponseModel>>(
                new[]
                {
                    new SwaggerExample<GetAccountValidityResponseModel>()
                    {
                        Name = "Status 1",
                        Value = new GetAccountValidityResponseModel()
                        {
                            Status = true,
                             Data = "Its a Valid Account."
                        },
                        Summary = "When the account is valid."
                    },
                    new SwaggerExample<GetAccountValidityResponseModel>()
                    {
                        Name = "Status 2",
                        Value = new GetAccountValidityResponseModel()
                        {
                            Status = true,
                            Data = "Its not a Valid Account."
                        },
                        Summary = "When the account is invalid."
                    }
                }
            );
        }


    }
}
