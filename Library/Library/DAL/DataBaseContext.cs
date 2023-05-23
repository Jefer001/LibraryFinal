﻿using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class DataBaseContext : DbContext
    {
        #region Builder
        public DataBaseContext(DbContextOptions<DataBaseContext> option) : base(option)
        {
        }
        #endregion

        #region Properties
        public DbSet<Book> Books { get; set; }
        public DbSet<LiteraryGenre> literaryGenres { get; set; }
        public DbSet<BookGenre> bookGenres { get; set; }
        public DbSet<BookImage> bookImages { get; set; }
        #endregion

        #region Indices
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasIndex("Name", "Author").IsUnique();
            modelBuilder.Entity<LiteraryGenre>().HasIndex(l => l.Name).IsUnique();
        }
        #endregion
    }
}
