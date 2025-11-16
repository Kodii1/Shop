using Shop.Api.Dtos;
using Shop.Api.Models;

namespace Shop.Api.Mappers;

public static class ItemMapper
{
    public static ItemCreateDto ToDto(this Item item)
    {
        return new ItemCreateDto(item.Name, item.Description, item.Price, item.Category);
    }

    public static Item ToModel(this ItemCreateDto itemCreateDto, int id)
    {
        return new Item
        {
            Id = id,
            Name = itemCreateDto.Name,
            Description = itemCreateDto.Description,
            Price = itemCreateDto.Price,
            Category = itemCreateDto.Category,
        };
    }
}
