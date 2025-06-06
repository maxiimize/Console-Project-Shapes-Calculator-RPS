using DataAcessLayer.ModelsCalculator;
using DataAcessLayer.ModelsRPS;
using DataAcessLayer.ModelsShapes;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAcessLayer
{
    public class AllDbContext : DbContext
    {
        public AllDbContext(DbContextOptions<AllDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Rectangle> Rectangles { get; set; }
        public DbSet<Parallelogram> Parallelograms { get; set; }
        public DbSet<Triangle> Triangles { get; set; }
        public DbSet<Rhombus> Rhombi { get; set; }

        public DbSet<Calculator> Calculations { get; set; }

        public DbSet<RPS> RpsGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shape>()
                .HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<Shape>()
                .HasDiscriminator<string>("ShapeType")
                .HasValue<Rectangle>("Rectangle")
                .HasValue<Parallelogram>("Parallelogram")
                .HasValue<Triangle>("Triangle")
                .HasValue<Rhombus>("Rhombus");

            modelBuilder.Entity<Calculator>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Calculator>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Operand1).IsRequired();
                b.Property(c => c.Operator).IsRequired().HasMaxLength(5);
                b.Property(c => c.Result).IsRequired();
                b.Property(c => c.DateCreated).IsRequired();
            });

            modelBuilder.Entity<RPS>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.PlayerMove).IsRequired();
                b.Property(r => r.ComputerMove).IsRequired();
                b.Property(r => r.Outcome).IsRequired();
                b.Property(r => r.DatePlayed).IsRequired();
                b.Property(r => r.WinRate).HasColumnType("decimal(5,2)").IsRequired();
            });

            // --- SEED DATA FÖR SHAPES ---
            modelBuilder.Entity<Rectangle>().HasData(
                new Rectangle
                {
                    Id = 1,
                    Width = 10,
                    Height = 5,
                    Area = 50,
                    Perimeter = 30,
                    DateCreated = new DateTime(2024, 6, 7, 10, 0, 0),
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Parallelogram>().HasData(
                new Parallelogram
                {
                    Id = 2,
                    BaseLength = 8,
                    SideLength = 6,
                    Height = 4,
                    Area = 32, // 8*4
                    Perimeter = 28, // 2*8 + 2*6
                    DateCreated = new DateTime(2024, 6, 7, 10, 5, 0),
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Triangle>().HasData(
                new Triangle
                {
                    Id = 3,
                    BaseLength = 6,
                    Height = 4,
                    SideA = 5,
                    SideB = 5,
                    Area = 12, // (6*4)/2
                    Perimeter = 16, // 6 + 5 + 5
                    DateCreated = new DateTime(2024, 6, 7, 10, 10, 0),
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Rhombus>().HasData(
                new Rhombus
                {
                    Id = 4,
                    SideLength = 7,
                    Height = 3,
                    Area = 21, // 7*3
                    Perimeter = 28, // 4*7
                    DateCreated = new DateTime(2024, 6, 7, 10, 15, 0),
                    IsDeleted = false
                }
            );
        }

    }
}
