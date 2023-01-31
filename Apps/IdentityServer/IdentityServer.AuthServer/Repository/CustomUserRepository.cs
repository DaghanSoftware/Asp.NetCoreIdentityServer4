using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;

        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            var userMail = await _context.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
            return userMail;
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.CustomUsers.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _context.CustomUsers.AnyAsync(x=>x.Email==email && x.Password==password);
        }
    }
}
