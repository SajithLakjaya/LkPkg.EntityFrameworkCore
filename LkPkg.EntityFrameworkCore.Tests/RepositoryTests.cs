using LkPkg.EntityFrameworkCore.Abstractions.Interfaces;
using LkPkg.EntityFrameworkCore.Repository;
using LkPkg.EntityFrameworkCore.Tests.Core;
using LkPkg.EntityFrameworkCore.Tests.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace LkPkg.EntityFrameworkCore.Tests
{
    public class RepositoryTests
    {
        private TestDbContext _context;
        private IRepository<TestEntity> _repository;

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
        public async Task InsertBulkTest()
        {

            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();

            await _repository.InsertRangeAsync(new[]
            {
                new TestEntity()
                {
                    Name = name1
                },
                new TestEntity()
                {
                    Name = name2
                }
            });

            await _context.SaveChangesAsync();

            //Test cases

            var entity1 = _repository.FindByCondition(q => q.Name == name1).FirstOrDefault();
            var entity2 = _repository.FindByCondition(q => q.Name == name2).FirstOrDefault();

            //Test cases

            if (entity1 == null || entity2 == null)
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
        public async Task BulkUpdateTest()
        {
            //Seed data

            var entity1 = await SeedData();
            var entity2 = await SeedData();

            //Update the name property

            var newNameforEntity1 = Guid.NewGuid().ToString();
            entity1.Name = newNameforEntity1;

            var newNameForEntity2 = Guid.NewGuid().ToString();
            entity2.Name = newNameForEntity2;

            _repository.UpdateRange(new[] { entity1, entity2 });

            await _context.SaveChangesAsync();

            var updatedEntity1 = await _repository.FindByIdAsync(entity1.Id);
            var updatedEntity2 = await _repository.FindByIdAsync(entity2.Id);

            //Test cases

            if (updatedEntity1 == null || updatedEntity2 == null)
            {
                Assert.Fail("Entity not found");
                return;
            }

            if (updatedEntity1.Name == newNameforEntity1 && updatedEntity2.Name == newNameForEntity2)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task RemoveTest()
        {
            //Seed data

            var entity = await SeedData();

            var deletingEntity = await _repository.FindByIdAsync(entity.Id);

            _repository.Remove(deletingEntity);
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
        public async Task BulkRemoveTest()
        {
            //Seed data

            var entity1 = await SeedData();
            var entity2 = await SeedData();

            _repository.RemoveRange(new[] { entity1, entity2 });
            await _context.SaveChangesAsync();

            var deletedEntity1 = await _repository.FindByIdAsync(entity1.Id);
            var deletedEntity2 = await _repository.FindByIdAsync(entity2.Id);

            if (deletedEntity1 == null && deletedEntity2 == null)
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
            var entity = await SeedData();

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
            _repository = new Repository<TestEntity>(_context);
        }

        #endregion


    }
}