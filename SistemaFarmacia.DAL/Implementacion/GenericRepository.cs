using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaFarmacia.DAL.DBContext;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SistemaFarmacia.DAL.Interfaces;


namespace sistemaFarmacia.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly FarmaciaTroyaContext _dbContext;

        public GenericRepository(FarmaciaTroyaContext dbContext)
        {
            _dbContext = dbContext;
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
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
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
