using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetMoveAccount : ISetMoveAccount
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private readonly GetTheCompanyFlag _companyFlag;
        private static IUserService _userService;

        public SetMoveAccount(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag, IUserService userService,
            TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _response = response;
            _companyFlag = companyFlag;
            _userService = userService;
            _contextProdOld = contextProdOld;
        }

        async Task<ResponseModel> ISetMoveAccount.SetMoveAccount(string debtorAcct, int toQueue, string environment)
        {
            if (environment == "P")
            {
                var oldQueue = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result
                          .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                if (oldQueue?.Employee == null)
                {
                    // Account is inactive
                    return _response.Response(true, false, "Invalid Request [This account is inactive].");
                }

                // New implementations check account for collectors queue
                var collectorsQueue = await _context.EmployeeInfos
                                    .FirstOrDefaultAsync(x => x.Employee == oldQueue.Employee);

                if (collectorsQueue?.EmployeeType == "B" || collectorsQueue?.EmployeeType == "C")
                {
                    // Account is in an employee queue
                    return _response.Response(true, false, "Invalid Request [This account is in an employee queue].");
                }

                // If it's only transfer into same company then check for it
                var toQueueResult = await _context.EmployeeInfos
                                    .FirstOrDefaultAsync(x => x.Employee == toQueue
                                                                && x.EmployeeType == "Q"
                                                                && x.AcctStatus == "A");

                if (toQueueResult == null)
                {
                    // Request queue not available
                    return _response.Response(true, false, "Invalid Request [Request queue not available].");
                }

                var targetQueue = await _companyFlag.GetFlagForQueueMaster(debtorAcct, environment).Result
                                    .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                if (targetQueue?.Employee == null)
                {
                    // Data corrupted, account not in any queue master tables
                    return _response.Response(true, false, "Invalid Request. [By any how data corrupted for " + debtorAcct + ", not in any queue master tables].");
                }

                // Create and save note
                var datetimeNow = DateTime.Now;
                var note = new NoteMaster
                {
                    DebtorAcct = debtorAcct,
                    NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                    Employee = 1994,
                    ActivityCode = "RA",
                    NoteText = "ONLINE MOVE ACCOUNT (" + oldQueue.Employee + " -> " + toQueueResult.Employee + ")"
                };
                await _context.NoteMasters.AddAsync(note);

                // Create and save log
                var log = new MoveAccountApiLogs
                {
                    DebtorAcct = debtorAcct,
                    FromQueue = (int)oldQueue.Employee,
                    ToQueue = toQueue,
                    MoveDate = datetimeNow.AddSeconds(-datetimeNow.Second),
                    Requestor = _userService.GetLoginUserName()
                };
                await _context.MoveAccountApiLogs.AddAsync(log);

                // Update queues
                oldQueue.Employee = toQueue;
                targetQueue.Employee = toQueue;
                _context.Update(oldQueue);
                _context.Update(targetQueue);

                await _context.SaveChangesAsync();

                return _response.Response(true, true, "Account moved successfully.");


            }
            else if (environment == "PO")
            {
                var oldQueue = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                if (oldQueue.Employee == null)
                {
                    return _response.Response(true, false, "Invalid Request[This account is inactive].");
                }

                //new implementtaions check account for collectors queue 

                var collectorsQueue = await _contextProdOld.EmployeeInfos.FirstOrDefaultAsync(x => x.Employee == oldQueue.Employee);
                if (collectorsQueue.EmployeeType == "B" || collectorsQueue.EmployeeType == "C")
                {
                    return _response.Response(true, false, "Invalid Request [This account is in an employee queue].");
                }

                //if it's only transfer into same company then check for it
                var toQueueResult = await _contextProdOld.EmployeeInfos.FirstOrDefaultAsync(x => x.Employee == toQueue && x.EmployeeType == "Q" && x.AcctStatus == "A");
                if (toQueueResult == null)
                {
                    return _response.Response(true, false, "Invalid Request[Request queue not available].");
                }
                var targetQueue = await _companyFlag.GetFlagForQueueMaster(debtorAcct, environment).Result
                    .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
                if (targetQueue.Employee == null) return _response.Response(true, false, "Invalid Request.[By any how data corrupted for" + debtorAcct + " its not in the any queue master tables].");
                var datetimeNow = DateTime.Now;
                var note = new NoteMaster()
                {
                    DebtorAcct = debtorAcct,
                    NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                    Employee = 1994,
                    ActivityCode = "RA",
                    NoteText = "ONLINE MOVE ACCOUNT (" + oldQueue.Employee + " -> " + toQueueResult.Employee + ")"
                };
                await _contextProdOld.NoteMasters.AddAsync(note);

                //for getting the user


                //datetime for remove seconds

                var log = new MoveAccountApiLogs()
                {
                    DebtorAcct = debtorAcct,
                    FromQueue = (int)oldQueue.Employee,
                    ToQueue = toQueue,
                    MoveDate = datetimeNow.AddSeconds(-datetimeNow.Second),
                    //todo get the current user
                    Requestor = _userService.GetLoginUserName()
                };
                await _contextProdOld.MoveAccountApiLogs.AddAsync(log);


                oldQueue.Employee = toQueue;
                targetQueue.Employee = toQueue;
                _contextProdOld.Update(oldQueue);
                _contextProdOld.Update(targetQueue);

                await _contextProdOld.SaveChangesAsync();

                return _response.Response(true, true, "Account moved successfully.");
            }
            else
            {
                var oldQueue = await _companyFlag.GetFlagForDebtorAccount(debtorAcct, environment).Result
                           .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                if (oldQueue?.Employee == null)
                {
                    // Account is inactive
                    return _response.Response(true, false, "Invalid Request [This account is inactive].");
                }

                // New implementations check account for collectors queue
                var collectorsQueue = await _contextTest.EmployeeInfos
                                    .FirstOrDefaultAsync(x => x.Employee == oldQueue.Employee);

                if (collectorsQueue?.EmployeeType == "B" || collectorsQueue?.EmployeeType == "C")
                {
                    // Account is in an employee queue
                    return _response.Response(true, false, "Invalid Request [This account is in an employee queue].");
                }

                // If it's only transfer into same company then check for it
                var toQueueResult = await _contextTest.EmployeeInfos
                                    .FirstOrDefaultAsync(x => x.Employee == toQueue
                                                                && x.EmployeeType == "Q"
                                                                && x.AcctStatus == "A");

                if (toQueueResult == null)
                {
                    // Request queue not available
                    return _response.Response(true, false, "Invalid Request [Request queue not available].");
                }

                var targetQueue = await _companyFlag.GetFlagForQueueMaster(debtorAcct, environment).Result
                                    .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);

                if (targetQueue?.Employee == null)
                {
                    // Data corrupted, account not in any queue master tables
                    return _response.Response(true, false, "Invalid Request. [By any how data corrupted for " + debtorAcct + ", not in any queue master tables].");
                }

                // Create and save note
                var datetimeNow = DateTime.Now;
                var note = new NoteMaster
                {
                    DebtorAcct = debtorAcct,
                    NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                    Employee = 1994,
                    ActivityCode = "RA",
                    NoteText = "ONLINE MOVE ACCOUNT (" + oldQueue.Employee + " -> " + toQueueResult.Employee + ")"
                };
                await _contextTest.NoteMasters.AddAsync(note);

                // Create and save log
                var log = new MoveAccountApiLogs
                {
                    DebtorAcct = debtorAcct,
                    FromQueue = (int)oldQueue.Employee,
                    ToQueue = toQueue,
                    MoveDate = datetimeNow.AddSeconds(-datetimeNow.Second),
                    Requestor = _userService.GetLoginUserName()
                };
                await _contextTest.MoveAccountApiLogs.AddAsync(log);

                // Update queues
                oldQueue.Employee = toQueue;
                targetQueue.Employee = toQueue;
                _contextTest.Update(oldQueue);
                _contextTest.Update(targetQueue);

                await _contextTest.SaveChangesAsync();

                return _response.Response(true, true, "Account moved successfully.");
            }


        }
    }
}
