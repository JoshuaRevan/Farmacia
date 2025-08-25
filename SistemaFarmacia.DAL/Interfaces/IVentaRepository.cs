using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.DAL.Interfaces;

using SistemaFarmacia.Entity;

namespace SistemaFarmacia.DAL.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta entidad);

        Task<List<Venta>> Reporte(DateTime FechaInicio, DateTime FechaFin);
    }
}
