using System;
using System.IO;
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

        public async Task<string> DownloadAsync(string endpoint, string path)
        {
            Authorize();
            
            var url = new Uri(_apiWebSettings.Address + endpoint);
            var response = await _httpClient.GetAsync(url);

            return null;
        }

        public async Task UploadAsync(string endpoint, string fullPath, Guid id)
        {
            var isExists = File.Exists(fullPath);
            
            using (var stream = File.OpenRead(fullPath))
            {
                var ext = Path.GetExtension(fullPath);
                var fileStreamContent = GetFileStreamContent(stream, id, ext);

                Authorize();

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);

                    var url = new Uri(_apiWebSettings.Address + endpoint);
                    var response = await _httpClient.PostAsync(url, formData);

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        HandleFailMessage(responseString);
                    }
                }
            }
        }

        private static StreamContent GetFileStreamContent(Stream stream, Guid id, string ext)
        {
            var fileStreamContent = new StreamContent(stream);

            fileStreamContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            fileStreamContent.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = id.ToString() + ext,
                };
            return fileStreamContent;
        }

        public async Task<TEntity> SendAsync<TEntity>(HttpMethod method, string endpoint, object body = null)
        {
            var request = new HttpRequestMessage();
            request.Method = method;
            request.RequestUri = new Uri(_apiWebSettings.Address + endpoint);
            
            Authorize();

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

        private void Authorize()
        {
            if (_apiWebSettings.JwtDto != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiWebSettings.JwtDto.Token);
            }
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