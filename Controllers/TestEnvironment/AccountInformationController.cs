using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Request;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
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
        ///  Returns the balance of a debtor account (Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Get the balance of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with any API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountBalance/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// </remarks>
        /// 
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
        ///  Returns account validity of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Check the validity of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountValidity/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
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
        ///  Returns account existences of a debtor account.(Test.)
        /// </summary>
        ///
        /// <remarks>
        /// 
        /// **Details**:
        /// Get the existences of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountExistences/0001-000001
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


        /// <summary>
        ///  Returns recent credit card approval of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets any recent credit card approval of the debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetRecentApproval/1850-190058
        /// 
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetRecentApprovalResponseModel), 200)]

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
        /// <summary>
        ///  Returns  Multiples account and it's balance from a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets multiples account and it's balance of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetMultiples/1850-190058
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
        /// <summary>
        ///  Returns account Info of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets account's Info of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountInfo/1850-190058
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

        /// <summary>
        ///  Returns the SIF of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets SIF of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetSIF/0001-000001
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

        /// <summary>
        ///  Returns Interactions Account Data.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetInteractionsAcctData
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
            Serilog.Log.Information("GetInteractionsAcctData => GET");
            try
            {
                //P for prod.
                var item = await _context.GetInteractionsAcctData(request, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns client invoice header.(Test)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client invoice header data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetClientInvoiceHeader/0001&amp;A
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
                var item = await _context.GetClientInvoiceHeader(clientAcct, company, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns Client Primary Contact.(Test)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client primary contact data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/test/AccountInformation/GetClientPrimaryContact/0001&amp;A
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
                var item = await _context.GetClientPrimaryContact(clientAcct, company, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Can Get Client Invoice Payments.(test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Get Client Invoice Payments.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/Test/AccountInformation/GetClientInvoicePayments
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
                var item = await _context.GetClientInvoicePayments(request, "T");

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
