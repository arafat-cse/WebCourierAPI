using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCourierAPI.Attributes;
using WebCourierAPI.Models;

namespace WebCourierAPI.Controllers
{
    [EnableCors("Policy1")]
    [AuthAttribute("", "Companies")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiversController : ControllerBase
    {
        private readonly WebCorierApiContext _context;

        public ReceiversController(WebCorierApiContext context)
        {
            _context = context;
        }

        // GET: api/Receivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receiver>>> GetReceivers()
        {
            return await _context.Receivers.ToListAsync();
        }

        // GET: api/Receivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receiver>> GetReceiver(int id)
        {
            var receiver = await _context.Receivers.FindAsync(id);

            if (receiver == null)
            {
                return NotFound();
            }

            return receiver;
        }

        // PUT: api/Receivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceiver(int id, Receiver receiver)
        {
            if (id != receiver.ReceiverId)
            {
                return BadRequest();
            }

            _context.Entry(receiver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiverExists(id))
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

        // POST: api/Receivers
        [HttpPost]
        public async Task<ActionResult<Receiver>> PostReceiver(Receiver receiver)
        {
            _context.Receivers.Add(receiver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceiver", new { id = receiver.ReceiverId }, receiver);
        }

        // DELETE: api/Receivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceiver(int id)
        {
            var receiver = await _context.Receivers.FindAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }

            _context.Receivers.Remove(receiver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceiverExists(int id)
        {
            return _context.Receivers.Any(e => e.ReceiverId == id);
        }
    }
}
