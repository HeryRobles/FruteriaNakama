using AutoMapper;
using DevilFruits.BLL.Response;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.ExternalModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DevilFruits.BLL.Services
{
    public class FrutaService : IFrutaService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<FrutaService> _logger;
        private const int CacheExpirationMinutes = 30;

        public FrutaService(HttpClient httpClient, IMapper mapper,
                          IMemoryCache cache, ILogger<FrutaService> logger)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<HttpResponseWrapper<List<FrutaDTO>>> ListadoFrutas()
        {
            try
            {
                if (_cache.TryGetValue("ListadoFrutasCache", out List<FrutaDTO> cachedFrutas))
                {
                    _logger.LogInformation("Retornando listado de frutas desde caché");
                    return new HttpResponseWrapper<List<FrutaDTO>>(
                        response: cachedFrutas,
                        error: false,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
                }

                var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error al obtener frutas. Código: {StatusCode}", response.StatusCode);
                    return new HttpResponseWrapper<List<FrutaDTO>>(
                        response: null,
                        error: true,
                        httpResponseMessage: response);
                }

                var content = await response.Content.ReadAsStringAsync();
                var frutas = JsonConvert.DeserializeObject<List<FrutaDTO>>(content);

                _cache.Set("ListadoFrutasCache", frutas,
                          TimeSpan.FromMinutes(CacheExpirationMinutes));

                _logger.LogInformation("Listado de frutas obtenido y almacenado en caché");
                return new HttpResponseWrapper<List<FrutaDTO>>(
                    response: frutas,
                    error: false,
                    httpResponseMessage: response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al obtener listado de frutas");
                return new HttpResponseWrapper<List<FrutaDTO>>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener lista de frutas: {ex.Message}")
                    });
            }
        }
        public async Task<HttpResponseWrapper<FrutaDTO>> ObtenerFrutaAsync(int id)
        {
            try
            {
                var cacheKey = $"Fruta_{id}";
                if (_cache.TryGetValue(cacheKey, out FrutaDTO cachedFruta))
                {
                    _logger.LogInformation("Retornando fruta {Id} desde caché", id);
                    return new HttpResponseWrapper<FrutaDTO>(
                        response: cachedFruta,
                        error: false,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
                }

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("No se encontró fruta con ID {Id}. Código: {StatusCode}",
                                     id, response.StatusCode);
                    return new HttpResponseWrapper<FrutaDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: response);
                }

                var content = await response.Content.ReadAsStringAsync();
                var fruta = JsonConvert.DeserializeObject<FrutaDTO>(content);

                _cache.Set(cacheKey, fruta, TimeSpan.FromMinutes(CacheExpirationMinutes));

                _logger.LogInformation("Fruta {Id} obtenida y almacenada en caché", id);
                return new HttpResponseWrapper<FrutaDTO>(
                    response: fruta,
                    error: false,
                    httpResponseMessage: response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener fruta con ID {Id}", id);
                return new HttpResponseWrapper<FrutaDTO>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener fruta: {ex.Message}")
                    }
                );
            }
        }
    }
}