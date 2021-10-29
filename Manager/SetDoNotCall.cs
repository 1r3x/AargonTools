using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static IAddNotes _addNotes;

        public SetDoNotCall(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, IAddNotes addNotes,
            ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _addNotes = addNotes;
            _contextProdOld = contextProdOld;
        }

        public async Task<ResponseModel> SetDoNotCallManager(string debtorAcct, string cellPhoneNo, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var areaCode = cellPhoneNo.Substring(0, 3);
                        var cellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _context.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                        if (targetData.CellPhone != null && targetData.CellPhone == cellNo && targetData.CellAreaCode == areaCode)
                        {
                            targetData.CellPhoneDontCall = "Y";
                            _context.Update(targetData);
                            await _context.SaveChangesAsync();
                            await _addNotes.CreateNotes(debtorAcct, "PUT ON NOTICE (" + areaCode + "-" + cellNo + ") BY CUSTOMER.", environment);
                            return _response.Response(true,true,"Successfully set the number to don't call status.");
                        }
                        else
                        {
                            return _response.Response(true,false,"This account is not associate with this cell number");
                        }
                    }
                    else
                    {
                        return _response.Response(true,false,"This is not a valid cell number for US.Just put areaCode+centralOffice+lineNumber. ex. 7025052773");
                    }



                }
                else if (environment=="PO")
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var areaCode = cellPhoneNo.Substring(0, 3);
                        var cellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _contextProdOld.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                        if (targetData.CellPhone != null && targetData.CellPhone == cellNo && targetData.CellAreaCode == areaCode)
                        {
                            targetData.CellPhoneDontCall = "Y";
                            _contextProdOld.Update(targetData);
                            await _contextProdOld.SaveChangesAsync();
                            await _addNotes.CreateNotes(debtorAcct, "PUT ON NOTICE (" + areaCode + "-" + cellNo + ") BY CUSTOMER.", environment);
                            return _response.Response(true,true,"Successfully set the number to don't call status.");
                        }
                        else
                        {
                            return _response.Response(true, false, "This account is not associate with this cell number");
                        }
                    }
                    else
                    {
                        return _response.Response(true, false, "This is not a valid cell number for US.Just put areaCode+centralOffice+lineNumber. ex. 7025052773");
                    }


                }
                else
                {
                    var rxCellPhoneUs = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    if (rxCellPhoneUs.IsMatch(cellPhoneNo))
                    {
                        var areaCode = cellPhoneNo.Substring(0, 3);
                        var cellNo = cellPhoneNo.Substring(3, 7);

                        var targetData = await _contextTest.DebtorPhoneInfos.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                        if (targetData.CellPhone != null && targetData.CellPhone == cellNo && targetData.CellAreaCode == areaCode)
                        {
                            targetData.CellPhoneDontCall = "Y";
                            _contextTest.Update(targetData);
                            await _context.SaveChangesAsync();
                            await _addNotes.CreateNotes(debtorAcct, "PUT ON NOTICE (" + areaCode + "-" + cellNo + ") BY CUSTOMER.", environment);
                        }
                        else
                        {
                            return _response.Response(true, false, "This account is not associate with this cell number");
                        }
                    }
                    else
                    {
                        return _response.Response(true, false, "This is not a valid cell number for US.Just put areaCode+centralOffice+lineNumber. ex. 7025052773");
                    }

                }

            }
            catch (Exception e)
            {
                return _response.Response(true,false,e);
                throw;
            }

            return _response.Response("Successfully set do not call. ");
        }
    }
}
