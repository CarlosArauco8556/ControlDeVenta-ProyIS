using System;
using System.Collections.Generic;
using System.Linq;
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
        

    }
}