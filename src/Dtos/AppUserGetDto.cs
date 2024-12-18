using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class AppUserGetDto
    {
        [Required]
        public string Id { get; set;} = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Rut { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}