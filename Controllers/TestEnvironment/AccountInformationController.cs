using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountInformationController : ControllerBase
    {
        private readonly IGetAccountInformation _context;

        public AccountInformationController(IGetAccountInformation context)
        {
            _context = context;
        }
        /// <summary>
        ///  Returns the balance of the debtor account(Test Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the balance of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountBalance/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [ProducesResponseType(typeof(GetAccountBalanceResponseModel), 200)]
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

        /// <summary>
        ///  Returns the account validity of the debtor account(Test Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the validity of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/Test/GetAccountValidity/GetAccountBalance/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [ProducesResponseType(typeof(GetAccountValidityResponseModel), 200)]
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

        /// <summary>
        ///  Returns the account existences of the debtor account(Test Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the existences of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/Test/GetAccountExistences/GetAccountExistences/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [ProducesResponseType(typeof(GetAccountExistenceResponseModel), 200)]
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


        [HttpGet("GetSIF/{debtorAcct}")]
        public async Task<IActionResult> GetSIF(string debtorAcct)
        {
            Serilog.Log.Information("Test GetSIF => GET");
            try
            {
                //T for test.
                var item = await _context.GetSIF(debtorAcct, "T");

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
