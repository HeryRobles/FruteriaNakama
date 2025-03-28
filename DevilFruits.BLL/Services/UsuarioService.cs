using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DevilFruits.BLL.Repositories;
using DevilFruits.Model.Entities;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.Models;
using DevilFruits.BLL.Response;
using System.Net;

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

        public async Task<HttpResponseWrapper<List<UsuarioDTO>>> ListaUsuarios()
        {
            
            try
            {
                var query = await _repository.QueryAsync();
                var listaUsuarios = await query.ToListAsync();
                var result = _mapper.Map<List<UsuarioDTO>>(listaUsuarios);

                return new HttpResponseWrapper<List<UsuarioDTO>>(
                    response: result,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK)
                );


            }
            catch (Exception ex)
            {
                return new HttpResponseWrapper<List<UsuarioDTO>>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(
                        HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener lista de usuarios: {ex.Message}")
                    }
                );
            }
        }

        public async Task<HttpResponseWrapper<UsuarioDTO>> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _repository.GetAsync(x => x.Id == id);
                if (usuario == null)
                {
                    return new HttpResponseWrapper<UsuarioDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(
                            HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Usuario no encontrado")
                        }
                    );

                }
                var result = _mapper.Map<UsuarioDTO>(usuario);
                return new HttpResponseWrapper<UsuarioDTO>(
                    response: result,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK)
                );

            }
            catch (Exception ex) 
            {
                return new HttpResponseWrapper<UsuarioDTO>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(
                        HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al obtener el usuario: {ex.Message}")
                    }
                );
            }
        }

        public async Task<HttpResponseWrapper<UsuarioDTO>> CrearUsuario(UsuarioDTO usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Email))
                {
                    return new HttpResponseWrapper<UsuarioDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("El email es requerido")
                        }
                    );
                }

                var nuevoUsuario = _mapper.Map<Usuario>(usuario);
                var usuarioCreado = await _repository.CreateAsync(nuevoUsuario);
                var result = _mapper.Map<UsuarioDTO>(usuarioCreado);

                return new HttpResponseWrapper<UsuarioDTO>(
                    response: result,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.Created)
                );
            }
            catch (Exception ex)
            {
                return new HttpResponseWrapper<UsuarioDTO>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al crear usuario: {ex.Message}")
                    }
                );
            }
        }

        public async Task<HttpResponseWrapper<bool>> EditarUsuario(UsuarioDTO usuario, int usuarioActual)
        {
            try
            {
                var usuarioExistente = await _repository.GetAsync(x => x.Id == usuario.Id);
                if (usuarioExistente == null)
                {
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(
                            HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Usuario no encontrado")
                        }
                    );
                }
                var usuarioEditado = _mapper.Map<Usuario>(usuario);
                usuarioExistente.Id = usuarioEditado.Id;
                var resultado = await _repository.UpdateAsync(usuarioExistente);

                return new HttpResponseWrapper<bool>(
                    response: resultado,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK)
                );

            }
            catch (Exception ex)
            {
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(
                                         HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al editar el usuario: {ex.Message}")
                    }
                );
            }
        }

        public async Task<HttpResponseWrapper<bool>> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _repository.GetAsync(x => x.Id == id);
                if(usuario == null)
                {
                    return new HttpResponseWrapper<bool>(
                        response: false,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(
                            HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Usuario no encontrado")
                        }
                    );
                }
                var resultado = await _repository.DeleteAsync(usuario);
                return new HttpResponseWrapper<bool>(
                    response: resultado,
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK)
                );
            }
            catch (Exception ex)
            {
                return new HttpResponseWrapper<bool>(
                    response: false,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(
                        HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al eliminar el usuario: {ex.Message}")
                    }
                );
            }
        }

    }

}