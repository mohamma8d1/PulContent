using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(Guid userId, string email, string fullName);
}
