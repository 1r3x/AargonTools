using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using static AargonTools.Data.ExamplesForDocumentation.Response.ViewingSchedulePaymentsResponse;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class ViewingPaymentsHistoryResponse : IExamplesProvider<ViewingPaymentsHistoryResponse>
    {
        public bool Status { get; set; }
        public List<ViewingPaymentsHistoryObject> Data { get; set; }
        public ViewingPaymentsHistoryResponse GetExamples()
        {
            return new ViewingPaymentsHistoryResponse()
            {
                Status = true,
                Data = new List<ViewingPaymentsHistoryObject>()
                {
                    new ViewingPaymentsHistoryObject()
                    {
                      paymentScheduleId= "43",
                      responseCode= "null",
                      responseMessage= "Declined",
                      timeLog= "2023-12-27T20:13:59.36",
                      transactionId= "3628491740",
                      authorizationNumber= "D",
                      authorizationText= "_username"
                    },
                    new ViewingPaymentsHistoryObject()
                    {
                      paymentScheduleId= "44",
                      responseCode= "null",
                      responseMessage= "Acepted",
                      timeLog= "2023-12-27T20:13:59.36",
                      transactionId= "3628496740",
                      authorizationNumber= "12312",
                      authorizationText= "_username"
                    },
                }
            };
        }

        public class ViewingPaymentsHistoryObject
        {
            public string paymentScheduleId { get; set; }
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
            public string timeLog { get; set; }
            public string transactionId { get; set; }
            public string authorizationNumber { get; set; }
            public string authorizationText { get; set; }
        }
    }
}
