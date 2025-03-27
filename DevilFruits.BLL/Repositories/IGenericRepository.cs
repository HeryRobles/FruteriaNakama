using System.Linq.Expressions;

namespace DevilFruits.BLL.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> filtro);
        Task<TModel> CreateAsync(TModel modelo);
        Task<bool> UpdateAsync(TModel modelo);
        Task<bool> DeleteAsync(TModel modelo);
        Task<IQueryable<TModel>> QueryAsync(Expression<Func<TModel, bool>> filtro = null!);
    }
}