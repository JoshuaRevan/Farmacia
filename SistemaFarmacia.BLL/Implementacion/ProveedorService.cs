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
    public class ProveedorService : IProveedorService
    {
        private readonly IGenericRepository<Proveedore> _ProveedorRepositorio;
        public ProveedorService(IGenericRepository<Proveedore> ProveedorRepositorio)
        {
            _ProveedorRepositorio = ProveedorRepositorio;
        }

        public async Task<List<Proveedore>> Lista()
        {
            IQueryable<Proveedore> query = await _ProveedorRepositorio.Consultar();
            return query.ToList();
        }
        public async Task<Proveedore> Crear(Proveedore entidad)
        {
            try
            {
                Proveedore proveedore_creado = await _ProveedorRepositorio.Crear(entidad);
                if(proveedore_creado.IdProveedor == 0)
                    throw new TaskCanceledException("No se puede crear el proveedor");

                return proveedore_creado;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<Proveedore> Editar(Proveedore entidad)
        {
            try
            {
               Proveedore proveedore_encontrado = await _ProveedorRepositorio.Obtener(c => c.IdProveedor == entidad.IdProveedor);
                proveedore_encontrado.NombreProveedor = entidad.NombreProveedor;
                proveedore_encontrado.TelefonoProveedor = entidad.TelefonoProveedor;
                bool respuestas = await _ProveedorRepositorio.Editar(proveedore_encontrado);

                if (!respuestas)
                    throw new TaskCanceledException("No se puede modificar el Proveedor");

                return proveedore_encontrado;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProveedor)
        {
            try
            {
                Proveedore proveedore_encontrado =  await _ProveedorRepositorio.Obtener(c => c.IdProveedor == idProveedor);
                if (proveedore_encontrado == null)
                    throw new TaskCanceledException("el Proveedor no existe");

                bool respuesta = await _ProveedorRepositorio.Eliminar(proveedore_encontrado);

                return respuesta;

            }
            catch
            {
                throw;//Q.どうしてケンカしつつもなんだかんだデート楽しかったカップルみたいに…！？
            }
        }
    }
}
