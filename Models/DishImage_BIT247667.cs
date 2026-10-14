using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[Table("BIT247667_DishImages")]
public class DishImage_BIT247667
{
    [Column("BIT247667_Id")]
    public int Id { get; set; }

    [Column("BIT247667_ImageUrl")]
    [Required(ErrorMessage = "Err_Required")]
    public string ImageUrl { get; set; } = "";

    [Column("BIT247667_IsThumbnail")]
    public bool IsThumbnail { get; set; }

    [Column("BIT247667_DishId")]
    public int DishId { get; set; }

    [ValidateNever]
    public Dish_BIT247667? Dish { get; set; }
}