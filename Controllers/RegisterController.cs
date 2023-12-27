using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoBlogApi.Models;
using System.Security.Claims;
using MongoBlogApi.DataAccess;

namespace MongoBlogWebSite.Controllers
{
	public class RegisterController : Controller
	{
		BlogDataAccess db = new();

		public IActionResult Index()
		{
			if (User.Identity?.Name != null)
				return RedirectToAction("Index", "Home");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(UserModel user)
		{
			UserModel userExist = await db.GetUserByEmail(user.Email);

			if (userExist != null)
				return RedirectToAction("Index", "Register");
			else
			{
				await db.CreateUser(user);

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
