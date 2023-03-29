using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetInteractionAcctDataExample : IExamplesProvider<GetInteractionAcctDataExample>
    {
        public bool Status { get; set; }
        public InteractionAcctData Data { get; set; }
        public class InteractionAcctData
        {
            public string debtorAcct { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string stateCode { get; set; }
            public string zip { get; set; }
            public string ssn { get; set; }
            public string birthDate { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string clientName { get; set; }
            public string debtType { get; set; }
            public double balance { get; set; }
            public string emailAddress { get; set; }
            public string homePhoneNumber { get; set; }
            public string cellPhoneNumber { get; set; }
            public string workPhoneNumber { get; set; }
            public string relatiovePhoneNumber { get; set; }
            public string otherPhoneNumer { get; set; }
            public string accountStatus { get; set; }
            public DateTime date { get; set; }

        }

        public GetInteractionAcctDataExample GetExamples()
        {
            return new GetInteractionAcctDataExample()
            {
                Status = true,
                Data = new InteractionAcctData()
                {
                    debtorAcct = "0001-000004",
                    address1 = "2875 DESTINO",
                    address2 = "",
                    city = "LAS VEGAS",
                    stateCode = "NV",
                    zip = "89117",
                    ssn = "910000004",
                    birthDate = "1971-01-01",
                    firstName = "NOA",
                    lastName = "CHAR",
                    clientName = "Joe's Garage",
                    debtType = "PERSONAL SERVICES",
                    balance = Math.Round(3167.703, 2),
                    emailAddress = "",
                    homePhoneNumber = "212-3037334",
                    cellPhoneNumber = "808-2223737",
                    workPhoneNumber = "-",
                    relatiovePhoneNumber = "-",
                    otherPhoneNumer = "-",
                    accountStatus="A",
                    date = Convert.ToDateTime("0001-01-01T00:00:00")
                }
            };
        }
    }
}
