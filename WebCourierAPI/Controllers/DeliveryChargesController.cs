﻿using System;
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
    //[EnableCors("Policy1")]
    //[AuthAttribute("", "ParcelTypes")]
    //[Route("api/[controller]")]
    //[ApiController]
    //public class DeliveryChargesController : ControllerBase
    //{


    //    //public DeliveryChargesController(WebCorierApiContext context)
    //    //{
    //    //    _context = context;
    //    //}

    //    // GET: api/DeliveryCharges
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<DeliveryCharge>>> GetDeliveryCharges()
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        //return await _context.DeliveryCharges.ToListAsync();
    //        return await _context.DeliveryCharges.Include(dc => dc.ParcelType).Include(dc => dc.Parcels).ToListAsync();
    //    }

    //    // GET: api/DeliveryCharges/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<DeliveryCharge>> GetDeliveryCharge(int id)
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        var deliveryCharge = await _context.DeliveryCharges.FindAsync(id);

    //        if (deliveryCharge == null)
    //        {
    //            return NotFound();
    //        }

    //        return deliveryCharge;
    //    }

    //    // PUT: api/DeliveryCharges/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutDeliveryCharge(int id, DeliveryCharge deliveryCharge)
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        if (id != deliveryCharge.DeliveryChargeId)
    //        {
    //            return BadRequest();
    //        }

    //        var existingDeliveryCharge = await _context.DeliveryCharges.FindAsync(id);
    //        if (existingDeliveryCharge == null)
    //        {
    //            return NotFound("Company not found.");
    //        }
    //        var token = Request.Headers["Token"].FirstOrDefault();
    //        var user = AuthenticationHelper.ValidateToken(token);

    //        if (user == null)
    //        {
    //            return Unauthorized("Invalid or expired token.");
    //        }
    //        //existingDeliveryCharge.del = parcelType.ParcelTypeName;
    //        existingDeliveryCharge.CreateBy = user.UserName;
    //        existingDeliveryCharge.CreateDate = DateTime.UtcNow;

    //        //existingParcelType.ParcelTypeName = parcelType.ParcelTypeName;
    //        //existingParcelType.UpdateBy = parcelType.UpdateBy;
    //        //existingParcelType.UpdateDate = DateTime.UtcNow;
    //        existingDeliveryCharge.IsActive = deliveryCharge.IsActive;

    //        _context.Entry(deliveryCharge).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!DeliveryChargeExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/DeliveryCharges
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<DeliveryCharge>> PostDeliveryCharge([FromBody]DeliveryCharge deliveryCharge)
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest("DeliveryCharge type name is required.");
    //        }
    //        var token = Request.Headers["Token"].FirstOrDefault();
    //        var user = AuthenticationHelper.ValidateToken(token);

    //        if (user == null)
    //        {
    //            return Unauthorized("Invalid or expired token.");
    //        }

    //        deliveryCharge.CreateBy = user.UserName;
    //        deliveryCharge.CreateDate = DateTime.UtcNow;


    //        _context.DeliveryCharges.Add(deliveryCharge);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetDeliveryCharge", new { id = deliveryCharge.DeliveryChargeId }, deliveryCharge);
    //    }

    //    // DELETE: api/DeliveryCharges/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteDeliveryCharge(int id)
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        var deliveryCharge = await _context.DeliveryCharges.FindAsync(id);
    //        if (deliveryCharge == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.DeliveryCharges.Remove(deliveryCharge);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool DeliveryChargeExists(int id)
    //    {
    //        WebCorierApiContext _context = new WebCorierApiContext();
    //        return _context.DeliveryCharges.Any(e => e.DeliveryChargeId == id);
    //    }
    //}
    [EnableCors("Policy1")]
    [AuthAttribute("", "ParcelTypes")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryChargesController : ControllerBase
    {
       

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryCharge>>> GetDeliveryCharges()
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return await _context.DeliveryCharges.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryCharge>> GetDeliveryCharge(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            var deliveryCharge = await _context.DeliveryCharges.FindAsync(id);
            if (deliveryCharge == null)
            {
                return NotFound("Delivery charge not found.");
            }

            return deliveryCharge;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryCharge(int id, DeliveryCharge deliveryCharge)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            if (id != deliveryCharge.DeliveryChargeId)
            {
                return BadRequest();
            }

            var deliveryChargeToUpdate = await _context.DeliveryCharges.FindAsync(id);
            if (deliveryChargeToUpdate == null)
            {
                return NotFound("Delivery charge not found.");
            }

            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);
   
           
            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            deliveryChargeToUpdate.IsActive = deliveryCharge.IsActive;
            deliveryChargeToUpdate.CreateBy = user.UserName;
            deliveryChargeToUpdate.CreateDate = DateTime.UtcNow;

            _context.Entry(deliveryChargeToUpdate).State = EntityState.Modified;

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
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryCharge>> PostDeliveryCharge([FromBody] DeliveryCharge deliveryCharge)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            deliveryCharge.CreateBy = user.UserName;
            deliveryCharge.CreateDate = DateTime.UtcNow;

            _context.DeliveryCharges.Add(deliveryCharge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryCharge", new { id = deliveryCharge.DeliveryChargeId }, deliveryCharge);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryCharge(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

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
            WebCorierApiContext _context = new WebCorierApiContext();

            return _context.DeliveryCharges.Any(e => e.DeliveryChargeId == id);
        }
    }
}
