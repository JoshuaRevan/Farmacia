using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface ICargoService
    {
        Task<List<Cargo>> Listar();
    }
}
