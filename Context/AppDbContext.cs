using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public virtual DbSet<BookCategory> BookCategories { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Translatable> Translatables { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Translation> Translations { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<FavouriteBook> FavouriteBooks { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "Librarian"
                },
                new Role
                {
                    Id = 3,
                    Name = "Customer"
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = _config["DefaultAdminUser:UserName"] ?? "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword(_config["DefaultAdminUser:Password"]),
                    RoleId = 1,
                }
            );

            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory
                {
                    Id = 1,
                    Name = "Uncategorized"
                }
            );
        }
    }
}
