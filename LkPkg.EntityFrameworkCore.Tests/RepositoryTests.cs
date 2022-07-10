using LkPkg.EntityFrameworkCore.Repository;
using LkPkg.EntityFrameworkCore.Tests.Core;
using LkPkg.EntityFrameworkCore.Tests.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace LkPkg.EntityFrameworkCore.Tests
{
    public class RepositoryTests
    {
        private TestDbContext _context;
        private IGenericRepository<TestEntity> _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            ConfigureRepository();
        }

        [Test]
        public async Task InsertTest()
        {
            var name = Guid.NewGuid().ToString();

            await _repository.InsertAsync(new TestEntity()
            {
                Name = name
            });

            await _context.SaveChangesAsync();

            var entity = _repository.FindByCondition(q => q.Name == name).FirstOrDefault();

            if (entity == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        private void ConfigureRepository()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(new Guid().ToString())
                .Options;
            _context = new TestDbContext(options);
            _repository = new GenericRepository<TestEntity>(_context);
        }
    }
}