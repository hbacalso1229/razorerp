using UserManagement.API.SwaggerExamples.Requests;
using UserManagement.Application.Commands.AuthenticateUser;
using UserManagement.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controller
{
    [ApiController]
    [Route("v{version:ApiVersion}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion("1.0")]
    public class AuthController : ApiControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public AuthController(ILogger<AuthController> logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            _logger = logger;
            _logger.LogInformation($"Initializing {nameof(AuthController)}...");
        }

        /// <summary>
        /// Authorize user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user access token</returns>
        [HttpPost("authorize")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK, Type = typeof(UserLoginRequestExample))]
        public async Task<ActionResult<UserLoginResponse>> AuthorizeUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var token = await Mediator.Send(new AuthorizeUserCommand { Username = request.Username, Password = request.Password}, cancellationToken);

            return Ok(new UserLoginResponse { Token = token });
        }
    }
}
