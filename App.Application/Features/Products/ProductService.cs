using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.ServiceBus;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Event;
using AutoMapper;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IServiceBus serviceBus) : IProductService
    {
        private const string ProductListCacheKey = "ProductListCacheKey";
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductAsync(count);
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);
            if (productListAsCached is not null) return ServiceResult<List<ProductDto>>.Success(productListAsCached);

            var products = await productRepository.GetAllAsync();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            await cacheService.AddAsync(ProductListCacheKey, productsAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);
            var productsAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            var productAsDto = mapper.Map<ProductDto>(product!);
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            //2. yol => validasyon kontrolü
            var productWithSameName = await productRepository.AnyAsync(x => x.Name == request.Name);
            if (productWithSameName)
            {
                return ServiceResult<CreateProductResponse>.Fail("Product name already exist.", HttpStatusCode.BadRequest);
            }
            var createdProduct = mapper.Map<Product>(request);
            await productRepository.AddAsync(createdProduct);
            await unitOfWork.SaveChangesAsync();

            await serviceBus.PublishAsync(new ProductAddedEvent(createdProduct.Id, createdProduct.Name, createdProduct.Price));

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(createdProduct.Id), $"api/products/{createdProduct.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            var productWithSameName = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);
            if (productWithSameName)
            {
                return ServiceResult.Fail("Product name already exist.", HttpStatusCode.BadRequest);
            }
            var product = mapper.Map<Product>(request);
            product.Id = id;
            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.productId);
            product!.Stock = request.quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            productRepository.Delete(product!);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}