using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingRecommender.Data;
using TrainingRecommender.Helpers;
using TrainingRecommender.Models;
using TrainingRecommender.Models.DTO;

namespace TrainingRecommender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTrainingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserTrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/UserTrainings/Search
        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<UserTraining>>> GetUserTrainings([FromBody] PaginationBase pagination)
        {
            return await _context.UserTraining
                .Skip(pagination.Page * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        // GET: api/UserTrainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTraining>> GetUserTraining(int id)
        {
            var userTraining = await _context.UserTraining
                .FirstOrDefaultAsync(el => el.Id == id);

            if (userTraining == null)
            {
                return NotFound();
            }

            return userTraining;
        }

        // PUT: api/UserTrainings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTraining(int id, UserTraining userTraining)
        {
            var currentId = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentId != userTraining.UserId && !User.IsInRole("admin"))
            {
                return BadRequest();
            }
            if (id != userTraining.Id)
            {
                return BadRequest();
            }

            
            _context.Entry(userTraining).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTrainingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // перерахунок рейтингу комлексу тренувань
            var training = await _context.Training.Include(el => el.UserTrainings)
                .FirstAsync(el => el.Id == userTraining.TrainingId);
            training.Score = training.UserTrainings.Sum(el => el.Score) / training.UserTrainings.Count();

            _context.Update(training);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/UserTrainings
        [HttpPost]
        public async Task<ActionResult<UserTraining>> PostUserTraining(UserTraining userTraining)
        {
            var currentId = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentId != userTraining.UserId && !User.IsInRole("admin"))
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Include(el => el.UserDiseases)
                .FirstAsync(el => el.Id == userTraining.UserId);
            var training = await _context.Training.FindAsync(userTraining.TrainingId);

            var trainingIndex = TrainingCalculator.CalculateExercise(user, training);
            userTraining.ExerciseIndex = trainingIndex;

            _context.UserTraining.Add(userTraining);
            await _context.SaveChangesAsync();
            userTraining.Training = null;
            return Ok(userTraining);
        }

        // POST: api/UserTrainings/Calculate
        [HttpPost("Calculate")]
        public async Task<ActionResult<UserTraining>> Calculate(UserTraining userTraining)
        {
            var user = await _context.Users
                .Include(el => el.UserDiseases)
                .FirstAsync(el => el.Id == userTraining.UserId);
            var training = await _context.Training.FindAsync(userTraining.TrainingId);

            var trainingIndex = TrainingCalculator.CalculateExercise(user, training);
            userTraining.ExerciseIndex = trainingIndex;

            userTraining.Training = null;
            return Ok(userTraining);
        }

        // DELETE: api/UserTrainings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserTraining>> DeleteUserTraining(int id)
        {
            var userTraining = await _context.UserTraining.FindAsync(id);
            if (userTraining == null)
            {
                return NotFound();
            }
            if (User.Identity.Name != userTraining.UserId && !User.IsInRole("admin"))
            {
                return BadRequest();
            }

            _context.UserTraining.Remove(userTraining);
            await _context.SaveChangesAsync();

            return userTraining;
        }

        private bool UserTrainingExists(int id)
        {
            return _context.UserTraining.Any(e => e.Id == id);
        }
    }
}