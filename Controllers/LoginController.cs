using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoBlogApi.DataAccess;
using MongoBlogApi.Models;
using System.Security.Claims;

namespace MongoBlogWebSite.Controllers
{
	public class LoginController : Controller
	{
		BlogDataAccess db = new();

		public IActionResult Index()
		{
			if (User.Identity?.Name != null)
				return RedirectToAction("Index", "Home");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(string email, string password)
		{
			UserModel user = await db.GetUserByEmailAndPassword(email, password);

			if (user == null)
				return RedirectToAction("Index", "Register");
			else
			{
				List<Claim> claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Email)
				};

				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				AuthenticationProperties authProperties = new AuthenticationProperties();

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

				//await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);

				return RedirectToAction("Index", "Home");
			}
		}
	}
}
