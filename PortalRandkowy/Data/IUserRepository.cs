using PortalRandkowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalRandkowy.Data
{
    public interface IUserRepository : IGenericRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int userId);
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
