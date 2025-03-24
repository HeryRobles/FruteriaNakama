using DevilFruits.BLL.Mappeo;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Services;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DAL.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevilFruits.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {

            //Configuracion de la cadena de conexion
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("conectSQL"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                     ));
            });


            //Configuracion de la autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey = configuration["Jwt:Key"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                    };
                });

            //Configuracion de AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));


            //Implementacion de los repositorios
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Dependencias de los servicios
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IFrutaService, FrutaService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            //Configuracion de la URL de la API externa
            services.AddHttpClient<IFrutaService, FrutaService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalApi:UrlAPI"]!);
            });

        }

    }
}
