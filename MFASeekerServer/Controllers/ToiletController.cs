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
        [HttpPost]
        public async Task<ActionResult<int>> AddToilet([FromBody] Toilet createdToilet)
        {
            _context.Toilets.Add(createdToilet);
            await _context.SaveChangesAsync();

            return Ok(createdToilet.Id);
        }
        [HttpPost("Image")]
        public async Task<ActionResult<int>> AddImage(ImageFile image)
        {
            // Папка для сохранения изображений
            var uploadsFolder = Path.Combine("images", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Создаем папку, если она не существует

            // Генерация уникального имени файла с расширением, соответствующим типу файла
            var fileExtension = Path.GetExtension(image.FileName) ?? ".jpg"; // Получаем расширение файла или используем ".jpg" по умолчанию
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                // Декодирование строки Base64 в массив байтов
                var fileBytes = Convert.FromBase64String(image.ByteBase64);

                // Сохранение массива байтов как файла на сервере
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                // Обновление пути в объекте ImageFile для сохранения в базе данных
                image.Path = $"/images/uploads/{uniqueFileName}";

                // Сохранение объекта ImageFile в базе данных
                await _context.ImageFiles.AddAsync(image);
                await _context.SaveChangesAsync();

                return Ok(image.Id);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string.");
            }
            catch (Exception ex)
            {
                // Логгирование ошибки, если это необходимо
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
                //User = user,
                //ImageFile = image,
                //Toilet = toilet
            };

            // Сохранение записи в базе данных
            _context.ToiletImages.Add(userImageToilet);
            await _context.SaveChangesAsync();

            return Ok(userImageToilet.Id);
        }
        /// <summary>
        /// Получение всех фото туалета
        /// </summary>
        /// <param name="toiletId"></param>
        /// <returns></returns>
        [HttpGet("ToiletPhotos/{toiletGuid}")]
        public async Task<ActionResult<List<UserImageToilet>>> GetPhotos(string toiletGuid)
        {
            // Проверка существования туалета по его guid
            var toilet = await _context.Toilets
                .FirstOrDefaultAsync(t => t.Guid == toiletGuid);

            if (toilet == null)
            {
                return NotFound("Toilet not found.");
            }

            // Извлечение объектов UserImageToilet, связанных с указанным туалетом, включая ImageFile
            var userImageToilets = await _context.ToiletImages
                .Where(ti => ti.ToiletID == toilet.Id)
                .Include(ti => ti.ImageFile) // Подключение связанных данных ImageFile
                .ToListAsync();

            return Ok(userImageToilets);
        }
    }
}
