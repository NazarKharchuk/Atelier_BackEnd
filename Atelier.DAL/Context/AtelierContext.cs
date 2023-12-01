using Atelier.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Context
{
    public class AtelierContext : DbContext
    {
        public DbSet<WorksType> WorksTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AtelierContext(DbContextOptions<AtelierContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /*public AtelierContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=LocalAtelierDB;Trusted_Connection=True;");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorksType>(builder =>
            {
                builder.ToTable("WorksTypes").HasKey(x => x.WorksTypeId);
                builder.Property(x => x.WorksTypeId).ValueGeneratedOnAdd();
                builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
                builder.HasIndex(u => u.Name).IsUnique();
                builder.Property(p => p.Cost).IsRequired();
            });

            modelBuilder.Entity<Client>(builder =>
            {
                builder.ToTable("Clients").HasKey(x => x.ClientId);
                builder.Property(x => x.ClientId).ValueGeneratedOnAdd();
                builder.Property(p => p.FirstName).IsRequired().HasMaxLength(20);
                builder.Property(p => p.LastName).IsRequired().HasMaxLength(20);
                builder.Property(p => p.MiddleName).HasMaxLength(20);
                builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(13);
                builder.HasIndex(u => u.PhoneNumber).IsUnique();
                builder.Property(p => p.Address).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.UserId);
                builder.Property(x => x.UserId).ValueGeneratedOnAdd();
                builder.Property(p => p.Login).IsRequired().HasMaxLength(30);
                builder.HasIndex(u => u.Login).IsUnique();
                builder.Property(p => p.Password).IsRequired();

                builder.HasOne(x => x.Employee)
                    .WithOne(x => x.User)
                    .HasPrincipalKey<User>(x => x.UserId);
            });

            modelBuilder.Entity<Employee>(builder =>
            {
                builder.ToTable("Employees").HasKey(x => x.EmployeeId);
                builder.Property(x => x.EmployeeId).ValueGeneratedOnAdd();
                builder.Property(p => p.FirstName).IsRequired().HasMaxLength(20);
                builder.Property(p => p.LastName).IsRequired().HasMaxLength(20);
                builder.Property(p => p.MiddleName).HasMaxLength(20);
                builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(13);
                builder.HasIndex(u => u.PhoneNumber).IsUnique();
                builder.Property(p => p.Address).HasMaxLength(50);
                builder.Property(p => p.UserId).IsRequired();

                builder.HasOne(x => x.User)
                    .WithOne(x => x.Employee)
                    .HasPrincipalKey<User>(x => x.UserId);
            });

            modelBuilder.Entity<Material>(builder =>
            {
                builder.ToTable("Materials").HasKey(x => x.MaterialId);
                builder.Property(x => x.MaterialId).ValueGeneratedOnAdd();
                builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
                builder.HasIndex(u => u.Name).IsUnique();
                builder.Property(p => p.Cost).IsRequired();
            });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders").HasKey(x => x.OrderId);
                builder.Property(x => x.OrderId).ValueGeneratedOnAdd();
                builder.Property(p => p.ReceivingDate).IsRequired();
                builder.Property(p => p.Status).IsRequired();
                builder.Property(p => p.WorkTypeId).IsRequired();
                //builder.Property(p => p.EmployeeId).IsRequired();
                builder.Property(p => p.ClientId).IsRequired();

                builder.HasOne(x => x.Client)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(r => r.ClientId);

                builder.HasOne(x => x.Employee)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(r => r.EmployeeId);

                builder.HasOne(x => x.WorksType)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(r => r.WorkTypeId);

                builder.HasMany(c => c.Materials)
                    .WithMany(s => s.Orders)
                    .UsingEntity<OrderMaterial>(
                       j => j
                        .HasOne(pt => pt.Material)
                        .WithMany(t => t.OrdersMaterials)
                        .HasForeignKey(pt => pt.MaterialId),
                        j => j
                            .HasOne(pt => pt.Order)
                            .WithMany(p => p.OrdersMaterials)
                            .HasForeignKey(pt => pt.OrderId),
                        j =>
                        {
                            j.HasKey(t => new { t.OrderId, t.MaterialId });
                            j.ToTable("OrdersMaterials");
                        });
            });
        }
    }
}
