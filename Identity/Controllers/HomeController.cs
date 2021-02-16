using Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Identity.Controllers
{
   public class HomeController : Controller
   {
      private readonly UserManager<IdentityUser> _userManager;
      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly IEmailService _emailService;

      public HomeController(
         UserManager<IdentityUser> userManager,
         SignInManager<IdentityUser> signInManager,
         IEmailService emailservice)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _emailService = emailservice;
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
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code = token }, Request.Scheme, Request.Host.ToString());

            await _emailService.SendAsync("test@test.com", "Email verify", $"<a href=\"{link}\">Click to verify</a>", true);

            return RedirectToAction(nameof(EmailVerification));
         }

         return RedirectToAction(nameof(Index));
      }
      public IActionResult Register() => View();

      public async Task<IActionResult> LogOut()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction(nameof(Index));
      }


      public async Task<IActionResult> VerifyEmail(string userId, string code)
      {
         var user = await _userManager.FindByIdAsync(userId);

         // Do not return information about the error (hackers)
         if (user is null) return BadRequest(); 

         var result = await _userManager.ConfirmEmailAsync(user, code);

         if (result.Succeeded)
         {
            return View();
         }

         return BadRequest();
      }

      public IActionResult EmailVerification()
      {
         return View();
      }
   }
}
