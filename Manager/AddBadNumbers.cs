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
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;

        public AddBadNumbers(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
        }
        async Task<ResponseModel> IAddBadNumbers.AddBadNumbers(string accountNo, string phoneNo,string environment)
        {
            if (environment=="P")
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
                    return _response.Response("Successfully enlisted a bad number.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                var debtorPhoneData = await _contextTest.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == accountNo);


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

                    await _contextTest.DebtorBadNumbers.AddAsync(badNumbers);

                }
                if (debtorPhoneData != null && debtorPhoneData.HomePhone == phoneNo)
                {
                    debtorPhoneData.HomePhone = null;
                    _contextTest.DebtorPhoneInfos.Update(debtorPhoneData);
                }

                else if (debtorPhoneData != null && debtorPhoneData.WorkPhone == phoneNo)
                {
                    debtorPhoneData.WorkPhone = null;
                    _contextTest.DebtorPhoneInfos.Update(debtorPhoneData);
                }
                else if (debtorPhoneData != null && debtorPhoneData.CellPhone == phoneNo)
                {
                    debtorPhoneData.CellPhone = null;
                    _contextTest.DebtorPhoneInfos.Update(debtorPhoneData);
                }
                else if (debtorPhoneData != null && debtorPhoneData.OtherPhone == phoneNo)
                {
                    debtorPhoneData.OtherPhone = null;
                    _contextTest.DebtorPhoneInfos.Update(debtorPhoneData);
                }

                try
                {
                    await _contextTest.SaveChangesAsync();
                    return _response.Response("Status.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
           

           
        }
    }
}
