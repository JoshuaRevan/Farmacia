using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaFarmacia.DAL.DBContext;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.Entity;


namespace sistemaFarmacia.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly FarmaciaTroyaContext _dbContext;

        public GenericRepository(FarmaciaTroyaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro, string includeProperties = " ")
        {
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' } , StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }
                return await query.FirstOrDefaultAsync(filtro);

            }
            catch 
            {
                throw;
            }
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                Console.WriteLine($"=== DEBUG REPOSITORY ===");
                Console.WriteLine($"Entidad recibida: {entidad != null}");
                if (entidad is Producto producto)
                {
                    Console.WriteLine($"Producto.Nombre: {producto.NombreProducto}");
                    Console.WriteLine($"Producto.IdMarca: {producto.IdMarca}");
                }
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch(Exception ex)
            {
               Console.WriteLine($"Error en Crear: {ex.Message}");
        throw;
            }
        }
        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbContext.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }


    }
}
