using Microsoft.EntityFrameworkCore;

namespace TestGeocoding.DataAccess
{
    public class CacheContext : DbContext
    {
        public CacheContext(DbContextOptions<CacheContext> opts)
            :base(opts) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
