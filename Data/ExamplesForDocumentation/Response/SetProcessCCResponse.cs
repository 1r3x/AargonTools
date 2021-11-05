using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class SetProcessCCResponse : IMultipleExamplesProvider<SetProcessCCResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public TransactionDetails Data { get; set; }
        public IEnumerable<SwaggerExample<SetProcessCCResponse>> GetExamples()
        {
            return new SwaggerExample<SetProcessCCResponse>[]
            {
                new SwaggerExample<SetProcessCCResponse>()
                {
                    Name = "Successful Example",
                    Value =new SetProcessCCResponse()
                    {
                        Data = new TransactionDetails()
                        {
                            type = "transaction",
                            key = "knfktk55t0rzpj7",
                            refnum="3124327616",
                            is_duplicate="N",
                            result_code="A",
                            result="Approved",
                            authcode="724281",
                            creditcard = new Creditcard()
                            {
                                number="4929xxxxxxxx0006",
                                category_code= "A",
                                entry_mode="Card Not Present, Manually Keyed"
                            },
                            avs = new Avs()
                            {
                                result_code= "YYY",
                                result="Address: Match & 5 Digit Zip: Match"
                            },
                            cvc = new Cvc()
                            {
                                result_code= "P",
                                result="Not Processed"
                            },
                            batch = new Batch()
                            {
                                type= "batch",
                                key= "8t1mf5xfyxt1sy6",
                                batchrefnum= "432362",
                                sequence= "1"
                            },
                            auth_amount= "13.00",
                            trantype="Credit Card Sale"
                        },
                        Status = true,
                        TransactionStatus=true
                    },
                    Summary = "Successful Response"
                }
            };
        }


        public class TransactionDetails
        {
            public string type { get; set; }
            public string key { get; set; }
            public string refnum { get; set; }
            public string is_duplicate { get; set; }
            public string result_code { get; set; }
            public string result { get; set; }
            public string authcode { get; set; }
            public Creditcard creditcard { get; set; }
            public Avs avs { get; set; }
            public Cvc cvc { get; set; }
            public Batch batch { get; set; }
            public string auth_amount { get; set; }
            public string trantype { get; set; }
        }

        public class Creditcard
        {
            public string number { get; set; }
            public string category_code { get; set; }
            public string entry_mode { get; set; }
        }

        public class Avs
        {
            public string result_code { get; set; }
            public string result { get; set; }
        }

        public class Cvc
        {
            public string result_code { get; set; }
            public string result { get; set; }
        }

        public class Batch
        {
            public string type { get; set; }
            public string key { get; set; }
            public string batchrefnum { get; set; }
            public string sequence { get; set; }
        }

    }

}
