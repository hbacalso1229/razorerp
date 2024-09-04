using UserManagement.API.Controller;
using UserManagement.Application.Commands.CreateUser;
using UserManagement.Application.Commands.UpdateUser;
using UserManagement.Application.Commands.DeleteUser;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Queries;
using UserManagement.Domain.Enums;
using UserManagement.UnitTests.MockData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UserManagement.UnitTests.Tests.API
{
    public class UserControllerTest
    {
        private Mock<ILogger<UserController>> _logger = null!;
        private Mock<IHttpContextAccessor> _contextAccessor = null!;

        public UserControllerTest()
        {
            _logger = new Mock<ILogger<UserController>>();
            _contextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOkObjectResultAsync()
        {
            // Arrange
            UserResponse technicianResponse = new UserData().GetData();

            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(technicianResponse);

            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator)))
                .Returns(mediator.Object);

            UserController controller = new UserController(_logger.Object, _contextAccessor.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { RequestServices = mockServiceProvider.Object },
                }
            };

            // Act
            var result = await controller.GetUsers(CancellationToken.None);

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

            UserResponse response = Assert.IsType<UserResponse>(okObjectResult.Value);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedAtActionResultAsync()
        {
            // Arrange
            Guid userResponse = Guid.NewGuid();

            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userResponse);

            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator)))
                .Returns(mediator.Object);

            UserController controller = new UserController(_logger.Object, _contextAccessor.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { RequestServices = mockServiceProvider.Object },
                }
            };

            // Act
            var result = await controller.CreateUser(new CreateUserCommand
            {
                UserName = "Test",
                Email = "helpdesk@gmail.com",
                FirstName = "Bill",
                LastName = "Gates",
                CompanyId = Guid.NewGuid(),
                Password = "Test",
                Role = RoleType.User
            }, CancellationToken.None);

            // Assert
            CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            Guid response = Assert.IsType<Guid>(createdAtActionResult.Value);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContentResultAsync()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);

            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator)))
                .Returns(mediator.Object);

            UserController controller = new UserController(_logger.Object, _contextAccessor.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { RequestServices = mockServiceProvider.Object },
                }
            };

            // Act
            var result = await controller.UpdateUser(userId, new UpdateUserCommand
            {
                UserId = userId,
                Email = "helpdesk@gmail.com",
                FirstName = "Bill",
                LastName = "Gates",
                UserName = "Test",
                CompanyId = Guid.NewGuid(),
                Password = "Test",
                Role = RoleType.User
            }, CancellationToken.None);

            // Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);           

            Assert.NotNull(noContentResult);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContentResultAsync()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);

            Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator)))
                .Returns(mediator.Object);

            UserController controller = new UserController(_logger.Object, _contextAccessor.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext { RequestServices = mockServiceProvider.Object },
                }
            };

            // Act
            var result = await controller.DeleteUser(userId, CancellationToken.None);

            // Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);

            Assert.NotNull(noContentResult);
        }
    }
}
