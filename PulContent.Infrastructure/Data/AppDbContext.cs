using Microsoft.EntityFrameworkCore;
using PulContent.Application.Interfaces;
using System;

namespace PulContent.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
}
