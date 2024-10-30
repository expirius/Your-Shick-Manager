using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Services
{
    public class UserService
    {
        HttpClient httpClient;
        List<User> userList = [];
        public UserService()
        {
            httpClient = new HttpClient();
        }
        public async Task<List<User>> GetUsers()
        {
            //if (userList.Count == 0)
            //    return userList;
            var url = "http://localhost:5000/api/Toilet/AllToilets";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                userList = await response.Content.ReadFromJsonAsync<List<User>>();
            }
            return userList;
        }
    }
}
