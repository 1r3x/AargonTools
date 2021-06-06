using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetMoveAccountController : ControllerBase
    {
        private readonly ISetMoveAccount _context;

        public SetMoveAccountController(ISetMoveAccount context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("  SetMoveAccount => POST");
            try
            {
                var data = await _context.SetMoveAccount(debtorAcct, toQueue);
                return Ok(data);
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }

        }
    }
}
