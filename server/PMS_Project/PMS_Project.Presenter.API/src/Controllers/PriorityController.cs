
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController : BaseController<PriorityDTO, PriorityDTO, PriorityDTO, PriorityDTO>
    {
        private readonly IPriorityService _priorityService;

        public PriorityController(IPriorityService priorityService) : base(priorityService)
        {
            _priorityService = priorityService;
        }
    }
}