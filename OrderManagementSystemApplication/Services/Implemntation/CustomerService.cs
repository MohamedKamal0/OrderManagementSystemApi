
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Customer;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using Org.BouncyCastle.Crypto.Generators;
namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class CustomerService(ICustomerRepository _repository, 
        ResponseHandler _responseHandler,IMapper _mapper, ILogger<CustomerService> _logger) : ICustomerService
    {
        public async Task<ApiResponse<string>> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _repository.GetTableNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (customer == null)
                {
                    _logger.LogWarning(CustomerLogMessages.CustomerNotFound, id);
                    return _responseHandler.NotFound<string>("Customer not found.");
                }

                // Soft Delete
                customer.IsActive = false;
                await _repository.DeleteAsync(customer);

                _logger.LogInformation(CustomerLogMessages.CustomerDeleted, id);
                return _responseHandler.Deleted<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CustomerLogMessages.ErrorDeletingCustomer, id);
                return _responseHandler.InternalServerError<string>("Error deleting customer.");
            }
        }

        public async Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _repository.GetTableNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

                if (customer == null)
                {
                    _logger.LogWarning(CustomerLogMessages.CustomerNotFound, id);
                    return _responseHandler.NotFound<CustomerResponseDto>("Customer not found.");
                }

                var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

                _logger.LogInformation(CustomerLogMessages.CustomerRetrieved, id);
                return _responseHandler.Success(customerResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CustomerLogMessages.ErrorRetrievingCustomer, id);
                return _responseHandler.InternalServerError<CustomerResponseDto>("Error retrieving customer.");
            }
        }

        public async Task<ApiResponse<string>> RegisterCustomerAsync(CustomerRegistrationDto customerDto)
        {
            try
            {
                if (await _repository.GetTableAsTracking()
                    .AnyAsync(c => c.Email.ToLower() == customerDto.Email.ToLower()))
                {
                    _logger.LogWarning(CustomerLogMessages.EmailConflict, customerDto.Email);
                    return _responseHandler.Conflict<string>("Email is already in use.");
                }

                var customer = _mapper.Map<Customer>(customerDto);
                customer.IsActive = true;

                await _repository.AddAsync(customer);

                _logger.LogInformation(CustomerLogMessages.CustomerCreated, customer.Id);
                return _responseHandler.Created<string>("Created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CustomerLogMessages.ErrorCreatingCustomer, customerDto.Email);
                return _responseHandler.InternalServerError<string>("Error creating customer.");
            }
        
        }

        public async Task<ApiResponse<string>> UpdateCustomerAsync(CustomerUpdateDto customerDto)
        {

            try
            {
                var customer = await _repository.GetByIdAsync(customerDto.CustomerId);
                if (customer == null)
                {
                    _logger.LogWarning(CustomerLogMessages.CustomerNotFound, customerDto.CustomerId);
                    return _responseHandler.NotFound<string>("Customer not found.");
                }

                if (customer.Email != customerDto.Email &&
                    await _repository.GetTableNoTracking()
                        .AnyAsync(c => c.Email == customerDto.Email))
                {
                    _logger.LogWarning(CustomerLogMessages.EmailConflict, customerDto.Email);
                    return _responseHandler.Conflict<string>("Email is already in use.");
                }

                _mapper.Map(customerDto, customer);
                await _repository.SaveChangesAsync();

                _logger.LogInformation(CustomerLogMessages.CustomerUpdated, customer.Id);
                return _responseHandler.Updated("Updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CustomerLogMessages.ErrorUpdatingCustomer, customerDto.CustomerId);
                return _responseHandler.InternalServerError<string>("Error updating customer.");
            }
        }
    }
    }

