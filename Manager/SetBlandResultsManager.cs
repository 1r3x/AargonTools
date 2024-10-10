using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AargonTools.ViewModel.BlandResultsViewModel;

namespace AargonTools.Manager
{
    public class SetBlandResultsManager : ISetBlandResults
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private readonly IAddNotesV2 _addNotesV2;

        public SetBlandResultsManager(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld, IAddNotesV2 addNotesV2)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _addNotesV2 = addNotesV2;
        }

        public async Task<ResponseModel> SetBlandResults(List<BlandResultsViewModel> interactResultModel, string environment)
        {
            foreach (var item in interactResultModel)
            {
                try
                {
                    var supportText = "";
                    int maxLength = 255;

                    if (item.summary.Length > maxLength)
                    {
                        supportText = item.summary[..maxLength];
                    }
                    else
                    {
                        supportText = item.summary;
                    }

                    //// Initialize an empty string to hold the support text
                    //var supportText = "";

                    //// Define the maximum length of the text to copy
                    //int maxLength = 255;

                    //// Define the starting index from where to begin copying the text
                    //int startIndex = 53;

                    //// Check if the length of the summary is greater than the starting index
                    //if (interactResultModel.summary.Length > startIndex)
                    //{
                    //    // Calculate the length of the text to copy, ensuring it does not exceed maxLength
                    //    int lengthToCopy = Math.Min(maxLength, interactResultModel.summary.Length - startIndex);

                    //    // Copy the substring from the summary starting at startIndex with the calculated length
                    //    supportText = interactResultModel.summary.Substring(startIndex, lengthToCopy);
                    //}
                    //else
                    //{
                    //    // If the summary length is less than or equal to the starting index, copy the entire summary
                    //    supportText = interactResultModel.summary;
                    //}



                    if (environment == "P")
                    {

                        var aiCallResult = new AiCallResult()
                        {
                            DebtorAcct = item.variables.debtorAccount1,
                            CallType = item.inbound ? "1" : "0",
                            CallPhoneNumber = item.inbound ? item.variables.short_from : item.variables.short_to,
                            CallTime = item.variables.timestamp,
                            CallLength = Convert.ToInt32(item.corrected_duration),
                            CallPaymentAmt = Convert.ToDecimal(item.variables.amount99),
                            CallStatus = item.status,
                            CallDisposition = item.disposition_tag,
                            CallrecordingUrl=item.recording_url,
                            CallrecordingFile=null
                        };
                        await _context.AiCallResults.AddAsync(aiCallResult);
                        await _context.SaveChangesAsync();
                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1901,
                            ActivityCode = "RA",
                            DebtorAcct = item.variables.debtorAccount1,
                            NoteText = supportText
                        }, environment);


                    }
                    else if (environment == "PO")
                    {
                        var aiCallResult = new AiCallResult()
                        {
                            DebtorAcct = item.variables.debtorAccount1,
                            CallType = item.inbound ? "1" : "0",
                            CallPhoneNumber = item.inbound ? item.variables.short_from : item.variables.short_to,
                            CallTime = item.variables.timestamp,
                            CallLength = Convert.ToInt32(item.corrected_duration),
                            CallPaymentAmt = Convert.ToDecimal(item.variables.amount99),
                            CallStatus = item.status,
                            CallDisposition = item.disposition_tag,
                            CallrecordingUrl = item.recording_url,
                            CallrecordingFile = null
                        };
                        await _contextProdOld.AiCallResults.AddAsync(aiCallResult);
                        await _contextProdOld.SaveChangesAsync();
                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1901,
                            ActivityCode = "RA",
                            DebtorAcct = item.variables.debtorAccount1,
                            NoteText = supportText
                        }, environment);
                    }
                    else
                    {
                        var aiCallResult = new AiCallResult()
                        {
                            DebtorAcct = item.variables.debtorAccount1,
                            CallType = item.inbound ? "1" : "0",
                            CallPhoneNumber = item.inbound ? item.variables.short_from : item.variables.short_to,
                            CallTime = item.variables.timestamp,
                            CallLength = Convert.ToInt32(item.corrected_duration),
                            CallPaymentAmt = Convert.ToDecimal(item.variables.amount99),
                            CallStatus = item.status,
                            CallDisposition = item.disposition_tag,
                            CallrecordingUrl = item.recording_url,
                            CallrecordingFile = null
                        };
                        await _contextTest.AiCallResults.AddAsync(aiCallResult);
                        await _contextTest.SaveChangesAsync();
                        await _addNotesV2.CreateNotes(new AddNotesRequestModel()
                        {
                            Employee = 1901,
                            ActivityCode = "RA",
                            DebtorAcct = item.variables.debtorAccount1,
                            NoteText = supportText
                        }, environment);
                    }

                }
                catch (Exception e)
                {
                    return _response.Response(true, false, e);
                }
            }
           

            return _response.Response(true, true, "Successfully added interact results");
        }
    }
}
