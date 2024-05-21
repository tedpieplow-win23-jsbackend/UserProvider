using System;
using System.Collections.Generic;

namespace UserProvider.Entities;

public partial class Address
{
    public string Id { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}
