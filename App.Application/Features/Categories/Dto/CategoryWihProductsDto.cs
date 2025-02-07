using App.Application.Features.Products.Dto;

namespace App.Application.Features.Categories.Dto;

public record CategoryWihProductsDto(int Id, string Name, List<ProductDto> Products);