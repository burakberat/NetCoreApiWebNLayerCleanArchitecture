using App.Services.Products;

namespace App.Services.Categories.Dto;

public record CategoryWihProductsDto(int Id, string Name, List<ProductDto> Products);