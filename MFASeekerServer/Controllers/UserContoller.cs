using MFASeekerServer.Core.Entities;
using MFASeekerServer.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserContoller : ControllerBase
    {
        private readonly SeekerDbContext _context;
        public UserContoller(SeekerDbContext context)
        {
            _context = context;
        }
        // GET: AllUsers
        [HttpGet("AllUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }
        // POST: UserContoller/Create
        [HttpPost("AddUser")]
        public ActionResult Create(User newUser)
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok($"User was added: {newUser.Id}, {newUser.UserName}");
            }
            catch
            {
                return BadRequest("Bad try :(");
            }
        }
    }
}
