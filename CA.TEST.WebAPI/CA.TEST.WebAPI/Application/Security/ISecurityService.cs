using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Security
{
    public interface ISecurityService
    {
        string HashPassword(string password);
        string GenerateJwtToken(User user);


    }
}
