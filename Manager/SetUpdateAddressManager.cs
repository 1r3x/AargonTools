﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetUpdateAddressManager : ISetUpdateAddress
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private readonly AdoDotNetConnection _adoConnection;
        private readonly IAddNotesV2 _addNotesV2;
        public SetUpdateAddressManager(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag,
            TestEnvironmentDbContext contextText, AdoDotNetConnection adoConnection, ProdOldDbContext contextProdOld, IAddNotesV2 addNotesV2)
        {
            _context = context;
            _contextTest = contextText;
            _response = response;
            _companyFlag = companyFlag;
            _adoConnection = adoConnection;
            _contextProdOld = contextProdOld;
            _addNotesV2 = addNotesV2;
        }


        public async Task<ResponseModel> SetUpdateAddress(SetUpdateAddressRequestModel setUpdateAddressRequestModelModel, string environment)
        {
            try
            {
                var approvalLIst = new List<string>
                {
                    "BUSINESS",
                    "NCOA",
                    "OTHER",
                    "OWNS",
                    "RELATIVES",
                    "RENTS",
                    "TEMP HOLD",
                    "UNDELIVERABLE",
                    "UNDELIVERABLE+",
                    "UNVERIFIED"
                };



                if (environment == "P")
                {

                    var debtorMaster = _companyFlag
                        .GetFlagForDebtorMaster(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    var debtorAccount = _companyFlag
                        .GetFlagForDebtorAccount(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    if (approvalLIst.Contains(setUpdateAddressRequestModelModel.ResidenceType))
                    {
                        if (debtorMaster != null)
                        {
                            await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                            {
                                Employee = 1950,
                                ActivityCode = "MA",
                                DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                                NoteText = "OLD ADDR1: " + debtorMaster.Address1 +
                                           " ADDR2: " + debtorMaster.Address2 +
                                           " CITY: " + debtorMaster.City +
                                           " ST: " + debtorMaster.StateCode +
                                           " ZIP: " + debtorMaster.Zip
                            }, environment);


                            debtorMaster.Address1 = setUpdateAddressRequestModelModel.Address1;
                            debtorMaster.Address2 = setUpdateAddressRequestModelModel.Address2;
                            debtorMaster.City = setUpdateAddressRequestModelModel.City;
                            debtorMaster.StateCode = setUpdateAddressRequestModelModel.State;
                            debtorMaster.Zip = setUpdateAddressRequestModelModel.Zip;
                            debtorMaster.ResidenceStatus = setUpdateAddressRequestModelModel.ResidenceType;
                            debtorMaster.AddressChangeDate = DateTime.Now;
                            //validateResidence.Result.Address1Changed = DateTime.Now;
                            //validateResidence.Result.Address2Changed = DateTime.Now;

                            _context.Update(debtorMaster);
                        }


                        if (debtorAccount != null)
                        {
                            debtorAccount.MailReturn = "N";
                            _context.Update(debtorAccount);
                        }

                        await _context.SaveChangesAsync();


                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1950,
                            ActivityCode = "MA",
                            DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                            NoteText = "ADDRESS UPDATED VIA API: "+setUpdateAddressRequestModelModel.Source
                        }, environment);

                       
                    }
                    else
                    {
                        return _response.Response(true, false, "Residence type is not valid");
                    }

                }
                else if (environment == "PO")
                {
                    var debtorMaster = _companyFlag
                       .GetFlagForDebtorMaster(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    var debtorAccount = _companyFlag
                        .GetFlagForDebtorAccount(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    if (approvalLIst.Contains(setUpdateAddressRequestModelModel.ResidenceType))
                    {
                        if (debtorMaster != null)
                        {
                            await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                            {
                                Employee = 1950,
                                ActivityCode = "MA",
                                DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                                NoteText = "OLD ADDR1: " + debtorMaster.Address1 +
                                           " ADDR2: " + debtorMaster.Address2 +
                                           " CITY: " + debtorMaster.City +
                                           " ST: " + debtorMaster.StateCode +
                                           " ZIP: " + debtorMaster.Zip
                            }, environment);


                            debtorMaster.Address1 = setUpdateAddressRequestModelModel.Address1;
                            debtorMaster.Address2 = setUpdateAddressRequestModelModel.Address2;
                            debtorMaster.City = setUpdateAddressRequestModelModel.City;
                            debtorMaster.StateCode = setUpdateAddressRequestModelModel.State;
                            debtorMaster.Zip = setUpdateAddressRequestModelModel.Zip;
                            debtorMaster.ResidenceStatus = setUpdateAddressRequestModelModel.ResidenceType;
                            debtorMaster.AddressChangeDate = DateTime.Now;
                            //validateResidence.Result.Address1Changed = DateTime.Now;
                            //validateResidence.Result.Address2Changed = DateTime.Now;

                            _contextProdOld.Update(debtorMaster);
                        }


                        if (debtorAccount != null)
                        {
                            debtorAccount.MailReturn = "N";
                            _contextProdOld.Update(debtorAccount);
                        }

                        await _contextProdOld.SaveChangesAsync();


                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1950,
                            ActivityCode = "MA",
                            DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                            NoteText = "ADDRESS UPDATED VIA API: " + setUpdateAddressRequestModelModel.Source
                        }, environment);


                    }
                    else
                    {
                        return _response.Response(true, false, "Residence type is not valid");
                    }
                }
                else
                {
                    var debtorMaster = _companyFlag
                      .GetFlagForDebtorMaster(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    var debtorAccount = _companyFlag
                        .GetFlagForDebtorAccount(setUpdateAddressRequestModelModel.DebtorAcct, environment).Result.FirstOrDefault();

                    if (approvalLIst.Contains(setUpdateAddressRequestModelModel.ResidenceType))
                    {
                        if (debtorMaster != null)
                        {
                            await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                            {
                                Employee = 1950,
                                ActivityCode = "MA",
                                DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                                NoteText = "OLD ADDR1: " + debtorMaster.Address1 +
                                           " ADDR2: " + debtorMaster.Address2 +
                                           " CITY: " + debtorMaster.City +
                                           " ST: " + debtorMaster.StateCode +
                                           " ZIP: " + debtorMaster.Zip
                            }, environment);


                            debtorMaster.Address1 = setUpdateAddressRequestModelModel.Address1;
                            debtorMaster.Address2 = setUpdateAddressRequestModelModel.Address2;
                            debtorMaster.City = setUpdateAddressRequestModelModel.City;
                            debtorMaster.StateCode = setUpdateAddressRequestModelModel.State;
                            debtorMaster.Zip = setUpdateAddressRequestModelModel.Zip;
                            debtorMaster.ResidenceStatus = setUpdateAddressRequestModelModel.ResidenceType;
                            debtorMaster.AddressChangeDate = DateTime.Now;
                            //validateResidence.Result.Address1Changed = DateTime.Now;
                            //validateResidence.Result.Address2Changed = DateTime.Now;

                            _contextTest.Update(debtorMaster);
                        }


                        if (debtorAccount != null)
                        {
                            debtorAccount.MailReturn = "N";
                            _contextTest.Update(debtorAccount);
                        }

                        await _contextTest.SaveChangesAsync();


                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1950,
                            ActivityCode = "MA",
                            DebtorAcct = setUpdateAddressRequestModelModel.DebtorAcct,
                            NoteText = "ADDRESS UPDATED VIA API: " + setUpdateAddressRequestModelModel.Source
                        }, environment);


                    }
                    else
                    {
                        return _response.Response(true, false, "Residence type is not valid");
                    }
                }

            }
            catch (Exception e)
            {
                return _response.Response(true, false, e);
            }

            return _response.Response(true, true, "Successfully update address");
        }
    }
}