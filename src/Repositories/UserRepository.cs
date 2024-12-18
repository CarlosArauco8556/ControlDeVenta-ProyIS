using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUserById(string id)
        {
            var user = await GetUserById(id);
            if(user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditProfile(AppUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.OfType<AppUser>().ToListAsync();
        }

        public async Task<AppUser?> GetUserById(string id)
        {
            return await _context.Users.OfType<AppUser>().FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}