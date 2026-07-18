using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Domain.Entities;

public class CreditTransaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public TransactionType Type { get; private set; }
    public int Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;

    public User User { get; private set; } = null!;
}
