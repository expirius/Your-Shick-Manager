using MFASeekerServer.Core.Entities;
using MFASeekerServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToiletController : ControllerBase
    {
        //private readonly IToiletService _toiletService;
        private readonly SeekerDbContext _context;
        ToiletController(SeekerDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("AllToilets")]
        public async Task<ActionResult<List<Toilet>>> GetToilets()
        {
            return Ok(await _context.Toilets.ToListAsync());
        }
        // В будущем сделать через слои приложения
        //public async Task<IActionResult> AddToilet([FromBody] ToiletDto toiletDto)
        //var createdToilet = await _toiletService.AddToiletAsync(toiletDto);

        // Временное решение
        [HttpPost]
        public async Task<ActionResult<Toilet>> AddToilet(Toilet createdToilet)
        {
            _context.Toilets.Add(createdToilet);
            await _context.SaveChangesAsync();

            return Ok(await _context.Toilets.ToListAsync());
        }

    }
}
