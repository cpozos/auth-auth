using Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Controllers
{
   public class HomeController : Controller
   {
      private readonly UserManager<IdentityUser> _userManager;
      private readonly SignInManager<IdentityUser> _signInManager;

      public HomeController(
         UserManager<IdentityUser> userManager,
         SignInManager<IdentityUser> signInManager)
      {
         _userManager = userManager;
         _signInManager = signInManager;
      }

      public IActionResult Index()
      {
         return View();
      }

      [Authorize]
      public IActionResult Secret()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> Login(string username, string password)
      {
         var user = await _userManager.FindByNameAsync(username);

         if (user != null)
         {
            // Sign in
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult.Succeeded)
            {
               return RedirectToAction("Index");
            }
         }

         return RedirectToAction(nameof(Index));
      }

      public IActionResult Login() => View();

      [HttpPost]
      public async Task<IActionResult> Register(string username, string password)
      {
         var user = new IdentityUser
         {
            UserName = username,
            Email = ""
            //,PasswordHash = "custom hash"
         };

         var result = await _userManager.CreateAsync(user, password);

         if (result.Succeeded)
         {
            // Sign user
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult.Succeeded)
            {
               return RedirectToAction(nameof(Index));
            }
         }

         return RedirectToAction(nameof(Index));
      }
      public IActionResult Register() => View();

      public async Task<IActionResult> LogOut()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction(nameof(Index));
      }
   }
}
