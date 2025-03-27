using Microsoft.EntityFrameworkCore;
using DevilFruits.DAL.DataContext;
using System.Linq.Expressions;


namespace DevilFruits.BLL.Repositories
{
    public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
    {
        private readonly AppDbContext _dbcontext;
        public GenericRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TModelo> GetAsync(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo? modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                return modelo!;
            }
            catch
            {
                throw;
            }

        }

        public async Task<TModelo> CreateAsync(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModelo>> QueryAsync(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> query = filtro == null ? _dbcontext.Set<TModelo>() :
                    _dbcontext.Set<TModelo>().Where(filtro);
                return query;
            }
            catch
            {
                throw;
            }
        }

    }
}