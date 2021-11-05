using System;
using AargonTools.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetInteractionResultsRequest : IExamplesProvider<InteractResult>
    {
        public InteractResult GetExamples()
        {
            return new InteractResult()
            {
                DebtorAcct = "0001-000001",
                Ani = "string",
                StartTime = DateTime.Now,
                EndTime = DateTime.Today,
                OpeningIntent = "string",
                LastDialogue = "string",
                TransferReason = "string",
                PaymentAmt = 0,
                CallResult = "string",
                TermReason = "string"
            };
        }
    }
}
