using DevilFruits.BLL.Services.Acciones;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DevilFruits.BLL.Services
{
    public class FrutaService : IFrutaService
    {
        private readonly HttpClient _httpClient;

        public FrutaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FrutaDTO>> ListadoFrutas()
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var frutas = JsonConvert.DeserializeObject<List<FrutaDTO>>(content);
            
            return frutas!;
        }

        public async Task<FrutaDTO> ObtenerFrutaAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var fruta = JsonConvert.DeserializeObject<FrutaDTO>(content);
            return fruta!;
        }
        
    }
}
