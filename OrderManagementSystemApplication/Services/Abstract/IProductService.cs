using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Product;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IProductService
    {
        Task<ApiResponse<string>> CreateProductAsync(ProductCreateDto productDto);
        Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int id);
        Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsAsync();
        Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsByCategoryAsync(int categoryId);
        // Task<ApiResponse<ConfirmationResponseDto>> UpdateProductAsync(ProductUpdateDto productDto);
        Task<ApiResponse<string>> DeleteProductAsync(int id);
        Task<ApiResponse<string>> UpdateProductStatusAsync(ProductStatusUpdateDto productStatusUpdateDTO);

    }
}
