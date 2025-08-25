using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.Entity;
using SistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.AplicacionWed.Models.ViewModels;

namespace SistemaFarmacia.BLL.Implementacion
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _ProductoRespositorio; 
        private readonly IGenericRepository<Transaccione> _TransaccioneRespositorio;
        public ProductoService(IGenericRepository<Producto> productoRespositorio, IGenericRepository<Transaccione> transaccioneRespositorio)
        {
            _ProductoRespositorio = productoRespositorio;
            _TransaccioneRespositorio = transaccioneRespositorio;
        }

        public async Task<List<VMProducto>> Lista()
        {

        }

        public async Task<Producto> Crear(Producto entidad)
        {
            try
            {
                Producto producto_creado = await _ProductoRespositorio.Crear(entidad);
                if (producto_creado.IdProducto == 0)
                    throw new TaskCanceledException("No se puede crear el Producto");

                return producto_creado;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<Producto> Editar(Producto entidad)
        {
            try

            {
                var productoEncontrado = await _ProductoRespositorio.Obtener(p => p.IdProducto == entidad.IdProducto);
                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe.");

                productoEncontrado.NombreProducto = entidad.NombreProducto;
                productoEncontrado.Descripcion = entidad.Descripcion;
                productoEncontrado.Ubicacion = entidad.Ubicacion;
                productoEncontrado.IdMarca = entidad.IdMarca;
                productoEncontrado.IdPresentacion = entidad.IdPresentacion;

                var respuesta = await _ProductoRespositorio.Editar(productoEncontrado);
                if (!respuesta)
                    throw new TaskCanceledException("No se puede modificar el producto.");

                return productoEncontrado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                var productoencontrado =  await _ProductoRespositorio.Obtener(p => p.IdProducto == idProducto);
                if (productoencontrado == null)
                    throw new TaskCanceledException("El producto no existe.");

                //Validar si tiene moviminetos en Transacciones
                var movimientos = (await _TransaccioneRespositorio.Consultar(t => t.IdProducto == idProducto)).Any();
                if (movimientos)
                    throw new TaskCanceledException("No se puede eliminar, el producto tiene movimientos ya realiazados.");

                //Si no tiene Movientos Procede a eliminar 
                var eliminado = await _ProductoRespositorio.Eliminar(productoencontrado);
                return eliminado;
            }
            catch(Exception ex) 
            {
                throw new Exception($"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
