using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.Entity;
using SistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.BLL.Interfaces;

namespace SistemaFarmacia.BLL.Implementacion
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoService(ICargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public async Task<List<Cargo>> Listar()
        {
            try
            {
                return await _cargoRepository.Listar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Cargos", ex);
            }
        }
    }
}
