using DevilFruits.BLL.Repositories;
using Microsoft.EntityFrameworkCore;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Models;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IFavoritoService
    {
        Task<bool> AgregarFavorito(FavoritoDTO favorito);
        Task<List<FrutaDTO>> ObtenerFavoritos(int idUsuario);
    }
    public class FavoritoService : IFavoritoService
    {
        private readonly IGenericRepository<Favorito> _favoritoRepository;
        private readonly IFrutaService _frutaService;

        public FavoritoService(IGenericRepository<Favorito> favoritoRepository, IFrutaService frutaService)
        {
            _favoritoRepository = favoritoRepository;
            _frutaService = frutaService;
        }

        public async Task<bool> AgregarFavorito(FavoritoDTO favoritoDTO)
        {
            var existeFavorito = await _favoritoRepository.
                Obtener(x => x.UsuarioId == favoritoDTO.UsuarioId && x.DevilFruitId == favoritoDTO.DevilFruitId);
            if (existeFavorito != null)
                throw new Exception("La fruta ya se encuentra en favoritos");

            var favorito = new Favorito
            {
                UsuarioId = favoritoDTO.UsuarioId,
                DevilFruitId = favoritoDTO.DevilFruitId
            };

            await _favoritoRepository.Crear(favorito);
            return true;
        }

        public async Task<List<FrutaDTO>> ObtenerFavoritos(int idUsuario)
        {
            var favoritosQuery = await _favoritoRepository.Consultar(x => x.UsuarioId == idUsuario);
            var favoritos = favoritosQuery.ToList();

            var frutasFavoritas = new List<FrutaDTO>();
            foreach (var favorito in favoritos)
            {
                try
                {
                    var fruta = await _frutaService.ObtenerFrutaAsync(favorito.DevilFruitId);
                    if (fruta != null)
                        frutasFavoritas.Add(fruta);
                }
                catch (Exception ex)
                {
                    continue;

                }
            }
            return frutasFavoritas;
        }
    }
}
