using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Dtos;

public record UserDto(
    [Required(ErrorMessage = "First name is required")]
    [MinLength(3, ErrorMessage = "First name needs to be at least 3 characters long")]
    [MaxLength(20, ErrorMessage = "First name needs to be a maximum of 20 characters long")]
        string FirstName,
    [Required(ErrorMessage = "Last name is required")]
    [MinLength(3, ErrorMessage = "Last name needs to be at least 3 characters long")]
    [MaxLength(20, ErrorMessage = "Last name needs to be a maximum of 20 characters long")]
        string LastName,
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email
);
