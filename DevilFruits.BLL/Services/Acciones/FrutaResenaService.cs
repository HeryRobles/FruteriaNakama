using AutoMapper;
using DevilFruits.BLL.Response;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IFrutaResenaService
    {
        Task<HttpResponseWrapper<FrutaDetalleDTO>> ObtenerFrutaConResenasAsync(int id);
    }

    public class FrutaResenaService : IFrutaResenaService
    {
        private readonly IFrutaService _frutaService;
        private readonly IResenaService _resenaService;
        private readonly IMapper _mapper;
        private readonly ILogger<FrutaResenaService> _logger;

        public FrutaResenaService(
            IFrutaService frutaService,
            IResenaService resenaService,
            IMapper mapper,
            ILogger<FrutaResenaService> logger)
        {
            _frutaService = frutaService;
            _resenaService = resenaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HttpResponseWrapper<FrutaDetalleDTO>> ObtenerFrutaConResenasAsync(int id)
        {
            try
            {
                var frutaResponse = await _frutaService.ObtenerFrutaAsync(id);
                if (frutaResponse.Error)
                {
                    _logger.LogWarning("Intento de obtener detalles para fruta inexistente: {FrutaId}", id);
                    return new HttpResponseWrapper<FrutaDetalleDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Fruta no encontrada")
                        });
                }

                var resenasResponse = await _resenaService.ObtenerResenasPorFrutaAsync(id);
                if (resenasResponse.Error)
                {
                    _logger.LogWarning("Error al obtener reseñas para fruta {FrutaId}", id);
                    return new HttpResponseWrapper<FrutaDetalleDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: resenasResponse.HttpResponseMessage);
                }

                var resenas = resenasResponse.Response ?? new List<ResenaDTO>();
                var puntajePromedio = resenas.Any() ? (int)resenas.Average(x => x.Puntaje) : 0;

                var frutaDetalle = _mapper.Map<FrutaDetalleDTO>(frutaResponse.Response);
                frutaDetalle.Resenas = resenas;
                frutaDetalle.PuntajePromedio = puntajePromedio;

                _logger.LogInformation("Detalles obtenidos para fruta {FrutaId} con {Count} reseñas", id, resenas.Count);

                return new HttpResponseWrapper<FrutaDetalleDTO>(
                    response: frutaDetalle,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles para fruta {FrutaId}", id);
                return new HttpResponseWrapper<FrutaDetalleDTO>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener detalles de fruta: {ex.Message}")
                    });
            }
        }
    }
}