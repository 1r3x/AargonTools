using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Models;
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
        private readonly ISetMoveToQueue _setMoveToQueue;
        private readonly ISetInteractResults _setInteractionResults;
        private readonly IAddNotesV2 _contextAddNotesV2;

        public SetAccountsDetailsController(IAddBadNumbers contextBadNumbers, ISetMoveAccount contextSetMoveAccount, IAddNotes contextAddNotes
            , ISetDoNotCall setDoNotCall, ISetNumber setNumber, ISetMoveToHouse setMoveToHouse, ISetMoveToDispute setMoveToDispute, ISetPostDateChecks setPostDateChecks
            , ISetMoveToQueue setMoveToQueue, ISetInteractResults setInteractionResults, IAddNotesV2 contextAddNotesV2)
        {
            _contextBadNumbers = contextBadNumbers;
            _contextSetMoveAccount = contextSetMoveAccount;
            _contextAddNotes = contextAddNotes;
            _setDoNotCall = setDoNotCall;
            _setNumber = setNumber;
            _setMoveToHouse = setMoveToHouse;
            _setMoveToDispute = setMoveToDispute;
            _setPostDateChecks = setPostDateChecks;
            _setMoveToQueue = setMoveToQueue;
            _setInteractionResults = setInteractionResults;
            _contextAddNotesV2 = contextAddNotesV2;
        }



        /// <summary>
        ///  This endpoint can set a bad number(prod_old environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a bad number , and the bad number will remove from debtor account and add a notes about the action.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetBadNumbers/0001-000001&amp;7025052773
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
            Serilog.Log.Information("prod_old SetBadNumbers => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextBadNumbers.AddBadNumbers(debtorAcct, phoneNo,"PO");

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
        ///  This endpoint can move debtor account to a queue(prod_old environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can move debtor account to a queue if the queue is available and the account is active
        /// then add a notes about the action and make a log to a specific log table.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetMoveAccount/0001-000001&amp;70
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveAccountResponse), 200)]
        [HttpPost("SetMoveAccount/{debtorAcct}&{toQueue}")]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("prod_old SetMoveAccount => POST");
            try
            {
                var data = await _contextSetMoveAccount.SetMoveAccount(debtorAcct, toQueue,"PO");
                return Ok(data);
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }

        }




        /// <summary>
        ///  This endpoint can add a note (prod_old environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a note to a specific debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetNotes/0001-000001&amp;This is the notes
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetAddNotesResponse), 200)]

        [HttpPost("SetNotes/{debtorAcct}&{notes}")]
        public async Task<IActionResult> SetNotes(string debtorAcct, string notes)
        {
            Serilog.Log.Information("prod_old SetNotes => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotes.CreateNotes(debtorAcct, notes,"PO");

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
        ///  This endpoint can set a phone number's status [don't call].(prod_old environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can can set a phone number's status [don't call].
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetDoNotCall/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetDoNotCallResponse), 200)]
        [HttpPost("SetDoNotCall/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetDoNotCall(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information(" prod_old SetDoNotCall => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setDoNotCall.SetDoNotCallManager(debtorAcct, cellPhoneNo, "PO");

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
        ///  This endpoint can set a phone number for a debtor(prod_old Environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set a phone number for a debtor.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetNumber/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetNumberResponses), 200)]
        [HttpPost("SetNumber/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetNumber(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("prod_old  SetNumber => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setNumber.SetNumber(debtorAcct, cellPhoneNo, "PO");

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
        ///  This endpoint can set move to house.(prod_old Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to house and make a movement log for a debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetMoveToHouse/0001-000001
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToHouseResponses), 200)]
        [HttpPost("SetMoveToHouse/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToHouse(string debtorAcct)
        {
            Serilog.Log.Information(" prod_old SetMoveToHouse => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToHouse.SetMoveToHouse(debtorAcct, "PO");

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
        ///  This endpoint can set move to dispute.(prod_old Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to dispute and make a movement log for a debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetMoveToDispute/0001-000001
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToDisputeResponses), 200)]
        [HttpPost("SetMoveToDispute/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToDispute(string debtorAcct, decimal amountDisputed)
        {
            Serilog.Log.Information("prod_old SetMoveToDispute => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToDispute.SetMoveToDispute(debtorAcct, amountDisputed, "PO");

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
        ///  This endpoint can set post date checks and take necessary actions.(prod_old Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set post date checks and take necessary actions.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetPostDateChecks/0001-000001&amp;12-20-2021&amp;10&amp;102&amp;9&amp;10&amp;Y
        /// (pass all the parameters separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetPostDateChecksResponse), 200)]
        [HttpPost("SetPostDateChecks")]
        public async Task<IActionResult> SetPostDateChecks([FromBody] SetPostDateChecksRequestModel requestModel)
        {
            Serilog.Log.Information("prod_old SetPostDateChecks => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setPostDateChecks.SetPostDateChecks(requestModel, "PO");

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
        ///  This endpoint can set move to queue.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to house and make a movement log for a debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/SetAccountsDetails/SetMoveToQueue/0001-000001&amp;'TYPE'
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToQueueResponse), 200)]
        [HttpPost("SetMoveToQueue/{debtorAcct}&{type}")]
        public async Task<IActionResult> SetMoveToQueue(string debtorAcct, string type)
        {
            Serilog.Log.Information("prod_old SetMoveToQueue => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToQueue.SetMoveToQueue(debtorAcct, type, "PO");

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
        ///  This endpoint can set Interaction Results.(prod_old)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set set Interaction Results.
        /// And please don't forget about a valid token.
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(SetInteractionResultsResponse), 200)]
        [HttpPost]
        [Route("SetInteractionResults")]
        
        public async Task<IActionResult> SetInteractionResults([FromBody] InteractResult interactResultModel)
        {
            Serilog.Log.Information("SetInteractionResults => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setInteractionResults.SetInteractResults(interactResultModel, "PO");

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
        ///  This endpoint can add a note (JSON body request prod_old).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a note to a specific debtor account.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNotesV2
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [HttpPost("SetNotesV2")]
        public async Task<IActionResult> SetNotesV2([FromBody] AddNotesRequestModel request)
        {
            Serilog.Log.Information("prod_old  SetNotesV2 => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotesV2.CreateNotes(request, "PO");

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
