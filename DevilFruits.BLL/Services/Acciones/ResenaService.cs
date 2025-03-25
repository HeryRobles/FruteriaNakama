using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.Models;
using DevilFruits.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IResenaService
    {
        Task<bool> CrearResenaAsync(ResenaDTO resenaDTO);
        Task<List<ResenaDTO>> ObtenerResenasPorFrutaAsync(int devilFruitId);
        Task<List<ResenaDTO>> ObtenerResenasPorUsuarioAsync(int usuarioId);
    }
    public class ResenaService : IResenaService
    {
        private readonly IGenericRepository<Reseña> _resenaRepository;
        private readonly IFrutaService _frutaService;
        private readonly ILogger<ResenaService> _logger;

        public ResenaService(IGenericRepository<Reseña> resenaRepository, IFrutaService frutaService, ILogger<ResenaService> logger)
        {
            _resenaRepository = resenaRepository;
            _frutaService = frutaService;
            _logger = logger;
        }

        public async Task<bool> CrearResenaAsync(ResenaDTO resenaDTO)
        {
            try
            {
                var fruta = await _frutaService.ObtenerFrutaAsync(resenaDTO.DevilFruitId);
                if (fruta == null)
                {
                    _logger.LogWarning($"No se encontró la fruta con ID {resenaDTO.DevilFruitId}");
                    return false;
                }
                    
                if (resenaDTO.Puntaje < 1 || resenaDTO.Puntaje > 5)
                {
                    _logger.LogWarning($"Puntaje inválido: {resenaDTO.Puntaje}. Debe ser entre 1 y 5");
                    return false;
                }
                    
                var resena = new Reseña
                {
                    UsuarioId = resenaDTO.UsuarioId,
                    DevilFruitId = resenaDTO.DevilFruitId,
                    Comentario = resenaDTO.Comentario,
                    Puntaje = resenaDTO.Puntaje,
                };

                var resenaCreada = await _resenaRepository.Crear(resena);
                if(resenaCreada == null)
                {
                    _logger.LogError("No se pudo crear la reseña");
                    return false;
                }
                _logger.LogInformation($"Reseña creada exitosamente para la fruta {resenaDTO.DevilFruitId} por el usuario {resenaDTO.UsuarioId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear reseña: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ResenaDTO>> ObtenerResenasPorFrutaAsync(int devilFruitId)
        {
            try
            {
                var resenasQuery = await _resenaRepository.Consultar(x => x.DevilFruitId == devilFruitId);
                var resenas = await resenasQuery.ToListAsync();

                return resenas.Select(x => new ResenaDTO
                {
                    UsuarioId = x.UsuarioId,
                    DevilFruitId = x.DevilFruitId,
                    Comentario = x.Comentario,
                    Puntaje = x.Puntaje,
                    FechaCreacion = x.FechaCreacion
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reseñas para fruta {devilFruitId}");
                throw; 
            }
        }

        public async Task<List<ResenaDTO>> ObtenerResenasPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var resenasQuery = await _resenaRepository.Consultar(x => x.UsuarioId == usuarioId);
                var resenas = await resenasQuery.ToListAsync();

                return resenas.Select(x => new ResenaDTO
                {
                    UsuarioId = x.UsuarioId,
                    DevilFruitId = x.DevilFruitId,
                    Comentario = x.Comentario,
                    Puntaje = x.Puntaje,
                    FechaCreacion = x.FechaCreacion
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reseñas para usuario {usuarioId}");
                throw;
            }
        }
    }
}
