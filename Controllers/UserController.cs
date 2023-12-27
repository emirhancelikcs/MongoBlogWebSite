using Microsoft.AspNetCore.Mvc;
using MongoBlogApi.DataAccess;
using MongoBlogApi.Models;

namespace MongoBlogWebSite.Controllers
{
  public class UserController : Controller
  {
		readonly BlogDataAccess db = new();

		public async Task<IActionResult> MyDetails()
    {
			UserModel user = await db.GetUserByEmail(User.Identity?.Name);

			if (string.IsNullOrEmpty(User.Identity?.Name) || user == null)
				return RedirectToAction("Index", "Blog");

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> MyDetails(UserModel user)
		{
			//UserModel getUser = await db.GetUserByEmail(User.Identity?.Name);

			if (user == null)
				return RedirectToAction("Index", "Home");

			await db.UpdateUser(user);

			return RedirectToAction("Index", "Blog");
		}

		public async Task<IActionResult> MyBlogs()
		{
			if (User.Identity?.Name == null)
				return RedirectToAction("Index", "Home");

			List<BlogModel> blogs = await db.GetBlogByUserEmail(User.Identity?.Name);

			return View(blogs);
		}
  }
}
