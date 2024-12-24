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
{[EnableCors("Policy1")]
[AuthAttribute("", "Companies")]
[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    //private readonly CompanyDbContext _context;

    //public Companies1Controller(CompanyDbContext context)
    //{
    //    _context = context;
    //}

    // GET: api/Companies
    //[EnableCors("Policy1")]
    [HttpGet]
    //[AuthAttribute("GetCompanies", "Companies")]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        WebCorierApiContext _context = new WebCorierApiContext();
        return await _context.Companys.ToListAsync();
    }

    // GET: api/Companies/5
    //[EnableCors("Policy1")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        WebCorierApiContext _context = new WebCorierApiContext();
        var company = await _context.Companys.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }

    // PUT: api/Companies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany(int id, Company company)
    {
        WebCorierApiContext _context = new WebCorierApiContext();
        if (id != company.CompanyId)
        {
            return BadRequest();
        }

        _context.Entry(company).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(Company company)
        //{
        //    WebCorierApiContext _context = new WebCorierApiContext();
        //    _context.Companys.Add(company);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        //}
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            Console.WriteLine($"Received Company Name: {company.CompanyName}");
            Console.WriteLine($"Created By: {company.CreateBy}, Created Date: {company.CreateDate}");

            _context.Companys.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }


        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        WebCorierApiContext _context = new WebCorierApiContext();
        var company = await _context.Companys.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        _context.Companys.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CompanyExists(int id)
    {
        WebCorierApiContext _context = new WebCorierApiContext();
        return _context.Companys.Any(e => e.CompanyId == id);
    }
}

}
