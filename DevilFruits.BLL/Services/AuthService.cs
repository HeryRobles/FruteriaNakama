using AutoMapper;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using DevilFruits.Model.Entities;

namespace DevilFruits.BLL.Services
{

    public interface IAuthService
    {
        Task<UsuarioDTO> Registro(UsuarioDTO usuarioDTO);
        Task<TokenDTO> Login(LoginDTO loginDTO);
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

        public async Task<TokenDTO> Login(LoginDTO loginDTO)
        {
            var usuario = await _usuarioRepository.Obtener(x => x.Email == loginDTO.Email);
            
            if (usuario == null || !VerifyPassword(loginDTO.Password, usuario.Pass))
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            return new TokenDTO {
                Token = _jwtService.GenerarToken(usuario),
                Expiration = DateTime.Now.AddHours(1).ToString()
            };
        }

        public async Task<UsuarioDTO> Registro(UsuarioDTO usuarioDTO)
        {
            var existeUsuario = await _usuarioRepository.Obtener(x => x.Email == usuarioDTO.Email);
            if (existeUsuario != null)
            
                throw new Exception("El usuario ya existe");
            
            var usuario = _mapper.Map<Usuario>(usuarioDTO);

            usuario.Rol = "User";
            usuario.Pass = HashPassword(usuario.Pass);

            await _usuarioRepository.Crear(usuario);
            return _mapper.Map<UsuarioDTO>(usuario);

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
