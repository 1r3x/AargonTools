﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetNumberManager : ISetNumber
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private readonly IAddNotes _addNotes;

        public SetNumberManager(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, IAddNotes addNotes,
            ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _addNotes = addNotes;
            _contextProdOld = contextProdOld;
        }

        public async Task<ResponseModel> SetNumber(string debtorAcct, string cellPhoneNo, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var onlyAreaCode = cellPhoneNo.Substring(0, 3);
                        var onlyCellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                        if (targetData != null)
                        {
                            if (targetData.CellPhone != null)
                            {
                                // Add new phone number to NewPhoneNumbers table
                                var newPhoneNumber = new NewPhoneNumber
                                {
                                    DebtorAcct = debtorAcct,
                                    AreaCode = onlyAreaCode,
                                    PhoneNum = onlyCellNo,
                                    DateAcquired = DateTime.Now,
                                    EnteredBy = 1994,
                                    NumberType = "CELL",
                                    Source = "API"
                                };

                                await _context.NewPhoneNumbers.AddAsync(newPhoneNumber);


                                // Update existing phone number
                                targetData.CellPhone = onlyCellNo;
                                targetData.CellAreaCode = onlyAreaCode;
                                _context.Update(targetData);


                                await _context.SaveChangesAsync();

                                await _addNotes.CreateNotes(debtorAcct, $"NEW PHONE NUMBER (API): {onlyAreaCode}-{onlyCellNo}", environment);

                                return _response.Response(true, true, $"Successfully set a new number on new phone number directory for debtor account {debtorAcct}.");
                            }

                            // Update existing phone number
                            targetData.CellPhone = onlyCellNo;
                            targetData.CellAreaCode = onlyAreaCode;
                            _context.Update(targetData);

                            await _addNotes.CreateNotes(debtorAcct, $"NEW PHONE NUMBER (API): {onlyAreaCode}-{onlyCellNo}", environment);

                            await _context.SaveChangesAsync();

                            return _response.Response(true, true, $"Successfully set the number for debtor account {debtorAcct}.");
                        }

                        return _response.Response(false, false, "Debtor account not found.");
                    }

                    return _response.Response(false, false, "This is not a valid US cell number.");

                }
                else if (environment == "PO")
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");
                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var areaCode = cellPhoneNo.Substring(0, 3);
                        var cellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _contextProdOld.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                        if (targetData.CellPhone != null)
                        {
                            //todo 
                            var newPhoneNumber = new NewPhoneNumber()
                            {
                                DebtorAcct = debtorAcct,
                                AreaCode = areaCode,
                                PhoneNum = cellNo,
                                DateAcquired = DateTime.Now,
                                EnteredBy = 1994,
                                NumberType = "CELL",
                                Source = "API"
                            };
                            await _contextProdOld.NewPhoneNumbers.AddAsync(newPhoneNumber);
                            await _contextProdOld.SaveChangesAsync();
                            await _addNotes.CreateNotes(debtorAcct, "NEW PHONE NUMBER (API): " +
                                                                    " {" + areaCode + "-" + cellNo + "}",
                                environment);
                            return _response.Response(true, true, "Successfully set a new number on new phone number directory " +
                                                                  "for debtor account " + debtorAcct + ".");
                        }

                        targetData.CellPhone = cellPhoneNo;
                        targetData.CellAreaCode = areaCode;
                        _contextProdOld.Update(targetData);
                        await _addNotes.CreateNotes(debtorAcct, "NEW PHONE NUMBER (API): " +
                                                                " {" + areaCode + "-" + cellNo + "}",
                             environment);
                        return _response.Response(true, true, "Successfully set the number for debtor account " + debtorAcct + ".");
                    }
                    return _response.Response(true, false, "This is not a valid US cell number.");
                }
                else
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var onlyAreaCode = cellPhoneNo.Substring(0, 3);
                        var onlyCellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _contextTest.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                        if (targetData != null)
                        {
                            if (targetData.CellPhone != null)
                            {
                                // Add new phone number to NewPhoneNumbers table
                                var newPhoneNumber = new NewPhoneNumber
                                {
                                    DebtorAcct = debtorAcct,
                                    AreaCode = onlyAreaCode,
                                    PhoneNum = onlyCellNo,
                                    DateAcquired = DateTime.Now,
                                    EnteredBy = 1994,
                                    NumberType = "CELL",
                                    Source = "API"
                                };

                                await _contextTest.NewPhoneNumbers.AddAsync(newPhoneNumber);


                                // Update existing phone number
                                targetData.CellPhone = onlyCellNo;
                                targetData.CellAreaCode = onlyAreaCode;
                                _contextTest.Update(targetData);


                                await _contextTest.SaveChangesAsync();

                                await _addNotes.CreateNotes(debtorAcct, $"NEW PHONE NUMBER (API): {onlyAreaCode}-{onlyCellNo}", environment);

                                return _response.Response(true, true, $"Successfully set a new number on new phone number directory for debtor account {debtorAcct}.");
                            }

                            // Update existing phone number
                            targetData.CellPhone = onlyCellNo;
                            targetData.CellAreaCode = onlyAreaCode;
                            _contextTest.Update(targetData);

                            await _addNotes.CreateNotes(debtorAcct, $"NEW PHONE NUMBER (API): {onlyAreaCode}-{onlyCellNo}", environment);

                            await _contextTest.SaveChangesAsync();

                            return _response.Response(true, true, $"Successfully set the number for debtor account {debtorAcct}.");
                        }

                        return _response.Response(false, false, "Debtor account not found.");
                    }

                    return _response.Response(false, false, "This is not a valid US cell number.");

                }

            }
            catch (Exception e)
            {
                return _response.Response(true, false, e);
            }
        }
    }
}
