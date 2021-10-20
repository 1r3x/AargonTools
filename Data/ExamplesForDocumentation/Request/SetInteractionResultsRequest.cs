using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;
using AargonTools.Models.DTOs.Requests;
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
