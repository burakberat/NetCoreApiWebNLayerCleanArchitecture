using App.Repositories.Products;
using App.Services.Filters;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            CreateActionResult(await productService.GetAllListAsync());

        [HttpGet("topprice/{count:int}")]
        public async Task<IActionResult> GetTopPrice(int count) =>
            CreateActionResult(await productService.GetTopPriceProductAsync(count));

        [HttpGet("{pageNumber:int}/{pageSize:int}")] //metodların daha sağlıklı olması için constraints kullanıyoruz.
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) =>
            CreateActionResult(await productService.GetPagedAllListAsync(pageNumber, pageSize));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) =>
            CreateActionResult(await productService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request) =>
            CreateActionResult(await productService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request) =>
            CreateActionResult(await productService.UpdateAsync(id, request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPatch("stock")] //Kısmi bir güncelleme yapılacaksa HttpPatch kullanmak daha uygundur.
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) =>
            CreateActionResult(await productService.UpdateStockAsync(request));

        //[HttpPut("UpdateStock")]
        //public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) =>
        //    CreateActionResult(await productService.UpdateStockAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            CreateActionResult(await productService.DeleteAsync(id));
    }
}