using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface IProveedorService
    {
        Task<List<Proveedore>> Lista();

        Task<Proveedore> Crear(Proveedore entidad);

        Task<Proveedore> Editar(Proveedore entidad);

        Task<bool> Eliminar(int idProveedor); 
    }
}
