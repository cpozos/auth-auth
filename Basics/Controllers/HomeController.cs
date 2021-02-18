using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Basics.Controllers
{
   public class HomeController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      [Authorize]
      public IActionResult Secret()
      {
         return View();
      }

      [Authorize(Policy = "Claim.DayOfBirth")]
      public IActionResult SecretCustomPolicy()
      {
         // If Authenticating action does not include the day of birth, this action will never be reach out
         // because of the policy applied, which requires the day of birth.
         return View("Secret");
      }

      public IActionResult Authenticate()
      {
         return View();
      }

      [HttpPost]
      public IActionResult Authenticating()
      {
         // We trust on Grandma. It says it is Bob
         var grandmaClaims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, "Bob"),
            new Claim(ClaimTypes.Email, "Bobo@mail.com"),
            new Claim("Grandam.Says", "Very nice")
         };
         var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");

         // We trust on facebook. It says it is Bob
         var facebookClaims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, "Bob"),
            new Claim(ClaimTypes.Email, "Bobo@mail.com"),
            new Claim("Grandam.Says", "Very nice")
            //, new Claim(ClaimTypes.Role, "Admin")
         };
         var facebookIdentity = new ClaimsIdentity(facebookClaims, "FB");

         // All the identities
         var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, facebookIdentity });

         HttpContext.SignInAsync(userPrincipal);
         return RedirectToAction(nameof(Index));
      }


      [Authorize(Roles = "Admin")]
      public IActionResult SecretRole()
      {
         // Role is just another claim
         // So it is only accessible for users with ClaimType.Role equals to Admin
         return View("Secret");
      }
   }
}
