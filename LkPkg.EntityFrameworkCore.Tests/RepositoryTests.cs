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

        #region Configurations

        [OneTimeSetUp]
        public void Setup()
        {
            ConfigureRepository();
        }

        #endregion

        #region Tests

        [Test]
        public async Task InsertTest()
        {
            //Add new entity

            var name = Guid.NewGuid().ToString();

            await _repository.InsertAsync(new TestEntity()
            {
                Name = name
            });

            await _context.SaveChangesAsync();

            var entity = _repository.FindByCondition(q => q.Name == name).FirstOrDefault();

            //Test cases

            if (entity == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public async Task UpdateTest()
        {
            //Seed data

            var entity = await SeedData();

            //Update the name property

            var newName = Guid.NewGuid().ToString();
            entity.Name = newName;

            _repository.Update(entity);

            await _context.SaveChangesAsync();

            var updatedEntity = await _repository.FindByIdAsync(entity.Id);

            //Test cases

            if (updatedEntity == null)
            {
                Assert.Fail("Entity not found");
                return;
            }

            if (updatedEntity.Name == newName)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }

        }

        [Test]
        public async Task DeleteTest()
        {
            //Seed data

            var entity = await SeedData();

            await _repository.DeleteAsync(entity.Id);
            await _context.SaveChangesAsync();

            var deletedEntity = await _repository.FindByIdAsync(entity.Id);

            if (deletedEntity == null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task GetByIdTest()
        {
            var entity = SeedData();

            var retrievalEntity = await _repository.FindByIdAsync(entity.Id);

            if (retrievalEntity != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        #endregion

        #region Private Methods

        private async Task<TestEntity> SeedData()
        {
            var name = Guid.NewGuid().ToString();

            //Add new entity

            var entity = new TestEntity()
            {
                Name = name
            };

            await _repository.InsertAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        private void ConfigureRepository()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(new Guid().ToString())
                .Options;
            _context = new TestDbContext(options);
            _repository = new GenericRepository<TestEntity>(_context);
        }

        #endregion


    }
}