using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;

namespace TicketProject.DataLayer.Context
{
    public class TPContext : IdentityDbContext<TicketUser,TicketUserRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer("Data Source=.\\MSSQLSERVER01;Initial Catalog=TicketApiNewDB;User ID=deniz;Password=Deniz1234");

        }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<City>? Cities { get; set; }
        public DbSet<Event>? EventDetails { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
    }
}
