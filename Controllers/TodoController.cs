using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AargonTools.Data;
using AargonTools.Models;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")] // api/todo
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            Serilog.Log.Information("  Todo => GET");
            try
            {
                var items = await _context.Items.ToListAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemData data)
        {
            Serilog.Log.Information("  Todo => POST");
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, data);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            Serilog.Log.Information("  Todo => POST");
            try
            {
                var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemData item)
        {
            if (id != item.Id)
                return BadRequest();

            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            existItem.Title = item.Title;
            existItem.Description = item.Description;
            existItem.Done = item.Done;

            // Implement the changes on the database level
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }
    }
}