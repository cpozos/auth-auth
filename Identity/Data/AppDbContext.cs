﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
   // IdentityDbContext contains all the user tables
   public class AppDbContext : IdentityDbContext
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) 
         : base(options)
      {

      }
   }
}
