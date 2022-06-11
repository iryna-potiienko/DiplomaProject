using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.ViewModels;

public class ShopCommentViewModel
{
    public int Id { get; set; }
    [Display(Name = "Коментар")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [MinLength(3)]
    public string CommentText { get; set; }
    
    [Display(Name = "Оцінка товару")]
    //[Required]
    [Range(1,5,ErrorMessage = "Введіть число від 1 до 5")]
    public int Estimation { get; set; }
    [Display(Name = "Магазин")]
    public int ShopProfileId { get; set; }
}