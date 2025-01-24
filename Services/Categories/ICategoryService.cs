using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;

namespace App.Services.Categories;

public interface ICategoryService
{
    Task<ServiceResult<List<CategoryDto>>> GetAllListAsync();

    Task<ServiceResult<CategoryDto?>> GetByIdAsync(int id);

    Task<ServiceResult<CategoryWihProductsDto>> GetCategoryWithProductsAsync(int categoryId);

    Task<ServiceResult<List<CategoryWihProductsDto>>> GetCategoryWithProductsAsync();

    Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request);

    Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request);

    Task<ServiceResult> DeleteAsync(int id);
}