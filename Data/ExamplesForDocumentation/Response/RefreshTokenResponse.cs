﻿using AargonTools.Configuration;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{


    public class RefreshTokenResponse : IExamplesProvider<AuthResult>
    {
        public AuthResult GetExamples()
        {
            return new AuthResult()
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
    }
}
