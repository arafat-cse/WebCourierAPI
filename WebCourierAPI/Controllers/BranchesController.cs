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
    public class BranchesController : ControllerBase
    {
        WebCorierApiContext _db = new WebCorierApiContext();

     

        //    // GET: api/Branches
        //    [HttpGet]
        //    public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        //    {
        //        return await _context.Branches.ToListAsync();
        //    }

        //    // GET: api/Branches/5
        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<Branch>> GetBranch(int id)
        //    {
        //        var branch = await _context.Branches.FindAsync(id);

        //        if (branch == null)
        //        {
        //            return NotFound();
        //        }

        //        return branch;
        //    }

        //    // PUT: api/Branches/5
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutBranch(int id, Branch branch)
        //    {
        //        if (id != branch.BranchId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(branch).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BranchExists(id))
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

        //    // POST: api/Branches
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPost]
        //    public async Task<ActionResult<Branch>> PostBranch(Branch branch)
        //    {
        //        _context.Branches.Add(branch);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetBranch", new { id = branch.BranchId }, branch);
        //    }

        //    // DELETE: api/Branches/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteBranch(int id)
        //    {
        //        var branch = await _context.Branches.FindAsync(id);
        //        if (branch == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Branches.Remove(branch);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //    private bool BranchExists(int id)
        //    {
        //        return _context.Branches.Any(e => e.BranchId == id);
        //    }
        //}
        //CommanResponse
        private readonly CommanResponse cp = new CommanResponse();

        // GET:
        [HttpGet]
        public IActionResult GetBranches()
        {
            try
            {
                var branches = _db.Branches
                    .Include(b => b.InverseParent)
                    .ToList();

                var branchDTOs = branches.Select(b => new BranchDTO
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    Address = b.Address,
                    ParentId = b.ParentId,
                    IsActive = b.IsActive,
                    ChildBranches = b.InverseParent?.Select(cb => new BranchDTO
                    {
                        BranchId = cb.BranchId,
                        BranchName = cb.BranchName,
                        Address = cb.Address,
                        ParentId = cb.ParentId,
                        IsActive = cb.IsActive
                    }).ToList()
                }).ToList();

                if (!branchDTOs.Any())
                {
                    cp.status = false;
                    cp.message = "No branches found.";
                    cp.content = null;
                    return Ok(cp);
                }

                cp.status = true;
                cp.message = "Branches retrieved successfully.";
                cp.content = branchDTOs;
                return Ok(cp);
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while retrieving branches.";
                cp.errorMessage = ex.Message;
                cp.content = null;
                return StatusCode(500, cp);
            }
        }

        // GET:/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranch(int id)
        {
            try
            {
                var branch = await _db.Branches
                    .Include(b => b.InverseParent)
                    .FirstOrDefaultAsync(b => b.BranchId == id);

                if (branch == null)
                {
                    cp.status = false;
                    cp.message = "Branch not found.";
                    cp.content = null;
                    return NotFound(cp);
                }

                var branchDto = new BranchDTO
                {
                    BranchId = branch.BranchId,
                    BranchName = branch.BranchName,
                    Address = branch.Address,
                    ParentId = branch.ParentId,
                    IsActive = branch.IsActive,
                    ChildBranches = branch.InverseParent?.Select(cb => new BranchDTO
                    {
                        BranchId = cb.BranchId,
                        BranchName = cb.BranchName,
                        Address = cb.Address,
                        ParentId = cb.ParentId,
                        IsActive = cb.IsActive
                    }).ToList()
                };

                cp.status = true;
                cp.message = "Branch retrieved successfully.";
                cp.content = branchDto;
                return Ok(cp);
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while retrieving the branch.";
                cp.errorMessage = ex.Message;
                cp.content = null;
                return StatusCode(500, cp);
            }
        }

        // POST:
        [HttpPost]
        public async Task<IActionResult> PostBranch(Branch branch)
        {
            WebCorierApiContext _db = new WebCorierApiContext();
            try
            {
                if (branch.ParentId.HasValue)
                {
                    var parentBranch = await _db.Branches.FindAsync(branch.ParentId.Value);
                    if (parentBranch == null)
                    {
                        cp.status = false;
                        cp.message = "Invalid ParentId.";
                        return BadRequest(cp);
                    }
                }

                _db.Branches.Add(branch);
                await _db.SaveChangesAsync();

                cp.status = true;
                cp.message = "Branch created successfully.";
                cp.content = branch;
                return CreatedAtAction(nameof(GetBranch), new { id = branch.BranchId }, cp);
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while creating the branch.";
                cp.errorMessage = ex.Message;
                cp.content = null;
                return StatusCode(500, cp);
            }
        }

        // PUT:/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, Branch branch)
        {
            if (id != branch.BranchId)
            {
                cp.status = false;
                cp.message = "Branch ID mismatch.";
                return BadRequest(cp);
            }

            try
            {
                if (branch.ParentId.HasValue && branch.ParentId != id)
                {
                    var parentBranch = await _db.Branches.FindAsync(branch.ParentId.Value);
                    if (parentBranch == null)
                    {
                        cp.status = false;
                        cp.message = "Invalid ParentId.";
                        return BadRequest(cp);
                    }
                }

                _db.Entry(branch).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                cp.status = true;
                cp.message = "Branch updated successfully.";
                return Ok(cp);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
                {
                    cp.status = false;
                    cp.message = "Branch not found.";
                    return NotFound(cp);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while updating the branch.";
                cp.errorMessage = ex.Message;
                return StatusCode(500, cp);
            }
        }

        // DELETE:/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var branch = await _db.Branches
                    .Include(b => b.InverseParent)
                    .FirstOrDefaultAsync(b => b.BranchId == id);

                if (branch == null)
                {
                    cp.status = false;
                    cp.message = "Branch not found.";
                    return NotFound(cp);
                }

                if (branch.InverseParent != null && branch.InverseParent.Any())
                {
                    cp.status = false;
                    cp.message = "Cannot delete a branch that has child branches.";
                    return BadRequest(cp);
                }

                _db.Branches.Remove(branch);
                await _db.SaveChangesAsync();

                cp.status = true;
                cp.message = "Branch deleted successfully.";
                return Ok(cp);
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while deleting the branch.";
                cp.errorMessage = ex.Message;
                return StatusCode(500, cp);
            }
        }

        private bool BranchExists(int id)
        {
            return _db.Branches.Any(e => e.BranchId == id);
        }
    }
}
