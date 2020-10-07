﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Helpers.Interfaces;
using RicardoLourencoWebStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Classes
{
    public class UserHelper : IUserHelper
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task CheckRolesAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.IsPersistent,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false
                );
        }

        public async Task<string> CheckUserRoleAsync(User user)
        {
            if (await IsUserInRoleAsync(user, "Admin"))
            {
                return "Admin";
            }

            if (await IsUserInRoleAsync(user, "ReSeller"))
            {
                return "ReSeller";
            }

            if (await IsUserInRoleAsync(user, "Client"))
            {
                return "Client";
            }

            return "Error";
        }

        public async Task<List<User>> GetAllUsersWithRolesAsync()
        {
            var result = await _userManager.Users.ToListAsync();

            foreach (var user in result)
            {
                user.RoleName = await CheckUserRoleAsync(user);
            }

            return result;
        }


        public async Task<IdentityResult> RemoveUserAsync(User user, string userName)
        {
            if (user.UserName != "ricardo.pinto.lourenco@formandos.cinel.pt")
            {
                if (user.UserName != userName)
                {
                    return await _userManager.DeleteAsync(user);
                }

            }
            return null;
        }

        public async Task ChangeUserRoleAsync(User user, string userName, string roleName)
        {


            if (user.UserName != "ricardo.pinto.lourenco@formandos.cinel.pt")
            {
                if (user.UserName != userName)
                {
                    await RemoveUserFromRoleAsync(user, "Admin");
                    await RemoveUserFromRoleAsync(user, "ReSeller");
                    await RemoveUserFromRoleAsync(user, "Client");

                    await AddUserToRoleAsync(user, roleName);
                }
            }
        }


        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = _roleManager.Roles.ToList().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Name
                }).ToList();

            return list;
        }
    }
}
