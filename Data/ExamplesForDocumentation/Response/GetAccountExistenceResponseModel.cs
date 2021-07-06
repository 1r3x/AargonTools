using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetAccountExistenceResponseModel : IMultipleExamplesProvider<GetAccountExistenceResponseModel>
    {
        public bool Status { get; set; }
        public List<string> Data { get; set; }


        IEnumerable<SwaggerExample<GetAccountExistenceResponseModel>> IMultipleExamplesProvider<GetAccountExistenceResponseModel>.GetExamples()
        {
            return new List<SwaggerExample<GetAccountExistenceResponseModel>>(
                new[]
                {
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 1",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"A","True"})
                        },
                        Summary = "When the Company Flag is A."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 2",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"D","True"})
                        },
                        Summary = "When the Company Flag is D."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 3",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"H","True"})
                        },
                        Summary = "When the Company Flag is H."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 4",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"L","True"})
                        },
                        Summary = "When the Company Flag is L."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 5",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"T","True"})
                        },
                        Summary = "When the Company Flag is T."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 6",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"W","True"})
                        },
                        Summary = "When the Company Flag is W."
                    },
                    new SwaggerExample<GetAccountExistenceResponseModel>()
                    {
                        Name = "Status 7",
                        Value = new GetAccountExistenceResponseModel()
                        {
                            Status = true,
                            Data = new List<string>(new []{"Not Found"})
                        },
                        Summary = "When the account doesn't exists."
                    }
                }
            );
        }

    }
}
