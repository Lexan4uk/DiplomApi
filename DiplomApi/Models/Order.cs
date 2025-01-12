using System;
using System.Collections.Generic;

namespace DiplomApi.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string ClientPhone { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string? BoquetName { get; set; }

    public decimal BoquetPrice { get; set; }

    public string OrderState { get; set; } = null!;

    public string? Cover { get; set; }

}
