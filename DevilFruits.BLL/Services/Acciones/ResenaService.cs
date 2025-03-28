using AutoMapper;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Response;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.Models;
using DevilFruits.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IResenaService
    {
        Task<HttpResponseWrapper<bool>> CrearResenaAsync(ResenaDTO resenaDTO);
        Task<HttpResponseWrapper<bool>> EliminarResenaAsync(int resenaId);
        Task<HttpResponseWrapper<List<ResenaDTO>>> ObtenerResenasPorFrutaAsync(int devilFruitId);
        Task<HttpResponseWrapper<List<ResenaDTO>>> ObtenerResenasPorUsuarioAsync(int usuarioId);
    }
    public class ResenaService : IResenaService
    {
        private readonly IGenericRepository<Reseña> _resenaRepository;
        private readonly IFrutaService _frutaService;
        private readonly IMapper _mapper;
        private readonly ILogger<ResenaService> _logger;

        public ResenaService(
            IGenericRepository<Reseña> resenaRepository,
            IFrutaService frutaService,
            IMapper mapper,
            ILogger<ResenaService> logger)
        {
            _resenaRepository = resenaRepository;
            _frutaService = frutaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HttpResponseWrapper<bool>> CrearResenaAsync(ResenaDTO resenaDTO)
        {
            try
            {
                var frutaResponse = await _frutaService.ObtenerFrutaAsync(resenaDTO.DevilFruitId);
                if (frutaResponse.Error)
                {
                    _logger.LogWarning("Intento de crear reseña para fruta inexistente: {FrutaId}",
                                     resenaDTO.DevilFruitId);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("La fruta especificada no existe")
                        });
                }

                if (resenaDTO.Puntaje < 1 || resenaDTO.Puntaje > 5)
                {
                    _logger.LogWarning("Intento de crear reseña con puntaje inválido: {Puntaje}",
                                     resenaDTO.Puntaje);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("El puntaje debe estar entre 1 y 5")
                        });
                }

                var resena = _mapper.Map<Reseña>(resenaDTO);
                await _resenaRepository.CreateAsync(resena);

                _logger.LogInformation("Reseña creada - Fruta: {FrutaId}, Usuario: {UsuarioId}",
                                     resenaDTO.DevilFruitId, resenaDTO.UsuarioId);

                return new HttpResponseWrapper<bool>(
                    response: true,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reseña");
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al crear reseña: {ex.Message}")
                    });
            }
        }

        public async Task<HttpResponseWrapper<bool>> EliminarResenaAsync(int id)
        {
            try
            {
                var resena = await _resenaRepository.GetAsync(x => x.Id == id);

                if (resena == null)
                {
                    _logger.LogWarning("Intento de eliminar reseña inexistente - ID: {ResenaId}", id);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("La reseña especificada no existe")
                        });
                }

                var resultado = await _resenaRepository.DeleteAsync(resena);

                if (!resultado)
                {
                    _logger.LogError("No se pudo eliminar la reseña - ID: {ResenaId}", id);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Error al eliminar la reseña")
                        });
                }

                _logger.LogInformation("Reseña eliminada exitosamente - ID: {ResenaId}", id);

                return new HttpResponseWrapper<bool>(
                    response: true,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar reseña - ID: {ResenaId}", id);
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al eliminar reseña: {ex.Message}")
                    });
            }
        }

        public async Task<HttpResponseWrapper<List<ResenaDTO>>> ObtenerResenasPorFrutaAsync(int devilFruitId)
        {
            try
            {
                var frutaResponse = await _frutaService.ObtenerFrutaAsync(devilFruitId);
                if (frutaResponse.Error)
                {
                    _logger.LogWarning("Intento de obtener reseñas para fruta inexistente: {FrutaId}",
                                     devilFruitId);
                    return new HttpResponseWrapper<List<ResenaDTO>>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("La fruta especificada no existe")
                        });
                }

                var resenasQuery = await _resenaRepository.QueryAsync(x => x.DevilFruitId == devilFruitId);
                var resenas = await resenasQuery.ToListAsync();

                var result = _mapper.Map<List<ResenaDTO>>(resenas);

                _logger.LogInformation("Obtenidas {Count} reseñas para fruta {FrutaId}",
                                     result.Count, devilFruitId);

                return new HttpResponseWrapper<List<ResenaDTO>>(
                    response: result,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reseñas para fruta {FrutaId}", devilFruitId);
                return new HttpResponseWrapper<List<ResenaDTO>>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener reseñas: {ex.Message}")
                    });
            }
        }

        public async Task<HttpResponseWrapper<List<ResenaDTO>>> ObtenerResenasPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var resenasQuery = await _resenaRepository.QueryAsync(x => x.UsuarioId == usuarioId);
                var resenas = await resenasQuery.ToListAsync();

                var result = _mapper.Map<List<ResenaDTO>>(resenas);

                _logger.LogInformation("Obtenidas {Count} reseñas para usuario {UsuarioId}",
                                     result.Count, usuarioId);

                return new HttpResponseWrapper<List<ResenaDTO>>(
                    response: result,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reseñas para usuario {UsuarioId}", usuarioId);
                return new HttpResponseWrapper<List<ResenaDTO>>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener reseñas: {ex.Message}")
                    });
            }
        }
    }
}