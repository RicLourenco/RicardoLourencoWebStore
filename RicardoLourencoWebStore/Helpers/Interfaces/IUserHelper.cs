﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task CheckRolesAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<User> GetUserByIdAsync(string userId);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task<List<User>> GetAllUsersWithRolesAsync();

        Task<string> CheckUserRoleAsync(User user);

        Task<IdentityResult> RemoveUserAsync(User user, string userName);

        Task ChangeUserRoleAsync(User user, string userName, string roleName);

        Task RemoveUserFromRoleAsync(User user, string roleName);

        IEnumerable<SelectListItem> GetComboRoles();
    }
}
