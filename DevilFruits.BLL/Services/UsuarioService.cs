using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DevilFruits.BLL.Repositories;
using DevilFruits.Model.Entities;
using DevilFruits.DTO;
using DevilFruits.BLL.Services.IServices;

namespace DevilFruits.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IGenericRepository<Usuario> _repository;
        public readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> ListaUsuarios()
        {
            try
            {
                var query = await _repository.Consultar();
                var listaUsuarios = await query.ToListAsync();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios", ex);
            }
        }

        public async Task<UsuarioDTO> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _repository.Obtener(x => x.Id == id);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuario)
        {
            try
            {
                var nuevoUsuario = _mapper.Map<Usuario>(usuario);
                var usuarioCreado = await _repository.Crear(nuevoUsuario);
                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EditarUsuario(UsuarioDTO usuario, int usuarioActual)
        {
            try
            {
                var usuarioEditar = _mapper.Map<Usuario>(usuario);
                usuarioEditar.Id = usuarioActual;
                return await _repository.Editar(usuarioEditar);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el usuario", ex);
            }
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _repository.Obtener(x => x.Id == id);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
                return await _repository.Eliminar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario", ex);
            }
        }
    }

}
