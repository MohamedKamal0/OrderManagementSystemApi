using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(IAddressService _addressService) : ControllerBase
    {

        [HttpPost("CreateAddress")]
        public async Task<ActionResult<ApiResponse<AddressResponseDto>>> CreateAddress([FromBody] AddressCreateDto addressDto)
        {
            var response = await _addressService.CreateAddressAsync(addressDto);

            return Ok(response);
        }
        [HttpGet("GetAddressById/{id}")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<AddressResponseDto>>> GetAddressById(int id)
        {
            var response = await _addressService.GetAddressByIdAsync(id);

            return Ok(response);
        }
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateAddress([FromBody] AddressUpdateDto addressDto)
        {
            var response = await _addressService.UpdateAddressAsync(addressDto);

            return Ok(response);
        }
        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> DeleteAddress([FromBody] AddressDeleteDto addressDeleteDTO)
        {
            var response = await _addressService.DeleteAddressAsync(addressDeleteDTO);

            return Ok(response);
        }
        [HttpGet("GetAddressesByCustomer/{customerId}")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<List<AddressResponseDto>>>> GetAddressesByCustomer(int customerId)
        {
            var response = await _addressService.GetAddressesByCustomerAsync(customerId);

            return Ok(response);
        }
    }
}
