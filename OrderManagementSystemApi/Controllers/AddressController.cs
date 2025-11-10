using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemApplication.Services.Implemntation;

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
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpGet("GetAddressById/{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponseDto>>> GetAddressById(int id)
        {
            var response = await _addressService.GetAddressByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateAddress([FromBody] AddressUpdateDto addressDto)
        {
            var response = await _addressService.UpdateAddressAsync(addressDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> DeleteAddress([FromBody] AddressDeleteDto addressDeleteDTO)
        {
            var response = await _addressService.DeleteAddressAsync(addressDeleteDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpGet("GetAddressesByCustomer/{customerId}")]
        public async Task<ActionResult<ApiResponse<List<AddressResponseDto>>>> GetAddressesByCustomer(int customerId)
        {
            var response = await _addressService.GetAddressesByCustomerAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
