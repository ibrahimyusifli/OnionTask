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
        IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderExpression = null, bool isDescending = false, bool isTracking = false, params string[] includes);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void Delete(T entity);
        Task SaveChangeAsync();
        void Update(T existed);
    }
}
