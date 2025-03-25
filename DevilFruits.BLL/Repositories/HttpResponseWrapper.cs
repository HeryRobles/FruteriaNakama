using System.Net;
using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevilFruits.BLL.Repositories
{
    public class HttpResponseWrapper<T> : IDisposable
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        private string? _cachedErrorMessage;
        private bool _disposed;

        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            _httpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
            StatusCode = httpResponseMessage.StatusCode;
            IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
        }

        public bool Error { get; }
        public T? Response { get; }
        public HttpStatusCode StatusCode { get; }
        public bool IsSuccessStatusCode { get; }
        public HttpResponseMessage HttpResponseMessage => _httpResponseMessage;

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            if (_cachedErrorMessage != null)
            {
                return _cachedErrorMessage;
            }

            _cachedErrorMessage = StatusCode switch
            {
                HttpStatusCode.NotFound => "Recurso no encontrado",
                HttpStatusCode.BadRequest => await GetBadRequestMessageAsync(),
                HttpStatusCode.Unauthorized => "Debe autenticarse para realizar esta operación",
                HttpStatusCode.Forbidden => "No tiene permisos para realizar esta operación",
                HttpStatusCode.InternalServerError => "Error interno del servidor",
                HttpStatusCode.ServiceUnavailable => "Servicio no disponible temporalmente",
                HttpStatusCode.GatewayTimeout => "Tiempo de espera agotado al conectar con el servidor",
                HttpStatusCode.TooManyRequests => "Demasiadas solicitudes. Por favor, intente nuevamente más tarde",
                _ => "Ha ocurrido un error inesperado"
            };

            return _cachedErrorMessage;
        }
        private async Task<string> GetBadRequestMessageAsync()
        {
            try
            {
                var content = await _httpResponseMessage.Content.ReadAsStringAsync();

                try
                {
                    var jObject = JObject.Parse(content);

                    var messageProperty = jObject["message"] ?? jObject["error"] ?? jObject["error_description"];
                    if (messageProperty != null)
                    {
                        return messageProperty.ToString();
                    }

                    var firstProperty = jObject.Properties().FirstOrDefault();
                    if (firstProperty != null)
                    {
                        return firstProperty.Value.ToString();
                    }
                }
                catch (JsonException)
                {
                    
                }

                return string.IsNullOrWhiteSpace(content) ? "Solicitud incorrecta" : content;
            }
            catch
            {
                return "Solicitud incorrecta";
            }
        }
        public async Task<TResult?> DeserializeContentAsync<TResult>()
        {
            var content = await _httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(content);
        }
        public async Task<string> GetResponseBodyAsync()
        {
            return await _httpResponseMessage.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpResponseMessage.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
        