using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.AplicacionWed.Models.ViewModels;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface IProductoService
    {
        Task<List<VMProducto>> Lista();

        Task<Producto> Crear(Producto entidad);

        Task<Producto> Editar(Producto entidad);

        Task<bool> Eliminar(int idProducto); 
    }
}
