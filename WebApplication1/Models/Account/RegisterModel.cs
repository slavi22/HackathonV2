using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account;

public class RegisterModel
{
    [Microsoft.Build.Framework.Required]
    [EmailAddress]
    public string Email { get; set; }
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage ="Passwords don't match!")]
    public string ConfirmPassword { get; set; }
}