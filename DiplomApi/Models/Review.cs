using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class Review
{
    public Guid Id { get; set; }

    public string ClientName { get; set; } = null!;

    public string BoquetName { get; set; } = null!;

    public string Text { get; set; } = null!;

    public Guid OrderId { get; set; }

    public int? Rating { get; set; }

    public virtual Order? Order { get; set; }
}
