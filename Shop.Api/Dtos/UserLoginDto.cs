using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Dtos;

public record UserLoginDto(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,
    [Required(ErrorMessage = "Password is required")] string Password
);
