using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using sistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.DAL.DBContext;
using SistemaFarmacia.Entity;

namespace SistemaFarmacia.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly FarmaciaTroyaContext _dbContext;

        public VentaRepository(FarmaciaTroyaContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta entidad)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = await _dbContext.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                try
                {
                    // Crear la venta en la base de datos
                    _dbContext.Ventas.Add(entidad);
                    await _dbContext.SaveChangesAsync();

                    // Recorrer los productos vendidos
                    foreach (var item in entidad.Transacciones)
                    {
                        // Obtener el stock actual sumando todas las transacciones previas
                        int idProducto = item.IdProducto ?? throw new Exception("El producto no tiene un ID válido.");
                        int stockDisponible = _dbContext.Transacciones
                            .Where(t => t.IdProducto == idProducto)
                            .Sum(t => t.Cantidad ?? 0); // SUMA todas las entradas y salidas

                        if (stockDisponible < item.Cantidad)
                        {
                            throw new Exception($"No hay suficiente stock para el producto {item.IdProducto}");
                        }

                        // Registrar la transacción de venta con cantidad negativa (salida)
                        var nuevaTransaccion = new Transaccione
                        {
                            TipoTransaccion = "Venta",
                            FechaHoraTransaccion = DateTime.Now,
                            IdProducto = item.IdProducto,
                            IdVenta = entidad.IdVenta,
                            Cantidad = -item.Cantidad // Salida del stock
                        };

                        _dbContext.Transacciones.Add(nuevaTransaccion);
                    }

                    await _dbContext.SaveChangesAsync();
                    await  transaction.CommitAsync();

                    return ventaGenerada;
                }
                catch (Exception ex) 
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error al registrar la venta", ex);

                }
            }
            
        }

        public async Task<List<Venta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                var reporteVentas = await _dbContext.Ventas
                    .Where(v => v.FechaHoraVenta >= FechaInicio && v.FechaHoraVenta <= FechaFin)
                    .Select(v => new Venta
                    {
                        IdVenta = v.IdVenta,
                        FechaHoraVenta = v.FechaHoraVenta,
                        TotalVenta = v.TotalVenta,
                        IdUsuario = v.IdUsuario // Solo traemos el ID del usuario
                    })
                    .ToListAsync();

                return reporteVentas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el reporte de ventas", ex);
            }
        }
    }
}
