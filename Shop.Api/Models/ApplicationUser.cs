using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Shop.Api.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MinLength(3, ErrorMessage = "First name need to be at least 3 characters long")]
    [MaxLength(20, ErrorMessage = "First name need to be a maximum 20 characters long")]
    public required string FirstName { set; get; }

    [Required]
    [MinLength(3, ErrorMessage = "Last name need to be at least 3 characters long")]
    [MaxLength(20, ErrorMessage = "Last name need to be a maximum 20 characters long")]
    public required string LastName { set; get; }

    [Required]
    public required DateTime CreatedAt { set; get; }

    [Required]
    public required DateTime LastLogin { set; get; }

    [PersonalData]
    [Required]
    [EmailAddress]
    public override required string? Email { get; set; }
}
