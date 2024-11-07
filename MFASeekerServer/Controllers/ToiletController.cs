using MFASeekerServer.Core.Entities;
using MFASeekerServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToiletController(SeekerDbContext dbContext) : ControllerBase
    {
        //private readonly IToiletService _toiletService;
        private readonly SeekerDbContext _context = dbContext;

        [HttpGet("AllToilets")]
        public async Task<ActionResult<List<Toilet>>> GetToilets()
        {
            return Ok(await _context.Toilets.ToListAsync());
        }
        // В будущем сделать через слои приложения
        //public async Task<IActionResult> AddToilet([FromBody] ToiletDto toiletDto)
        //var createdToilet = await _toiletService.AddToiletAsync(toiletDto);

        // Временное решение
        [HttpPost("Toilet")]
        public async Task<ActionResult<int>> AddToilet(Toilet createdToilet)
        {
            _context.Toilets.Add(createdToilet);
            await _context.SaveChangesAsync();

            return Ok(createdToilet.Id);
        }
        [HttpPost("Image")]
        public async Task<ActionResult<int>> AddImage(ImageFile image)
        {
            await _context.ImageFiles.AddAsync(image);

            if(image.Id != 0)
            await _context.SaveChangesAsync();
            return Ok(image.Id);
        }
        [HttpPost("UserImageToilet")]
        public async Task<ActionResult<int>> AddUserImageToilet(UserImageToilet userImageToiletDto)
        {
            // Проверка существования связанных сущностей
            var user = await _context.Users.FindAsync(userImageToiletDto.UserID);
            if (user == null) return BadRequest("User not found.");

            var image = await _context.ImageFiles.FindAsync(userImageToiletDto.ImageID);
            if (image == null) return BadRequest("Image not found.");

            var toilet = await _context.Toilets.FindAsync(userImageToiletDto.ToiletID);
            if (toilet == null) return BadRequest("Toilet not found.");

            // Создание объекта UserImageToilet
            var userImageToilet = new UserImageToilet
            {
                UserID = userImageToiletDto.UserID,
                ImageID = userImageToiletDto.ImageID,
                ToiletID = userImageToiletDto.ToiletID,
                User = user,
                ImageFile = image,
                Toilet = toilet
            };

            // Сохранение записи в базе данных
            _context.ToiletImages.Add(userImageToilet);
            await _context.SaveChangesAsync();

            return Ok(userImageToilet.Id);
        }
    }
}
