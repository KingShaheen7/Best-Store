namespace BestStoreMVC.Models
{
    public class ProductPageViewModel
    {
        public ProductDto NewProduct { get; set; } = new ProductDto();
        public List<Product> ProductList { get; set; } = new List<Product>();
    }
}
