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
                Name = appUserModel.Name,
                Rut = appUserModel.Rut,
                Email = appUserModel.Email,
                PhoneNumber = appUserModel.PhoneNumber
            };
        }
    }
}