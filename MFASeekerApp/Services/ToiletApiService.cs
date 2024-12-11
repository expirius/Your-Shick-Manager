using MFASeekerApp.Model;
using MFASeekerApp.Model.Interfaces;
using MFASeekerApp.ViewModel;
using Microsoft.Maui.Graphics.Text;
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
        public readonly HttpClient _httpClient = httpClient;

        public async Task<int?> AddToilet(Toilet toilet)
        {
            toilet.User = _userSession.AuthUser;

            try
            {
                toilet.User = null;
                var response = await _httpClient.PostAsJsonAsync("api/Toilet", toilet);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(JsonSerializer.Serialize(toilet));
                    var createdToilet = await response.Content.ReadFromJsonAsync<int>();
                    Console.WriteLine($"Туалет добавлен как \nId: {createdToilet}");

                    return createdToilet; // Возвращаем новый Id, если он существует
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "Ошибка при добавлении туалета");
                return null;
            }
        }
        //public async Task<int?> AddImageFile(ImageFile image)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync("api/Image", image);
        //        return image.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
        public async Task<int?> AddUserImageToilet(UserImageToilet userImageToilet)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Toilet/Image", userImageToilet.ImageFile);
                Console.WriteLine("ImageFile: " + JsonSerializer.Serialize(userImageToilet.ImageFile));

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<int>();
                    userImageToilet.ImageID = responseData;
                    userImageToilet.ImageFile.Path = "";
                    //userImageToilet.ImageFile = null;

                    Console.WriteLine(JsonSerializer.Serialize(userImageToilet));
                    response = await _httpClient.PostAsJsonAsync("api/Toilet/UserImageToilet", userImageToilet);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultUIT = await response.Content.ReadFromJsonAsync<int>(); // возвращать строку мб потом
                        return resultUIT;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при отправке данных о UserImageToilet: " + response.ReasonPhrase);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка при отправке изображения: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public Task DeleteToilet(string guid)
        {
            throw new NotImplementedException();
        }
        public async Task<List<string>> GetPhotoLinks(string toiletGuid)
        {
            try
            {
                // Запрос к серверу для получения изображений, связанных с заданным toiletGuid
                var response = await _httpClient.GetAsync($"api/Toilet/ToiletPhotos/{toiletGuid}");
                
                // Проверка успешности запроса
                if (response.IsSuccessStatusCode)
                {
                    // Десериализация полученного списка изображений
                    var imagestring = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(imagestring);
                    var images = await response.Content.ReadFromJsonAsync<List<string>>();

                    if (images?.Count > 0)
                    {
                        // Логирование информации для отладки (можно удалить или адаптировать)
                        Console.WriteLine($"Получено {images.Count} изображений для туалета с ID {toiletGuid}");

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
                    var toilets = await response.Content.ReadFromJsonAsync<List<Toilet>>();
                    return toilets ?? [];
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
                return []; // Возвращаем пустой список в случае исключения
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
