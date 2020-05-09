using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using TrainingRecommender.Data;
using TrainingRecommender.Models;
using TrainingRecommender.Models.DTO;

namespace TrainingRecommender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiseasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiseasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Diseases/Search
        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<Disease>>> GetDiseases([FromBody] PaginationBase pagination)
        {
            return await _context.Disease
                .Skip(pagination.Page * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        // GET: api/Diseases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Disease>> GetDisease(int id)
        {
            var disease = await _context.Disease
                .FirstOrDefaultAsync(el => el.Id == id);

            if (disease == null)
            {
                return NotFound();
            }

            return disease;
        }

        // PUT: api/Diseases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisease(int id, Disease disease)
        {
            if (id != disease.Id)
            {
                return BadRequest();
            }

            _context.Entry(disease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiseaseExists(id))
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

        // POST: api/Diseases
        [HttpPost]
        public async Task<ActionResult<Disease>> PostDisease(Disease disease)
        {
            _context.Disease.Add(disease);
            await _context.SaveChangesAsync();

            return Ok(disease);
        }

        // DELETE: api/Diseases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Disease>> DeleteDisease(int id)
        {
            var disease = await _context.Disease.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }

            _context.Disease.Remove(disease);
            await _context.SaveChangesAsync();

            return disease;
        }

        private bool DiseaseExists(int id)
        {
            return _context.Disease.Any(e => e.Id == id);
        }
    }
}