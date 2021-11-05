using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;

namespace AargonTools.Manager
{
    public class InteractResultsManager:ISetInteractResults
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;

        public InteractResultsManager(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
        }

        public async Task<ResponseModel> SetInteractResults(InteractResult interactResultModel,string environment)
        {
            try
            {
                if (environment == "P")
                {
                    
                    var interactModel = new InteractResult()
                    {
                       Ani = interactResultModel.Ani,
                       CallResult = interactResultModel.CallResult,
                       DebtorAcct = interactResultModel.DebtorAcct,
                       EndTime = interactResultModel.EndTime,
                       LastDialogue = interactResultModel.LastDialogue,
                       OpeningIntent = interactResultModel.OpeningIntent,
                       PaymentAmt = interactResultModel.PaymentAmt,
                       StartTime = interactResultModel.StartTime,
                       TermReason = interactResultModel.TermReason,
                       TransferReason = interactResultModel.TransferReason
                    };
                    await _context.InteractResults.AddAsync(interactModel);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var interactModel = new InteractResult()
                    {
                        Ani = interactResultModel.Ani,
                        CallResult = interactResultModel.CallResult,
                        DebtorAcct = interactResultModel.DebtorAcct,
                        EndTime = interactResultModel.EndTime,
                        LastDialogue = interactResultModel.LastDialogue,
                        OpeningIntent = interactResultModel.OpeningIntent,
                        PaymentAmt = interactResultModel.PaymentAmt,
                        StartTime = interactResultModel.StartTime,
                        TermReason = interactResultModel.TermReason,
                        TransferReason = interactResultModel.TransferReason
                    };
                    await _contextProdOld.InteractResults.AddAsync(interactModel);
                    await _contextProdOld.SaveChangesAsync();
                }
                else
                {
                    var interactModel = new InteractResult()
                    {
                        Ani = interactResultModel.Ani,
                        CallResult = interactResultModel.CallResult,
                        DebtorAcct = interactResultModel.DebtorAcct,
                        EndTime = interactResultModel.EndTime,
                        LastDialogue = interactResultModel.LastDialogue,
                        OpeningIntent = interactResultModel.OpeningIntent,
                        PaymentAmt = interactResultModel.PaymentAmt,
                        StartTime = interactResultModel.StartTime,
                        TermReason = interactResultModel.TermReason,
                        TransferReason = interactResultModel.TransferReason
                    };
                    await _contextTest.InteractResults.AddAsync(interactModel);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return _response.Response(true,false,e);
            }

            return _response.Response(true,true,"Successfully added interact results");
        }

      
    }
}
