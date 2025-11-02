using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Address;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IAddressService
    {
        Task<ApiResponse<AddressResponseDto>> CreateAddressAsync(AddressCreateDto addressDto);
        Task<ApiResponse<AddressResponseDto>> GetAddressByIdAsync(int id);
        Task<ApiResponse<ConfirmationResponseDto>> UpdateAddressAsync(AddressUpdateDto addressDto);
        Task<ApiResponse<ConfirmationResponseDto>> DeleteAddressAsync(AddressDeleteDto addressDeleteDTO);
        Task<ApiResponse<List<AddressResponseDto>>> GetAddressesByCustomerAsync(int customerId);

    }
}
