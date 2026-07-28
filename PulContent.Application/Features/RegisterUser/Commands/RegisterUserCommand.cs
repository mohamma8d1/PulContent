using MediatR;
using PulContent.Application.Features.RegisterUser.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.RegisterUser.Commands;

public record RegisterUserCommand(string Email, string Password, string FullName): IRequest<AuthResponseDto>;

