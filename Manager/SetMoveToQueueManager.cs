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
    public class SetMoveToQueueManager : ISetMoveToQueue
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private static IAddNotes _addNotes;

        public SetMoveToQueueManager(ExistingDataDbContext context, ResponseModel response,
            GetTheCompanyFlag companyFlag, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld, IAddNotes addNotes)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
            _contextProdOld = contextProdOld;
            _addNotes = addNotes;
        }
        public async Task<ResponseModel> SetMoveToQueue(string debtorAcct, string type, string environment)
        {
            if (environment == "P")
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _context.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == type);//for dynamic move queue
                if (targetAcctInfo.Employee != null && targetAcctInfo.Employee >= apiMoveSetting.FromEmployee && targetAcctInfo.Employee <= apiMoveSetting.ToEmployee)
                {
                    var logForMove = new ApiMoveLog()
                    {
                        DebtorAcc = debtorAcct,
                        MoveSetupId = apiMoveSetting.MoveSetupId,
                        PreviousEmployee = (int)targetAcctInfo.Employee,
                        NewEmployee = apiMoveSetting.TargetEmployee,
                        MoveDate = DateTime.Now
                    };



                    targetAcctInfo.Employee = apiMoveSetting.TargetEmployee;
                    _context.Update(targetAcctInfo);
                    await _context.ApiMoveLogs.AddAsync(logForMove);
                    await _context.SaveChangesAsync();
                    await _addNotes.CreateNotes(debtorAcct, "PUT a custom Notes", environment);
                    return _response.Response("Successfully Move " + targetAcctInfo.DebtorAcct + "  to House.");
                }

                return _response.Response(targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to house setup.");

            }
            else if (environment == "PO")
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _contextProdOld.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == type);//for dynamic move queue
                if (targetAcctInfo.Employee != null && targetAcctInfo.Employee >= apiMoveSetting.FromEmployee && targetAcctInfo.Employee <= apiMoveSetting.ToEmployee)
                {
                    var logForMove = new ApiMoveLog()
                    {
                        DebtorAcc = debtorAcct,
                        MoveSetupId = apiMoveSetting.MoveSetupId,
                        PreviousEmployee = (int)targetAcctInfo.Employee,
                        NewEmployee = apiMoveSetting.TargetEmployee,
                        MoveDate = DateTime.Now
                    };


                    targetAcctInfo.Employee = apiMoveSetting.TargetEmployee;
                    _contextProdOld.Update(targetAcctInfo);
                    await _contextProdOld.ApiMoveLogs.AddAsync(logForMove);
                    await _contextProdOld.SaveChangesAsync();
                    await _addNotes.CreateNotes(debtorAcct, "PUT a custom Notes", environment);
                    return _response.Response("Successfully Move " + targetAcctInfo.DebtorAcct + "  to House.");
                }

                return _response.Response(targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to house setup.");

            }
            else
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _contextTest.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == type);//for dynamic move queue
                if (targetAcctInfo.Employee != null && targetAcctInfo.Employee >= apiMoveSetting.FromEmployee && targetAcctInfo.Employee <= apiMoveSetting.ToEmployee)
                {
                    var logForMove = new ApiMoveLog()
                    {
                        DebtorAcc = debtorAcct,
                        MoveSetupId = apiMoveSetting.MoveSetupId,
                        PreviousEmployee = (int)targetAcctInfo.Employee,
                        NewEmployee = apiMoveSetting.TargetEmployee,
                        MoveDate = DateTime.Now
                    };

                    targetAcctInfo.Employee = apiMoveSetting.TargetEmployee;
                    _contextTest.Update(targetAcctInfo);
                    await _contextTest.ApiMoveLogs.AddAsync(logForMove);
                    await _contextTest.SaveChangesAsync();
                    await _addNotes.CreateNotes(debtorAcct, "PUT a custom Notes", environment);
                    return _response.Response("Successfully Move " + targetAcctInfo.DebtorAcct + "  to House.");
                }

                return _response.Response(targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to house setup.");
            }
        }
    }
}
