using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class AddressService(IAddressRepository _repository, ICustomerRepository _customerRepository) : IAddressService
    {
        public async Task<ApiResponse<AddressResponseDto>> CreateAddressAsync(AddressCreateDto addressDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(addressDto.CustomerId);
                if (customer == null)
                    return new ApiResponse<AddressResponseDto>(404, "Customer not found.");

                var address = new Address
                {
                    CustomerId = addressDto.CustomerId,
                    AddressLine = addressDto.AddressLine,
                    City = addressDto.City,
                    State = addressDto.State,
                    PostalCode = addressDto.PostalCode,
                };

                await _repository.AddAsync(address);

                var response = new AddressResponseDto
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                };

                return new ApiResponse<AddressResponseDto>(200, response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDto>(500, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> DeleteAddressAsync(AddressDeleteDto addressDeleteDTO)
        {
            try
            {
                var address = await _repository.GetTableAsTracking()
                    .FirstOrDefaultAsync(a => a.Id == addressDeleteDTO.AddressId && a.CustomerId == addressDeleteDTO.CustomerId);

                if (address == null)
                    return new ApiResponse<ConfirmationResponseDto>(404, "Address not found.");

                await _repository.DeleteAsync(address);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Address with Id {addressDeleteDTO.AddressId} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
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
                    return new ApiResponse<AddressResponseDto>(404, "Address not found.");

                var response = new AddressResponseDto
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                };

                return new ApiResponse<AddressResponseDto>(200, response);
            }
            catch (Exception ex)
            {
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
                    return new ApiResponse<List<AddressResponseDto>>(404, "Customer not found.");

                var addresses = customer.Addresses.Select(a => new AddressResponseDto
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AddressLine = a.AddressLine,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                }).ToList();

                return new ApiResponse<List<AddressResponseDto>>(200, addresses);
            }
            catch (Exception ex)
            {
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
                    return new ApiResponse<ConfirmationResponseDto>(404, "Address not found.");

                address.AddressLine = addressDto.AddressLine;
               address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
              
                await _repository.UpdateAsync(address);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDto>(500, $"Error: {ex.Message}");
            }
        }
    }
}
