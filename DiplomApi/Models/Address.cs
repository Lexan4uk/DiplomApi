using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public string? Address1 { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }
}
