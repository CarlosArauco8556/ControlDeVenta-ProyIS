using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                if (await _userManager.Users.AnyAsync(p => p.Email == registerDto.Email)) return BadRequest("Email already exists.");

                if (string.IsNullOrEmpty(registerDto.Rut)) return BadRequest("RUT is required.");

                if (await _userManager.Users.AnyAsync(p => p.Rut == registerDto.Rut)) return BadRequest("Rut already exists.");

                if (string.IsNullOrEmpty(registerDto.Password) || string.IsNullOrEmpty(registerDto.ConfirmPassword)) return BadRequest("Password is required.");

                if (!string.Equals(registerDto.Password, registerDto.ConfirmPassword, StringComparison.Ordinal)) return BadRequest("Passwords do not match.");

                var user = new AppUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    Rut = registerDto.Rut,
                    Name = registerDto.Name,
                };
                
                var createUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Worker");

                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            UserName = user.UserName!,
                            Email = user.Email!,
                            Token = _tokenService.CreateToken(user)
                        });
                    }

                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }else
                {
                    return StatusCode(500, createUser.Errors);
                }

            } 
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try {

                if (User.Identity?.IsAuthenticated == true)
                {
                    return BadRequest(new { message = "Active session." });
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if(user == null) return Unauthorized("Invalid email or password.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if(!result.Succeeded) return Unauthorized("Invalid email or password.");

                await _signInManager.SignInAsync(user, isPersistent: true);

                var token = _tokenService.CreateToken(user);

                if (string.IsNullOrEmpty(token)) return Unauthorized("Invalid token.");

                return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Token = token
                    }
                );
                
            }catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

    }
}