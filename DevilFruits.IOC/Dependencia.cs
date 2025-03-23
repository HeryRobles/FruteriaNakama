using DevilFruits.BLL.Mappeo;
using DevilFruits.BLL.Repositories;
using DevilFruits.BLL.Services;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevilFruits.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {

            //Configuracion de la cadena de conexion
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("conectSQL"));
            });

            //Configuracion de AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));


            //Implementacion de los repositorios
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Dependencias de los servicios
            services.AddScoped<IUsuarioService, UsuarioService>();

        }

    }
}
