using UserManagement.API.SwaggerExamples.Responses;
using UserManagement.Application.Commands.CreateUser;
using UserManagement.Application.Commands.DeleteUser;
using UserManagement.Application.Commands.UpdateUser;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace UserManagement.API.Controller
{
    [ApiController]
    [Route("v{version:ApiVersion}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        private string role;
        private Guid companyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public UserController(ILogger<UserController> logger, IHttpContextAccessor contextAccessor)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(contextAccessor);

            _logger = logger;
            _contextAccessor = contextAccessor;

            var userClaims = _contextAccessor.HttpContext.User;

            role = userClaims?.FindFirst(ClaimTypes.Role)?.Value;
            companyId = new Guid(userClaims?.FindFirst("CompanyId")?.Value);

            _logger.LogInformation($"Initializing {nameof(UserController)}...");            
        }

        /// <summary>
        /// List of users
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of users by user identity</returns>
        [Authorize(Roles = "Admin, User")]
        [HttpGet("users")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK, Type = typeof(UserResponseExample))]
        public async Task<ActionResult<UserResponse>> GetUsers(CancellationToken cancellationToken)
        {
            UserResponse response = await Mediator.Send(new GetUsersQuery(role, companyId), cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Gets specific user
        /// </summary>
        /// <param name="userId" example ="6075e99b-c6a9-430a-a8e8-3703dd5483d7">A unique identifier of an existing user</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Specific user</returns>
        [Authorize(Roles = "Admin, User")]
        [HttpGet("users/{userId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK, Type = typeof(UserResponseExample))]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute][Required()] Guid userId, CancellationToken cancellationToken)
        {
            UserDto response = await Mediator.Send(new GetUserByIdQuery(userId, companyId), cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="command">A user profile request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Newly created user</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost("users")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created, Type = typeof(CreateUserResponseExample))]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            Guid response = await Mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateUser), new { response }, response);
        }

        /// <summary>
        /// Updates a user profile
        /// </summary>
        /// <param name="userId" example ="6075e99b-c6a9-430a-a8e8-3703dd5483d7">A unique identifier of an existing user</param>
        /// <param name="command">A user profile request</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Updated existing user profile</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> UpdateUser([FromRoute][Required()] Guid userId,
            [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {

            if (userId != command.UserId)
            {
                return BadRequest($"The user id '{userId.ToString()}' route path value does not match the request payload '{command.UserId}'.");
            }

            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete existing user
        /// </summary>
        /// <param name="userId" example ="6075e99b-c6a9-430a-a8e8-3703dd5483d7">A unique identifier of an existing user</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Removed existing user profile</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteUser([FromRoute][Required()] Guid userId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteUserCommand { UserId = userId }, cancellationToken);

            return NoContent();
        }
    }
}
