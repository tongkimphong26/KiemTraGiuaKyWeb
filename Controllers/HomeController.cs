using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Dishes");

    // Đổi ngôn ngữ, quay lại đúng trang cũ (giữ nguyên bộ lọc nhờ Referer)
    public IActionResult SetLang(string lang)
    {
        Response.Cookies.Append("lang", lang == "vi" ? "vi" : "en",
            new CookieOptions { Expires = DateTimeOffset.Now.AddYears(1) });
        var back = Request.Headers.Referer.ToString();
        return string.IsNullOrEmpty(back) ? RedirectToAction("Index", "Dishes") : Redirect(back);
    }

    public IActionResult Error() => Content("Error");
}