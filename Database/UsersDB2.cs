//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using AuthorizationFormWPF.Entities;

//namespace AuthorizationFormWPF.Database
//{
//    public class UsersDB : DbContext
//    {
//        private DbSet<User> _users => Set<User>();

//        public UsersDB() => Database.EnsureCreated();

//        protected override void OnModelCreating(ModelBuilder modelBuilder) // в этом не уверен, если не работает - снесу
//        {
//            modelBuilder.Entity<User>()
//                .HasIndex(u => u.Username)
//                .IsUnique();
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=usersdb;Trusted_Connection=True;");

//        }

//        public IEnumerable<User> GetAllUsers() => _users;

//        public User? GetUserById(Guid id) => _users.FirstOrDefault(u => u.Id == id);
//        public User? GetUserByUsername(string username) => _users.FirstOrDefault(u => u.Username == username);


//    }
//}
