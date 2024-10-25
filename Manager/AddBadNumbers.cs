using System;
using System.Text.RegularExpressions;
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
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static IAddNotes _addNotes;

        public AddBadNumbers(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            IAddNotes addNotes)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _addNotes = addNotes;
        }
        async Task<ResponseModel> IAddBadNumbers.AddBadNumbers(string accountNo, string phoneNo, string environment)
        {
            var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");


            if (environment == "P")
            {
                if (rxCellPhoneUs.IsMatch(phoneNo))
                {
                    var phoneNoWithOutAreaCode = phoneNo.Substring(3, 7);
                    var phoneAreaCode = phoneNo.Substring(0, 3);

                    var debtorPhoneData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == accountNo);

                    if (debtorPhoneData != null)
                    {
                        var badNumbers = new DebtorBadNumber
                        {
                            DebtorAcct = accountNo,
                            HomeAreaCode = phoneAreaCode,
                            HomePhone = phoneNoWithOutAreaCode,
                            TimeAttempted = DateTime.Now,
                            Reason = "REMOVED FROM ACCOUNT"
                        };

                        await _context.DebtorBadNumbers.AddAsync(badNumbers);

                        

                        if (debtorPhoneData.HomePhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.HomePhone = null;
                            debtorPhoneData.HomeAreaCode = null;
                        }
                        else if (debtorPhoneData.WorkPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.WorkPhone = null;
                            debtorPhoneData.WorkAreaCode = null;
                        }
                        else if (debtorPhoneData.CellPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.CellPhone = null;
                            debtorPhoneData.CellAreaCode = null;
                        }
                        else if (debtorPhoneData.OtherPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.OtherPhone = null;
                            debtorPhoneData.OtherAreaCode = null;
                        }

                        _context.DebtorPhoneInfos.Update(debtorPhoneData);


                        await _addNotes.CreateNotes(accountNo, $"PHONE NUMBER REMOVED (API): {phoneAreaCode}-{phoneNoWithOutAreaCode}", environment);

                        try
                        {
                            await _context.SaveChangesAsync();
                            return _response.Response(true, true, "Successfully enlisted a bad number.");
                        }
                        catch (Exception e)
                        {
                            return _response.Response(true, false, e.Message);
                        }
                    }
                    else
                    {
                        return _response.Response(false, false, "Debtor account not found.");
                    }
                }
                else
                {
                    return _response.Response(false, false, "Phone no. not valid.");
                }
            }
            else if (environment == "PO")
            {
                var debtorPhoneData = await _contextProdOld.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == accountNo);


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

                    await _contextProdOld.DebtorBadNumbers.AddAsync(badNumbers);

                }
                if (debtorPhoneData != null && debtorPhoneData.HomePhone == phoneNo)
                {
                    debtorPhoneData.HomePhone = null;
                    _contextProdOld.DebtorPhoneInfos.Update(debtorPhoneData);
                }

                else if (debtorPhoneData != null && debtorPhoneData.WorkPhone == phoneNo)
                {
                    debtorPhoneData.WorkPhone = null;
                    _contextProdOld.DebtorPhoneInfos.Update(debtorPhoneData);
                }
                else if (debtorPhoneData != null && debtorPhoneData.CellPhone == phoneNo)
                {
                    debtorPhoneData.CellPhone = null;
                    _contextProdOld.DebtorPhoneInfos.Update(debtorPhoneData);
                }
                else if (debtorPhoneData != null && debtorPhoneData.OtherPhone == phoneNo)
                {
                    debtorPhoneData.OtherPhone = null;
                    _contextProdOld.DebtorPhoneInfos.Update(debtorPhoneData);
                }

                try
                {
                    await _contextProdOld.SaveChangesAsync();
                    return _response.Response(true, true, "Successfully enlisted a bad number.");
                }
                catch (Exception e)
                {
                    return _response.Response(true, false, e);
                }
            }
            else
            {
                if (rxCellPhoneUs.IsMatch(phoneNo))
                {
                    var phoneNoWithOutAreaCode = phoneNo.Substring(3, 7);
                    var phoneAreaCode = phoneNo.Substring(0, 3);

                    var debtorPhoneData = await _contextTest.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == accountNo);

                    if (debtorPhoneData != null)
                    {
                        var badNumbers = new DebtorBadNumber
                        {
                            DebtorAcct = accountNo,
                            HomeAreaCode = phoneAreaCode,
                            HomePhone = phoneNoWithOutAreaCode,
                            TimeAttempted = DateTime.Now,
                            Reason = "REMOVED FROM ACCOUNT"
                        };

                        await _contextTest.DebtorBadNumbers.AddAsync(badNumbers);



                        if (debtorPhoneData.HomePhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.HomePhone = null;
                            debtorPhoneData.HomeAreaCode = null;
                        }
                        else if (debtorPhoneData.WorkPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.WorkPhone = null;
                            debtorPhoneData.WorkAreaCode = null;
                        }
                        else if (debtorPhoneData.CellPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.CellPhone = null;
                            debtorPhoneData.CellAreaCode = null;
                        }
                        else if (debtorPhoneData.OtherPhone == phoneNoWithOutAreaCode)
                        {
                            debtorPhoneData.OtherPhone = null;
                            debtorPhoneData.OtherAreaCode = null;
                        }

                        _contextTest.DebtorPhoneInfos.Update(debtorPhoneData);


                        await _addNotes.CreateNotes(accountNo, $"PHONE NUMBER REMOVED (API): {phoneAreaCode}-{phoneNoWithOutAreaCode}", environment);

                        try
                        {
                            await _contextTest.SaveChangesAsync();
                            return _response.Response(true, true, "Successfully enlisted a bad number.");
                        }
                        catch (Exception e)
                        {
                            return _response.Response(true, false, e.Message);
                        }
                    }
                    else
                    {
                        return _response.Response(false, false, "Debtor account not found.");
                    }
                }
                else
                {
                    return _response.Response(false, false, "Phone no. not valid.");
                }
            }
        }
    }
}
