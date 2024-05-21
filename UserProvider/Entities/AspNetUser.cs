using System;
using System.Collections.Generic;

namespace UserProvider.Entities;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string? Biography { get; set; }

    public DateTime? Created { get; set; }

    public string FirstName { get; set; } = null!;

    public bool IsDarkMode { get; set; }

    public bool IsExternalAccount { get; set; }

    public string LastName { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }

    public DateTime? Updated { get; set; }

    public bool IsSubscribed { get; set; }

    public string? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
