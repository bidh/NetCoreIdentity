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
        public IActionResult Index()
        {
            var roles = _context.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).FirstOrDefault();
            var userRoles = _context.UserRoles.Where(x => x.RoleId != roles).ToList();

            //var users = _context.Users.Where(x => x.Id == userRoles.First()).ToList();
            var users = _context.Users.Where(item => userRoles.Any(u => u.UserId.Equals(item.Id))).ToList();
             
            var usersViewModel = users.Select(user => new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Enabled = user.Enabled,
                From = user.FromDate,
                To = user.ToDate
            })            
            .ToList();
            return View(usersViewModel);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = _context.Users.SingleOrDefault(x => x.Id == id);
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
        /// edit method for editing the user
        /// </summary>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            DotNetCoreUser user = _context.Users.SingleOrDefault(x => x.Id == userViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user.UserName = userViewModel.UserName;
                user.Enabled = userViewModel.Enabled;
                user.FromDate = userViewModel.From;
                user.ToDate = userViewModel.To;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }
    }
}