using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Dtos;
using Shop.Api.Interfaces;
using Shop.Api.Mappers;
using Shop.Api.Models;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _singInManager;
    private readonly IJwtService _jwtService;

    public UserController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService
    )
    {
        _userManager = userManager;
        _singInManager = signInManager;
        _jwtService = jwtService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(string id)
    {
        Console.WriteLine($"GetUserAsync called with id: {id}");
        var user = await _userManager.FindByIdAsync(id);

        return user == null ? NotFound() : user.ToDto();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = userRegisterDto.ToModel();
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _jwtService.GenerateToken(user);

            return Ok(token);
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null)
            return BadRequest("Wrong Email");

        var result = await _singInManager.CheckPasswordSignInAsync(
            user,
            userLoginDto.Password,
            false
        );
        if (!result.Succeeded)
            return BadRequest("Wrong Password");

        var token = await _jwtService.GenerateToken(user);

        return Ok(token);
    }
}
