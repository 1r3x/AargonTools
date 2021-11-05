using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetRecentApprovalResponseModel: IMultipleExamplesProvider<GetRecentApprovalResponseModel>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        IEnumerable<SwaggerExample<GetRecentApprovalResponseModel>> IMultipleExamplesProvider<GetRecentApprovalResponseModel>.GetExamples()
        {
            return new List<SwaggerExample<GetRecentApprovalResponseModel>>(
                new[]
                {
                    new SwaggerExample<GetRecentApprovalResponseModel>()
                    {
                        Name = "Status 1",
                        Value = new GetRecentApprovalResponseModel()
                        {
                            Status = true,
                            Data = "true"
                        },
                        Summary = "When the account is have recent approval."
                    },
                    new SwaggerExample<GetRecentApprovalResponseModel>()
                    {
                        Name = "Status 2",
                        Value = new GetRecentApprovalResponseModel()
                        {
                            Status = true,
                            Data = "false"
                        },
                        Summary = "When the account doesn't have recent approval."
                    }
                }
            );
        }
    }
}
