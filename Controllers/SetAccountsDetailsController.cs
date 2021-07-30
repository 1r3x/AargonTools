using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetAccountsDetailsController : ControllerBase
    {
        private readonly IAddBadNumbers _contextBadNumbers;
        private readonly ISetMoveAccount _contextSetMoveAccount;
        private readonly IAddNotes _contextAddNotes;
        private readonly ISetDoNotCall _setDoNotCall;
        private readonly ISetNumber _setNumber;
        private readonly ISetMoveToHouse _setMoveToHouse;
        private readonly ISetMoveToDispute _setMoveToDispute;
        private readonly ISetPostDateChecks _setPostDateChecks;

        public SetAccountsDetailsController(IAddBadNumbers contextBadNumbers, ISetMoveAccount contextSetMoveAccount, IAddNotes contextAddNotes
        ,ISetDoNotCall setDoNotCall, ISetNumber setNumber, ISetMoveToHouse setMoveToHouse, ISetMoveToDispute setMoveToDispute, ISetPostDateChecks setPostDateChecks)
        {
            _contextBadNumbers = contextBadNumbers;
            _contextSetMoveAccount = contextSetMoveAccount;
            _contextAddNotes = contextAddNotes;
            _setDoNotCall = setDoNotCall;
            _setNumber = setNumber;
            _setMoveToHouse = setMoveToHouse;
            _setMoveToDispute = setMoveToDispute;
            _setPostDateChecks = setPostDateChecks;
        }

        /// <summary>
        ///  This endpoint can set a bad number.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a bad number , and the bad number will remove from debtor account and add a notes about the action.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetBadNumbers/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(AddBadNumberResponse), 200)]
        [HttpPost("SetBadNumbers/{debtorAcct}&{phoneNo}")]
        public async Task<IActionResult> SetBadNumbers(string debtorAcct, string phoneNo)
        {
            Serilog.Log.Information("  SetBadNumbers => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextBadNumbers.AddBadNumbers(debtorAcct, phoneNo,"P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        /// <summary>
        ///  This endpoint can move debtor account to a queue.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can move debtor account to a queue if the queue is available and the account is active
        /// then add a notes about the action and make a log to a specific log table.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveAccount/0001-000001&amp;70
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveAccountResponse), 200)]
        [HttpPost("SetMoveAccount/{debtorAcct}&{toQueue}")]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("  SetMoveAccount => POST");
            try
            {
                var data = await _contextSetMoveAccount.SetMoveAccount(debtorAcct, toQueue,"P");
                return Ok(data);
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }

        }

        /// <summary>
        ///  This endpoint can add a note.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a note to a specific debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNotes/0001-000001&amp;This is the notes
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetAddNotesResponse), 200)]
        [HttpPost("SetNotes/{debtorAcct}&{notes}")]
        public async Task<IActionResult> SetNotes(string debtorAcct, string notes)
        {
            Serilog.Log.Information("  SetNotes => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotes.CreateNotes(debtorAcct, notes,"P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        /// <summary>
        ///  This endpoint can set a phone number's status [don't call].
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can can set a phone number's status [don't call].
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetDoNotCall/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetDoNotCallResponse), 200)]
        [HttpPost("SetDoNotCall/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetDoNotCall(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("  SetDoNotCall => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setDoNotCall.SetDoNotCallManager(debtorAcct, cellPhoneNo, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  This endpoint can set a phone number for a debtor.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set a phone number for a debtor.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNumber/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetNumberResponses), 200)]
        [HttpPost("SetNumber/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetNumber(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("  SetNumber => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setNumber.SetNumber(debtorAcct, cellPhoneNo, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  This endpoint can set move to house.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to house and make a movement log for a debtor acccount.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveToHouse/0001-000001
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToHouseResponses), 200)]
        [HttpPost("SetMoveToHouse/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToHouse(string debtorAcct)
        {
            Serilog.Log.Information("  SetMoveToHouse => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToHouse.SetMoveToHouse(debtorAcct, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  This endpoint can set move to dispute.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to dispute and make a movement log for a debtor acccount.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveToDispute/0001-000001
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToDisputeResponses), 200)]
        [HttpPost("SetMoveToDispute/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToDispute(string debtorAcct,decimal amountDisputed)
        {
            Serilog.Log.Information("SetMoveToDispute => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToDispute.SetMoveToDispute(debtorAcct, amountDisputed, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  This endpoint can set post date checks and take necessary actions.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set post date checks and take necessary actions.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetPostDateChecks/0001-000001&amp;12-20-2021&amp;10&amp;102&amp;9&amp;10&amp;Y
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetPostDateChecksResponse), 200)]
        [HttpPost("SetPostDateChecks/{debtorAcct}&{postDate}&{amount}&{accountNumber}&{routingNumber}&{totalPd}&{sif}")]
        public async Task<IActionResult> SetPostDateChecks(string debtorAcct, DateTime postDate, decimal amount, string accountNumber, string routingNumber,
        int totalPd, char sif)
        {
            Serilog.Log.Information("SetPostDateChecks => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setPostDateChecks.SetPostDateChecks(debtorAcct, postDate, amount, accountNumber,
                        routingNumber, totalPd, sif, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


    }
}
