using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Request;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
        ///  Returns the balance of a debtor account (Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Get the balance of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with any API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountBalance/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// </remarks>
        ///
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
        ///  Returns account validity of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Check the validity of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountValidity/0001-000001
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
        ///  Returns account existences of a debtor account.(Prod.)
        /// </summary>
        ///
        /// <remarks>
        /// 
        /// **Details**:
        /// Get the existences of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountExistences/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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
        ///  Returns recent credit card approval of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets any recent credit card approval of the debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetRecentApproval/1850-190058
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


        /// <summary>
        ///  Returns  Multiples account and it's balance from a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets multiples account and it's balance of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetMultiples/1850-190058
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

        /// <summary>
        ///  Returns account Info of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets account's Info of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountInfo/1850-190058
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
        /// <summary>
        ///  Returns the SIF of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets SIF of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetSIF/0001-000001
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



        /// <summary>
        ///  Returns Interactions Account Data.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetInteractionsAcctData
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
                var item = await _context.GetInteractionsAcctData(request, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns client invoice header.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client invoice header data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetClientInvoiceHeader/0001&amp;A
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
                var item = await _context.GetClientInvoiceHeader(clientAcct, company, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns Client Primary Contact.(prod)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client primary contact data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetClientPrimaryContact/0001&amp;A
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
                var item = await _context.GetClientPrimaryContact(clientAcct, company, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Can Get Client Invoice Payments.(prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Get Client Invoice Payments.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/AccountInformation/GetClientInvoicePayments
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
                var item = await _context.GetClientInvoicePayments(request, "P");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns Next Payment Info.(prod)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client's new payment info by passing parameterized clientAcct like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetNextPaymentInfo/0001-000001
        /// and it's a GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for company ex. A,
        /// Required a valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetNextPaymentInfoExample), 200)]
        [HttpGet("GetNextPaymentInfo/{clientAcct}")]
        public async Task<IActionResult> GetNextPaymentInfo(string clientAcct)
        {
            Serilog.Log.Information("GetNextPaymentInfo => GET");
            try
            {
                //P for prod.
                var item = await _context.GetNextPaymentInfo(clientAcct, "P");

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
