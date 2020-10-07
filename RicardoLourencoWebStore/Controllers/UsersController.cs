using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data;
using RicardoLourencoWebStore.Helpers.Interfaces;
using RicardoLourencoWebStore.Models;

namespace RicardoLourencoWebStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        readonly DataContext _context;
        readonly IUserHelper _userHelper;

        public UsersController(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _userHelper.GetAllUsersWithRolesAsync());
        }



        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangeUserRoleViewModel
            {
                UserId = user.Id,
                Roles = _userHelper.GetComboRoles()
            };

            return View(model);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChangeUserRoleViewModel model)
        {

            if (model == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(model.UserId);

            if (ModelState.IsValid)
            {
                await _userHelper.UpdateUserAsync(user);

                await _userHelper.ChangeUserRoleAsync(user, User.Identity.Name, model.RoleName);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            await _userHelper.RemoveUserAsync(user, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
