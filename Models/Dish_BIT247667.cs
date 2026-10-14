using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[Table("BIT247667_Dishes")]
public class Dish_BIT247667
{
    [Column("BIT247667_Id")]
    public int Id { get; set; }

    [Column("BIT247667_Name")]
    [Required(ErrorMessage = "Err_Required"), StringLength(200, ErrorMessage = "Err_Len")]
    public string Name { get; set; } = "";

    [Column("BIT247667_Price", TypeName = "decimal(18,2)")]
    [Range(0.01, 999999999, ErrorMessage = "Err_Price")]
    public decimal Price { get; set; }

    [Column("BIT247667_PreparationTime")]
    [Range(1, 100000, ErrorMessage = "Err_Prep")]
    public int PreparationTime { get; set; }

    [Column("BIT247667_IsAvailable")]
    public bool IsAvailable { get; set; } = true;

    [Column("BIT247667_Description")]
    public string? Description { get; set; }

    [Column("BIT247667_DishCategoryId")]
    public int DishCategoryId { get; set; }

    [ValidateNever]
    public DishCategory_BIT247667? DishCategory { get; set; }

    [ValidateNever]
    public List<DishImage_BIT247667> Images { get; set; } = new();
}