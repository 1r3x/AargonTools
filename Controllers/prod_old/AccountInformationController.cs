using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.prod_old
{
    [Route("api/prod_old/[controller]")]
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
        ///  Returns the balance of the debtor account(prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the balance of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountBalance/0001-000001
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
            Serilog.Log.Information("prod_old GetAccountBalance => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns the account validity of the debtor account(prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the validity of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountValidity/0001-000001
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
            Serilog.Log.Information("prod_old GetAccountValidity => GET");
            try
            {
                //T for test.
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns the account existences of the debtor account(prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the existences of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountExistences/0001-000001
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
            Serilog.Log.Information("prod_old GetAccountExistences => GET");
            try
            {
                //T for test.
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns the recent approval of debtor account (prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check any recent approval of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetRecentApproval/1850-190058
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
            Serilog.Log.Information("prod_old GetRecentApproval => GET");
            try
            {
                //T for test.
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }
        /// <summary>
        ///  Returns the Multiples account and its balance of debtor account (prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check Multiples account and its balance of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetMultiples/1850-190058
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetMultiplesResponseModel), 200)]
        [HttpGet("GetMultiples/{debtorAcct}")]
        public async Task<IActionResult> GetMultiples(string debtorAcct)
        {
            Serilog.Log.Information("prod_old GetMultiples => GET");
            try
            {
                //T for test.
                var item = await _context.GetMultiples(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }
        /// <summary>
        ///  Returns the Account Info of debtor account (prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check Account Info of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountInfo/1850-190058
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetAccountInfoResponse), 200)]
        [HttpGet("GetAccountInfo/{debtorAcct}")]
        public async Task<IActionResult> GetAccountInfo(string debtorAcct)
        {
            Serilog.Log.Information("prod_old GetAccountInfo => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountInfo(debtorAcct, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns the SIF of debtor account (prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check SIF of any debtor account by passing the parametrize debtor account no. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetSIF/0001-000001
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetSifResponse), 200)]
        [HttpGet("GetSIF/{debtorAcct}")]
        public async Task<IActionResult> GetSIF(string debtorAcct)
        {
            Serilog.Log.Information("prod_old GetSIF => GET");
            try
            {
                //T for test.
                var item = await _context.GetSIF(debtorAcct, "PO");

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
