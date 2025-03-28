using AutoMapper;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Response;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Models;
using DevilFruits.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IFavoritoService
    {
        Task<HttpResponseWrapper<bool>> AgregarFavorito(FavoritoDTO favoritoDTO);
        Task<HttpResponseWrapper<List<FrutaDTO>>> ObtenerFavoritos(int idUsuario);
        Task<HttpResponseWrapper<bool>> EliminarFavorito(int idUsuario, int idFruta);
    }
    public class FavoritoService : IFavoritoService
    {
        private readonly IGenericRepository<Favorito> _favoritoRepository;
        private readonly IFrutaService _frutaService;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoritoService> _logger;

        public FavoritoService(
            IGenericRepository<Favorito> favoritoRepository,
            IFrutaService frutaService,
            IMapper mapper,
            ILogger<FavoritoService> logger)
        {
            _favoritoRepository = favoritoRepository;
            _frutaService = frutaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HttpResponseWrapper<bool>> AgregarFavorito(FavoritoDTO favoritoDTO)
        {
            try
            {
                var frutaResponse = await _frutaService.ObtenerFrutaAsync(favoritoDTO.DevilFruitId);
                if (frutaResponse.Error)
                {
                    _logger.LogWarning("Intento de agregar favorito con fruta inexistente: {FrutaId}",
                                     favoritoDTO.DevilFruitId);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("La fruta especificada no existe")
                        });
                }

                var existeFavorito = await _favoritoRepository.GetAsync(
                    x => x.UsuarioId == favoritoDTO.UsuarioId &&
                         x.DevilFruitId == favoritoDTO.DevilFruitId);

                if (existeFavorito != null)
                {
                    _logger.LogWarning("Intento de agregar favorito duplicado para usuario {UsuarioId}",
                                     favoritoDTO.UsuarioId);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.Conflict)
                        {
                            Content = new StringContent("La fruta ya está en favoritos")
                        });
                }

                var favorito = _mapper.Map<Favorito>(favoritoDTO);
                await _favoritoRepository.CreateAsync(favorito);

                _logger.LogInformation("Favorito agregado - Usuario: {UsuarioId}, Fruta: {FrutaId}",
                                     favoritoDTO.UsuarioId, favoritoDTO.DevilFruitId);

                return new HttpResponseWrapper<bool>(
                    response: true,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar favorito");
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al agregar favorito: {ex.Message}")
                    });
            }
        }

        public async Task<HttpResponseWrapper<bool>> EliminarFavorito(int usuarioId, int frutaId)
        {
            try
            {
                var favorito = await _favoritoRepository.GetAsync(
                    x => x.UsuarioId == usuarioId &&
                         x.DevilFruitId == frutaId);

                if (favorito == null)
                {
                    _logger.LogWarning("Intento de eliminar favorito inexistente - Usuario: {UsuarioId}, Fruta: {FrutaId}",
                                    usuarioId, frutaId);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("El favorito especificado no existe")
                        });
                }

                var resultado = await _favoritoRepository.DeleteAsync(favorito);

                if (!resultado)
                {
                    _logger.LogError("No se pudo eliminar el favorito - Usuario: {UsuarioId}, Fruta: {FrutaId}",
                                  usuarioId, frutaId);
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Error al eliminar el favorito")
                        });
                }

                _logger.LogInformation("Favorito eliminado exitosamente - Usuario: {UsuarioId}, Fruta: {FrutaId}",
                                     usuarioId, frutaId);

                return new HttpResponseWrapper<bool>(
                    response: true,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar favorito - Usuario: {UsuarioId}, Fruta: {FrutaId}",
                                usuarioId, frutaId);
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al eliminar favorito: {ex.Message}")
                    });
            }
        }

        public async Task<HttpResponseWrapper<List<FrutaDTO>>> ObtenerFavoritos(int idUsuario)
        {
            try
            {
                var favoritosQuery = await _favoritoRepository.QueryAsync(x => x.UsuarioId == idUsuario);
                var favoritos = await favoritosQuery.ToListAsync();

                var frutasFavoritas = new List<FrutaDTO>();
                foreach (var favorito in favoritos)
                {
                    var frutaResponse = await _frutaService.ObtenerFrutaAsync(favorito.DevilFruitId);
                    if (!frutaResponse.Error && frutaResponse.Response != null)
                    {
                        frutasFavoritas.Add(frutaResponse.Response);
                    }
                }

                _logger.LogInformation("Obtenidos {Count} favoritos para usuario {UsuarioId}",
                                     frutasFavoritas.Count, idUsuario);

                return new HttpResponseWrapper<List<FrutaDTO>>(
                    response: frutasFavoritas,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener favoritos para usuario {UsuarioId}", idUsuario);
                return new HttpResponseWrapper<List<FrutaDTO>>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener favoritos: {ex.Message}")
                    });
            }
        }
        
    }
}