using MediatR;
using Microsoft.EntityFrameworkCore;
using PulContent.Application.Features.RegisterUser.DTOs;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.RegisterUser.Commands;

public class RegisterUserCommandHandler(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var isEmailExist = await dbContext.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);
        if (isEmailExist)
            throw new Exception("This email already exists");

        var hashedPassword = passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            FullName = request.FullName,
            CreditBalance = 5
        }; 

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtTokenGenerator.GenerateJwtToken(user.Id, user.Email, user.FullName);

        return new AuthResponseDto(token);
    }
}
