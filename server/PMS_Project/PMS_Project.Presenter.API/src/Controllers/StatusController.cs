
using Microsoft.AspNetCore.Mvc;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;

namespace PMS_Project.Presenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : BaseController<StatusDTO, StatusDTO, StatusDTO, StatusDTO>
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService) : base(statusService)
        {
            _statusService = statusService;
        }
    }
}