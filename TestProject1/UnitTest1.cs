using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoundofSilence.Controllers;
using SoundofSilence.Models;
using SoundofSilence.Services;
using System.Collections.ObjectModel;

namespace TestProject1
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void Test_Post_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var serviceContext = new ServiceContext(options))
            {
                var usersService = new UsersService(serviceContext);
                var controller = new UsersController(configuration, usersService, serviceContext);

                var userToInsert = new Users
                {
                    Name_user = "pamela",
                    Email = "pame@gmai.com",
                    Password = "123",
                    Id_rol = 2 // Puedes establecer otras propiedades según sea necesario
                };

                // Act
                var result = controller.Post(userToInsert);
                            serviceContext.SaveChanges();
                


                // Assert
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void Test_Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var serviceContext = new ServiceContext(options))
            {
                var usersService = new UsersService(serviceContext);
                var controller = new UsersController(configuration, usersService, serviceContext);

                var loginRequest = new LoginRequestModel
                {
                    Email = "pame@gmai.com",
                    Password = "123"
                };

                // Suponiendo que has configurado tu base de datos de prueba con datos de usuario de muestra

                // Act
                var result = controller.Login(loginRequest) ;

                // Assert
                var token = result;
                Assert.IsNotNull(token);
            }
        }
    }
}