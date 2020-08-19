using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Entity
{
  public  class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
          : base(options)
        {

        }
        public DbSet<User> tblUser { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.;Database=myCoreDb;Trusted_Connection=True;providerName:System.Data.SqlClient");
        //}
    }
}
