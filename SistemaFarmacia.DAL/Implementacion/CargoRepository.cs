using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.Entity;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.DAL.DBContext;

namespace SistemaFarmacia.DAL.Implementacion
{
    public class CargoRepository : ICargoRepository
    {
        private readonly FarmaciaTroyaContext _dbContext;

        public CargoRepository(FarmaciaTroyaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Cargo>> Listar()
        {
            try
            {
                return await _dbContext.Cargos.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al listar los Cargos", ex);
            }
        }
    }
}
