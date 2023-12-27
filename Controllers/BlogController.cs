using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoBlogApi.DataAccess;
using MongoBlogApi.Models;

namespace MongoBlogWebSite.Controllers
{
	public class BlogController : Controller
	{
		readonly BlogDataAccess db = new();

		public async Task<IActionResult> Index()
		{
			//BlogModel blog = new();
			//UserModel user = db.GetAllUsers().Result.Last();

			//blog.CreatedTime = DateTime.Now.ToString();
			//blog.Header = "Header";
			//blog.Title = "Title";
			//blog.Description = "Description";
			//blog.User = user;

			//await db.CreateBlog(blog);

			List<BlogModel> blogs = await db.GetBlogs();

			return View(blogs);
		}

		[Authorize]
		public async Task<IActionResult> CreateBlog()
		{
			List<string> emails = [];
			List<UserModel> usersEmails = await db.GetAllUsers();

			foreach (var user in usersEmails)
				emails.Add(user.Email);

			ViewBag.Emails = emails;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateBlog(BlogModel blog)
		{
			if (blog == null)
				return RedirectToAction("Index", "Home");

			blog.User = await db.GetUserByEmail(User.Identity?.Name);
			blog.CreatedTime = DateTime.Now.ToString();
			
			await db.CreateBlog(blog);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(string id)
		{
			if (id.Length < 24)
				return RedirectToAction("Index", "Blog");

			BlogModel blog = await db.GetBlogById(id);

			if (string.IsNullOrEmpty(id) || blog == null)
				return RedirectToAction("Index", "Blog");

			return View(blog);
		}

		public async Task<IActionResult> Delete(string id)
		{
			if (id.Length < 24)
				return RedirectToAction("Index", "Blog");

			BlogModel blog = await db.GetBlogById(id);

			if (string.IsNullOrEmpty(id) || blog == null)
				return RedirectToAction("Index", "Blog");

			await db.DeleteBlog(id);

			return RedirectToAction("MyBlogs", "User");
		}
	}
}
