using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.Entity;
using SistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.BLL.Interfaces;
using SistemaFarmacia.BLL.Models;
using Microsoft.EntityFrameworkCore;


namespace SistemaFarmacia.BLL.Implementacion
{
    public class CompraService : ICompraService
    { 
        private readonly IGenericRepository<Compra> _compraRepositorio;
        private readonly IGenericRepository<Transaccione> _TransaccioneRespositorio;
        private readonly IGenericRepository<PrecioProducto> _PrecioProductoRespositorio;
        public CompraService(IGenericRepository<Compra> compraRepositorio, IGenericRepository<Transaccione> transaccioneRespositorio,
            IGenericRepository<PrecioProducto> precioProductoRespositorio)
        {
            _compraRepositorio = compraRepositorio;
            _TransaccioneRespositorio = transaccioneRespositorio;
            _PrecioProductoRespositorio = precioProductoRespositorio;

        }

        public async Task<List<CompraListaDto>> Lista()
        {
            try
            {
                // 1. Obtener compras con relaciones básicas
                var comprasQuery = await _compraRepositorio.Consultar();

                var comprasConRelaciones = comprasQuery
                    .Include(c => c.IdProveedorNavigation)
                    .Include(c => c.IdProductoNavigation);

                var compras = await comprasConRelaciones.ToListAsync();

                // 2. Cargar precios
                var preciosQuery = await _PrecioProductoRespositorio.Consultar();
                var precios = await preciosQuery.ToListAsync();

                // 3. Mapeo manual (sin productos separados - usar lo que ya viene en las navegaciones)
                var resultado = new List<CompraListaDto>();

                foreach (var compra in compras)
                {
                    var dto = new CompraListaDto
                    {
                        IdCompra = compra.IdCompra,
                        TotalCompra = compra.TotalCompra,
                        CantidadProductosCompra = compra.CantidadProductosCompra,
                        LoteInterno = compra.LoteInterno,
                        LoteProveedor = compra.LoteProveedor,
                        FechaHoraCompra = compra.FechaHoraCompra,
                        fechaVencimiento = compra.FechaVencimiento,

                        // Proveedor
                        IdProveedor = compra.IdProveedor,
                        NombreProveedor = compra.IdProveedorNavigation?.NombreProveedor ?? "Proveedor no encontrado",

                        // Producto
                        IdProducto = compra.IdProducto,
                        NombreProducto = compra.IdProductoNavigation?.NombreProducto ?? "Producto no encontrado",

                        // Marca y Presentación (de las navegaciones que ya incluiste)
                        IdMarca = compra.IdProductoNavigation?.IdMarca ?? 0,
                        NombreMarca = compra.IdProductoNavigation?.IdMarcaNavigation?.NombreMarca ?? "Sin marca",
                        IdPresentacion = compra.IdProductoNavigation?.IdPresentacion ?? 0,
                        TipoPresentacion = compra.IdProductoNavigation?.IdPresentacionNavigation?.TipoPresentacion ?? "Sin presentación",

                        // Precio
                        Precio = precios.FirstOrDefault(pp =>
                            pp.IdProducto == compra.IdProducto &&
                            pp.IdProveedor == compra.IdProveedor)?.Precio ?? 0,

                        IdPrecioProducto = precios.FirstOrDefault(pp =>
                            pp.IdProducto == compra.IdProducto &&
                            pp.IdProveedor == compra.IdProveedor)?.IdPrecioProducto ?? 0
                    };

                    resultado.Add(dto);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de compras: {ex.Message}");
            }
        }

        public async Task<Compra> Crear(CompraRequestDto modelo)
        {
            try
            {
                if (modelo.IdProveedor <= 0)
                {
                    throw new ArgumentException("Debe de seleccionar un proveedor valido.");
                }
                if (modelo.IdProducto <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un producto válido.");
                }
                if (modelo.TotalCompra == null || modelo.TotalCompra <= 0)
                {
                    throw new ArgumentException("El precio de compra debe ser mayor a cero.");
                }
                if (modelo.CantidadProductosCompra == null || modelo.CantidadProductosCompra <= 0)
                {
                    throw new ArgumentException("Debe de ingresar una cantidad valida.");
                }
                if (modelo.Precio == null || modelo.Precio <= 0)
                {
                    throw new ArgumentException("El precio de venta debe ser mayor a cero.");
                }

                //calculate the total
                decimal precioUnitarioCalculado = modelo.TotalCompra.Value / modelo.CantidadProductosCompra.Value;

                //Crear la compra
                var nuevaCompra = new Compra
                {
                   IdProveedor = modelo.IdProveedor,
                   IdProducto = modelo.IdProducto,
                   TotalCompra = modelo.TotalCompra.Value,
                   CantidadProductosCompra = modelo.CantidadProductosCompra,
                   LoteInterno = modelo.LoteInterno,
                   LoteProveedor = modelo.LoteProveedor,
                   FechaHoraCompra = modelo.FechaHoraCompra ?? DateTime.Now,
                   FechaVencimiento = modelo.fechaVencimiento
                };

                var CompraCreada = await _compraRepositorio.Crear(nuevaCompra);

                if (CompraCreada.IdCompra == 0)
                    throw new TaskCanceledException("No se puede registrar la compra.");


                // Actualizar o Crear precio precio de vente , ademas de Buscar existentente 
                // precio existente para este producto-proveedor 
                var preciosQuery = await _PrecioProductoRespositorio.Consultar();
                var preciosExistente = await preciosQuery
                    .FirstOrDefaultAsync(p => p.IdProducto == modelo.IdProducto && p.IdProveedor == modelo.IdProveedor);

                if(preciosExistente != null)
                {
                    preciosExistente.Precio = modelo.Precio.Value;
                    await _PrecioProductoRespositorio.Editar(preciosExistente);
                }
                else
                {
                    var nuevoPrecio = new PrecioProducto
                    {
                        IdProducto = modelo.IdProducto,
                        IdProveedor = modelo.IdProveedor,

                        Precio = modelo.Precio.Value
                    };
                    await _PrecioProductoRespositorio.Crear(nuevoPrecio);
                }

                var transaccionesQuery = await _TransaccioneRespositorio.Consultar();
                var transaccionesExistente = transaccionesQuery
                    .FirstOrDefault(t =>
                    t.IdProducto == modelo.IdProducto &&
                    t.TipoTransaccion == "Compra" &&
                    t.IdCompra == null);

                if (transaccionesExistente != null)
                {
                    transaccionesExistente.Cantidad += modelo.CantidadProductosCompra.Value;
                    transaccionesExistente.FechaHoraTransaccion = DateTime.Now;
                    transaccionesExistente.IdCompra = CompraCreada.IdCompra;
                    await _TransaccioneRespositorio.Editar(transaccionesExistente);
                }
                else
                {
                    var nuevaTransaccion = new Transaccione
                    {
                        IdProducto = modelo.IdProducto,
                        IdCompra = CompraCreada.IdCompra,
                        Cantidad = modelo.CantidadProductosCompra.Value,
                        TipoTransaccion = "Compra",
                        FechaHoraTransaccion = DateTime.Now
                    };
                    await _TransaccioneRespositorio.Crear(nuevaTransaccion);
                }

                return CompraCreada;

            }
            catch (Exception ex) 
            { 
                throw new Exception("Error al creae compra", ex);            
            }
        }

        public async Task<Compra> Editar(Compra entidad)
        {
            try
            {
                var compraEncontrada = await _compraRepositorio.Obtener(c => c.IdCompra == entidad.IdCompra);
                if (compraEncontrada == null)
                    throw new TaskCanceledException("La compra no existe. ");

                compraEncontrada.LoteInterno = entidad.LoteInterno;
                compraEncontrada.LoteProveedor = entidad.LoteProveedor;
                compraEncontrada.FechaVencimiento = entidad.FechaVencimiento;
                compraEncontrada.TotalCompra = entidad.TotalCompra;

                var respuesta = await _compraRepositorio.Editar(compraEncontrada);
                if (!respuesta)
                    throw new TaskCanceledException("No se puede modificar la compra.");
                return compraEncontrada;

            }
            catch
            { 
                throw;
            }
        }
    }
}
