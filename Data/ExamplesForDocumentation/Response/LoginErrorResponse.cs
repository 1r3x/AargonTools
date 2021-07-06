using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class LoginErrorResponse : IMultipleExamplesProvider<LoginErrorResponse>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string Errors { get; set; }

        IEnumerable<SwaggerExample<LoginErrorResponse>> IMultipleExamplesProvider<LoginErrorResponse>.GetExamples()
        {
            return new List<SwaggerExample<LoginErrorResponse>>(
                new[]
                {
                    new SwaggerExample<LoginErrorResponse>()
                    {
                        Name = "Error 1",
                        Value = new LoginErrorResponse()
                        {
                            Token = null,
                            RefreshToken = null,
                            Success = false,
                            Errors = "User not exists"
                        },
                        Summary = "If user not found"
                    },
                    new SwaggerExample<LoginErrorResponse>()
                    {
                        Name = "Error 2",
                        Value = new LoginErrorResponse()
                        {
                            Token = null,
                            RefreshToken = null,
                            Success = false,
                            Errors = "Invalid login request, password or user doesn't match"
                        },
                        Summary = "If user/password doesn't match."
                    },
                    new SwaggerExample<LoginErrorResponse>()
                    {
                        Name = "Error 3",
                        Value = new LoginErrorResponse()
                        {
                            Token = null,
                            RefreshToken = null,
                            Success = false,
                            Errors =  "Invalid payload"
                        },
                        Summary =  "If invalid payload"
                    }
                }
            );
        }
    }
}
