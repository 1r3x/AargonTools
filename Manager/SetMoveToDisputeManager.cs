using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetMoveToDisputeManager : ISetMoveToDispute
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;

        public SetMoveToDisputeManager(ExistingDataDbContext context, ResponseModel response,
            GetTheCompanyFlag companyFlag, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
            _contextProdOld = contextProdOld;
        }
        //amountDisputed doesn't have any implementation 
        public async Task<ResponseModel> SetMoveToDispute(string debtorAcct, string environment)
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
                    return _response.Response(true, true, "Successfully Move " + targetAcctInfo.DebtorAcct + "  to dispute.");
                }

                return _response.Response(true, false, targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to dispute setup.");

            }
            else if (environment=="PO")
            {
                var targetAcctInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                var targetAcctFlag = await _companyFlag.GetStringFlag(debtorAcct, environment);
                var apiMoveSetting = await _contextProdOld.ApiMoveSettings.SingleOrDefaultAsync(x => x.Company == targetAcctFlag && x.Type == "DISPUTE");//for move to DISPUTE only
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
                    return _response.Response(true, true, "Successfully Move " + targetAcctInfo.DebtorAcct + "  to dispute.");
                }

                return _response.Response(true, false, targetAcctInfo.DebtorAcct + " setup employee is out of the range from current move to dispute setup.");
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
                    return _response.Response(true, true, "Successfully Move " + targetAcctInfo.DebtorAcct + "  to dispute.");
                }

                return _response.Response(true, false, targetAcctInfo.DebtorAcct + " setup employee is out of the range from current dispute to Dispute setup.");
            }
        }
    }
}
