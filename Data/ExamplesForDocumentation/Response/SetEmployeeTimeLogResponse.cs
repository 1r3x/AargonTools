﻿using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetEmployeeTimeLogResponse : IMultipleExamplesProvider<SetEmployeeTimeLogResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetEmployeeTimeLogResponse>> GetExamples()
        {
            return new SwaggerExample<SetEmployeeTimeLogResponse>[]
            {
                new SwaggerExample<SetEmployeeTimeLogResponse>()
                {
                    Name = "Successful Example",
                    Value = new SetEmployeeTimeLogResponse()
                    {
                        Data = "Time Log Saved Successfully.",
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "Successful Entry."
                },
                //new SwaggerExample<SetEmployeeTimeLogResponse>()
                //{
                //    Name = "Error Example 1",
                //    Value = new SetEmployeeTimeLogResponse()
                //    {
                //        Data = "Invalid Request[This account is inactive].",
                //        Status = true
                //    },
                //    Summary = "Inactive Account"
                //},
            };
        }
    }
}
