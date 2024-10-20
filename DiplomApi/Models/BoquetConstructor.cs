using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class BoquetConstructor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Cover { get; set; }

    public decimal? Price { get; set; }
}
