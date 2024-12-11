using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser?> GetUserById(string id);
        Task<bool> DeleteUserById(string id);
    }
}