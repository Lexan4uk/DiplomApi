using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class BoquetCompleted
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Link { get; set; } = null!;

    public decimal? Price { get; set; }

    public decimal? OldPrice { get; set; }

    public bool? Promo { get; set; }

    public string? Cover { get; set; }

    public string? Description { get; set; }

    public string? Composition { get; set; }
}
