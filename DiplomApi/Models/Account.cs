using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class Account
{
    public Guid Token { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual UserProfile? UserProfile { get; set; }
}
