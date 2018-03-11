using Microsoft.EntityFrameworkCore;
using BlogApis.Models.Entities;

namespace BlogApis.Models
{
    public class BlogDbContext:DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options):base(options)
        {}
        //public BlogDbContext(){}  
        public DbSet<Admin> Admin {get;set;}
        public DbSet<Tags> Tags { get; set; }

        public DbSet<Blog> Blog { get; set; }

    }
}