using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace AargonTools.Manager
{
    public class AddNotes:IAddNotes
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;

        public AddNotes(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
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
                Console.WriteLine(e);
                throw;
            }

            return _response.Response("Success.");
        }


    }
}
