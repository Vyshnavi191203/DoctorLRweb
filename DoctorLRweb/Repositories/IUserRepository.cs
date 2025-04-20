using DoctorLRweb.Models;
using DoctorLRweb.Repositories;

namespace DoctorLRweb.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int userId);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
        public IEnumerable<User> GetUsersByRole(string role);

    }
}