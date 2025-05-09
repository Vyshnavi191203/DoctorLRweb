﻿using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Data;
using DoctorLRweb.Models;
using DoctorLRweb.Repositories;

namespace DoctorLRweb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddUser(User user)
        {
            // Generate a unique 4-digit UserId
            int userId;
            var random = new Random();
            do
            {
                userId = random.Next(1000, 10000); // 4-digit number: 1000 to 9999
            } while (await _context.Users.AnyAsync(u => u.UserId == userId));
            user.UserId = userId;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<User> GetUsersByRole(string role)
        {
            return _context.Users.Where(u => u.Role.ToLower() == role.ToLower()).ToList();
        }
       
    }
}