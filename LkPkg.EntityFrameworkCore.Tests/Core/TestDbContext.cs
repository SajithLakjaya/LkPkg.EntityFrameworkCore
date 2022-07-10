using LkPkg.EntityFrameworkCore.Tests.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace LkPkg.EntityFrameworkCore.Tests.Core
{
    internal class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
