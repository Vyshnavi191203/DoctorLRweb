using System.Collections.Generic;
using DoctorLRweb.Models;
namespace DoctorLRweb.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
        public IEnumerable<User> GetUsersByRole(string role);

    }
}

