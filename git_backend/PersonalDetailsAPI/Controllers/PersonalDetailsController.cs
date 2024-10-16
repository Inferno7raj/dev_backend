// PersonalDetailsController.cs
using Microsoft.AspNetCore.Mvc;
using PersonalDetailsAPI.Models;
using PersonalDetailsAPI.DAL;
using System.Collections.Generic;
using System.Linq;

namespace PersonalDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalDetailsController : ControllerBase
    {
        private readonly PersonalDetailsDAL _dal;

        public PersonalDetailsController(PersonalDetailsDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonalDetails details)
        {
            if (ModelState.IsValid)
            {
                _dal.InsertPersonalDetails(details);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Details saved successfully."
                });
            }
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid data.",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        [HttpGet]
        public IActionResult GetAllDetails()
        {
            var details = _dal.GetAllPersonalDetails();
            if (details == null || !details.Any())
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No personal details found."
                });
            }
            return Ok(new ApiResponse<List<PersonalDetails>>
            {
                Success = true,
                Data = details
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, [FromBody] PersonalDetails details)
        {
            if (ModelState.IsValid)
            {
                var existingPerson = _dal.GetPersonalDetailsById(id);
                if (existingPerson == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Person not found."
                    });
                }

                details.Id = id;
                _dal.UpdatePersonalDetails(details);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Details updated successfully."
                });
            }
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid data.",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            try
            {
                var isDeleted = _dal.DeletePersonalDetails(id);
                if (isDeleted)
                {
                    return Ok(new ApiResponse<string>
                    {
                        Success = true,
                        Message = "Person deleted successfully."
                    });
                }
                else
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Person not found."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Error deleting person.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
