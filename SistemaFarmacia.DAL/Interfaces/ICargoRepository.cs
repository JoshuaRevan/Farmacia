using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaFarmacia.Entity;

namespace SistemaFarmacia.DAL.Interfaces
{
    public interface ICargoRepository
    {
        Task<List<Cargo>> Listar();
    }
}
