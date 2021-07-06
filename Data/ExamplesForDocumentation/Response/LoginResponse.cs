using AargonTools.Models.DTOs.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class LoginResponse : IExamplesProvider<RegistrationResponse>
    {
        public RegistrationResponse GetExamples()
        {
            return new()
            {

                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBmNTc1OGVlLWJmMjctNGZiNC1iNDkzL" +
                        "TAxOGQ1YjUxZjliOSIsImVtYWlsIjoiYWtpYkBleGFtcGxlLmNvbSIsInN1YiI6ImFraWJAZXhhbXBsZS5jb20i" +
                        "LCJqdGkiOiI2ZjUyNjcyYi00MmY2LTRmMWYtYjBhNi1mNjczM2I0ZGZmZDAiLCJuYmYiOjE2MjUyNTE4ODMsImV" +
                        "4cCI6MTYyNTI1NDg4MywiaWF0IjoxNjI1MjUxODgzfQ.B8PthYKj9p-uJlFDOB77_VDGdwx9ETITg_ccg6et05k",
                RefreshToken = "Z5JW2XQXU5IG4JAFMVZ7AWUF68M549AID7Wcfc76d94-17e9-4613-adc1-59d32de1eb67",
                Success = true,
                Errors = null
            };
        }

        //IEnumerable<SwaggerExample<RegistrationResponse>> IMultipleExamplesProvider<RegistrationResponse>.GetExamples()
        //{
        //    return new List<SwaggerExample<RegistrationResponse>>(
        //        new[]
        //        {
        //            new SwaggerExample<RegistrationResponse>()
        //            {
        //                Name = "Status",
        //                Value = new RegistrationResponse()
        //                {
        //                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBmNTc1OGVlLWJmMjctNGZiNC1iNDkzL" +
        //                            "TAxOGQ1YjUxZjliOSIsImVtYWlsIjoiYWtpYkBleGFtcGxlLmNvbSIsInN1YiI6ImFraWJAZXhhbXBsZS5jb20i" +
        //                            "LCJqdGkiOiI2ZjUyNjcyYi00MmY2LTRmMWYtYjBhNi1mNjczM2I0ZGZmZDAiLCJuYmYiOjE2MjUyNTE4ODMsImV" +
        //                            "4cCI6MTYyNTI1NDg4MywiaWF0IjoxNjI1MjUxODgzfQ.B8PthYKj9p-uJlFDOB77_VDGdwx9ETITg_ccg6et05k",
        //                    RefreshToken = "Z5JW2XQXU5IG4JAFMVZ7AWUF68M549AID7Wcfc76d94-17e9-4613-adc1-59d32de1eb67",
        //                    Status = true,
        //                    Errors = null
        //                },
        //                Summary = "This is a success response"
        //            },
        //            new SwaggerExample<RegistrationResponse>()
        //            {
        //                Name = "Error",
        //                Value = new RegistrationResponse()
        //                {
        //                    Token = null,
        //                    RefreshToken = null,
        //                    Status = false,
        //                    Errors = new List<string>(new []{"Oops","Wrong"})
        //                },
        //                Summary = "This is a Error response"
        //            }
        //        }
        //    );
        //}
    }
}
