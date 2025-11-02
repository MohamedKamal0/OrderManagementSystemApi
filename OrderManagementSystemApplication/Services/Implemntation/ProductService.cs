using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Product;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class ProductService(IProductRepository _productRepository,ICategoryRepository _categoryRepository) : IProductService
    {
        public async Task<ApiResponse<ProductResponseDto>> CreateProductAsync(ProductCreateDto productDto)
        {

            try
            {
                // Check if product name already exists
                if (await _productRepository.GetTableNoTracking()
                    .AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
                    return new ApiResponse<ProductResponseDto>(400, "Product name already exists.");

                // Check if category exists
                if (!await _categoryRepository.GetTableNoTracking()
                    .AnyAsync(c => c.Id == productDto.CategoryId))
                    return new ApiResponse<ProductResponseDto>(400, "Specified category does not exist.");

                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    ImageUrl = productDto.ImageUrl,
                    DiscountPercentage = productDto.DiscountPercentage,
                    CategoryId = productDto.CategoryId,
                    IsAvailable = true
                };

                await _productRepository.AddAsync(product);

                var response = new ProductResponseDto
                {
                    
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImageUrl = product.ImageUrl,
                    DiscountPercentage = product.DiscountPercentage,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable
                };

                return new ApiResponse<ProductResponseDto>(200, response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDto>(500, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return new ApiResponse<ConfirmationResponseDto>(404, "Product not found.");

            product.IsAvailable = false;
            await _productRepository.UpdateAsync(product);

            return new ApiResponse<ConfirmationResponseDto>(200,
                new ConfirmationResponseDto { Message = $"Product {id} deleted successfully." });

        }


        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetTableNoTracking()
                .Where(p => p.CategoryId == categoryId && p.IsAvailable)
                .ToListAsync();

            if (!products.Any())
                return new ApiResponse<List<ProductResponseDto>>(404, "No products found for this category.");

            var result = products.Select(p => new ProductResponseDto
            {
               
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                DiscountPercentage = p.DiscountPercentage,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable
            }).ToList();

            return new ApiResponse<List<ProductResponseDto>>(200, result);
        }

        public async Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetTableNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return new ApiResponse<ProductResponseDto>(404, "Product not found.");

            var response = new ProductResponseDto
            {
                
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                DiscountPercentage = product.DiscountPercentage,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable
            };

            return new ApiResponse<ProductResponseDto>(200, response);
        }

        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetTableNoTracking().ToListAsync();

            var result = products.Select(p => new ProductResponseDto
            {
            
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                DiscountPercentage = p.DiscountPercentage,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable
            }).ToList();

            return new ApiResponse<List<ProductResponseDto>>(200, result);
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> UpdateProductStatusAsync(ProductStatusUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                return new ApiResponse<ConfirmationResponseDto>(404, "Product not found.");

            product.IsAvailable = dto.IsAvailable;
            await _productRepository.UpdateAsync(product);

            return new ApiResponse<ConfirmationResponseDto>(200,
                new ConfirmationResponseDto { Message = $"Product {dto.ProductId} status updated successfully." });

        }
    }
}
