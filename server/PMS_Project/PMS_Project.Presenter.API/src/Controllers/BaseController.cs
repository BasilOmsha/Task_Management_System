using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class BaseController<T, TCreateDTO, TGetDto, TUpdateDTO> : ControllerBase
    {

        private readonly IBaseService<TCreateDTO, TGetDto, TUpdateDTO> _service;

        public BaseController(IBaseService<TCreateDTO, TGetDto, TUpdateDTO> service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(500)] // Internal server error
        public virtual async Task<ActionResult<TGetDto>> AddAsync([FromBody] TCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest();
                }
                var newEntity = await _service.AddAsync(createDTO);
                return Created(string.Empty, new { message = $"Resource {newEntity.GetType} created successfully.", data = newEntity });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        public virtual async Task<ActionResult<TGetDto>> GetByIdAsync([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }
                var entity = await _service.GetByIdAsync(id);
                return Ok(new { message = "Entity Retrieved", data = entity });
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Get all entities
        [HttpGet]
        [Authorize]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        public virtual async Task<ActionResult<IEnumerable<TGetDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _service.GetAllAsync();
                foreach (var entity in entities)
                {
                    if (entity == null)
                    {
                        return BadRequest();
                    }
                }
                return Ok(new { message = "Entities Retrieved", data = entities });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)] 
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 405)] // methode not allowed
        [ProducesResponseType(statusCode: 500)]
        public virtual async Task<ActionResult<TGetDto>> UpdateAsync([FromRoute] Guid id, [FromBody] TUpdateDTO updateDTO)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }
                if (updateDTO == null)
                {
                    return NoContent();
                }
                var updatedEntity = await _service.UpdateAsync(id, updateDTO);
                return Ok(new { message = "Entity updated successfully", data = updatedEntity });
            }
            catch (Exception ex)
            {
                return StatusCode(405, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)] // Bad request
        [ProducesResponseType(statusCode: 404)] // Not found
        [ProducesResponseType(statusCode: 500)] // Internal server error
        [ProducesResponseType(statusCode: 204)] // No content
        public virtual async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
               var userToDelete = await _service.GetByIdAsync(id);
                if (userToDelete == null)
                {
                    return NotFound();
                }
                var isDeleted = await _service.DeleteAsync(id);
                if (isDeleted)
                {
                    return Ok(new { message = "Entity deleted successfully" });

                }
                return StatusCode(500, "An error occurred while deleting the entity");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}