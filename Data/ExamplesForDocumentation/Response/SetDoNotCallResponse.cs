using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetDoNotCallResponse : IMultipleExamplesProvider<SetDoNotCallResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetDoNotCallResponse>> GetExamples()
        {
            return new SwaggerExample<SetDoNotCallResponse>[]
            {
                new SwaggerExample<SetDoNotCallResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetDoNotCallResponse()
                    {
                        Data = "Successfully set the number to don't call status.",
                        Status = true
                    },
                    Summary = "Successful Response."
                },
                new SwaggerExample<SetDoNotCallResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetDoNotCallResponse()
                    {
                        Data = "This account is not associate with this cell number",
                        Status = true
                    },
                    Summary = "Validation Error"
                },
                new SwaggerExample<SetDoNotCallResponse>()
                {
                    Name = "Error Example 2",
                    Value =new SetDoNotCallResponse()
                    {
                        Data = "This is not a valid cell number for US.Just put areaCode+centralOffice+lineNumber. ex. 7025052773",
                        Status = true
                    },
                    Summary = "Format Error"
                }
            };
        }
    }
}
