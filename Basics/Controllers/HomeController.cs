﻿using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
   public class HomeController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      public IActionResult Secret()
      {
         return View();
      }
   }
}
