using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Domain.Entities;

public class CreditTransaction : BaseEntity
{
    public Guid UserId { get; init; }
    public TransactionType Type { get; init; }
    public int Amount { get; init; }
    public string Description { get; init; } = string.Empty;

    public User User { get; init; } = null!;
}
