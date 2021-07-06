using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Request;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
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
        ///  Returns the balance of the debtor account.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the balance of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountBalance/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 

        [HttpGet("GetAccountBalance/{debtorAcct}")]
        [ProducesResponseType(typeof(GetAccountBalanceResponseModel), 200)]
        public async Task<IActionResult> GetAccountBalance(string debtorAcct)
        {
            Serilog.Log.Information(" GetAccountBalance => GET");
            try
            {
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }





        /// <summary>
        ///  Returns the account validity of the debtor account.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the validity of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/GetAccountValidity/GetAccountBalance/0001-000001
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
            Serilog.Log.Information(" GetAccountValidity => GET");
            try
            {
                //P for prod.
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns the account existences of the debtor account.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the existences of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/GetAccountExistences/GetAccountExistences/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [HttpGet("GetAccountExistences/{debtorAcct}")]
        [ProducesResponseType(typeof(GetAccountExistenceResponseModel), 200)]
        public async Task<IActionResult> GetAccountExistences(string debtorAcct)
        {
            Serilog.Log.Information(" GetAccountExistences => GET");
            try
            {
                //P for prod.
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns the recent approval of debtor account.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check any recent approval of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/GetRecentApproval/GetAccountExistences/1850-190058
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetRecentApprovalResponseModel), 200)]
        [HttpGet("GetRecentApproval/{debtorAcct}")]
        public async Task<IActionResult> GetRecentApproval(string debtorAcct)
        {
            Serilog.Log.Information(" GetRecentApproval => GET");
            try
            {
                //P for prod.
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct, "P");

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
            Serilog.Log.Information(" GetMultiples => GET");
            try
            {
                //P for prod.
                var item = await _context.GetMultiples(debtorAcct, "P");

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
            Serilog.Log.Information(" GetAccountInfo => GET");
            try
            {
                //P for prod.
                var item = await _context.GetAccountInfo(debtorAcct, "P");

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
            Serilog.Log.Information("GetSIF => GET");
            try
            {
                //P for prod.
                var item = await _context.GetSIF(debtorAcct, "P");

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
