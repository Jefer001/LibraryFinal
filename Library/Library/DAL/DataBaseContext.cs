using Library.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        #region Builder
        public DataBaseContext(DbContextOptions<DataBaseContext> option) : base(option)
        {
        }
        #endregion

        #region Properties
        public DbSet<Book> Books { get; set; }
        public DbSet<Catalogue> Catalogues { get; set; }
        public DbSet<BookCatalogue> BookCatalogues { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<University> Universities { get; set; }
        #endregion

        #region Indices
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasIndex("Name", "Author").IsUnique();
            modelBuilder.Entity<Catalogue>().HasIndex(l => l.Name).IsUnique();
            modelBuilder.Entity<University>().HasIndex(u => u.Name).IsUnique();
        }
        #endregion
    }
}
