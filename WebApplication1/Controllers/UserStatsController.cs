using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Authorize]
public class UserStatsController : Controller
{
    public IActionResult TodayStats()
    {
        return View();
    }

    public IActionResult LastSession()
    {
        return View();
    }
}