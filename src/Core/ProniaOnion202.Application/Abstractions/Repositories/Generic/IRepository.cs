using ProniaOnion202.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool isTracking = false, , bool ignoreQuery = false, params string[] includes);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderExpression = null, bool isDescending = false, bool isTracking = false,bool ignoreQuery=false, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T, bool>>? expression);
        Task<T> GetByIdAsync(int id, bool isTracking = false,, bool ignoreQuery = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T,bool>> expression, bool isTracking = false,, bool ignoreQuery = false, params string[] includes);

        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseDelete(T entity);


        Task SaveChangeAsync();
        void Update(T existed);
    }
}
