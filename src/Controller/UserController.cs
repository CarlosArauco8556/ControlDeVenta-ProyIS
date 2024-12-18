using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserGetDto>>> GetUsers()
        {
            var appUsers = await _userRepository.GetAllUsers();
            var appUsersDto = appUsers.Select(appUser => appUser.ToGetAppUsuarioDto());
            return Ok(appUsersDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var resultado = await _userRepository.DeleteUserById(id);
            if(!resultado)
            {
                return NotFound(new {message = "Usuario NO encontrado."});
            }
            return Ok(new {message = "Usuario eliminado exitosamente"});
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditUser([FromBody] AppUserUpdateDto appUserUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("Usuario NO autenticado. Inicie sesión primero.");

            var user = await _userRepository.GetUserById(userId);
            if (user == null) return NotFound("Usuario no encontrado.");

            user.Name = appUserUpdateDto.Name;
            user.PhoneNumber = appUserUpdateDto.PhoneNumber;

            var result = await _userRepository.EditProfile(user);
            if (!result) return StatusCode(500, "Error en editar perfil.");

            return Ok(user.ToResponseAppUserDto());
        }
    }
}