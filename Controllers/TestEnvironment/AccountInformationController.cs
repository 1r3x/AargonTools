using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
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
            Serilog.Log.Information("Test GetAccountBalance => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct, "T");

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
            Serilog.Log.Information("Test GetAccountValidity => GET");
            try
            {
                //T for test.
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct, "T");

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
            Serilog.Log.Information("Test GetAccountExistences => GET");
            try
            {
                //T for test.
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct, "T");

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
            Serilog.Log.Information("Test GetRecentApproval => GET");
            try
            {
                //T for test.
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpGet("GetMultiples/{debtorAcct}")]
        public async Task<IActionResult> GetMultiples(string debtorAcct)
        {
            Serilog.Log.Information("Test GetMultiples => GET");
            try
            {
                //T for test.
                var item = await _context.GetMultiples(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpGet("GetAccountInfo/{debtorAcct}")]
        public async Task<IActionResult> GetAccountInfo(string debtorAcct)
        {
            Serilog.Log.Information("Test GetAccountInfo => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountInfo(debtorAcct, "T");

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
