using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetClientPrimaryContactExample : IExamplesProvider<GetClientPrimaryContactExample>
    {
        public bool Status { get; set; }
        public GetClientPrimaryContactData Data { get; set; }
        public GetClientPrimaryContactExample GetExamples()
        {
            return new GetClientPrimaryContactExample()
            {
                Status = true,
                Data = new GetClientPrimaryContactData()
                {
                    first_name= "BOBS THE BROTHER",
                    last_name= "SMITH"
                }
            };
        }
    }

    public class GetClientPrimaryContactData
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
