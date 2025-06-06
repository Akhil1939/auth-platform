﻿using System.ComponentModel.DataAnnotations;

namespace API.Data.Models.Central;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Salt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserRole> UserRoles { get; set; }

}

