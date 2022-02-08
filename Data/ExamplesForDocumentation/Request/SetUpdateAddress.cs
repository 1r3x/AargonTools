using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetUpdateAddress : IExamplesProvider<SetUpdateAddressRequestModel>
    {
        public SetUpdateAddressRequestModel GetExamples()
        {
            return new SetUpdateAddressRequestModel()
            {
                Address1 = "3601 SEDONA CREEK",
                Address2 = "APT.23",
                City = "LAS VEGAS",
                DebtorAcct = "NV",
                ResidenceType = "OWNS",
                Zip = "89117",
                Source = "INTERACTIONS",
                State = "NV"
            };
        }
    }
}
