using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaFarmacia.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.DAL.Implementacion;
using sistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.BLL.Implementacion;
using SistemaFarmacia.BLL.Interfaces;
//using SistemaFarmacia.BLL.Interfaces;
//using SistemaFarmacia.BLL.Implementacion; 


namespace SistemaFarmacia.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration) {
            services.AddDbContext<FarmaciaTroyaContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IUtilidadesService, UtilidadesServices>();
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IPresentacionService, PresentacionService>();
            services.AddScoped<IProveedorService, ProveedorService>();
            services.AddScoped<IMarcaService, MarcaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ICompraService, CompraService>();
        }

    }
}
