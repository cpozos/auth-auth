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

      public IActionResult Authenticate()
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
         };
         var facebookIdentity = new ClaimsIdentity(facebookClaims, "FB");

         // All the identities
         var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, facebookIdentity });

         HttpContext.SignInAsync(userPrincipal);
         return RedirectToAction(nameof(Index));
      }
   }
}
