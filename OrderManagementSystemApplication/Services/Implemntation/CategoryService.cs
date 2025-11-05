using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class CategoryService(ICategoryRepository _categoryRepository) : ICategoryService
    {
        public async Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            try
            {
                if (await _categoryRepository.GetTableNoTracking().AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
                {
                    return new ApiResponse<CategoryResponseDto>(400, "Category name already exists.");
                }
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    IsActive = true
                };
              
                await _categoryRepository.AddAsync(category);
              
                var categoryResponse = new CategoryResponseDto
                {
                   
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };
                return new ApiResponse<CategoryResponseDto>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public Task<ApiResponse<ConfirmationResponseDto>> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository
                    .GetTableNoTracking()
                    .ToListAsync();
                var categoryList = categories.Select(c => new CategoryResponseDto
                {       
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                }).ToList();
                return new ApiResponse<List<CategoryResponseDto>>(200, categoryList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryResponseDto>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
