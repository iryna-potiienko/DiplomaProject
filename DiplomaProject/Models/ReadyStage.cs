using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class ReadyStage
{
    public int Id { get; set; }
    [Display(Name = "Стадія готовності")]
    public string Name { get; set; }
}