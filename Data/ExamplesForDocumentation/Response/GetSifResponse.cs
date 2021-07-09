using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models.DTOs.Responses;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetSifResponse : IExamplesProvider<GetSifResponse>
    {
        public bool Status { get; set; }
        public SIFViewModel Data { get; set; }
        public GetSifResponse GetExamples()
        {
            return new GetSifResponse()
            {
                Status = true,
                Data = new SIFViewModel()
                {
                    AccountBalance = 600,
                    SifDiscount = 120,
                    SifPayNow = 480,
                    SifPct = 20
                }

            };
        }
    }
}
