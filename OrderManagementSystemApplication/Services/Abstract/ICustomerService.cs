using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Customer;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface ICustomerService
    {
        Task<ApiResponse<String>> RegisterCustomerAsync(CustomerRegistrationDto customerDto);
       // Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
        Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int id);
        Task<ApiResponse<string>> UpdateCustomerAsync(CustomerUpdateDto customerDto);
        Task<ApiResponse<String>> DeleteCustomerAsync(int id);
       // Task<ApiResponse<ConfirmationResponseDto>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

    }
}
