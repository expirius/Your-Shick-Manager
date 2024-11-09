using MFASeekerApp.Model;
using MFASeekerApp.Model.Interfaces;
using MFASeekerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace MFASeekerApp.Services
{
    public class ToiletApiService(UserSession userSession, HttpClient httpClient) : IToiletService
    {
        private readonly UserSession _userSession = userSession;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<int?> AddToilet(Toilet toilet)
        {
            toilet.User = _userSession.AuthUser;

            try
            {
                var toiletDto = new
                {
                    Guid = toilet.Guid,
                    Location = toilet.Location != null
                        ? $"{toilet.Location.Latitude}, {toilet.Location.Longitude}"
                        : null, // Преобразование Location в строку
                    Name = toilet.Name,
                    Description = toilet.Description,
                    Rating = toilet.Rating,
                    IsPrivate = toilet.IsPrivate,
                    CreatedDate = toilet.CreatedDate,
                    UpdatedDateTime = toilet.UpdatedDateTime,
                    UserID = toilet.User.Id,

                };
                
                Console.WriteLine(JsonSerializer.Serialize(toiletDto));
                var response = await _httpClient.PostAsJsonAsync("api/Toilet", toiletDto);
                if (response.IsSuccessStatusCode)
                {
                    var createdToilet = await response.Content.ReadFromJsonAsync<int>();
                    Console.WriteLine($"Туалет добавлен как \nId: {createdToilet}");

                    return createdToilet; // Возвращаем новый Id, если он существует
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "Ошибка при добавлении пользователя");
                return null;
            }
        }
        public async Task<int?> AddImageFile(ImageFile image)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Image", image);
                return image.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task AddUserImageToilet(UserImageToilet userImageToilet)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Toilet/UserImageToilet/", userImageToilet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public Task DeleteToilet(string guid)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UserImageToilet>> GetPhotos(string toiletGuid)
        {
            try
            {
                // Запрос к серверу для получения изображений, связанных с заданным toiletGuid
                var response = await _httpClient.GetAsync($"api/Toilet/ToiletPhotos/{toiletGuid}");

                // Проверка успешности запроса
                if (response.IsSuccessStatusCode)
                {
                    // Десериализация полученного списка изображений
                    var images = await response.Content.ReadFromJsonAsync<IEnumerable<UserImageToilet>>();

                    if (images != null)
                    {
                        // Логирование информации для отладки (можно удалить или адаптировать)
                        Console.WriteLine($"Получено {images.Count()} изображений для туалета с ID {toiletGuid}");

                        return images;
                    }
                    else
                    {
                        Console.WriteLine("Список изображений пуст.");
                    }
                }
                else
                {
                    Console.WriteLine($"Ошибка при получении изображений: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // Возвращаем пустой список, если запрос не удался
            return [];

        }
        public async Task<List<Toilet>> GetAllToilets()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Toilet/AllToilets");
                if (response.IsSuccessStatusCode)
                {
                    // Десериализация данных в список объектов Toilet
                    var toilets = await response.Content.ReadFromJsonAsync<List<Toilet>>();
                    return toilets;
                }
                else
                {
                    // Обработка ошибки
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    return []; // Возвращаем пустой список в случае ошибки
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе: {ex.Message}");
                return new List<Toilet>(); // Возвращаем пустой список в случае исключения
            }
        }
        public Task<Toilet> GetToiletByGuid(string guid)
        {
            throw new NotImplementedException();
        }
        public Task UpdateToilet(Toilet toilet)
        {
            throw new NotImplementedException();
        }
    }
}
