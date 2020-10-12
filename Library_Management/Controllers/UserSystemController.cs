using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management.Areas.Identity.Data;
using Library_Management.Data;
using Library_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library_Management.Controllers
{
     // [Authorize]
    public class UserSystemController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordhasher;
        public IEnumerable<ApplicationUser> ApplicationUserList;
        public ApplicationUser ApplicationUserSingle;


        public UserSystemController(AuthDbContext context, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordhasher, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _passwordhasher = passwordhasher;
            _roleManager = roleManager;
        }

        [Authorize(Roles ="Admin, Manager")]
        public IActionResult Index()
        {
            ApplicationUserList = _context.ApplicationUser.ToList();
            return View(ApplicationUserList);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult>  Create(ApplicationUser model, string password, int Id)
        {

            IdentityResult identityResult = await _userManager.CreateAsync(model, password);

            return RedirectToAction("index");
        }
        public  IActionResult Edit(string Id )
        {

           
            ApplicationUserSingle = _context.ApplicationUser.Find(Id);


            return View(ApplicationUserSingle);
        }
        [HttpPost]
        public IActionResult Edit(ApplicationUser model, string password)
        {
            ApplicationUserSingle  = _context.ApplicationUser.Find(model.Id);

            ApplicationUserSingle.StudentId = model.StudentId;
            ApplicationUserSingle.StudentName = model.StudentName;
            ApplicationUserSingle.UserName = model.UserName;
            ApplicationUserSingle.Email = model.UserName;
            if (!string.IsNullOrEmpty(password))
            {
                ApplicationUserSingle.PasswordHash = _passwordhasher.HashPassword(model, password);
            }

            _context.Update(ApplicationUserSingle);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public  IActionResult Details(string Id)
        {
            ApplicationUserSingle = _context.ApplicationUser.Find(Id);
            return View(ApplicationUserSingle);
        }

        public IActionResult Delete(string Id)
        {
            ApplicationUserSingle = _context.ApplicationUser.Find(Id);
           
            return View(ApplicationUserSingle);
            
        }
        [HttpPost]
        public async Task <IActionResult> Delete(ApplicationUser model)
        {
            ApplicationUserSingle = _context.ApplicationUser.Find(model.Id);
            await _userManager.DeleteAsync(ApplicationUserSingle);

            return RedirectToAction("Index");
        }

        public IActionResult AssignRole(string id)
        {

            var users = _context.ApplicationUser.Where(x => x.Id == id).SingleOrDefault();
            var userRoles = _context.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToList();
            ManageUserRole userRolesVM = new ManageUserRole();
            userRolesVM.AppUser = users;
            userRolesVM.Roles = _roleManager.Roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id,
                Selected = userRoles.Contains(x.Id)
            }).ToList();

            return View(userRolesVM);
        }


        [HttpPost]
        public IActionResult AssignRole(ManageUserRole model)
        {

            var selectedRoleId = model.Roles.Where(x => x.Selected).Select(x => x.Value);
            var alreadyExitsRoleId = _context.UserRoles.Where(x => x.UserId == model.AppUser.Id).Select(x => x.RoleId).ToList();
            var toAdd = selectedRoleId.Except(alreadyExitsRoleId);
            var toRemove = alreadyExitsRoleId.Except(selectedRoleId);

            foreach (var item in toRemove)
            {
                _context.UserRoles.Remove(new IdentityUserRole<string>
                {
                    RoleId = item,
                    UserId = model.AppUser.Id
                });
            }

            foreach (var item in toAdd)
            {
                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = item,
                    UserId = model.AppUser.Id
                });
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
