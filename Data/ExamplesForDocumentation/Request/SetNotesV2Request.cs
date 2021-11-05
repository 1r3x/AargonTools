using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class SetNotesV2Request:IExamplesProvider<AddNotesRequestModel>
    {
        public AddNotesRequestModel GetExamples()
        {
            return new AddNotesRequestModel()
            {
               DebtorAcct = "0001-000001",
               ActivityCode = "RA",
               Employee = 1,
               NoteText = "This is the notes"
            };
        }
    }
}
