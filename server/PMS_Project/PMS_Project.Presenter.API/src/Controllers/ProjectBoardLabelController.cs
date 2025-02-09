using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectBoardLabelController : ControllerBase
    {
        private readonly IProjectBoardLabelService _projectBoardLabelService;

        public ProjectBoardLabelController(IProjectBoardLabelService projectBoardLabelService)
        {
            _projectBoardLabelService = projectBoardLabelService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ProjectBoardLabelDTO projectBoardLabelDTO)
        {
            var result = await _projectBoardLabelService.AddAsync(projectBoardLabelDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, ProjectBoardLabelDTO projectBoardLabelDTO)
        {
            var result = await _projectBoardLabelService.UpdateAsync(id, projectBoardLabelDTO);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _projectBoardLabelService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _projectBoardLabelService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _projectBoardLabelService.GetAllAsync();
            return Ok(result);
        }
    }
}