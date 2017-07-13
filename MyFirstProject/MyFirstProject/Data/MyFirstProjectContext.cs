using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.Models;

namespace MyFirstProject.Models
{
    public class MyFirstProjectContext : DbContext
    {
        public MyFirstProjectContext (DbContextOptions<MyFirstProjectContext> options)
            : base(options)
        {
        }

        public DbSet<MyFirstProject.Models.Movie> Movie { get; set; }

        public DbSet<MyFirstProject.Models.Episode> Episode { get; set; }
    }
}
