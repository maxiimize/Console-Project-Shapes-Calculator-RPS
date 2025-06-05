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

            // --- Calculator-konfiguration ---
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

                b.Property(r => r.PlayerMove)
                    .IsRequired();

                b.Property(r => r.ComputerMove)
                    .IsRequired();

                b.Property(r => r.Outcome)
                    .IsRequired();

                b.Property(r => r.DatePlayed)
                    .IsRequired();

                b.Property(r => r.WinRate)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();
            });
        }
    }
}
