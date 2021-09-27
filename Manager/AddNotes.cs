using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;

namespace AargonTools.Manager
{
    public class AddNotes:IAddNotes
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;

        public AddNotes(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
        }

        async Task<ResponseModel> IAddNotes.CreateNotes(string debtorAccount, string notes,string environment)
        {
            try
            {
                if (environment=="P")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = debtorAccount,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = 1994,
                        ActivityCode = "RA",
                        NoteText = notes
                    };
                    await _context.NoteMasters.AddAsync(note);
                    await _context.SaveChangesAsync();
                }
                else if (environment=="PO")
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = debtorAccount,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = 1994,
                        ActivityCode = "RA",
                        NoteText = notes
                    };
                    await _contextProdOld.NoteMasters.AddAsync(note);
                    await _contextProdOld.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var note = new NoteMaster()
                    {
                        DebtorAcct = debtorAccount,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        Employee = 1994,
                        ActivityCode = "RA",
                        NoteText = notes
                    };
                    await _contextTest.NoteMasters.AddAsync(note);
                    await _contextTest.SaveChangesAsync();
                }
               
            }
            catch (Exception e)
            {
                return _response.Response(e);
            }

            return _response.Response("Successfully added a notes.");
        }


    }
}
