using MediatR;
using Microsoft.EntityFrameworkCore;
using PulContent.Application.Features.RegisterUser.DTOs;
using PulContent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.LoginUser.Commands;

public class LoginCommandHandler(
        IAppDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email or Password wrong!");

        var token = jwtTokenGenerator.GenerateJwtToken(user.Id, user.Email, user.FullName);

        return new AuthResponseDto(token);
    }
}
