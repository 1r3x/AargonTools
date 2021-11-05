using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetInteractionResultsResponse : IExamplesProvider<SetInteractionResultsResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }

        SetInteractionResultsResponse IExamplesProvider<SetInteractionResultsResponse>.GetExamples()
        {
            return new SetInteractionResultsResponse()
            {
                Status = true,
                Data = "Successfully added interact results.",
                TransactionStatus=true
            };
        }
    }
}
