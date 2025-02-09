
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Interfaces.Services;

namespace PMS_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCardChecklistItemController : ControllerBase
    {
        private readonly ITaskCardChecklistItemService _service;

        public TaskCardChecklistItemController(ITaskCardChecklistItemService service)
        {
            _service = service;
        }

        [HttpGet("checklist/{checklistId}")]
        public async Task<IActionResult> GetByChecklistId(Guid checklistId)
        {
            var items = await _service.GetByChecklistIdAsync(checklistId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskCardChecklistItemDTO dto)
        {
            var item = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetByChecklistId), new { checklistId = item.TaskCardChecklistId }, item);
        }

        // ...other CRUD actions...
    }
}