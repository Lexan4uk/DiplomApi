using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class UserProfile
{
    public Guid Token { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Account TokenNavigation { get; set; } = null!;
}
