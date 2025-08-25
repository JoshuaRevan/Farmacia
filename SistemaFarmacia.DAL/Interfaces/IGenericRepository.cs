using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.Entity;
using System.Linq.Expressions;

namespace SistemaFarmacia.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity:class
    {
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filto, string includeProperties = " ");

        Task<TEntity> Crear(TEntity entidad);

        Task<bool> Editar(TEntity entidad);

        Task<bool> Eliminar(TEntity entidad);

        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity,bool>>filtro = null);
    }
}
