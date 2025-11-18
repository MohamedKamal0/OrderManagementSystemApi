using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Product;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class ProductService(IProductRepository _productRepository, ICategoryRepository _categoryRepository,
        ResponseHandler _responseHandler, IMapper _mapper, ILogger<ProductService> _logger) : IProductService
    {
        public async Task<ApiResponse<String>> CreateProductAsync(ProductCreateDto productDto)
        {
            try
            {
                if (await _productRepository.GetTableNoTracking()
                    .AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
                {
                    _logger.LogWarning(ProductLogMessages.ProductNameConflict, productDto.Name);
                    return _responseHandler.Conflict<string>("Product name already exists.");
                }

                if (!await _categoryRepository.GetTableNoTracking()
                    .AnyAsync(c => c.Id == productDto.CategoryId))
                {
                    _logger.LogWarning(ProductLogMessages.CategoryNotFound, productDto.CategoryId);
                    return _responseHandler.NotFound<string>("Specified category does not exist.");
                }

                var product = _mapper.Map<Product>(productDto);
                product.IsAvailable = product.StockQuantity > 0;
                await _productRepository.UpdateAsync(product);

                _logger.LogInformation(ProductLogMessages.ProductCreated, product.Id);
                return _responseHandler.Created<string>("Created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorCreatingProduct, productDto.Name);
                return _responseHandler.InternalServerError<string>("An error occurred while creating the product.");
            }
        }

        public async Task<ApiResponse<string>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning(ProductLogMessages.ProductNotFound, id);
                    return _responseHandler.NotFound<string>("Product not found.");
                }

                product.IsAvailable = false;
                await _productRepository.DeleteAsync(product);

                _logger.LogInformation(ProductLogMessages.ProductDeleted, id);
                return _responseHandler.Deleted<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorDeletingProduct, id);
                return _responseHandler.InternalServerError<string>("An error occurred while deleting the product.");
            }
        }


        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetTableNoTracking()
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId && p.IsAvailable)
                    .ToListAsync();

                if (!products.Any())
                {
                    _logger.LogWarning(ProductLogMessages.CategoryNotFound, categoryId);
                    return _responseHandler.NotFound<List<ProductResponseDto>>("No products found for this category.");
                }

                var result = _mapper.Map<List<ProductResponseDto>>(products);

                _logger.LogInformation(ProductLogMessages.ProductsByCategoryRetrieved, result.Count, categoryId);
                return _responseHandler.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorRetrievingProducts);
                return _responseHandler.InternalServerError<List<ProductResponseDto>>("Error retrieving products.");
            }
        }

        public async Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetTableNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    _logger.LogWarning(ProductLogMessages.ProductNotFound, id);
                    return _responseHandler.NotFound<ProductResponseDto>("Product not found.");
                }

                var response = _mapper.Map<ProductResponseDto>(product);

                _logger.LogInformation(ProductLogMessages.ProductsRetrieved, 1);
                return _responseHandler.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorRetrievingProducts);
                return _responseHandler.InternalServerError<ProductResponseDto>("Error retrieving product.");
            }

        }

        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetTableNoTracking().ToListAsync();
                var result = _mapper.Map<List<ProductResponseDto>>(products);

                _logger.LogInformation(ProductLogMessages.ProductsRetrieved, result.Count);
                return _responseHandler.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorRetrievingProducts);
                return _responseHandler.InternalServerError<List<ProductResponseDto>>("Error retrieving products.");
            }
        }

        public async Task<ApiResponse<string>> UpdateProductStatusAsync(ProductStatusUpdateDto dto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning(ProductLogMessages.ProductNotFound, dto.ProductId);
                    return _responseHandler.NotFound<string>("Product not found.");
                }

                product.IsAvailable = dto.IsAvailable;
                await _productRepository.UpdateAsync(product);

                _logger.LogInformation(ProductLogMessages.ProductStatusUpdated, dto.ProductId);
                return _responseHandler.Updated<string>("Updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ProductLogMessages.ErrorUpdatingProduct, dto.ProductId);
                return _responseHandler.InternalServerError<string>("Error updating product status.");
            }

        }
    }
}
