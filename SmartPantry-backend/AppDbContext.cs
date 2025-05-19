using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartPantry_backend.Models;

namespace SmartPantry_backend;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableProduct> AvailableProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductRecipe> ProductRecipes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Barakeh-HP;Database=mdp;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableProduct>(entity =>
        {
            entity.HasKey(e => e.AvailableProductId).HasName("PK__Availabl__2D5FE39415670D13");

            entity.ToTable("Available_Products", tb =>
                {
                    tb.HasTrigger("calculate_expiry_date");
                    tb.HasTrigger("delete_zero_quantity");
                    tb.HasTrigger("trg_prevent_negative_quantity");
                });

            entity.Property(e => e.AvailableProductId).HasColumnName("available_product_id");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.OriginalUnit)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("original_unit");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchasingTime)
                .HasColumnType("datetime")
                .HasColumnName("purchasing_time");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.AvailableProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__47027DF541CA8845");

            entity.HasIndex(e => e.Name, "uq_product_name").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.ExpiryDurationSummer).HasColumnName("expiry_duration_summer");
            entity.Property(e => e.ExpiryDurationWinter).HasColumnName("expiry_duration_winter");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProductRecipe>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.RecipeId }).HasName("PK__Product___E455632C8E04D3F8");

            entity.ToTable("Product_Recipe");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.QuantityRequired).HasColumnName("quantity_required");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRecipes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_product_recipe");

            entity.HasOne(d => d.Recipe).WithMany(p => p.ProductRecipes)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("fk_recipe");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipes__3571ED9BC9D9D6A2");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.CookingTime).HasColumnName("cooking_time");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Origin)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("origin");
            entity.Property(e => e.Servings).HasColumnName("servings");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
