using Crayon.Services.GenericHttpClients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace Crayon.Services.GenericHttpClient
{
    public class GenericHttpClient : IGenericHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;

        public GenericHttpClient(IConfiguration configuration, HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _client = client;
            _contextAccessor = contextAccessor;
            _client.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }

        public async Task<T> DeleteAsAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            //string token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
            var token = _contextAccessor.HttpContext?.User
    ?.FindFirst("token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            //throw new NotImplementedException();
            throw new Exception($"{response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            //string token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
            var token = _contextAccessor.HttpContext?.User?.FindFirst("token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            throw new Exception($"{response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
        }

        public async Task<T> PostAsAsync<T>(string url, dynamic dynamicdata)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new StringContent(JsonConvert.SerializeObject(dynamicdata), null, "application/json");
            request.Content = content;

            //string token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
            var token = _contextAccessor.HttpContext?.User
    ?.FindFirst("token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            Console.WriteLine(request.Headers.Authorization);
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            throw new Exception($"{response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
        }

        public async Task<T> PutAsAsync<T>(string url, dynamic dynamicdata)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var content = new StringContent(JsonConvert.SerializeObject(dynamicdata), null, "application/json");
            request.Content = content;

            //string token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
            var token = _contextAccessor.HttpContext?.User
    ?.FindFirst("token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            //throw new NotImplementedException();
            throw new Exception($"{response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
        }
    
    }
}
