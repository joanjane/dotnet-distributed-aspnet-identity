using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoC.DistributedAspNetIdentity.Api.Domain;
using PoC.DistributedAspNetIdentity.Api.Models;

namespace PoC.DistributedAspNetIdentity.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("check-credentials")]
        public async Task<ActionResult<UserResponse>> CheckCredentials([FromBody] CheckCredentialsRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return ValidationProblem(
                    BuildValidationError(ErrorConstants.InvalidCredentials, "User or password are invalid"));
            }

            var validCredentials = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validCredentials)
            {
                return ValidationProblem(
                    BuildValidationError(ErrorConstants.InvalidCredentials, "User or password are invalid"));
            }

            return Ok(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            });
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null)
            {
                return ValidationProblem(
                    BuildValidationError(ErrorConstants.UserAlreadyExists, $"User {request.Email} already exists"));
            }
            
            var newUser = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                UserName = request.Email,

                EmailConfirmed = true // for demo purposes
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => (errorCode: e.Code, message: e.Description));
                return ValidationProblem(BuildValidationErrors(errors));
            }

            return Ok(new UserResponse
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Name = newUser.Name,
                Surname = newUser.Surname
            });
        }

        private static ValidationProblemDetails BuildValidationError(string errorCode, string message)
        {
            return new ValidationProblemDetails(
                new Dictionary<string, string[]> { { errorCode, new[] { message } } }
            );
        }

        private static ValidationProblemDetails BuildValidationErrors(IEnumerable<(string errorCode, string message)> errors)
        {
            var err = new Dictionary<string, string[]> { };
            errors.ToList().ForEach(error =>
            {
                if (!err.ContainsKey(error.errorCode))
                {
                    err[error.errorCode] = new[] { error.message };
                } else
                {
                    err[error.errorCode].Append(error.message);
                }
            });
            return new ValidationProblemDetails(err);
        }
    }
}