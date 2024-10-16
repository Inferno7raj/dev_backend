// ItemsController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using PersonalDetailsAPI.Exceptions; // Import the custom exception
using System.Collections.Generic;

namespace PersonalDetailsAPI.Controllers // Update with your actual namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ItemDto>>> CreateItem(ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Validation errors occurred",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                };
                return BadRequest(response);
            }

            try
            {
                var result = await _service.CreateItem(itemDto);
                var response = new ApiResponse<ItemDto>
                {
                    Success = true,
                    Message = "Item created successfully",
                    Data = result
                };
                return CreatedAtAction(nameof(GetItem), new { id = result.Id }, response);
            }
            catch (DuplicateEntryException ex) // Custom exception for duplicate entries
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Duplicate entry error",
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log the exception if needed, or let the middleware handle it
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> GetItem(int id)
        {
            var result = await _service.GetItem(id);
            if (result == null)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Item not found",
                    Errors = new List<string> { "The requested item does not exist." }
                };
                return NotFound(response);
            }

            var successResponse = new ApiResponse<ItemDto>
            {
                Success = true,
                Message = "Item retrieved successfully",
                Data = result
            };
            return Ok(successResponse);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateItem(int id, ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Validation errors occurred",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                };
                return BadRequest(response);
            }

            try
            {
                itemDto.Id = id; // Ensure that the correct ID is passed
                await _service.UpdateItem(itemDto);

                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "Item updated successfully"
                };
                return Ok(response);
            }
            catch (DuplicateEntryException ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Duplicate entry error",
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                throw; // Let the middleware handle the error
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteItem(int id)
        {
            try
            {
                await _service.DeleteItem(id);
                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "Item deleted successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions related to deletion
                throw; // Let the middleware handle it
            }
        }
    }
}
