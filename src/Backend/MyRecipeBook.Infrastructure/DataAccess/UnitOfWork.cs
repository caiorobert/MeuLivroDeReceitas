using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyRecipeBookDbContext _dbcontext;

        public UnitOfWork(MyRecipeBookDbContext dbContext) => _dbcontext = dbContext;

        public async Task Commit() => await _dbcontext.SaveChangesAsync();
    }
}
