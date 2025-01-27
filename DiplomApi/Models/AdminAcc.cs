using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class AdminAcc
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
