using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetDialingRequestModelExample : IExamplesProvider<SetDialingRequestModel>
    {
        public SetDialingRequestModel GetExamples()
        {
            return new SetDialingRequestModel()
            {
                AreaCode = "212",
                DebtorAccount = "0001-000001",
                ListAccount = 1,
                PhoneNumber = "3037334"
            };
        }
    }
}
