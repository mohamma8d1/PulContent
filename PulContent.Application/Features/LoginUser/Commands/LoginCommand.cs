using MediatR;
using PulContent.Application.Features.RegisterUser.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.LoginUser.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;

