using Microsoft.EntityFrameworkCore;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<DishCategory_BIT247667> DishCategories => Set<DishCategory_BIT247667>();
    public DbSet<Dish_BIT247667> Dishes => Set<Dish_BIT247667>();
    public DbSet<DishImage_BIT247667> DishImages => Set<DishImage_BIT247667>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Dish_BIT247667>()
            .HasOne(d => d.DishCategory).WithMany(c => c.Dishes)
            .HasForeignKey(d => d.DishCategoryId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<DishImage_BIT247667>()
            .HasOne(i => i.Dish).WithMany(d => d.Images)
            .HasForeignKey(i => i.DishId).OnDelete(DeleteBehavior.Cascade);

        // Tên món không trùng trong cùng một loại
        b.Entity<Dish_BIT247667>().HasIndex(d => new { d.DishCategoryId, d.Name }).IsUnique();

        b.Entity<DishCategory_BIT247667>().HasData(
            new DishCategory_BIT247667 { Id = 1, Name = "Main", Description = "Main dishes" },
            new DishCategory_BIT247667 { Id = 2, Name = "Drink", Description = "Drinks" },
            new DishCategory_BIT247667 { Id = 3, Name = "Dessert", Description = "Desserts" });

        b.Entity<Dish_BIT247667>().HasData(
            new Dish_BIT247667 { Id = 1, Name = "Pho", Price = 50000, PreparationTime = 20, DishCategoryId = 1, IsAvailable = true },
            new Dish_BIT247667 { Id = 2, Name = "Bun cha", Price = 45000, PreparationTime = 25, DishCategoryId = 1, IsAvailable = true },
            new Dish_BIT247667 { Id = 3, Name = "Tra da", Price = 5000, PreparationTime = 2, DishCategoryId = 2, IsAvailable = true },
            new Dish_BIT247667 { Id = 4, Name = "Che", Price = 20000, PreparationTime = 10, DishCategoryId = 3, IsAvailable = true },
            new Dish_BIT247667 { Id = 5, Name = "Cafe sua", Price = 25000, PreparationTime = 5, DishCategoryId = 2, IsAvailable = false });
    }
}