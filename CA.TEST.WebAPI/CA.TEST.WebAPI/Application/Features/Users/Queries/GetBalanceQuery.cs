using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetBalanceQuery : IRequest<GetBalanceResponse>
    {
        public int UserId { get; set; }
    }

    public class GetBalanceResponse
    {
        public decimal Balance { get; set; }

    }
    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, GetBalanceResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetBalanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetBalanceResponse> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var balance = await _userRepository.GetBalanceAsync(request.UserId);
            return new GetBalanceResponse { Balance = balance };
        }
    }
}
