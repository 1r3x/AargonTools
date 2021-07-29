﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation
{
    public class SetAddNotesResponse : IMultipleExamplesProvider<SetAddNotesResponse>
    {
        public bool Status { get; set; }
        public string Data { get; set; }
        public IEnumerable<SwaggerExample<SetAddNotesResponse>> GetExamples()
        {
            return new SwaggerExample<SetAddNotesResponse>[]
            {
                new SwaggerExample<SetAddNotesResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetAddNotesResponse()
                    {
                        Data = "Successfully added a notes.",
                        Status = true
                    },
                    Summary = "Successfully added"
                },
                new SwaggerExample<SetAddNotesResponse>()
                {
                    Name = "Error Example 1",
                    Value =new SetAddNotesResponse()
                    {
                        Data = "If something went wrong then you get a specific message about it.",
                        Status = true
                    },
                    Summary = "If something went wrong"
                },
               
            };
        }
    }
}