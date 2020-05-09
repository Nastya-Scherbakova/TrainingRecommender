using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // POST: api/Users/Search
        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromBody] PaginationBase pagination)
        {
            return Ok(_mapper.Map<IEnumerable<User>>(await _context.Users
                .Include(a=>a.UserDiseases)
                .Skip(pagination.Page * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync()));
        }

        // GET: api/Users/Current
        [HttpGet("Current")]
        public async Task<ActionResult<User>> Current()
        {
            var id = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Include(el => el.UserTrainings)
                .Include(el => el.UserDiseases)
                .FirstOrDefaultAsync(el => el.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<User>(user);
            var roleIds = await _context.UserRoles.Where(el => el.UserId == id).Select(el=>el.RoleId).ToListAsync();
            mappedUser.Roles = await _context.Roles.Where(el => roleIds.Contains(el.Id)).Select(el => el.Name).ToListAsync();
            return mappedUser;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(el => el.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<User>(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            var currentId = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            if (id != user.Id || currentId != user.Id && !User.IsInRole("admin"))
            {
                return BadRequest();
            }

            var dbUser = await _context.Users.FindAsync(id);
            _mapper.Map(user, dbUser);
            var idsToSave = user.UserDiseases.Select(a => a.Id);
            var diseasesRelToDelete = await _context.UserDisease
                .Where(el => el.UserId == user.Id && idsToSave.Contains(el.Id)).ToListAsync();
            
            _context.UserDisease.RemoveRange(diseasesRelToDelete);

            var diseasesRelToAdd = user.UserDiseases.Where(el => el.Id == 0);
            _context.UserDisease.AddRange(diseasesRelToAdd);
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    await _userManager.AddToRoleAsync(dbUser, role);
                }
            }
            
            _context.Entry(dbUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostUser(User user)
        {
            var dbUser = _mapper.Map<ApplicationUser>(user);
            dbUser.UserName = user.Email;
            await _userManager.CreateAsync(dbUser, $"{user.Email}-12345");
            await _context.SaveChangesAsync();
            foreach (var role in user.Roles)
            {
                await _userManager.AddToRoleAsync(dbUser, role);
            }
            return Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var currentId = User.Claims.FirstOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentId != id && !User.IsInRole("admin"))
            {
                return BadRequest();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<User>(user);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}