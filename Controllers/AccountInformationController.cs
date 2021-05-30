using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountInformationController : ControllerBase
    {
        private readonly IGetAccountInformation _context;

        public AccountInformationController(IGetAccountInformation context)
        {
            _context = context;
        }

        [HttpGet("GetAccountBalance/{debtorAcct}")]
        public async Task<IActionResult> GetAccountBalance(string debtorAcct)
        {
            Serilog.Log.Information(" GetAccountBalance => GET");
            try
            {
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct);

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        [HttpGet("GetAccountValidity/{debtorAcct}")]
        public IActionResult GetAccountValidity(string debtorAcct)
        {
            Serilog.Log.Information(" GetAccountValidity => GET");
            try
            {
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct);

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        [HttpGet("GetAccountExistences/{debtorAcct}")]
        public async Task<IActionResult> GetAccountExistences(string debtorAcct)
        {
            Serilog.Log.Information(" GetAccountExistences => GET");
            try
            {
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct);

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpGet("GetRecentApproval/{debtorAcct}")]
        public async Task<IActionResult> GetRecentApproval(string debtorAcct)
        {
            Serilog.Log.Information(" GetRecentApproval => GET");
            try
            {
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct);

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

    }
}
