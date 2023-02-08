using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;

namespace AargonTools.Manager
{
    public class AddNotesV3:IAddNotesV3
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _contextCurrentBackupTest;
        private static ResponseModel _response;

        public AddNotesV3(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            CurrentBackupTestEnvironmentDbContext contextCurrentBackupTest)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _contextCurrentBackupTest = contextCurrentBackupTest;
        }
        public async Task<ResponseModel> CreateNotes(NoteMaster request, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = request.DebtorAcct,
                        ActionCode = request.ActionCode,
                        Important = request.Important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText
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
                        ActionCode = request.ActionCode,
                        Important = request.Important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText
                    };
                    await _contextProdOld.NoteMasters.AddAsync(note);
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = request.DebtorAcct,
                        ActionCode = request.ActionCode,
                        Important = request.Important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText
                    };
                    await _contextCurrentBackupTest.NoteMasters.AddAsync(note);
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = request.DebtorAcct,
                        ActionCode = request.ActionCode,
                        Important = request.Important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = request.Employee,
                        ActivityCode = request.ActivityCode,
                        NoteText = request.NoteText
                    };
                    await _contextTest.NoteMasters.AddAsync(note);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return _response.Response(true, false, e);
            }

            return _response.Response(true, true, "Successfully added a notes.");
        }
    }
}
