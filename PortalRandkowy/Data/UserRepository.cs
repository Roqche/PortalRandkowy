using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.Models;

namespace PortalRandkowy.Data
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context) : base (context)
        {
            this.context = context;
        }

        public async Task<User> GetUser(int id)
        {
            return await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(i => i.UserId == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await context.Photos.FirstOrDefaultAsync(p => p.PhotoId == id);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}
