using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Entities;
using SoundofSilence.Services;
using SoundofSilence.IServices;
using SoundofSilence.Models;
using Microsoft.AspNetCore.Mvc;
using Data;
using SoundofSilence.Controllers;
using Microsoft.Extensions.Configuration;

namespace TestProject1
{
    [TestClass]
    public class UsersTest
    {
        [TestMethod]
        public void InsertUsers_ReturnsCorrectId()
        {
            // Arrange 
            //var mockServiceContext = new Mock<IServiceContext>();
            //var mockUsersService = new Mock<IUsersService>();
            //var mockIConfiguration = new Mock<IConfiguration>();
            //var user = new Users()
            //{
            //    Name_user = "Rebe",
            //    Email = "rebe@solquemado",
            //    Id_rol = 1,
            //    Id_user = 1,




            //};
            //var expectedId = 1;

            //mockUsersService.Setup(service => service.InsertUsers(user)).Returns(expectedId);

            //var controller = new UsersController(mockIConfiguration.Object, mockUsersService.Object, mockServiceContext.Object);


            //// Act
            //var result = controller.Post(user) as OkObjectResult;

            //// Assert
            //Assert.AreEqual(expectedId, result.Value);


            // Arrange 
            var mockServiceContext = new Mock<IServiceContext>();
            var mockUsersService = new Mock<IUsersService>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var user = new Users()
            {
                Name_user = "Rebe",
                Email = "rebe@solquemado",
                Id_rol = 1,
                Id_user = 1,
            };
            var expectedId = 1;

            mockUsersService.Setup(service => service.InsertUsers(user)).Returns(expectedId);

            var controller = new UsersController(mockIConfiguration.Object, mockUsersService.Object, mockServiceContext.Object);

            // Act
            var result = controller.Post(user) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedId, result.Value);



        }

        [TestMethod]
        public void DeleteUser_ReturnsNoContent()
        {
            // Arrange
            var mockServiceContext = new Mock<IServiceContext>();
            var mockUsersService = new Mock<IUsersService>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var userId = 1;

            mockUsersService.Setup(service => service.DeleteUser(userId)).Returns(true);

            var controller = new UsersController(mockIConfiguration.Object, mockUsersService.Object, mockServiceContext.Object);

            // Act
            var result = controller.Delete(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
