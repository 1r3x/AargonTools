using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager.GenericManager
{
    public class GetTheCompanyFlag
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        public GetTheCompanyFlag(ExistingDataDbContext context, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
        }

        private IQueryable<IDebtorAcctInfo> _debtorTable;
        private IQueryable<IQueueMaster> _queueTable;
        private IQueryable<IClientAcctInfo> _accountInfoTable;
        private IQueryable<IDebtorMaster> _debtorMasterTable;
        private IQueryable<IClientMaster> _clientMasterTable;

        public async Task<IQueryable<IDebtorAcctInfo>> GetFlagForDebtorAccount(string debtorAcct, string environment)
        {

            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfos;
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoDs;
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoHs;
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoLs;
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoTs;
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoWs;
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfos;
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoDs;
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoHs;
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoLs;
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoTs;
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoWs;
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextProdOld.DebtorAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfos;
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoDs;
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _context.DebtorAcctInfoHs;
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoLs;
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoTs;
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoWs;
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorTable = _contextTest.DebtorAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }


        }

        public async Task<IQueryable<IQueueMaster>> GetFlagForQueueMaster(string debtorAcct, string environment)
        {
            if (environment == "P")
            {

                if (await _context.QueueMasters.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasters;
                }
                else if (await _context.QueueMasterDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterDs;
                }
                else if (await _context.QueueMasterHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterHs;
                }
                else if (await _context.QueueMasterLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterLs;
                }
                else if (await _context.QueueMasterTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterTs;
                }
                else if (await _context.QueueMasterWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterWs;
                }
                else if (await _context.QueueMasterPs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _context.QueueMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                if (await _contextTest.QueueMasters.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasters;
                }
                else if (await _contextTest.QueueMasterDs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterDs;
                }
                else if (await _contextTest.QueueMasterHs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterHs;
                }
                else if (await _contextTest.QueueMasterLs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterLs;
                }
                else if (await _contextTest.QueueMasterTs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterTs;
                }
                else if (await _contextTest.QueueMasterWs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterWs;
                }
                else if (await _contextTest.QueueMasterPs.Where(x => x.DebtorAcct == debtorAcct).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _queueTable = _contextTest.QueueMasterPs;
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
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "W";
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "P";
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "W";
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "P";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "A";
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "D";
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "H";
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "L";
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "T";
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "W";
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "P";
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<string> GetStringFlagForAdoQuery(string debtorAcct, string environment)
        {
            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "";
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_d";
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_h";
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_l";
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_t";
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_w";
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_p";
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "";
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_d";
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_h";
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_l";
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_t";
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_w";
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_p";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "";
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_d";
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_h";
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_l";
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_t";
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_w";
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return "_p";
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<IQueryable<IClientAcctInfo>> GetFlagForClientAccountInfo(string debtorAcct, string environment)
        {

            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfos;
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoDs;
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoHs;
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoLs;
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoTs;
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoWs;
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _context.ClientAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfos;
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoDs;
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoHs;
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoLs;
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoTs;
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoWs;
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextProdOld.ClientAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfos;
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoDs;
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoHs;
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoLs;
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoTs;
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoWs;
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _accountInfoTable = _contextTest.ClientAcctInfoPs;
                }
                else
                {
                    return null;
                }
            }


        }
        //for sake of performance 
        public async Task<IQueryable<IDebtorMaster>> GetFlagForDebtorMasterBySsn(string ssn, string environment)
        {

            if (environment == "P")
            {
                if (await _context.DebtorMasters.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasters;
                }
                else if (await _context.DebtorMasterDs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterDs;
                }
                else if (await _context.DebtorMasterHs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterHs;
                }
                else if (await _context.DebtorMasterLs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterLs;
                }
                else if (await _context.DebtorMasterTs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterTs;
                }
                else if (await _context.DebtorMasterWs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterWs;
                }
                else if (await _context.DebtorMasterPs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.DebtorMasters.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasters;
                }
                else if (await _contextProdOld.DebtorMasterDs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterDs;
                }
                else if (await _contextProdOld.DebtorMasterHs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterHs;
                }
                else if (await _contextProdOld.DebtorMasterLs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterLs;
                }
                else if (await _contextProdOld.DebtorMasterTs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterTs;
                }
                else if (await _contextProdOld.DebtorMasterWs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterWs;
                }
                else if (await _contextProdOld.DebtorMasterPs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.DebtorMasters.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasters;
                }
                else if (await _contextTest.DebtorMasterDs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterDs;
                }
                else if (await _contextTest.DebtorMasterHs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterHs;
                }
                else if (await _contextTest.DebtorMasterLs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterLs;
                }
                else if (await _contextTest.DebtorMasterTs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterTs;
                }
                else if (await _contextTest.DebtorMasterWs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterWs;
                }
                else if (await _contextTest.DebtorMasterPs.Where(x => x.Ssn == ssn).Select(x => x.DebtorAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }


        }

        public async Task<IQueryable<IDebtorMaster>> GetFlagForDebtorMaster(string debtorAcct, string environment)
        {
            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasters;
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterDs;
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterHs;
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterLs;
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterTs;
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterWs;
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _context.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasters;
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterDs;
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterHs;
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterLs;
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterTs;
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterWs;
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextProdOld.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasters;
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterDs;
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterHs;
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterLs;
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterTs;
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterWs;
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _debtorMasterTable = _contextTest.DebtorMasterPs;
                }
                else
                {
                    return null;
                }
            }


        }

        public async Task<IQueryable<IClientMaster>> GetFlagForClientMaster(string debtorAcct, string environment)
        {

            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasters;
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterDs;
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterHs;
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterLs;
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterTs;
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterWs;
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _context.ClientMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasters;
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterDs;
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterHs;
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterLs;
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterTs;
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterWs;
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextProdOld.ClientMasterPs;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasters;
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterDs;
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterHs;
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterLs;
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterTs;
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterWs;
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return _clientMasterTable = _contextTest.ClientMasterPs;
                }
                else
                {
                    return null;
                }
            }


        }



    }
}
