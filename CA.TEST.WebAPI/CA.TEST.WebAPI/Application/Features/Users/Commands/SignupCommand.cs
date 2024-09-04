using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
using System.Security.Cryptography;
using Application.Features.Users.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Security;

namespace Application.Features.Users.Commands
{
    public class SignupCommand : IRequest<Unit>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Device { get; set; }
        public string IPAddress { get; set; }
    }


    public class SignupCommandHandler : IRequestHandler<SignupCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;


        public SignupCommandHandler(IUserRepository userRepository,ISecurityService securityService)
        {
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public async Task<Unit> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _securityService.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Device = request.Device,
                IPAddress = request.IPAddress,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);

            return Unit.Value;
        }
    }
}
