
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Customer;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using Org.BouncyCastle.Crypto.Generators;
namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class CustomerService(ICustomerRepository _repository) : ICustomerService
    {
        public async Task<ApiResponse<ConfirmationResponseDto>> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _repository.GetTableNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDto>(404, "Customer not found.");
                }
                //Soft Delete
                customer.IsActive = false;
                await _repository.DeleteAsync(customer);

                var confirmationMessage = new ConfirmationResponseDto
                {
                    Message = $"Customer with Id {id} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDto>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDto>
                    (500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");

            }
        }

        public async Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _repository.GetTableNoTracking()

                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);
                if (customer == null)
                {
                    return new ApiResponse<CustomerResponseDto>(404, "Customer not found.");
                }
                var customerResponse = new CustomerResponseDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };
                return new ApiResponse<CustomerResponseDto>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CustomerResponseDto>> RegisterCustomerAsync(CustomerRegistrationDto customerDto)
        {
            try
            {
                if (await _repository.GetTableAsTracking().AnyAsync(c => c.Email.ToLower() == customerDto.Email.ToLower()))
                    return new ApiResponse<CustomerResponseDto>(400, "Email is already in use.");

                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                    DateOfBirth = customerDto.DateOfBirth,
                    IsActive = true,
                };

                await _repository.AddAsync(customer);

                var customerResponse = new CustomerResponseDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };

                return new ApiResponse<CustomerResponseDto>(200, customerResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerResponseDto>(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> UpdateCustomerAsync(CustomerUpdateDto customerDto)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(customerDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDto>(404, "Customer not found.");
                }
                if (customer.Email != customerDto.Email && await _repository.GetTableNoTracking().AnyAsync(c => c.Email == customerDto.Email))
                {
                    return new ApiResponse<ConfirmationResponseDto>(400, "Email is already in use.");
                }
                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.Email = customerDto.Email;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.DateOfBirth = customerDto.DateOfBirth;
                await _repository.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDto
                {
                    Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDto>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                
                return new ApiResponse<ConfirmationResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
