using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Areas.Identity.Data;
using DotNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly SignInManager<DotNetCoreUser> _signInManager;
        private readonly DotNetCoreContext _context;
        public UsersController(SignInManager<DotNetCoreUser> signInManager, DotNetCoreContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }   
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var roles = _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefault();
            //var userRoles = _context.UserRoles.Where(x => x.RoleId != roles).ToList();
            //var users = _context.Users.Where(item => userRoles.Any(u => u.UserId.Equals(item.Id))).ToList();
            //var usersViewModel = users.Select(user => new UserViewModel()
            //{
            //    Id = user.Id,
            //    UserName = user.UserName,
            //    Enabled = user.Enabled,
            //    From = user.FromDate,
            //    To = user.ToDate
            //})            
            //.ToList();
            //var result = _context.Users.Join(_context.UserRoles, u => u.Id, ur => ur.UserId,
            //                            (u, ur) => new { u, ur })
            //                            .Join(_context.Roles, r => r.ur.RoleId, ro => ro.Id, (r, ro) => new { r, ro })
            //                            .Where(m => m.r.u.UserName == "dp@sensus.dk")
            //                            .Select(m => new UserViewModel
            //                            {
            //                                UserName=m.r.u.UserName,
            //                                Enabled=m.r.u.Enabled,
            //                                From=m.r.u.FromDate,
            //                                To=m.r.u.ToDate
            //                            }).ToList();
            var users =await _context.Roles.Join(_context.UserRoles, r => r.Id, ur => ur.RoleId,
                                            (r, ur) => new { r, ur })
                                            .Join(_context.Users, a => a.ur.UserId, u => u.Id, (a, u) => new { a, u })
                                            .Where(m => m.a.r.Name != "Admin")
                                            .AsNoTracking()
                                            .Select(m => new UserViewModel
                                            {
                                                Id=m.u.Id,
                                                UserName=m.u.UserName,
                                                Enabled=m.u.Enabled,
                                                From=m.u.FromDate,
                                                To=m.u.ToDate
                                            }).ToListAsync();            
            
            return View(users);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user =await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                var usersViewModel = new UserViewModel
                {
                    Id=user.Id,
                    UserName=user.UserName,
                    Enabled = user.Enabled,
                    From = user.FromDate,
                    To = user.ToDate
                };
                return View(usersViewModel);
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Edit the user
        /// </summary>
        /// <param name="userViewModel">Pass UserViewModel for POST</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userViewModel)
        {
            DotNetCoreUser user = _context.Users.SingleOrDefault(x => x.Id == userViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    user.UserName = userViewModel.UserName;
                    user.Enabled = userViewModel.Enabled;
                    user.FromDate = userViewModel.From;
                    user.ToDate = userViewModel.To;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(userViewModel);
        }
        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <param name="saveChangesError">To throw the error in exception.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Delete(string id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var usersViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Enabled = user.Enabled,
                    From = user.FromDate,
                    To = user.ToDate
                };
                return View(usersViewModel);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="user">Pass the user to delete.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(UserViewModel user)
        {
            try
            {
                var userToDelete = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
                _context.Entry(userToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id=user.Id, saveChangesError = true });
            }           
        }
    }
}