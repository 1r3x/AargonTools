using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetDoNotCall : ISetDoNotCall
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;
        private static IAddNotes _addNotes;

        public SetDoNotCall(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, IAddNotes addNotes)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _addNotes = addNotes;
        }

        public async Task<ResponseModel> SetDoNotCallManager(string debtorAcct, string cellPhoneNo, string environment)
        {
            try
            {
                if (environment == "P")
                {

                    var targetData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                    if (targetData.CellPhone != null && targetData.CellPhone == cellPhoneNo)
                    {
                        targetData.CellPhoneDontCall = "Y";
                        _context.Update(targetData);
                        await _addNotes.CreateNotes(debtorAcct, "PUT ON NOTICE (" + cellPhoneNo + ") BY CUSTOMER.", "P");
                    }
                    else
                    {
                        return _response.Response("This account is not associate with this cell number");
                    }

                }
                else
                {
                    var targetData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                    if (targetData.CellPhone == cellPhoneNo)
                    {
                        targetData.CellPhoneDontCall = "Y";
                        _contextTest.Update(targetData);
                        await _addNotes.CreateNotes(debtorAcct, "PUT ON NOTICE (" + cellPhoneNo + ") BY CUSTOMER.", "T");
                    }
                    else
                    {
                        return _response.Response("This account is not associate with this cell number");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return _response.Response("Successfully set do not call.");
        }
    }
}
