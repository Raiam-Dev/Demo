using Demo.Domain.Produto;
using Demo.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.Infra.Data
{
    public class ContextBase : IdentityDbContext<UserAplication>
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) {}

        public DbSet<Produto> Produtos{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAplication>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder); 
        }
    }
}
