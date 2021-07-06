using System.Collections.Generic;
using AargonTools.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class TodoControllerGetItemResponse : IExamplesProvider<List<TestApiData>>
    {
        public List<TestApiData> GetExamples()
        {
            return new()
            {
                new()
                {
                    Id = 123,Title = "Title One",Description = "Description 1",Done = true
                },
                new()
                {
                    Id = 124,Title = "Title Two",Description = "Description 2",Done = true
                }
            };

        }
    }
}
