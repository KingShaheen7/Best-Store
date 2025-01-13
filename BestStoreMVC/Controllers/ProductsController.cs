using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BestStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;

		public ProductsController(ApplicationDbContext context,IWebHostEnvironment environment)
        {
            this.context = context;
			this.environment = environment;
		}
		
		public IActionResult Admin()
        {
            var viewModel = new ProductPageViewModel
            {
                ProductList = context.Products.OrderByDescending(p => p.Id).ToList()
            };
            
			string Email = HttpContext.Session.GetString("UserEmail");
            if(Email == null)
            {
                Email = HttpContext.Request.Cookies["UserEmail"];
            }
            if (!string.IsNullOrEmpty(Email))
            {
				var user = context.Users.FirstOrDefault(u => u.Email == Email);
                if(user.Username=="Admin")
                {
					return View(viewModel);
				}
                else
                {
					return Content("<script> alert('This user account not allowed !!!'); window.location.reload(true); window.history.go(-1); </script>", "text/html");
				}
			}
            else 
            {
                return RedirectToAction("LogIn", "Users");
            }
        }

        public IActionResult Phones()
        {
            var products = context.Products.Where(item =>item.Category=="Phones").OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Laptops()
        {
            var products = context.Products.Where(item => item.Category == "Laptops").OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
		public IActionResult Screens()
		{
			var products = context.Products.Where(item => item.Category == "Screens").OrderByDescending(p => p.Id).ToList();
			return View(products);
		}

        [HttpPost]
        public IActionResult Create(ProductPageViewModel model)
        {
            if (model.NewProduct.ImageFile == null)
            {
                ModelState.AddModelError("model.NewProduct.ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            // Save image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(model.NewProduct.ImageFile.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "products", newFileName);

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                model.NewProduct.ImageFile.CopyTo(stream);
            }

            // Save the new product to the database
            Product product = new Product
            {
                Name = model.NewProduct.Name,
                Brand = model.NewProduct.Brand,
                Category = model.NewProduct.Category,
                Price = model.NewProduct.Price,
                Description = model.NewProduct.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Admin", "Products");
        }

        

        [HttpPost]
		public IActionResult Edit(ProductPageViewModel model)
        {
            var product = context.Products.Find(model.NewProduct.Id);
            if(product == null)
            {
                return RedirectToAction("Admin", "Products");
            }

            if(!ModelState.IsValid)
            {
				ViewData["ProductId"] = product.Id;
				ViewData["ImageFileName"] = product.ImageFileName;
				ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
				return View(model);
            }
            // Update the image file if we have a new image file
            string newFileName=product.ImageFileName;
            if(model.NewProduct.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(model.NewProduct.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream=System.IO.File.Create(imageFullPath))
                {
                    model.NewProduct.ImageFile.CopyTo(stream);
                }
                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
			}

            //update the product in the database
            product.Name = model.NewProduct.Name;
            product.Brand = model.NewProduct.Brand;
            product.Category = model.NewProduct.Category;
            product.Price = model.NewProduct.Price;
            product.Description = model.NewProduct.Description;
            product.ImageFileName = newFileName;
            
            context.SaveChanges();
            return RedirectToAction("Admin", "Products");

        }


        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Admin", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

			return RedirectToAction("Admin", "Products");
		}
	}
}
