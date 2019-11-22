using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NoteMe.Common.Exceptions;
using NoteMe.Common.Services.Json;

namespace NoteMe.Client.Domain
{
    public interface IApiWebService
    {
        Task<TEntity> SendAsync<TEntity>(HttpMethod method, string endpoint, object body = null);
    }
    
    public class ApiWebService : IApiWebService
    {
        private readonly ApiWebSettings _apiWebSettings;
        private readonly HttpClient _httpClient;
        
        public ApiWebService(ApiWebSettings apiWebSettings)
        {
            _apiWebSettings = apiWebSettings;
            
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = 
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            
            _httpClient = new HttpClient(handler);
        }
        
        public async Task<TEntity> SendAsync<TEntity>(HttpMethod method, string endpoint, object body = null)
        {
            var request = new HttpRequestMessage();
            request.Method = method;
            request.RequestUri = new Uri(_apiWebSettings.Address + endpoint);
            
            var hasToken = _httpClient.DefaultRequestHeaders.Contains("Authorization");

            if (_apiWebSettings.JwtDto != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiWebSettings.JwtDto.Token);
            }

            if (body != null)
            {
                var json = JsonSerializeService.Serialize(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var httpResponseMessage = await _httpClient.SendAsync(request);
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                HandleFailMessage(responseString);
            }

            return JsonSerializeService.Deserialize<TEntity>(responseString);
        }

        private void HandleFailMessage(string responseString)
        {
            var errorDto = JsonSerializeService.Deserialize<ErrorDto>(responseString);

            switch (errorDto.ErrorCode)
            {
                case ErrorCodes.InvalidCredentials:
                    throw new UnauthorizedAccessException();
            }
        }
    }
}