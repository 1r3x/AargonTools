﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SchedulePostDateResponse : IMultipleExamplesProvider<SchedulePostDateResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }

        public IEnumerable<SwaggerExample<SchedulePostDateResponse>> GetExamples()
        {
            return new SwaggerExample<SchedulePostDateResponse>[]
            {
                new SwaggerExample<SchedulePostDateResponse>()
                {
                    Name = "Successful Example",
                    Value =new SchedulePostDateResponse()
                    {
                        Data = "Successfully Set Post Date Checks.",
                        Status = true
                    },
                    Summary = "When Successfully Set Post Date Checks."
                },
                new SwaggerExample<SchedulePostDateResponse>()
                {
                    Name = "Error Example",
                    Value =new SchedulePostDateResponse()
                    {
                        Data = "Oops Something went wrong..",
                        Status = true
                    },
                    Summary = "When Something went wrong."
                }
            };
        }
    }
}