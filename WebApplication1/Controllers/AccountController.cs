using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models.Account;

namespace WebApplication1.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _dbContext;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        AppDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }
     public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityUser user = new IdentityUser() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var userFound = await _userManager.FindByEmailAsync(model.Email);
            if (userFound == null)
            {
                ModelState.AddModelError("", "User not found!");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login attempt");
            }
        }

        return View();
    }

    [Authorize]
    public IActionResult UserPage()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var model = _dbContext.UserInfo.FirstOrDefault(i => i.UserId == userId);
        if (model == null)
        {
            ViewData["NotSetUp"] = true;
            return View();
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult UserPage(UserInfoModel model, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    model.ProfileImage = memoryStream.ToArray();
                }
            }
            _dbContext.UserInfo.Add(model);
            _dbContext.SaveChanges();
            return RedirectToAction("UserPage");
        }
        ViewData["NotSetUp"] = true;
        return View();
    }
    
    public IActionResult DisplayProfileImage(int userId)
    {
        var user = _dbContext.UserInfo.Find(userId);

        if (user != null && user.ProfileImage != null)
        {
            return File(user.ProfileImage, "image/jpeg");
        }
        return NotFound();
    }

    public IActionResult Settings()
    {
        return View();
    }
}