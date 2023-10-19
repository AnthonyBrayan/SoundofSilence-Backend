using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundofSilence.Controllers;
using SoundofSilence.IServices;
using SoundofSilence.Services;

namespace TestProject1
{
    [TestClass]
    public class CategoryControllerTests
    {
        private ServiceContext _serviceContext;
        private ICategoryService _categoryService;
        private CategoryController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _serviceContext = new ServiceContext(options);
            _categoryService = new CategoryService(_serviceContext);
            _controller = new CategoryController(_categoryService, _serviceContext);
        }

        [TestMethod]
        public void PostCategory_ReturnsOkResult_WhenCategoryIsValid()
        {
            // Arrange
            var category = new Category();

            // Act
            var result = _controller.PostCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

    }





}
