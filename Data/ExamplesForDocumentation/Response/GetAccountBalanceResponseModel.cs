using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetAccountBalanceResponseModel : IExamplesProvider<GetAccountBalanceResponseModel>
    {
        public bool Status { get; set; }
        public decimal Data { get; set; }

        public GetAccountBalanceResponseModel GetExamples()
        {
            return new GetAccountBalanceResponseModel()
            {
                Status = true,
                Data = new decimal(600.0000)
            };
        }
    }
}
