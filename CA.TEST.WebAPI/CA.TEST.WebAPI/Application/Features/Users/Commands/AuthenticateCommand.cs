using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Application.Security;

namespace Application.Features.Users.Commands
{
    public class AuthenticateCommand : IRequest<AuthenticateResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
    }

    public class AuthenticateResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;


        public AuthenticateCommandHandler(IUserRepository userRepository, IConfiguration configuration,ISecurityService securityService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _securityService= securityService;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _securityService.HashPassword(request.Password);
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || user.Password != passwordHash)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _securityService.GenerateJwtToken(user);

            var log = new LoginLog
            {
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IPAddress = request.IPAddress,
                Device = request.Device,
                Browser = request.Browser
            };
            await _userRepository.AddLoginLogAsync(log);

            if (await _userRepository.IsFirstLoginAsync(user.Id))
            {
                await _userRepository.UpdateBalanceAsync(user.Id, 5.0m);
                user.Balance += 5.0m;
            }

            return new AuthenticateResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };
        }

    }
}
