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
using TrainingRecommenderML.Model;

namespace TrainingRecommender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrainingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Trainings/Search
        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainings([FromBody] SearchTrainings search = null)
        {
            var id = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            if (search == null)
            {
                search = new SearchTrainings();
            }

            var trainings = _context.Training
                .Include(el => el.UserTrainings)
                .Include(el => el.Muscles).AsQueryable();
            if (search.Muscle != 0)
            {
                trainings = trainings
                    .Where(el => el.Muscles.Any(m => m.MuscleId == search.Muscle));
            }

            if (search.My)
            {
                trainings = trainings.Where(el => el.UserTrainings.Any(d => d.UserId == id));
            }

            trainings = trainings.Skip(search.Page * search.PageSize).Take(search.PageSize);
            foreach (var training in trainings)
            {
                training.UserTrainings = null;
            }

            return await trainings
                    .ToListAsync();
        }

        [HttpGet("Recommend/{id}")]
        public async Task<ActionResult<IEnumerable<Training>>> Recommend(string id)
        {
            if (!_context.UserTraining.Any(el => el.UserId == id))
            {
                return BadRequest("Неможливо побудувати рекомендації для данного користувача через відсутність його оцінок у моделі");
            }

            var trainingsToCheck = await _context.Training
                .Include(el=>el.UserTrainings)
                .Where(el=>el.UserTrainings.All(s=>s.UserId != id)
                && el.UserTrainings.Any())
                .ToListAsync();
            var user = await _context.Users
                .Include(el => el.UserDiseases)
                .FirstAsync(el=>el.Id == id);
            var result = new List<Training>();
            foreach (var training in trainingsToCheck)
            {
                var input = new ModelInput
                {
                    UserId = user.Id,
                    TrainingId = float.Parse(training.Id.ToString()),
                    Activity = float.Parse(((int)user.Activity).ToString()),
                    Age = float.Parse(user.Age.ToString()),
                    Duration = float.Parse(training.Duration.ToString()),
                    ExerciseIndex = float.Parse(TrainingCalculator.CalculateExercise(user, training).ToString()),
                    FigureType = float.Parse(((int)user.FigureType).ToString()),
                    Gender = float.Parse(((int)user.Gender).ToString()),
                    Goal = float.Parse(((int)user.Goal).ToString()),
                    Height = float.Parse(user.Height.ToString()),
                    Weight = float.Parse(user.Weight.ToString()),
                    Level = float.Parse(((int)training.Level).ToString())
                };
                ModelOutput output;
                try
                {
                    output = ConsumeModel.Predict(input);
                }
                catch (Exception ex)
                {
                    output = new ModelOutput()
                    {
                        Score = 0
                    };
                }

                if (output.Score > 3.5)
                {
                    training.UserTrainings = new[] { new UserTraining()
                    {
                        Score = (int)Math.Round(output.Score, 0)
                    } };
                    result.Add(training);
                }
            }

            return result;
        }

        // GET: api/Trainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> GetTraining(int id)
        {
            var training = await _context.Training
                .Include(el => el.Exercises)
                .Include("Muscles.Muscle")
                .FirstOrDefaultAsync(el=> el.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            return training;
        }

        // PUT: api/Trainings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(int id, Training training)
        {
            if (id != training.Id)
            {
                return BadRequest();
            }

            var musclesRelToDelete = await _context.TrainingMuscle
                .Where(el => el.TrainingId == training.Id && training.Muscles.All(a => a.Id != el.Id)).ToListAsync();
            
            _context.TrainingMuscle.RemoveRange(musclesRelToDelete);

            var musclesRelToAdd = training.Muscles.Where(el => el.Id == 0);
            _context.TrainingMuscle.AddRange(musclesRelToAdd);

            var exRelToDelete = await _context.Exercise
                .Where(el => el.TrainingId == training.Id && training.Exercises.All(a => a.Id != el.Id)).ToListAsync();

            _context.Exercise.RemoveRange(exRelToDelete);

            _context.Entry(training).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExists(id))
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

        // POST: api/Trainings
        [HttpPost]
        public async Task<ActionResult<Training>> PostTraining(Training training)
        {
            _context.Training.Add(training);
            await _context.SaveChangesAsync();

            return Ok(training);
        }

        // DELETE: api/Trainings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Training>> DeleteTraining(int id)
        {
            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }

            _context.Training.Remove(training);
            await _context.SaveChangesAsync();

            return training;
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.Id == id);
        }
    }
}