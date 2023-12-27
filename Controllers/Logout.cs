using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MongoBlogWebSite.Controllers
{
	public class Logout : Controller
	{
		public async Task<IActionResult> IndexAsync()
		{
			string email = User.Identity?.Name;

			if (email == null)
				return RedirectToAction("Index", "Home");

			AuthenticationProperties authProperties = new AuthenticationProperties();
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);

			return RedirectToAction("Index", "Login");
		}
	}
}
