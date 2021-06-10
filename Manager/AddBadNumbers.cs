using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class AddBadNumbers : IAddBadNumbers
    {
        private static ExistingDataDbContext _context;
        private static ResponseModel _response;

        public AddBadNumbers(ExistingDataDbContext context, ResponseModel response)
        {
            _context = context;
            _response = response;
        }
        async Task<ResponseModel> IAddBadNumbers.AddBadNumbers(string accountNo, string phoneNo)
        {

            var debtorPhoneData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == accountNo);


            if (debtorPhoneData != null)
            {
                var badNumbers = new DebtorBadNumber
                {
                    DebtorAcct = accountNo,
                    HomeAreaCode = debtorPhoneData.HomeAreaCode,
                    HomePhone = phoneNo,
                    TimeAttempted = DateTime.Now,
                    Reason = "REMOVED FROM ACCOUNT"
                };

                await _context.DebtorBadNumbers.AddAsync(badNumbers);

            }
            if (debtorPhoneData != null && debtorPhoneData.HomePhone == phoneNo)
            {
                debtorPhoneData.HomePhone = null;
                _context.DebtorPhoneInfos.Update(debtorPhoneData);
            }

            else if (debtorPhoneData != null && debtorPhoneData.WorkPhone == phoneNo)
            {
                debtorPhoneData.WorkPhone = null;
                _context.DebtorPhoneInfos.Update(debtorPhoneData);
            }
            else if (debtorPhoneData != null && debtorPhoneData.CellPhone == phoneNo)
            {
                debtorPhoneData.CellPhone = null;
                _context.DebtorPhoneInfos.Update(debtorPhoneData);
            }
            else if (debtorPhoneData != null && debtorPhoneData.OtherPhone == phoneNo)
            {
                debtorPhoneData.OtherPhone = null;
                _context.DebtorPhoneInfos.Update(debtorPhoneData);
            }

            try
            {
                await _context.SaveChangesAsync();
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
