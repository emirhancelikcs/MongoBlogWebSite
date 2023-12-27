using Microsoft.AspNetCore.Mvc;
using MongoBlogApi.DataAccess;
using MongoBlogApi.Models;

namespace MongoBlogWebSite.Controllers
{
	public class HomeController : Controller
	{
		readonly BlogDataAccess db = new();

		public async Task<IActionResult> Index()
		{
			List<UserModel> users = await db.GetAllUsers();

			return View(users.ToList());
		}
	}
}
