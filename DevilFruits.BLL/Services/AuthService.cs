using AutoMapper;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Response;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using DevilFruits.Model.Entities;
using System.Net;

namespace DevilFruits.BLL.Services
{

    public interface IAuthService
    {
        Task<HttpResponseWrapper<UsuarioDTO>> Registro(UsuarioDTO usuarioDTO);
        Task<HttpResponseWrapper<TokenDTO>> Login(LoginDTO loginDTO);
    }
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<HttpResponseWrapper<TokenDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                var usuario = await _usuarioRepository.GetAsync(x => x.Email == loginDTO.Email);
                if(usuario == null || !VerifyPassword(loginDTO.Password, usuario.Pass))
                {
                    return new HttpResponseWrapper<TokenDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new StringContent("Usuario o contraseña incorrectos")
                        }
                    );
                }
                var token = _jwtService.GenerarToken(usuario);
                return new HttpResponseWrapper<TokenDTO>(
                    response: new TokenDTO 
                    { 
                        Token = token,
                        Expiration = DateTime.UtcNow.AddHours(5).ToString("o")
                    },
                    error: false,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.OK)
                );

            }
            catch (Exception ex)
            {
                return new HttpResponseWrapper<TokenDTO>(
                    response: null,
                    error: true,
                    httpResponseMessage: new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error al iniciar sesión: {ex.Message}")
                    }
                );

            }
        }

        public async Task<HttpResponseWrapper<UsuarioDTO>> Registro(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuarioExistente = await _usuarioRepository.GetAsync(x => x.Email == usuarioDTO.Email);
                if(usuarioExistente != null)
                {
                    return new HttpResponseWrapper<UsuarioDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("El usuario ya existe")
                        }
                    );
                }
                var usuario = _mapper.Map<Usuario>(usuarioDTO);
                if (string.IsNullOrWhiteSpace(usuarioDTO.Pass))
                {
                    return new HttpResponseWrapper<UsuarioDTO>(
                        response: null,
                        error: true,
                        httpResponseMessage: new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("La contraseña es obligatoria")
                        }
                    );
                }

                usuario.Rol = "user";
                usuario.Pass = HashPassword(usuarioDTO.Pass);

                var usuarioCreado = await _usuarioRepository.CreateAsync(usuario);
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
                        Content = new StringContent($"Error al registrar el usuario: {ex.Message}")
                    }
                );

            }

        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}