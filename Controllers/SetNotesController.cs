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
    public class SetNotesController : ControllerBase
    {
        private readonly IAddNotes _context;

        public SetNotesController(IAddNotes context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> SetNotes(NoteMaster notesData)
        {
            Serilog.Log.Information("  SetNotes => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _context.CreateNotes(notesData);

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException,e.Message,e.Data);
                throw;
            }
           

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


    }
}
