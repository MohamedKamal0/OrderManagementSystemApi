using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class CategoryService(ICategoryRepository _categoryRepository,
        ResponseHandler _responseHandler,IMapper _mapper, ILogger<CategoryService> _logger) : ICategoryService
    {
        public async Task<ApiResponse<string>> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            try
            {
                if (await _categoryRepository.GetTableNoTracking().AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
                {
                    _logger.LogWarning(CategoryLogMessages.CategoryNameConflict, categoryDto.Name);
                    return _responseHandler.Conflict<string>("Category name already exists.");
                }
                var category = _mapper.Map<Category>(categoryDto);
                category.IsActive = true;
                await _categoryRepository.AddAsync(category);

                _logger.LogInformation(CategoryLogMessages.CategoryCreated, categoryDto.Name);
                return _responseHandler.Created("Created Successfully.");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, CategoryLogMessages.ErrorCreatingCategory, categoryDto.Name);
                return _responseHandler.InternalServerError<string>("An error occurred while creating the category.");

            }
        }
        public Task<ApiResponse<string>> DeleteCategoryAsync(int id)
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
                var categoryList = _mapper.Map<List<CategoryResponseDto>>(categories);
                _logger.LogInformation(CategoryLogMessages.CategoriesRetrieved, categories.Count);
                return _responseHandler.Success(categoryList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CategoryLogMessages.ErrorRetrievingCategories);
                return _responseHandler.InternalServerError<List<CategoryResponseDto>>("An error occurred while retrieving categories.");
            }
        }

        public Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
