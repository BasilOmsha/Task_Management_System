using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Services;

namespace PMS_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCardActivityController : ControllerBase
    {
        private readonly ITaskCardActivityService _service;

        public TaskCardActivityController(ITaskCardActivityService service)
        {
            _service = service;
        }

        [HttpGet("taskcard/{taskCardId}")]
        public async Task<IActionResult> GetByTaskCardId(Guid taskCardId)
        {
            var activities = await _service.GetByTaskCardIdAsync(taskCardId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskCardActivityDTO dto)
        {
            var activity = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetByTaskCardId), new { taskCardId = activity.TaskCardId }, activity);
        }

        // ...other CRUD actions...
    }
}