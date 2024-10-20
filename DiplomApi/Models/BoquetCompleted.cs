using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class BoquetCompleted
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public int? OldPrice { get; set; }

    public bool? Promo { get; set; }

    public string? Cover { get; set; }

    public string? Description { get; set; }
}
