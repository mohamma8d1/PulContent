using System;

namespace PulContent.Domain.Entities;

public class User : BaseEntity
{
    public string Email {  get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;

    public int CreditBalance { get; private set; }

    // For Race Condition
    public byte[]? RowVersion { get; set; }

    public ICollection<CreditTransaction> CreditTransactions { get; set; } = new List<CreditTransaction>();
    public ICollection<MediaAsset> MediaAssets { get; set; } = new List<MediaAsset>();

}
