using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class CategoryService(IUnitOfWork _unitOfWork,
        ResponseHandler _responseHandler, IMapper _mapper, ILogger<CategoryService> _logger, HybridCache _cache) : ICategoryService
    {
        public async Task<ApiResponse<string>> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            try
            {
                if (await _unitOfWork.Categorys.GetTableNoTracking().AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
                {
                    _logger.LogWarning(CategoryLogMessages.CategoryNameConflict, categoryDto.Name);
                    return _responseHandler.Conflict<string>("Category name already exists.");
                }
                var category = _mapper.Map<Category>(categoryDto);
                category.IsActive = true;
                await _unitOfWork.Categorys.AddAsync(category);
                await _cache.RemoveAsync("all_categories_cache_key");
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


                var categories = await _cache.GetOrCreateAsync("all_categories_cache_key",
                    async ct =>
                      {
                          var result = await _unitOfWork.Categorys
                          .GetTableNoTracking()
                          .ToListAsync(ct);
                          var categoryList = _mapper.Map<List<CategoryResponseDto>>(result);
                          _logger.LogInformation(CategoryLogMessages.cachBb);

                          return categoryList;
                      });
                _logger.LogInformation(CategoryLogMessages.CategoriesRetrieved, categories.Count);
                return _responseHandler.Success(categories);
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
