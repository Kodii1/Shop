using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.Dtos;
using Shop.Api.Mappers;
using Shop.Api.Models;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private ApplicationDbContext _dbContext;

    public ItemController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}", Name = "GetItemAsync")]
    public async Task<ActionResult<Item>> GetItemAsync(int id)
    {
        try
        {
            var result = await _dbContext.Item.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItemsAsync()
    {
        try
        {
            var result = await _dbContext.Item.AsNoTracking().ToListAsync();
            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Item>> CreateItemAsync(ItemCreateDto itemCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = new Item
            {
                Name = itemCreateDto.Name,
                Description = itemCreateDto.Description,
                Price = itemCreateDto.Price,
                Category = itemCreateDto.Category,
            };

            _dbContext.Add(item);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetItemAsync", new { id = item.Id }, item);
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<Item>> UpdateItemAsync(int id, ItemCreateDto itemCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _dbContext.Item.FindAsync(id);
            if (item == null)
            {
                return BadRequest();
            }
            _dbContext.Entry(item).CurrentValues.SetValues(itemCreateDto.ToModel(id));
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteItemAsync(int id)
    {
        try
        {
            await _dbContext.Item.Where(item => item.Id == id).ExecuteDeleteAsync();
            return NoContent();
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }
}
