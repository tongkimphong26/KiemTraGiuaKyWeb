using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[Table("BIT247667_DishCategories")]
public class DishCategory_BIT247667
{
    [Column("BIT247667_Id")]
    public int Id { get; set; }

    [Column("BIT247667_Name")]
    [Required(ErrorMessage = "Err_Required"), StringLength(100, ErrorMessage = "Err_Len")]
    public string Name { get; set; } = "";

    [Column("BIT247667_Description")]
    public string? Description { get; set; }

    [ValidateNever]
    public List<Dish_BIT247667> Dishes { get; set; } = new();
}