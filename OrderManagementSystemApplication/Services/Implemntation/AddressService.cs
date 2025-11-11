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
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class AddressService(IAddressRepository _repository,
        ICustomerRepository _customerRepository, ILogger<AddressService> _logger,IMapper _mapper) : IAddressService
    {
        public async Task<ApiResponse<ConfirmationResponseDto>> CreateAddressAsync(AddressCreateDto addressDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(addressDto.CustomerId);
                if (customer == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound, addressDto.CustomerId);
                    return new ApiResponse<ConfirmationResponseDto>(404, "Customer not found.");
                }
                var address = _mapper.Map<Address>(addressDto);
                
                await _repository.AddAsync(address);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Address  Add successfully."
                };
                _logger.LogInformation(AddressLogMessages.AddressCreated, address.Id, address.CustomerId);
                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,AddressLogMessages.ErrorCreatingAddress, addressDto.CustomerId);
                return new ApiResponse<ConfirmationResponseDto>(500, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> DeleteAddressAsync(AddressDeleteDto addressDeleteDTO)
        {
            try
            {
                var address = await _repository.GetTableAsTracking()
                    .FirstOrDefaultAsync(a => a.Id == addressDeleteDTO.AddressId && a.CustomerId == addressDeleteDTO.CustomerId);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.ErrorDeletingAddress, addressDeleteDTO.AddressId, addressDeleteDTO.CustomerId);
                    return new ApiResponse<ConfirmationResponseDto>(404, "Address not found.");
                }

                await _repository.DeleteAsync(address);
                _logger.LogInformation(AddressLogMessages.AddressDeleted, addressDeleteDTO.AddressId, addressDeleteDTO.CustomerId);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Address with Id {addressDeleteDTO.AddressId} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorDeletingAddress, addressDeleteDTO.AddressId, addressDeleteDTO.CustomerId);

                return new ApiResponse<ConfirmationResponseDto>(500, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AddressResponseDto>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _repository.GetTableNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound, id);
                    return new ApiResponse<AddressResponseDto>(404, "Address not found.");
                }
                var response = _mapper.Map<AddressResponseDto>(address);

                _logger.LogInformation(AddressLogMessages.AddressesRetrieved, id);

                return new ApiResponse<AddressResponseDto>(200, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,AddressLogMessages.AddressNotFound, id);

                return new ApiResponse<AddressResponseDto>(500, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AddressResponseDto>>> GetAddressesByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetTableNoTracking()
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    _logger.LogWarning(AddressLogMessages.CustomerNotFound, customerId);
                    return new ApiResponse<List<AddressResponseDto>>(404, "Customer not found.");
                }
                var addresses = _mapper.Map<List<AddressResponseDto>>(customer.Addresses);

                _logger.LogInformation(AddressLogMessages.AddressesRetrieved, addresses.Count, customerId);

                return new ApiResponse<List<AddressResponseDto>>(200, addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.CustomerNotFound, customerId);

                return new ApiResponse<List<AddressResponseDto>>(500, $"Error: {ex.Message}");
            }
        }
    


        public async Task<ApiResponse<ConfirmationResponseDto>> UpdateAddressAsync(AddressUpdateDto addressDto)
        {
            try
            {
                var address = await _repository.GetTableAsTracking()
                    .FirstOrDefaultAsync(a => a.Id == addressDto.AddressId && a.CustomerId == addressDto.CustomerId);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound, addressDto.AddressId, addressDto.CustomerId);
                    return new ApiResponse<ConfirmationResponseDto>(404, "Address not found.");
                }
                _mapper.Map(addressDto, address);

                await _repository.UpdateAsync(address);
                _logger.LogInformation(AddressLogMessages.AddressUpdated, addressDto.AddressId, addressDto.CustomerId);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorUpdatingAddress, addressDto.AddressId, addressDto.CustomerId);
                return new ApiResponse<ConfirmationResponseDto>(500, $"Error: {ex.Message}");
            }
        }
    }
}
