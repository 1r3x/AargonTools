using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;

namespace AargonTools.Manager
{
    public class AddNotesV2Manager:IAddNotesV2
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;

        public AddNotesV2Manager(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
        }

        public async Task<ResponseModel> CreateNotes(AddNotesRequestModel request, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster
                    {
                        DebtorAcct = request.DebtorAcct,
                        NoteDate = new DateTime(datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, datetimeNow.Hour, datetimeNow.Minute, 0),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText.ToUpper()
                    };

                    await _context.NoteMasters.AddAsync(note);
                    await _context.SaveChangesAsync();

                }
                else if (environment == "PO")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = request.DebtorAcct,
                        NoteDate = new DateTime(datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, datetimeNow.Hour, datetimeNow.Minute, 0),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText.ToUpper()
                    };
                    await _contextProdOld.NoteMasters.AddAsync(note);
                    await _contextProdOld.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = request.DebtorAcct,
                        NoteDate = new DateTime(datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, datetimeNow.Hour, datetimeNow.Minute, 0),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText.ToUpper()
                    };
                    await _contextTest.NoteMasters.AddAsync(note);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return _response.Response(true,false,e);
            }

            return _response.Response(true,true,"Successfully added a notes.");
        }
    }
}
