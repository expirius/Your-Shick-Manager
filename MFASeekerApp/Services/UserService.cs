using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Services
{
    public class UserService(HttpClient _httpClient)
    {
        private readonly HttpClient httpClient = _httpClient;
        private List<User> userList = [];

        public async Task<List<User>> GetUsers()
        {
            try
            {
                //if (userList.Count == 0)
                //    return userList;
                //var url = "http://192.168.0.2:7226/api/User/AllUsers";

                var response = await httpClient.GetAsync("api/User/AllUsers");

                if (response.IsSuccessStatusCode)
                {
                    userList = await response.Content.ReadFromJsonAsync<List<User>>();
                }
                return userList;
            }
            catch (Exception ex)
            {
                return userList;
            }
        }
    }
}
