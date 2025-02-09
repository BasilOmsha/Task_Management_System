using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    [Authorize]
    public class AppListController : BaseController<AppList, CreateAppListDTO, AppListDTO, UpdateAppListDTO>
    {
        private readonly IAppListService _appListService;

        public AppListController(IAppListService appListService) : base(appListService)
        {
            _appListService = appListService;
        }

        [HttpGet("projectBoard/{projectBoardId}")]
        public async Task<ActionResult<IEnumerable<AppListDTO>>> GetByProjectBoardAsync(Guid projectBoardId)
        {
            try
            {
                var lists = await _appListService.GetListsByProjectBoardAsync(projectBoardId);
                return Ok(new { message = "Lists retrieved successfully", data = lists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(Guid id, [FromBody] int newPosition)
        {
            try
            {
                await _appListService.UpdateListPositionAsync(id, newPosition);
                return Ok(new { message = "List position updated successfully" });
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
        public async Task<IActionResult> UpdateMultiplePositions([FromBody] UpdateListPositionsDTO positionsDTO)
        {
            try
            {
                await _appListService.UpdateMultipleListPositionsAsync(positionsDTO);
                return Ok(new { message = "List positions updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}