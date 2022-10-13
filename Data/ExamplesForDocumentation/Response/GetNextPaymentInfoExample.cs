using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetNextPaymentInfoExample : IExamplesProvider<GetNextPaymentInfoExample>
    {
        public bool Status { get; set; }
        public GetNextPaymentInfoData Data { get; set; }
        public GetNextPaymentInfoExample GetExamples()
        {
            return new GetNextPaymentInfoExample()
            {
                Status = true,
                Data = new GetNextPaymentInfoData()
                {
                    name_first_last= "DENNIS CALLANAN",
                    balance= 856,
                    amount_paid_life= (decimal)112.9,
                    date_of_service= Convert.ToDateTime("1996-05-15 00:00:00.000"),
                    ssn9 = 900000001,
                    street_number= "1234",
                    street_name= "MONEY ST TEST2",
                    city= "LAS VEGAS",
                    state= "NV",
                    zip_code= "900000001",
                    client_name = "Joe's Garage",
                    client_description = "RETAIL",
                    client_interst_bearingB= "Y",
                    client_credit_reportableB= "N",
                    home_phone_number= "",
                    home_phone_verifiedB= "N",
                    home_phone_ponB="N",
                    cell_phone_number = "4147367881",
                    cell_phone_verifiedB= "Y",
                    cell_phone_ponB= "N",
                    last_payment_amount= (decimal)9.9,
                    last_payment_date= Convert.ToDateTime("2022-08-10T00:00:00"),
                    birth_date= Convert.ToDateTime("1998-01-25T00:00:00"),
                    promise_amount= (decimal)935.25,
                    promise_date= Convert.ToDateTime("2022-08-01T00:00:00"),
                    check_amt= 0,
                    check_date= Convert.ToDateTime("2000-01-01T00:00:00"),
                    cc_amt= 0,
                    cc_date= Convert.ToDateTime("2000-01-01T00:00:00")

                }
            };
        }
    }

    public class GetNextPaymentInfoData
    {
        public string name_first_last { get; set; }
        public decimal balance { get; set; }
        public decimal amount_paid_life { get; set; }
        public DateTime date_of_service { get; set; }
        public decimal ssn9 { get; set; }
        public string street_number { get; set; }
        public string street_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string client_name { get; set; }
        public string client_description { get; set; }
        public string client_interst_bearingB { get; set; }
        public string client_credit_reportableB { get; set; }
        public string home_phone_number { get; set; }
        public string home_phone_verifiedB { get; set; }
        public string home_phone_ponB { get; set; }
        public string cell_phone_number { get; set; }
        public string cell_phone_verifiedB { get; set; }
        public string cell_phone_ponB { get; set; }
        public decimal last_payment_amount { get; set; }
        public DateTime last_payment_date { get; set; }
        public DateTime birth_date { get; set; }
        public decimal promise_amount { get; set; }
        public DateTime promise_date { get; set; }
        public decimal check_amt { get; set; }
        public DateTime check_date { get; set; }
        public decimal cc_amt { get; set; }
        public DateTime cc_date { get; set; }
    }
}
