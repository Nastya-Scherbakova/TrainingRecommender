using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingRecommender.Data;
using TrainingRecommender.Models;
using TrainingRecommender.Models.DTO;

namespace TrainingRecommender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MusclesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Muscles/Search
        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<Muscle>>> GetMuscles([FromBody] PaginationBase pagination)
        {
            return await _context.Muscle
                .Skip(pagination.Page * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        // GET: api/Muscles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Muscle>> GetMuscle(int id)
        {
            var muscle = await _context.Muscle
                .FirstOrDefaultAsync(el => el.Id == id);

            if (muscle == null)
            {
                return NotFound();
            }

            return muscle;
        }

        // PUT: api/Muscles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMuscle(int id, Muscle muscle)
        {
            if (id != muscle.Id)
            {
                return BadRequest();
            }

            _context.Entry(muscle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MuscleExists(id))
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

        // POST: api/Muscles
        [HttpPost]
        public async Task<ActionResult<Muscle>> PostMuscle(Muscle muscle)
        {
            _context.Muscle.Add(muscle);
            await _context.SaveChangesAsync();

            return Ok(muscle);
        }

        // DELETE: api/Muscles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Muscle>> DeleteMuscle(int id)
        {
            var muscle = await _context.Muscle.FindAsync(id);
            if (muscle == null)
            {
                return NotFound();
            }

            _context.Muscle.Remove(muscle);
            await _context.SaveChangesAsync();

            return muscle;
        }

        private bool MuscleExists(int id)
        {
            return _context.Muscle.Any(e => e.Id == id);
        }
    }
}