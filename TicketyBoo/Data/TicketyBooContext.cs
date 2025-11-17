using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketyBoo.Models;

namespace TicketyBoo.Data
{
    public class TicketyBooContext : DbContext
    {
        public TicketyBooContext (DbContextOptions<TicketyBooContext> options)
            : base(options)
        {
        }

        public DbSet<TicketyBoo.Models.Haunt> Haunt { get; set; } = default!;
        public DbSet<TicketyBoo.Models.Category> Category { get; set; } = default!;
        //public DbSet<TicketyBoo.Models.Purchase> Purchases { get; set; } = default!;
    }
}
