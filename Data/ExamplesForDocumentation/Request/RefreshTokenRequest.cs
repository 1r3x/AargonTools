using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models.DTOs.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{


    public class RefreshTokenRequest : IExamplesProvider<TokenRequest>
    {
        public TokenRequest GetExamples()
        {
            return new TokenRequest()
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBmNTc1OGVlLWJmMjctNGZiNC1iNDkzL" +
                "TAxOGQ1YjUxZjliOSIsImVtYWlsIjoiYWtpYkBleGFtcGxlLmNvbSIsInN1YiI6ImFraWJAZXhhbXBsZS5jb20i" +
                "LCJqdGkiOiI2ZjUyNjcyYi00MmY2LTRmMWYtYjBhNi1mNjczM2I0ZGZmZDAiLCJuYmYiOjE2MjUyNTE4ODMsImV" +
                "4cCI6MTYyNTI1NDg4MywiaWF0IjoxNjI1MjUxODgzfQ.B8PthYKj9p-uJlFDOB77_VDGdwx9ETITg_ccg6et05k",
                RefreshToken = "Z5JW2XQXU5IG4JAFMVZ7AWUF68M549AID7Wcfc76d94-17e9-4613-adc1-59d32de1eb67"
            };
        }
    }
}
