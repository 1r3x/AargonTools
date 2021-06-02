using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager.GenericManager
{
    public class DebtorAccountAreaManager
    {
        private static ExistingDataDbContext _context;
        public DebtorAccountAreaManager(ExistingDataDbContext context)
        {
            _context = context;
        }

        private IQueryable<IDebtorAcctInfo> _table;

        public async Task<IQueryable<IDebtorAcctInfo>> GetTheProperTable(string debtorAcct)
        {
            var account = debtorAcct.Substring(0, 4);

            if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfos;
            }
            else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfoDs;
            }
            else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfoHs;
            }
            else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfoLs;
            }
            else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfoTs;
            }
            else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
            {
                return _table = _context.DebtorAcctInfoWs;
            }
            else
            {
                return null;
            }
        }


    }
}
