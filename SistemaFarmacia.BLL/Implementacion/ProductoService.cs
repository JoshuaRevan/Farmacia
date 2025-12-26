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
using SistemaFarmacia.BLL.Models;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<List<ProductoListaDto>> Lista()
        {
            try
            {
                IQueryable<Producto> query = await _ProductoRespositorio.Consultar();

               query = query
                    .Include(p => p.IdMarcaNavigation)
                    .Include(p => p.IdPresentacionNavigation)
                    .Include(p => p.Transacciones)
                        .ThenInclude(t => t.IdCompraNavigation)
                    .Include(p => p.PrecioProductos);

                var productos = await query
                    .Select(p => new ProductoListaDto
                    {
                        IdProducto = p.IdProducto,
                        NombreProducto = p.NombreProducto,
                        Descripcion = p.Descripcion,
                        Ubicacion = p.Ubicacion,
                        IdMarca = p.IdMarca,
                        NombreMarca = p.IdMarcaNavigation.NombreMarca,
                        IdPresentacion = p.IdPresentacion,
                        NombrePresentacion = p.IdPresentacionNavigation.TipoPresentacion,


                        // Cantidad
                        Cantidad = p.Transacciones.Sum(t => (int?)t.Cantidad ?? 0),

                        //Precio
                        Precio = p.PrecioProductos
                        .OrderByDescending(pp => pp.IdPrecioProducto)
                        .Select(pp => pp.Precio)
                        .FirstOrDefault(),
                        //Fecha de Vencimineto del Producto
                        FechaVencimiento = p.Transacciones
                        .Where(t => t.TipoTransaccion == "Compras" && t.IdCompraNavigation != null)
                        .OrderBy(t => t.IdCompraNavigation.FechaVencimiento)
                        .Select(t => t.IdCompraNavigation.FechaVencimiento)
                        .FirstOrDefault(),
                        //Lote Interno asignado
                        LoteInterno = p.Transacciones
                        .Where(t => t.TipoTransaccion == "Compras" && t.IdCompraNavigation != null)
                        .OrderByDescending(t => t.FechaHoraTransaccion)
                        .Select(t => t.IdCompraNavigation.LoteInterno)
                        .FirstOrDefault()
                    })
                    .ToListAsync();

                foreach (var producto in productos)
                {
                    // Puede editar si no tiene stock y no tiene precio
                    producto.PuedeEditar = (producto.Cantidad ?? 0) == 0 && (producto.Precio ?? 0) == 0;

                    // Puede eliminar si no tiene stock
                    producto.PuedeEliminar = (producto.Cantidad ?? 0) == 0;

                    /*producto.PuedeEditar = producto.Cantidad == 0 &&
                                           producto.Precio == 0 &&
                                           producto.FechaVencimiento == default &&
                                           producto.LoteInterno == null;

                    producto.PuedeEliminar = producto.Cantidad == 0;*/
                }

                return productos;

            }
            catch(Exception ex) 
            {
                throw new Exception($"Error al obtener la lista de productos: {ex.Message}");
            }

        }

        public async Task<Producto> Crear(Producto entidad)
        {
            try
            {
                Producto producto_creado = await _ProductoRespositorio.Crear(entidad);

                if(producto_creado.IdProducto == 0)
                    throw new TaskCanceledException("No se puee crear el producto");

                var query = await _ProductoRespositorio.Consultar(p => p.IdProducto == producto_creado.IdProducto);

                producto_creado = await query
                    .Include(p =>  p.IdMarcaNavigation)
                    .Include(p => p.IdPresentacionNavigation)
                    .FirstOrDefaultAsync();

                return producto_creado; 
            }
            catch(Exception ex)
            {
                throw new Exception("Error al crear producto", ex);
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

        public async Task<bool> PuedeEditar(int idProducto)
        {
            try
            {
                var producto = await _ProductoRespositorio.Obtener(p => p.IdProducto == idProducto);

                if (producto == null)
                    return false;

                var transacciones = await (await _TransaccioneRespositorio.Consultar(t => t.IdProducto == idProducto)).ToListAsync();

               bool tieneVencimiento = transacciones.Any(t => t.IdCompraNavigation != null && 
               t.IdCompraNavigation.FechaHoraCompra != null);

                bool tieneLoteInterno = transacciones.Any(t => t.IdCompraNavigation != null &&
                t.IdCompraNavigation.LoteInterno.HasValue);

                if (tieneLoteInterno || tieneVencimiento)
                    return false;

                return true;

            }
            catch(Exception) 
            {

                return false;
            }
        }

        public async Task<bool> PuedeEliminar(int idProducto)
        {
            try
            {
                var producto = await _ProductoRespositorio.Obtener(p => p.IdProducto == idProducto);

                if (producto == null) 
                    return false;

                var transacciones = await (await _TransaccioneRespositorio.Consultar(t => t.IdProducto == idProducto)).ToListAsync();

                // Si ya hubo transacciones → nunca permitir eliminar
                if (transacciones.Any())
                    return false;

                // Si ya tiene precios asociados → tampoco permitir eliminar
                if (producto.PrecioProductos.Any())
                    return false;

                return true;

            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}
