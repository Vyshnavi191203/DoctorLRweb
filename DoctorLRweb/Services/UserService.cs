using DoctorLRweb.Models;
using DoctorLRweb.Repositories;

namespace DoctorLRweb.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task CreateUser(User user)
        {
            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
        }
        public IEnumerable<User> GetUsersByRole(string role)
        {
            return _userRepository.GetUsersByRole(role);
        }
       
    }
}
