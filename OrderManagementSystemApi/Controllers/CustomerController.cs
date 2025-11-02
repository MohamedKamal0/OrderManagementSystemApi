using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Customer;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService _customerService) : ControllerBase
    {
        [HttpPost("RegisterCustomer")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> RegisterCustomer([FromBody] CustomerRegistrationDto customerDto)
        {
            var response = await _customerService.RegisterCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpGet("GetCustomerById/{id}")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetCustomerById(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateCustomer([FromBody] CustomerUpdateDto customerDto)
        {
            var response = await _customerService.UpdateCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> DeleteCustomer(int id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
