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
    public class MarcaService : IMarcaService
    {
        private readonly IGenericRepository<Marca> _marcaRepositorio;
        public MarcaService(IGenericRepository<Marca> marcaRepositorio)
        {
            _marcaRepositorio = marcaRepositorio;
        }

        public async Task<List<Marca>> Lista()
        {
            IQueryable<Marca> query = await _marcaRepositorio.Consultar();
            return query.ToList();
        }

        public async Task<Marca> Crear (Marca entidad)
        {
            try
            {
                Marca marca_creada = await _marcaRepositorio.Crear(entidad);
                if (marca_creada.IdMarca == 0)
                    throw new TaskCanceledException("No se puede crear la marca");

                return marca_creada;

            }
            catch
            {
                throw;
            }
        }

        public async Task<Marca> Editar(Marca entidad)
        {
            try
            {
                Marca marca_encontrada = await _marcaRepositorio.Obtener(c => c.IdMarca == entidad.IdMarca);
                marca_encontrada.NombreMarca = entidad.NombreMarca;
                bool respuesta = await _marcaRepositorio.Editar(marca_encontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se puedo modificar la marca");

                return marca_encontrada;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar (int idMarca)
        {
            try
            {
                Marca marca_encontrada = await _marcaRepositorio.Obtener(c => c.IdMarca == idMarca);
                if (marca_encontrada == null)
                    throw new TaskCanceledException("la Marca no existe");

                bool respuesta = await _marcaRepositorio.Eliminar(marca_encontrada);

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
