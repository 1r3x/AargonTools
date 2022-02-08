using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Request;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
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
        ///  Returns the balance of a debtor account (prod_old.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Get the balance of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with any API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountBalance/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// </remarks>
        ///
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        ///
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
        ///  Returns account validity of a debtor account.(prod_old.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Check the validity of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountValidity/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        ///
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
        ///  Returns account existences of a debtor account.(prod_old.)
        /// </summary>
        ///
        /// <remarks>
        /// 
        /// **Details**:
        /// Get the existences of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountExistences/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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
        ///  Returns recent credit card approval of a debtor account.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets any recent credit card approval of the debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetRecentApproval/1850-190058
        /// 
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
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
        ///  Returns  Multiples account and it's balance from a debtor account.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets multiples account and it's balance of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetMultiples/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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
        ///  Returns account Info of a debtor account.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets account's Info of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetAccountInfo/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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
        ///  Returns the SIF of a debtor account.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets SIF of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetSIF/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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


        /// <summary>
        ///  Returns Interactions Account Data.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetInteractionsAcctData
        /// and it's POST request for getting the data.
        ///
        /// **Details**:
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for phone [@"\d{10}"] ex. 2123037334,
        /// please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetInteractionAcctDataExample), 200)]
        [HttpPost("GetInteractionsAcctData")]
        public async Task<IActionResult> GetInteractionsAcctData([FromBody] GetInteractionAcctDateRequestModel request)
        {
            Serilog.Log.Information("prod_old GetInteractionsAcctData => GET");
            try
            {
                //P for prod.
                var item = await _context.GetInteractionsAcctData(request, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns client invoice header.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client invoice header data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetClientInvoiceHeader/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 

        [ProducesResponseType(typeof(GetClientInvoiceHeaderExample), 200)]
        [HttpGet("GetClientInvoiceHeader/{clientAcct}&{company}")]
        public async Task<IActionResult> GetClientInvoiceHeader(string clientAcct, string company)
        {
            Serilog.Log.Information("GetClientInvoiceHeader => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoiceHeader(clientAcct, company, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns Client Primary Contact.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client primary contact data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/prod_old/AccountInformation/GetClientPrimaryContact/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetClientPrimaryContactExample), 200)]
        [HttpGet("GetClientPrimaryContact/{clientAcct}&{company}")]

        public async Task<IActionResult> GetClientPrimaryContact(string clientAcct, string company)
        {
            Serilog.Log.Information("GetClientPrimaryContact => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientPrimaryContact(clientAcct, company, "PO");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Can Get Client Invoice Payments.(prod_old.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Get Client Invoice Payments.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/AccountInformation/GetClientInvoicePayments
        /// (pass JSON body like the request example)
        ///  A valid token is required for setting the data.
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(GetClientInvoicePaymentsResponseExample), 200)]
        [HttpPost("GetClientInvoicePayments")]
        public async Task<IActionResult> GetClientInvoicePayments([FromBody] GetClientInvoiceRequestModel request)
        {
            Serilog.Log.Information("GetClientInvoicePayments  => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoicePayments(request, "PO");

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
