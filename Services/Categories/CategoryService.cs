﻿using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
{
    public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
    {
        var categories = await categoryRepository.GetAll().ToListAsync();
        var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);
        return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
    }

    public async Task<ServiceResult<CategoryDto?>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);        
        var categoryAsDto = mapper.Map<CategoryDto>(category!);
        return ServiceResult<CategoryDto>.Success(categoryAsDto)!;
    }

    public async Task<ServiceResult<CategoryWihProductsDto>> GetCategoryWithProductsAsync(int categoryId)
    {
        var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);
        //if (category is null)
        //{
        //    return ServiceResult<CategoryWihProductsDto>.Fail("Category is not found", HttpStatusCode.NotFound);
        //}
        var categoryAsDto = mapper.Map<CategoryWihProductsDto>(category!);
        return ServiceResult<CategoryWihProductsDto>.Success(categoryAsDto);
    }

    public async Task<ServiceResult<List<CategoryWihProductsDto>>> GetCategoryWithProductsAsync()
    {
        var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();
        var categoryAsDto = mapper.Map<List<CategoryWihProductsDto>>(category);
        return ServiceResult<List<CategoryWihProductsDto>>.Success(categoryAsDto);
    }

    public async Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request)
    {
        var categoryWithSameName = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();
        if (categoryWithSameName)
        {
            return ServiceResult<CreateCategoryResponse>.Fail("Category name already exist.", HttpStatusCode.BadRequest);
        }
        var createdCategory = mapper.Map<Category>(request);
        await categoryRepository.AddAsync(createdCategory);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new CreateCategoryResponse(createdCategory.Id), $"api/categories/{createdCategory.Id}");
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var categoryWithSameName = await categoryRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();
        if (categoryWithSameName)
        {
            return ServiceResult.Fail("Category name already exist.", HttpStatusCode.BadRequest);
        }
        var category = mapper.Map<Category>(request);
        category.Id = id;
        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        categoryRepository.Delete(category!);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}