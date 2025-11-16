using System.ComponentModel.DataAnnotations;
using Shop.Api.Models;

namespace Shop.Api.Dtos;

public record ItemCreateDto(
    [Required] string Name,
    string? Description,
    [Required] decimal Price,
    [Required] Category Category
);
