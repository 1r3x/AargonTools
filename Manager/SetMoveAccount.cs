﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Serilog.Context;

namespace AargonTools.Manager
{
    public class SetMoveAccount : ISetMoveAccount
    {
        private static ExistingDataDbContext _context;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private static IUserService _userService;

        public SetMoveAccount(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag, IUserService userService)
        {
            _context = context;
            _response = response;
            _companyFlag = companyFlag;
            _userService = userService;
        }

        async Task<ResponseModel> ISetMoveAccount.SetMoveAccount(string debtorAcct, int toQueue)
        {

            var oldQueue = await _companyFlag.GetFlagForDebtorAccount(debtorAcct).Result.FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
            if (oldQueue.Employee == null)
            {
                return _response.Response("Invalid Request[This account is inactive].");
            }
            //if it's only transfer into same company then check for it
            var toQueueResult = await _context.EmployeeInfos.FirstOrDefaultAsync(x => x.Employee == toQueue && x.EmployeeType == "Q" && x.AcctStatus == "A");
            if (toQueueResult == null)
            {
                return _response.Response("Invalid Request[Request queue not available].");
            }
            var targetQueue = await _companyFlag.GetFlagForQueueMaster(debtorAcct).Result
                .FirstOrDefaultAsync(x => x.DebtorAcct == debtorAcct);
            if (targetQueue.Employee == null) return _response.Response("Invalid Request.[By any how data data corrupted for" + debtorAcct + " its not in the any queue master tables].");
            var note = new NoteMaster()
            {
                DebtorAcct = debtorAcct,
                NoteDate = DateAndTime.Now,
                Employee = 1994,
                ActivityCode = "RA",
                NoteText = "ONLINE MOVE ACCOUNT (" + oldQueue.Employee + " -> " + toQueueResult.Employee + ")"
            };
            await _context.NoteMasters.AddAsync(note);

            //for getting the user


            //datetime for remove seconds
            var datetimeNow = DateTime.Now;
            var log = new MoveAccountApiLogs()
            {
                DebtorAcct = debtorAcct,
                FromQueue = (int)oldQueue.Employee,
                ToQueue = toQueue,
                MoveDate = datetimeNow.AddSeconds(-datetimeNow.Second),
                //todo get the current user
                Requestor = _userService.GetLoginUserName()
            };
            await _context.MoveAccountApiLogs.AddAsync(log);


            oldQueue.Employee = toQueue;
            targetQueue.Employee = toQueue;
            _context.Update(oldQueue);
            _context.Update(targetQueue);

            await _context.SaveChangesAsync();

            return _response.Response("Success.");
        }
    }
}
