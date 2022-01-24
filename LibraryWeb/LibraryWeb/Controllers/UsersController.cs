using LibraryWeb.Data;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, 
            UserManager<IdentityUser> manager) : base (context)
        {
            _userManager = manager;
        }

        public IActionResult Index() => View(_userManager.Users.ToArray());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();
            
            var user = new IdentityUser { Email = model.Email, UserName = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)                    
                    ModelState.AddModelError(string.Empty, error.Description);                    
            }            

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)            
                return NotFound();
            
            EditUserViewModel model = new EditUserViewModel 
                { Id = user.Id, Name = user.UserName, Email = user.Email };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Name;

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);                        
                }
            }            

            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)            
                await _userManager.DeleteAsync(user);            

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {            
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
                return View(user);

            return NotFound();
        }
    }
}
