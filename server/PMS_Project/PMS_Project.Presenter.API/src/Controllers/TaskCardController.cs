using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class TaskCardController : BaseController<TaskCardDTO, CreateTaskCardDTO, TaskCardDTO, TaskCardDTO>
    {
        private readonly ITaskCardService _taskCardService;

        public TaskCardController(ITaskCardService taskCardService) : base(taskCardService)
        {
            _taskCardService = taskCardService;
        }

        /*public async Task<IActionResult> GetById(Guid id)
        {
            var taskCard = await _taskCardService.GetByIdAsync(id);
            return Ok(taskCard);
        }*/

        [HttpPost]
        public override async Task<ActionResult<TaskCardDTO>> AddAsync([FromBody] CreateTaskCardDTO createTaskCardDTO)
        {
            try
            {
                if (createTaskCardDTO == null)
                {
                    return BadRequest("Task card data is required");
                }

                var newEntity = await _taskCardService.AddAsync(createTaskCardDTO);
                return Created(string.Empty, new
                {
                    message = "Task card created successfully",
                    data = newEntity
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the task card",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("list/{listId}")]
        public async Task<ActionResult<IEnumerable<TaskCardDTO>>> GetByListAsync(Guid listId)
        {
            try
            {
                var taskCards = await _taskCardService.GetTaskCardsByListAsync(listId);
                return Ok(new { message = "Task cards retrieved successfully", data = taskCards });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(Guid id, [FromBody] TaskCardPositionUpdateDTO updateDTO)
        {
            try
            {
                await _taskCardService.UpdateTaskCardPositionAsync(id, updateDTO.ListId, updateDTO.Position);
                return Ok(new { message = "Task card position updated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updatePositions")]
        public async Task<IActionResult> UpdateMultiplePositions([FromBody] IEnumerable<TaskCardPositionUpdateDTO> updates)
        {
            try
            {
                await _taskCardService.UpdateMultipleTaskCardPositionsAsync(updates);
                return Ok(new { message = "Task card positions updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}