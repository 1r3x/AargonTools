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
    public class SetMoveToDisputeManager : ISetMoveToDispute
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;

        public SetMoveToDisputeManager(ExistingDataDbContext context, ResponseModel response,
            GetTheCompanyFlag companyFlag, TestEnvironmentDbContext contextTest)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
        }
        //amountDisputed doesn't have any implementation 
        public async Task<ResponseModel> SetMoveToDispute(string debtorAcct, decimal amountDisputed, string environment)
        {
            if (environment == "P")
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _context.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == "DISPUTE");//for move to DISPUTE only
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
                    return _response.Response("Successfully Move " + targetAcctInfo.DebtorAcct + "  to dispute.");
                }

                return _response.Response(targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to dispute setup.");

            }
            else
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _contextTest.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == "DISPUTE");//for move to DISPUTE only
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
                    return _response.Response("Successfully Move " + targetAcctInfo.DebtorAcct + "  to dispute.");
                }

                return _response.Response(targetAcctInfo.DebtorAcct + " setup employee is out of the range from current dispute to Dispute setup.");
            }
        }
    }
}
