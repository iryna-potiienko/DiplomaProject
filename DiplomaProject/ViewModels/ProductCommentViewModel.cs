using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.ViewModels;

public class ProductCommentViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [MinLength(3)]
    [Display(Name = "Коментар")]
    public string CommentText { get; set; }
    //[Required]
    [Range(1,5,ErrorMessage = "Введіть число від 1 до 5")]
    //[RegularExpression("([1-9][0-9]*)",ErrorMessage = "Введіть число більше за 0")] 
    [Display(Name = "Оцінка товару")]
    public int Estimation { get; set; }
    [Display(Name = "Товар")]
    public int ProductId { get; set; }
}