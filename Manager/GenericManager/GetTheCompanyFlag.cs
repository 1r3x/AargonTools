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
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        public GetTheCompanyFlag(ExistingDataDbContext context, TestEnvironmentDbContext contextTest,ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
        }

        private IQueryable<IDebtorAcctInfo> _debtorTable;
        private IQueryable<IQueueMaster> _queueTable;

        public async Task<IQueryable<IDebtorAcctInfo>> GetFlagForDebtorAccount(string debtorAcct,string environment)
        {

            var account = debtorAcct.Substring(0, 4);
            if (environment=="P")
            {
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
            else if (environment=="PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfos;
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoDs;
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoHs;
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoLs;
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoTs;
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoWs;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfos;
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoDs;
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoHs;
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoLs;
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoTs;
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoWs;
                }
                else
                {
                    return null;
                }
            }

          
        }

        public async Task<IQueryable<IQueueMaster>> GetFlagForQueueMaster(string debtorAcct, string environment)
        {
            if (environment=="P")
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
            else
            {

                if (await _contextTest.QueueMasters.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasters;
                }
                else if (await _contextTest.QueueMasterDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterDs;
                }
                else if (await _contextTest.QueueMasterHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterHs;
                }
                else if (await _contextTest.QueueMasterLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterLs;
                }
                else if (await _contextTest.QueueMasterTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterTs;
                }
                else if (await _contextTest.QueueMasterWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).SingleOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterWs;
                }
                else
                {
                    return null;
                }
            }

        }


        public async Task<string> GetStringFlag(string debtorAcct, string environment)
        {
            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "W";
                }
                else
                {
                    return null;
                }
            }
            else if (environment=="PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "W";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).SingleOrDefaultAsync() != null)
                {
                    return "W";
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
