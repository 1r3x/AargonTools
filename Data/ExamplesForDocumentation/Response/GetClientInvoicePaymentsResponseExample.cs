using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetClientInvoicePaymentsResponseExample : IExamplesProvider<GetClientInvoicePaymentsResponseExample>
    {
        public bool Status { get; set; }
        public List<GetClientInvoicePaymentsResponseExampleModel> Data { get; set; }
        public GetClientInvoicePaymentsResponseExample GetExamples()
        {
            return new GetClientInvoicePaymentsResponseExample()
            {
                Status = true,
                Data = new List<GetClientInvoicePaymentsResponseExampleModel>()
                {
                    new GetClientInvoicePaymentsResponseExampleModel()
                    {
                        debtor_acct = "0001-000001",
                        supplied_acct = "1234",
                        date_of_service = "5/15/1996 12:00:00 AM",
                        date_placed = "6/15/1996 12:00:00 AM",
                        first_name = "DENNIS",
                        last_name = "CALLANAN",
                        client_amt = "300.0000",
                        agency_amt_decl = "100.0000",
                        fee_pct = "25.0000",
                        tran_date = "10/15/2014 8:58:59 AM",
                        balance = "0.0000",
                        status_code = "SIF",
                        payment_type= "CREDIT CARD",
                        total_payments_amt= "400.0000",
                        amount_due_agency= "0.0000",
                        amount_due_client = "300.0000",
                        cosigner_last_name = ""
                    },
                    new GetClientInvoicePaymentsResponseExampleModel()
                    {
                        debtor_acct = "0001-000002",
                        supplied_acct = "1234",
                        date_of_service = "5/15/1996 12:00:00 AM",
                        date_placed = "6/15/1996 12:00:00 AM",
                        first_name = "DENNIS",
                        last_name = "CALLANAN",
                        client_amt = "300.0000",
                        agency_amt_decl = "100.0000",
                        fee_pct = "25.0000",
                        tran_date = "10/15/2014 8:58:59 AM",
                        balance = "0.0000",
                        status_code = "SIF",
                        payment_type= "CREDIT CARD",
                        total_payments_amt= "400.0000",
                        amount_due_agency= "0.0000",
                        amount_due_client = "300.0000",
                        cosigner_last_name = ""
                    },
                    new GetClientInvoicePaymentsResponseExampleModel()
                    {
                        debtor_acct = "0001-000003",
                        supplied_acct = "1234",
                        date_of_service = "5/15/1996 12:00:00 AM",
                        date_placed = "6/15/1996 12:00:00 AM",
                        first_name = "DENNIS",
                        last_name = "CALLANAN",
                        client_amt = "300.0000",
                        agency_amt_decl = "100.0000",
                        fee_pct = "25.0000",
                        tran_date = "10/15/2014 8:58:59 AM",
                        balance = "0.0000",
                        status_code = "SIF",
                        payment_type= "CREDIT CARD",
                        total_payments_amt= "400.0000",
                        amount_due_agency= "0.0000",
                        amount_due_client = "300.0000",
                        cosigner_last_name = ""
                    }
                  
                }
            };
        }
    }

    public class GetClientInvoicePaymentsResponseExampleModel
    {
        public string debtor_acct { get; set; }
        public string supplied_acct { get; set; }
        public string date_of_service { get; set; }
        public string date_placed { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string client_amt { get; set; }
        public string agency_amt_decl { get; set; }
        public string fee_pct { get; set; }
        public string tran_date { get; set; }
        public string balance { get; set; }
        public string status_code { get; set; }
        public string payment_type { get; set; }
        public string total_payments_amt { get; set; }
        public string amount_due_agency { get; set; }
        public string amount_due_client { get; set; }
        public string cosigner_last_name { get; set; }
    }


}
