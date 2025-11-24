using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class AddressService(IUnitOfWork _unitOfWork, ILogger<AddressService> _logger,
        IMapper _mapper, ResponseHandler _responseHandler) : IAddressService
    {
        public async Task<ApiResponse<string>> CreateAddressAsync(AddressCreateDto addressDto)
        {

            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(addressDto.CustomerId);
                if (customer == null)
                {
                    _logger.LogWarning(AddressLogMessages.CustomerNotFound, addressDto.CustomerId);
                    return _responseHandler.NotFound<string>($"Customer with ID {addressDto.CustomerId} not found.");
                }

                var address = _mapper.Map<Address>(addressDto);
                await _unitOfWork.Addresses.AddAsync(address);

                _logger.LogInformation(AddressLogMessages.AddressCreated, address.Id, address.CustomerId);
                return _responseHandler.Created($"Address created successfully with ID: {address.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorCreatingAddress, addressDto.CustomerId);
                return _responseHandler.InternalServerError<string>("An error occurred while creating the address.");
            }
        }

        public async Task<ApiResponse<string>> DeleteAddressAsync(AddressDeleteDto addressDeleteDto)
        {

            try
            {
                var address = await _unitOfWork.Addresses.GetTableAsTracking()
                    .FirstOrDefaultAsync(a => a.Id == addressDeleteDto.AddressId &&
                                            a.CustomerId == addressDeleteDto.CustomerId);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound,
                        addressDeleteDto.AddressId, addressDeleteDto.CustomerId);
                    return _responseHandler.NotFound<string>(
                        $"Address with ID {addressDeleteDto.AddressId} not found for customer {addressDeleteDto.CustomerId}.");
                }

                await _unitOfWork.Addresses.DeleteAsync(address);
                _logger.LogInformation(AddressLogMessages.AddressDeleted,
                    addressDeleteDto.AddressId, addressDeleteDto.CustomerId);

                return _responseHandler.Deleted<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorDeletingAddress, addressDeleteDto.AddressId, addressDeleteDto.CustomerId);

                return _responseHandler.InternalServerError<string>("An error occurred while deleting the address.");
            }
        }

        public async Task<ApiResponse<AddressResponseDto>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _unitOfWork.Addresses.GetTableNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound, id);
                    return _responseHandler.NotFound<AddressResponseDto>($"Address with ID {id} not found.");
                }

                var response = _mapper.Map<AddressResponseDto>(address);
                _logger.LogInformation(AddressLogMessages.AddressesRetrieved, id);

                return _responseHandler.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorRetrievingAddress, id);
                return _responseHandler.InternalServerError<AddressResponseDto>("An error occurred while retrieving the address.");
            }
        }

        public async Task<ApiResponse<List<AddressResponseDto>>> GetAddressesByCustomerAsync(int customerId)
        {

            try
            {
                var customer = await _unitOfWork.Customers.GetTableNoTracking()
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    _logger.LogWarning(AddressLogMessages.CustomerNotFound, customerId);
                    return _responseHandler.NotFound<List<AddressResponseDto>>($"Customer with ID {customerId} not found.");
                }

                var addresses = _mapper.Map<List<AddressResponseDto>>(customer.Addresses);
                _logger.LogInformation(AddressLogMessages.AddressesRetrieved, addresses.Count, customerId);

                return _responseHandler.Success(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorRetrievingAddress, customerId);
                return _responseHandler.InternalServerError<List<AddressResponseDto>>("An error occurred while retrieving addresses.");
            }
        }

        public async Task<ApiResponse<string>> UpdateAddressAsync(AddressUpdateDto addressDto)
        {
            try
            {
                var address = await _unitOfWork.Addresses.GetTableAsTracking()
                    .FirstOrDefaultAsync(a => a.Id == addressDto.AddressId &&
                                            a.CustomerId == addressDto.CustomerId);

                if (address == null)
                {
                    _logger.LogWarning(AddressLogMessages.AddressNotFound,
                        addressDto.AddressId, addressDto.CustomerId);

                    return _responseHandler.NotFound<string>(
                        $"Address with ID {addressDto.AddressId} not found for customer {addressDto.CustomerId}.");
                }

                _mapper.Map(addressDto, address);
                await _unitOfWork.Addresses.UpdateAsync(address);

                _logger.LogInformation(AddressLogMessages.AddressUpdated,
                    addressDto.AddressId, addressDto.CustomerId);

                return _responseHandler.Updated($"Address {addressDto.AddressId} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AddressLogMessages.ErrorRetrievingAddress,
                    addressDto.AddressId, addressDto.CustomerId);
                return _responseHandler.InternalServerError<string>("An error occurred while updating the address.");
            }
        }
    }
}
