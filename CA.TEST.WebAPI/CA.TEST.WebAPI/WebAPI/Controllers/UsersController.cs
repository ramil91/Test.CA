using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Signup a new user.
        /// </summary>
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var command = new SignupCommand
            {
                Username = request.Username,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Device = request.Device,
                IPAddress = request.IPAddress
            };

            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Authenticate a user and generate JWT token.
        /// </summary>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var command = new AuthenticateCommand
            {
                Username = request.Username,
                Password = request.Password,
                IPAddress = request.IPAddress,
                Device = request.Device,
                Browser = request.Browser
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Get user balance.
        /// </summary>
        [Authorize]
        [HttpPost("auth/balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var query = new GetBalanceQuery { UserId = userId };
            var response = await _mediator.Send(query);
            return Ok(new { Balance = response.Balance.ToString("F2") });
        }
    }

}
