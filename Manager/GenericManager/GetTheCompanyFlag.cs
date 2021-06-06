using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager.GenericManager
{
    public class GetTheCompanyFlag
    {
        private static ExistingDataDbContext _context;
        public GetTheCompanyFlag(ExistingDataDbContext context)
        {
            _context = context;
        }

        private IQueryable<IDebtorAcctInfo> _debtorTable;
        private IQueryable<IQueueMaster> _queueTable;

        public async Task<IQueryable<IDebtorAcctInfo>> GetFlagForDebtorAccount(string debtorAcct)
        {
            var account = debtorAcct.Substring(0, 4);

            if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfos;
            }
            else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfoDs;
            }
            else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfoHs;
            }
            else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfoLs;
            }
            else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfoTs;
            }
            else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _debtorTable = _context.DebtorAcctInfoWs;
            }
            else
            {
                return null;
            }
        }

        public async Task<IQueryable<IQueueMaster>> GetFlagForQueueMaster(string debtorAcct)
        {

            if (await _context.QueueMasters.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasters;
            }
            else if (await _context.QueueMasterDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasterDs;
            }
            else if (await _context.QueueMasterHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasterHs;
            }
            else if (await _context.QueueMasterLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasterLs;
            }
            else if (await _context.QueueMasterTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasterTs;
            }
            else if (await _context.QueueMasterWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
            {
                return _queueTable = _context.QueueMasterWs;
            }
            else
            {
                return null;
            }
        }



    }
}
