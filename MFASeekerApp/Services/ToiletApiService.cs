using MFASeekerApp.Model;
using MFASeekerApp.Model.Interfaces;
using MFASeekerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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
                var response = await _httpClient.PostAsJsonAsync("api/Toilet/", toilet);
                if (response.IsSuccessStatusCode)
                {
                    var createdToilet = await response.Content.ReadFromJsonAsync<Toilet>();
                    Console.WriteLine($"Туалет добавлен как \nId: {createdToilet?.Id}, \nGuid: {createdToilet?.Guid}") ;

                    return createdToilet?.Id; // Возвращаем новый Id, если он существует
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
                var response = await _httpClient.PostAsJsonAsync("api/ImageFile", image);
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
                var response = await _httpClient.PostAsJsonAsync("api/UserImageToilet/", userImageToilet);

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
        public Task<List<Toilet>> GetAllToilets()
        {
            throw new NotImplementedException();
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
