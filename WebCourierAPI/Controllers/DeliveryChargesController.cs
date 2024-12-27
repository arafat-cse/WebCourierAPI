using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCourierAPI.Models;

namespace WebCourierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryChargesController : ControllerBase
    {
        WebCorierApiContext _context = new WebCorierApiContext();

        //public DeliveryChargesController(WebCorierApiContext context)
        //{
        //    _context = context;
        //}

        // GET: api/DeliveryCharges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryCharge>>> GetDeliveryCharges()
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            //return await _context.DeliveryCharges.ToListAsync();
            return await _context.DeliveryCharges.Include(dc => dc.ParcelType).Include(dc => dc.Parcels).ToListAsync();
        }

        // GET: api/DeliveryCharges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryCharge>> GetDeliveryCharge(int id)
        {
            var deliveryCharge = await _context.DeliveryCharges.FindAsync(id);

            if (deliveryCharge == null)
            {
                return NotFound();
            }

            return deliveryCharge;
        }

        // PUT: api/DeliveryCharges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryCharge(int id, DeliveryCharge deliveryCharge)
        {
            if (id != deliveryCharge.DeliveryChargeId)
            {
                return BadRequest();
            }

            _context.Entry(deliveryCharge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryChargeExists(id))
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

        // POST: api/DeliveryCharges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryCharge>> PostDeliveryCharge(DeliveryCharge deliveryCharge)
        {
            _context.DeliveryCharges.Add(deliveryCharge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryCharge", new { id = deliveryCharge.DeliveryChargeId }, deliveryCharge);
        }

        // DELETE: api/DeliveryCharges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryCharge(int id)
        {
            var deliveryCharge = await _context.DeliveryCharges.FindAsync(id);
            if (deliveryCharge == null)
            {
                return NotFound();
            }

            _context.DeliveryCharges.Remove(deliveryCharge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryChargeExists(int id)
        {
            return _context.DeliveryCharges.Any(e => e.DeliveryChargeId == id);
        }
    }
}
