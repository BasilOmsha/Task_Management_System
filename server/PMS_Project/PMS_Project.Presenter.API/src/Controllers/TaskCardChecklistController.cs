using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Services;

namespace PMS_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCardChecklistController : ControllerBase
    {
        private readonly ITaskCardChecklistService _service;

        public TaskCardChecklistController(ITaskCardChecklistService service)
        {
            _service = service;
        }

        [HttpGet("taskcard/{taskCardId}")]
        public async Task<IActionResult> GetByTaskCardId(Guid taskCardId)
        {
            var checklists = await _service.GetByTaskCardIdAsync(taskCardId);
            return Ok(checklists);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskCardChecklistDTO dto)
        {
            var checklist = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetByTaskCardId), new { taskCardId = checklist.TaskCardId }, checklist);
        }

        // ...other CRUD actions...
    }
}