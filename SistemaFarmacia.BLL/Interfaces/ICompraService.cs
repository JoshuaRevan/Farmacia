using SistemaFarmacia.BLL.Models;
using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface ICompraService
    {
        Task<List<CompraListaDto>> Lista();

        Task<Compra> Crear(CompraRequestDto entidad); //se cambio de Compra a CompraRequestDto

        Task<Compra> Editar(Compra entidad);

        //Task<bool> Eliminar(int idCompra);

    }
}
