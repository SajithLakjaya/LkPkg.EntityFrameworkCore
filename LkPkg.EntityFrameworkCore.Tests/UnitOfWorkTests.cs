using LkPkg.EntityFrameworkCore.Abstractions.Interfaces;
using LkPkg.EntityFrameworkCore.Tests.Core;
using LkPkg.EntityFrameworkCore.Tests.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace LkPkg.EntityFrameworkCore.Tests
{
    public class UnitOfWorkTests
    {
        private TestDbContext _context;
        private IUnitOfWork _unitOfWork;

        #region Configurations

        [OneTimeSetUp]
        public void Setup()
        {
            ConfigureUnitOfWork();
        }

        #endregion

        #region Tests

        [Test]
        public void GetRepositoryTest()
        {
            var testEntityRepository = _unitOfWork.Repository<TestEntity>();
            Assert.IsNotNull(testEntityRepository);
        }

        [Test]
        public void NotCreateDuplicateRespositoryTest()
        {
            var testEntityRepository = _unitOfWork.Repository<TestEntity>();
            Assert.IsNotNull(testEntityRepository);

            var testEntityRepository1 = _unitOfWork.Repository<TestEntity>();
            Assert.IsNotNull(testEntityRepository1);

            Assert.AreEqual(testEntityRepository, testEntityRepository1);
        }

        [Test]
        public void ShouldCreateDifferentRepositoryForDifferentEntity()
        {
            var testEntityRepository = _unitOfWork.Repository<TestEntity>();
            var testEntityRepository1 = _unitOfWork.Repository<TestEntity1>();

            Assert.IsNotNull(testEntityRepository);
            Assert.IsNotNull(testEntityRepository1);

            Assert.AreNotEqual(testEntityRepository, testEntityRepository1);
        }

        #endregion


        #region Private Methods

        private void ConfigureUnitOfWork()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(new Guid().ToString())
                .Options;
            _context = new TestDbContext(options);
            _unitOfWork = new UnitOfWork.UnitOfWork(_context);
        }
    }

    #endregion
}
