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
    public class PresentacionService : IPresentacionService
    {
        private readonly IGenericRepository<Presentacione> _PresentacioRepositorio;
        public PresentacionService(IGenericRepository<Presentacione> PresentacioRepositorio)
        {
            _PresentacioRepositorio = PresentacioRepositorio;
        }
        public async Task<List<Presentacione>> Lista()
        {
            IQueryable<Presentacione> query = await _PresentacioRepositorio.Consultar();
            return query.ToList();
        }
        public async Task<Presentacione> Crear(Presentacione entidad)
        {
            try
            {
                Presentacione presentacione_creada = await _PresentacioRepositorio.Crear(entidad);
                if(presentacione_creada.IdPresentacion == 0) 
                    throw new TaskCanceledException("No se puede crear la presentacion");

                return presentacione_creada;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Presentacione> Editar(Presentacione entidad)
        {
            try
            {
                Presentacione presentacione_encontrada = await _PresentacioRepositorio.Obtener(c => c.IdPresentacion == entidad.IdPresentacion);
                presentacione_encontrada.TipoPresentacion = entidad.TipoPresentacion;
                bool respuesta = await _PresentacioRepositorio.Editar(presentacione_encontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo modificar la Presentacion");

                return presentacione_encontrada;
            }
            catch
            {
                throw;
            }
        }

        public  async Task<bool> Eliminar(int idPresentacion)
        {
            try
            {
                Presentacione presentacione_encontrada = await _PresentacioRepositorio.Obtener(c => c.IdPresentacion == idPresentacion);
                if (presentacione_encontrada == null)
                    throw new TaskCanceledException("La Presentacion no existe");

                bool respuesta = await _PresentacioRepositorio.Eliminar(presentacione_encontrada);

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

       
    }
}
