using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface IPresentacionService
    {
        Task<List<Presentacione>> Lista();

        Task<Presentacione> Crear(Presentacione entidad);

        Task<Presentacione> Editar(Presentacione entidad);

        Task<bool> Eliminar (int idPresentacion);
    }
}
