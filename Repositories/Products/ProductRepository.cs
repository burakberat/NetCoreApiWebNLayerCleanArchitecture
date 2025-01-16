using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Products
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProduct(int count)
        {
            return Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();
        }
    }
}
