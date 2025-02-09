using PMS_Project.Application.DTOs;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IAppListService : IBaseService<CreateAppListDTO, AppListDTO, UpdateAppListDTO>
    {
        Task<IEnumerable<AppListDTO>> GetListsByProjectBoardAsync(Guid projectBoardId);
        Task UpdateListPositionAsync(Guid listId, int newPosition);
        Task UpdateMultipleListPositionsAsync(UpdateListPositionsDTO positionsDTO);
    }
}