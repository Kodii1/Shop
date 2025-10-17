using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Dtos;
using Shop.Api.Mappers;
using Shop.Api.Models;


namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // [HttpGet("")]
    // public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync()
    // {
    //     var users = await _userManager.Users
    //                                   .Select(user => user.ToDto())
    //                                   .AsNoTracking()
    //                                   .ToListAsync();
    //
    //     if (users == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return users;
    // }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<ActionResult<UserDto>> GetUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user.ToDto();
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var user = new ApplicationUser
            {
                UserName = userRegisterDto.FirstName + userRegisterDto.LastName,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                NormalizedEmail = userRegisterDto.Email.ToUpper(),
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,

            };
            //TODO: VALIDATE PASSWORD
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);

            }

            return CreatedAtRoute("GetUser", new { id = user.Id }, user.ToDto());        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }

    }

}
