﻿using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.Model.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevilFruits.BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
                throw new Exception("No se ha configurado la clave secreta del token");

            try
            {

                if (usuario == null)
                    throw new Exception("Usuario no encontrado");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            }
            catch
            {
                throw new Exception("Error al generar el token");
            }
            
        }
    }
}
