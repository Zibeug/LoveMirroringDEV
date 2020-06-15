using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Policy = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class BotCommandsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public BotCommandsController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/BotCommands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BotCommand>>> GetBotCommands()
        {
            return await _context.BotCommands.ToListAsync();
        }

        // GET: api/BotCommands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BotCommand>> GetBotCommand(short id)
        {
            var botCommand = await _context.BotCommands.FindAsync(id);

            if (botCommand == null)
            {
                return NotFound();
            }

            return botCommand;
        }

        // PUT: api/BotCommands/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBotCommand(short id, BotCommand botCommand)
        {
            if (id != botCommand.Id)
            {
                return BadRequest();
            }

            _context.Entry(botCommand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BotCommandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BotCommands
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BotCommand>> PostBotCommand(BotCommand botCommand)
        {
            _context.BotCommands.Add(botCommand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBotCommand", new { id = botCommand.Id }, botCommand);
        }

        // DELETE: api/BotCommands/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BotCommand>> DeleteBotCommand(short id)
        {
            var botCommand = await _context.BotCommands.FindAsync(id);
            if (botCommand == null)
            {
                return NotFound();
            }

            _context.BotCommands.Remove(botCommand);
            await _context.SaveChangesAsync();

            return botCommand;
        }

        private bool BotCommandExists(short id)
        {
            return _context.BotCommands.Any(e => e.Id == id);
        }
    }
}
