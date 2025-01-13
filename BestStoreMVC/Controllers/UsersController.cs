using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BestStoreMVC.Controllers
{
    public class UsersController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;

		public UsersController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			this.context = context;
			this.environment = environment;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult SignUp(int id)
		{
			return View();
		}

		[HttpPost]
		public IActionResult SignUp(UsersDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return View(userDto);
			}
			var user = context.Users.FirstOrDefault(u => u.Email == userDto.Email);
			if (user == null)
			{
				User NewUser = new User()
				{
					Email = userDto.Email,
					FullName = userDto.FullName,
					Username = userDto.Username,
					PasswordHash = userDto.PasswordHash,
				};
				context.Users.Add(NewUser);
				context.SaveChanges();
				if(userDto.remember == "yes")
				{
					CookieOptions obj = new CookieOptions();
					obj.Expires = DateTime.Now.AddDays(7);
					Response.Cookies.Append("UserId", NewUser.UserId.ToString(), obj);
					Response.Cookies.Append("UserEmail", NewUser.Email, obj);
					Response.Cookies.Append("UserFullName", NewUser.FullName, obj);
					Response.Cookies.Append("UserName", NewUser.Username, obj);
					Response.Cookies.Append("CartItems", "");
                    return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-2); </script>", "text/html");
                }
                else
				{
                    // Set session for the new user
                    HttpContext.Session.SetInt32("UserId", NewUser.UserId);
                    HttpContext.Session.SetString("UserEmail", NewUser.Email);
                    HttpContext.Session.SetString("UserFullName", NewUser.FullName);
                    HttpContext.Session.SetString("UserName", NewUser.Username);
                    HttpContext.Session.SetString("CartItems", ""); // Initialize an empty cart
                    return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-2); </script>", "text/html");
                }
            }
            else 
			{
                return Content("<script> alert('User already found'); window.location.reload(true); window.history.go(-1); </script>", "text/html");

            }
        }

		public IActionResult LogIn()
		{
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["UserEmail"]))
			{
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
		public IActionResult LogIn(UsersDto userDto)
		{
            var user = context.Users.FirstOrDefault(u => u.Email == userDto.Email);
			if (user == null)
			{
                TempData["Message"] = "Can not find this account !!";
				return RedirectToAction(null);
			}
			else if (user.PasswordHash != userDto.PasswordHash)
			{
				TempData["Message"] = "Error in passowrd";
				return RedirectToAction(null);
			}
			else
			{
				if (userDto.remember=="yes")
				{
                    Console.WriteLine(userDto.UserId);
                    CookieOptions obj = new CookieOptions();
					obj.Expires = DateTime.Now.AddDays(7);
					Response.Cookies.Append("UserId", user.UserId.ToString(), obj);
					Response.Cookies.Append("UserEmail", userDto.Email, obj);
					Response.Cookies.Append("CartItems", "");
                    return RedirectToAction("Index", "Home");
                }
                else
				{
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserEmail", userDto.Email);
                    HttpContext.Session.SetString("CartItems", ""); 
                    return RedirectToAction("Index", "Home");
                }
            }
		}
		public async Task<IActionResult> LogOut()
		{
			HttpContext.Session.Clear(); // Clear all session data
            foreach (var cookie in Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "Users");
        }
    }
}
