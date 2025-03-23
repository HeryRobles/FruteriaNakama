using DevilFruits.BLL.Mappeo;
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

        }

    }
}
