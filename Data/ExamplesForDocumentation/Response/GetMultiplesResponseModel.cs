using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetMultiplesResponseModel : IExamplesProvider<GetMultiplesResponseModel>
    {
        public bool Status { get; set; }
        public List<MultiplesObj> Data { get; set; }


        public GetMultiplesResponseModel GetExamples()
        {
            return new GetMultiplesResponseModel()
            {
                Status = true,
                Data = new List<MultiplesObj>()
                {
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-196625",
                        Balance = 25.6
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-198249",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-195329",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-195907",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-190059",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-197965",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-197022",
                        Balance = 44.59
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-197966",
                        Balance = 107.36
                    },
                    new MultiplesObj()
                    {
                        DebtorAcct = "1850-195908",
                        Balance = 117.51
                    },
                }
            };
        }
    }

    public class MultiplesObj
    {
        public string DebtorAcct { get; set; }
        public double Balance { get; set; }
    }
}
