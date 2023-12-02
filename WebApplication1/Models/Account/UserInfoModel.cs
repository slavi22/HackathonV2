using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication1.Models.Account;

public class UserInfoModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Discipline { get; set; }
    public int AccountLifeTime { get; set; }
    public int TotalDistance { get; set; }
    public int TotalJourneys { get; set; }
    public string OtherHobbies { get; set; }
    [ValidateNever]
    public byte[] ProfileImage { get; set; }
    [ValidateNever]
    public string UserId { get; set; }
    [ValidateNever]
    public IdentityUser User { get; set; }
}