using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BestStoreMVC.Extensions; 
using System.Text.Json;



namespace BestStoreMVC.Controllers
{
	public class CartController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;

		public CartController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			this.context = context;
			this.environment = environment;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddToCart(int id, RequestItemsDto requestItemsDto)
		{
			string Email = HttpContext.Session.GetString("UserEmail");
			if (Email == null)
			{
				Email = HttpContext.Request.Cookies["UserEmail"];
				if (Email == null)
				{
					return RedirectToAction("LogIn", "Users");
				}
				var cartCook = GetCartFromCookies() ?? new List<RequestItems>();
				var existingItemCook = cartCook.FirstOrDefault(i => i.RequestProduct.Id == id);
				if (existingItemCook != null)
				{
					existingItemCook.Cuantity += requestItemsDto.Cuantity;
				}
				else
				{
					// Add new item to the cart
					var product = context.Products.Find(id);
					RequestItems requestItems = new RequestItems()
					{
						RequestProduct = product,
						Cuantity = requestItemsDto.Cuantity,
					};
					cartCook.Add(requestItems);
				}
				SaveCartToCookies(cartCook);

				return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");

			}
			var cart = HttpContext.Session.Get<List<RequestItems>>("Cart") ?? new List<RequestItems>();
			var existingItem = cart.FirstOrDefault(i => i.RequestProduct.Id == id);
			if (existingItem != null)
			{
				existingItem.Cuantity += requestItemsDto.Cuantity;
			}
			else
			{
				var product = context.Products.Find(id);
				RequestItems requestItems = new RequestItems()
				{
					RequestProduct = product,
					Cuantity = requestItemsDto.Cuantity,
				};
				cart.Add(requestItems);
			}
			HttpContext.Session.Set("Cart", cart);

			return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");
		}
		public List<RequestItems> GetCartFromCookies()
		{
			// Try to retrieve the cart from the "Cart" cookie
			if (HttpContext.Request.Cookies.TryGetValue("Cart", out var cookieValue))
			{
				// Deserialize the JSON string back to a List<RequestItems>
				return JsonSerializer.Deserialize<List<RequestItems>>(cookieValue) ?? new List<RequestItems>();
			}
			return null; // Return null if no cart is found
		}
		private void SaveCartToCookies(List<RequestItems> cart)
		{
			// Serialize the cart to JSON and set it in a cookie
			var cookieValue = JsonSerializer.Serialize(cart);
			HttpContext.Response.Cookies.Append("Cart", cookieValue, new CookieOptions
			{
				HttpOnly = true, // Make cookie accessible only via HTTP requests
				MaxAge = TimeSpan.FromDays(30), // Set cookie expiration
				SameSite = SameSiteMode.Strict // Adjust SameSite policy as needed
			});
		}
		public IActionResult ShowCart()
		{
			string Email = HttpContext.Session.GetString("UserEmail");
			if (Email == null)
			{
				Email = HttpContext.Request.Cookies["UserEmail"];
				if (Email == null)
				{
					return RedirectToAction("LogIn", "Users");
				}
			}
			var cart = HttpContext.Session.Get<List<RequestItems>>("Cart") ?? GetCartFromCookies() ?? new List<RequestItems>();

			return View(cart);

		}
		public IActionResult EditQuantity(int id)
		{
			// Use Include to load the related RequestProduct entity
			var Reqproduct = context.RequestItem.Include(ri => ri.RequestProduct).FirstOrDefault(ri => ri.Id == id);

			if (Reqproduct == null || Reqproduct.RequestProduct == null)
			{
				return RedirectToAction("ShowCart", "Cart");
			}

			// Create requestItemsDto from RequestItemsDto
			var requestItemsDto = new RequestItemsDto()
			{
				Name = Reqproduct.RequestProduct.Name,
				Price = Reqproduct.RequestProduct.Price,
				Cuantity = Reqproduct.Cuantity,
			};

			ViewData["ImageFileName"] = Reqproduct.RequestProduct.ImageFileName;
			return View(requestItemsDto);
		}

		[HttpPost]
		public IActionResult EditQuantity(int id, RequestItemsDto requestItemsDto)
		{
			var Reqproduct = context.RequestItem.Find(id);
			if (Reqproduct == null)
			{
				return RedirectToAction("ShowCart", "Cart");
			}
			if (!ModelState.IsValid)
			{
				ViewData["Cuantity"] = Reqproduct.Cuantity;
				return View(requestItemsDto);
			}
			Reqproduct.Cuantity = requestItemsDto.Cuantity;

			context.SaveChanges();
			return RedirectToAction("ShowCart", "Cart");
		}
		public IActionResult Delete(int id)
		{
			var Reqproduct = context.RequestItem.Find(id);
			if (Reqproduct == null)
			{
				return RedirectToAction("ShowCart", "Cart");
			}
			context.RequestItem.Remove(Reqproduct);
			context.SaveChanges(true);
			return RedirectToAction("ShowCart", "Cart");
		}
		public IActionResult SendRequest()
		{
			var UserId = HttpContext.Session.GetString("UserId");
			if (UserId == null)
			{
				UserId = HttpContext.Request.Cookies["UserId"];
				if (UserId == null)
				{
					return RedirectToAction("LogIn", "Users");
				}
				var cartCook = GetCartFromCookies() ?? new List<RequestItems>();
				if (cartCook != null)
				{
					foreach (var item in cartCook)
					{
						RequestItems requestItems = new RequestItems()
						{
							RequestProduct = item.RequestProduct,
							Cuantity = item.Cuantity,
							UserId = UserId,
							RequestDate = DateTime.Now,
						};
						//context.RequestItem.Add(requestItems);
						context.RequestItem.Update(requestItems);
						context.SaveChanges();

					}
					return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");
				}
				//     cart in cooky is empty
				return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");
			}
			else
			{
				var cart = HttpContext.Session.Get<List<RequestItems>>("Cart") ?? new List<RequestItems>();
				if (cart != null)
				{
					foreach (var item in cart)
					{
						RequestItems requestItems = new RequestItems()
						{
							RequestProduct = item.RequestProduct,
							Cuantity = item.Cuantity,
							UserId = UserId,
						};
						context.RequestItem.Update(requestItems);
						context.SaveChanges();
					}
					return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");
				}

				//    cart in session is empty
				return Content("<script>sessionStorage.setItem('reloadPage', 'true'); window.history.go(-1); </script>", "text/html");
			}
		}
	}
}
