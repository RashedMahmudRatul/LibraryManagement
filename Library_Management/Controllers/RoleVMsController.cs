using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Management.Data;
using Library_Management.Models;
using Microsoft.AspNetCore.Identity;

namespace Library_Management.Controllers
{
    public class RoleVMsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleVMsController(AuthDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: RoleVMs
        public IActionResult Index()
        {
            var roles =  _roleManager.Roles.ToList();

            List<RoleVM> vm = new List<RoleVM>();
            foreach (var item in roles)
            {
                vm.Add(
                    new RoleVM
                    {
                        Id = item.Id,
                        Name = item.Name
                    });

            }


            return View(vm);
        }

        // GET: RoleVMs/Details/5
        public IActionResult Details(string id)
        {
            var roles = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            RoleVM vm = new RoleVM();

            vm.Id = roles.Id;
            vm.Name = roles.Name;

            return View(vm);
        }

        // GET: RoleVMs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleVMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(roleVM);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                bool ext = await _roleManager.RoleExistsAsync(roleVM.Name);
                if (ext)
                {
                    ViewBag.msg = "Role Already Exists";
                    return View();
                }

              await  _roleManager.CreateAsync(new IdentityRole(roleVM.Name));
            
            }
            return RedirectToAction("Index");
        }

        // GET: RoleVMs/Edit/5
        public IActionResult Edit(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var roleVM = await _context.RoleVM.FindAsync(id);
            //if (roleVM == null)
            //{
            //    return NotFound();
            //}
            var roles = _roleManager.Roles.Where(x=>x.Id==id).FirstOrDefault();

            RoleVM vm = new RoleVM();

            vm.Id = roles.Id;
            vm.Name = roles.Name;

            return View(vm);
        }

        // POST: RoleVMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleVM roleVM)
        {
            if (id != roleVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var role = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();
                    role.Name = roleVM.Name;
                    await _roleManager.UpdateAsync(role);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleVMExists(roleVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        // GET: RoleVMs/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            RoleVM vm = new RoleVM();

            vm.Id = roles.Id;
            vm.Name = roles.Name;

           // return View(vm);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: RoleVMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var roles = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            RoleVM vm = new RoleVM();

            vm.Id = roles.Id;
            vm.Name = roles.Name;

            await _roleManager.DeleteAsync(roles);

            return RedirectToAction("Index");
           
        }

        private bool RoleVMExists(string id)
        {
            return _context.RoleVM.Any(e => e.Id == id);
        }




    }
}
