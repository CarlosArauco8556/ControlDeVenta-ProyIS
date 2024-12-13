using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class UserMapper
    {
        public static AppUserGetDto ToGetAppUsuarioDto(this AppUser appUserModel)
        {
            return new AppUserGetDto
            {
                Id = appUserModel.Id,
                Name = appUserModel.Name,
                Rut = appUserModel.Rut,
                Email = appUserModel.Email,
                PhoneNumber = appUserModel.PhoneNumber
            };
        }

        public static AppUserResponseDto ToResponseAppUserDto(this AppUser user)
        {
            return new AppUserResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}