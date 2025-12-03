using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace YBTTS.Infrastructure.Data;

public class YbttsDbContextFactory : IDesignTimeDbContextFactory<YbttsDbContext>
{
    public YbttsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<YbttsDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ybtts_db;Username=postgres;Password=1233");

        return new YbttsDbContext(optionsBuilder.Options);
    }
}
