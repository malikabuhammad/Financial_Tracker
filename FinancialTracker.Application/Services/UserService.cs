using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTracker.Domain.Entites;

namespace FinancialTracker.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Validate user during login
        public async Task<Users> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password) // Add password hashing for security
            {
                return null;
            }
            return user;
        }

        // Register a new user
        public async Task<(bool Success, string Message)> RegisterUserAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return (false, "Username is already taken.");
            }

            var newUser = new Users
            {
                Username = registerDto.Username,
                Password = registerDto.Password, // Hash this before saving
                Email = registerDto.Email
            };

            await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return (true, "User registered successfully.");
        }

        // Get user by ID
        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Get all users
        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // Update user details
        public async Task<(bool Success, string Message)> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(updateUserDto.Id);
            if (user == null)
            {
                return (false, "User not found.");
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = updateUserDto.Password; // Hash the password before saving
            }

            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "User updated successfully.");
        }

        // Delete a user
        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return (false, "User not found.");
            }

            await _userRepository.DeleteUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "User deleted successfully.");
        }
    }
}
