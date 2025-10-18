using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.Api.Dtos;
using Shop.Api.Mappers;
using Shop.Api.Models;


namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _singInManager;
    private readonly IConfiguration _configuration;
    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _singInManager = signInManager;
        _configuration = configuration;
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


    [HttpGet("testauth")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult TestAuth()
    {
        return Ok("Authorized!");
    }


    [HttpGet("{id}", Name = "GetUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUserAsync(string id)
    {
        Console.WriteLine($"GetUserAsync called with id: {id}");
        var user = await _userManager.FindByIdAsync(id);

        return user == null ? NotFound() : user.ToDto();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

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

            return Ok();
        }
        catch
        {
            return StatusCode(500, new { Error = "Internal server error" });
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user == null) return BadRequest("Wrong Email");

        var result = await _singInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
        if (!result.Succeeded) return BadRequest("Wrong Password");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(tokenHandler.WriteToken(token));

    }
}
