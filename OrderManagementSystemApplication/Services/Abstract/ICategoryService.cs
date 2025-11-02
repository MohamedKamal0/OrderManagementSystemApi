using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Category;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryResponseDto>> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<ApiResponse<CategoryResponseDto>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoriesAsync();
        Task<ApiResponse<ConfirmationResponseDto>> DeleteCategoryAsync(int id);

    }
}
