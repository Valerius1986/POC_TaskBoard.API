using Microsoft.EntityFrameworkCore;
using POC_TaskBoard.API.Entities;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;

namespace POC_TaskBoard.API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Bords { get; set; }
        public DbSet<BoardUser> BoardUsers { get; set; }
        public DbSet<TaskOfBoard> Tasks { get; set; }
        public DbSet<SectionStatus> Statuses { get; set; }

    }
}
