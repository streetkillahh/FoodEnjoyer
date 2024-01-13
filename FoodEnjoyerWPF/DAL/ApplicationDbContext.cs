using FoodEnjoyerWPF.Domain.Entity;
using FoodEnjoyerWPF.Domain.Enum;
using FoodEnjoyerWPF.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace FoodEnjoyerWPF.FoodEnjoyer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        //public DbSet<Product> Cars { get; set; }

        public DbSet<Address> Addresses { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        //public DbSet<Course> Courses { get; set; } = null!;
        //public DbSet<Student> Students { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=NewDataBase.db");
        }



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Course>()
        //        .HasMany(c => c.Students)
        //        .WithMany(s => s.Courses)
        //        .UsingEntity<Enrollment>(
        //           j => j
        //            .HasOne(pt => pt.Student)
        //            .WithMany(t => t.Enrollments)
        //            .HasForeignKey(pt => pt.StudentId),
        //            j => j
        //            .HasOne(pt => pt.Course)
        //            .WithMany(p => p.Enrollments)
        //            .HasForeignKey(pt => pt.CourseId));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.DateCreate).IsRequired();
                builder.Property(x => x.Sum).IsRequired();
                builder.Property(x => x.PayMethod).IsRequired();
                builder.Property(x => x.Status).IsRequired();
                builder.Property(x => x.Comment).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Address>(builder =>
            {
                builder.ToTable("Addresses").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Street).IsRequired().HasMaxLength(50);
                builder.Property(x => x.Home).IsRequired().HasMaxLength(10);
                builder.Property(x => x.Apartment).IsRequired();
                builder.Property(x => x.Entrance).IsRequired();

                builder.HasMany(x => x.Orders).WithOne(x => x.Address)
                        .HasForeignKey(x => x.AddressId)
                        .OnDelete(DeleteBehavior.Cascade);
            });
            
            //modelBuilder.Entity<Address>()
            //    .HasMany(c => c.Users)
            //    .WithMany(s => s.Addresses)
            //    .UsingEntity<Relation>(
            //        j => j
            //            .HasOne(pt => pt.User)
            //            .WithMany(t => t.Relation)
            //            .HasForeignKey(pt => pt.UserId),
            //        j => j
            //            .HasOne(a => a.Address)
            //            .WithMany(u => u.Relation)
            //            .HasForeignKey(fk => fk.AddressId),
            //        j =>
            //        {
            //            j.HasKey(t => new { t.AddressId, t.UserId });
            //            j.ToTable("Relations");
            //        });

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.Id);

                builder.HasData(new User[]
                {
                    new User()
                    {
                        Id = 1,
                        Login = "Admin",
                        Password = HashPasswordHelper.HashPassowrd("123456"),
                        Role = Role.Admin,
                        Telephone = "79220038233",
                        FIO = "Kimmel Dimitory Sergeevich",
                        Age = 20
                    },

                    new User()
                    {
                        Id = 2,
                        Login = "Sergey",
                        Password = HashPasswordHelper.HashPassowrd("654321"),
                        Role = Role.Admin,
                        Telephone = "79526897990",
                        FIO = "Kukarsky Sergey Andreevich",
                        Age = 20
                    }
                });
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Login).HasMaxLength(100).IsRequired();
                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Role).IsRequired();
                builder.Property(x => x.Age).IsRequired();
                builder.Property(x => x.FIO).HasMaxLength(100);
                builder.Property(x => x.Telephone).HasMaxLength(11);



                //builder.HasOne(x => x.Profile)
                //    .WithOne(x => x.User)
                //    .HasPrincipalKey<User>(x => x.Id)
                //    .OnDelete(DeleteBehavior.Cascade);

                builder.HasMany(r => r.Orders).WithOne(t => t.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                //builder.HasOne(x => x.Basket)
                //    .WithOne(x => x.User)
                //    .HasPrincipalKey<User>(x => x.Id)
                //    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Products").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Name).IsRequired().HasMaxLength(25);
                builder.Property(x => x.Category).IsRequired();
                builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
                builder.Property(x => x.Price).IsRequired();
                builder.Property(x => x.ImgSource).IsRequired();


                //builder.HasData(new Product
                //{
                //    Id = 1,
                //    Name = "Молоко",
                //    Description = new string('A', 50),
                //    Count = 230,
                //    ImgSource = null,
                //    Category = Category.Напитки,
                //    Price = 50
                //});
            });

        }
    }
}
