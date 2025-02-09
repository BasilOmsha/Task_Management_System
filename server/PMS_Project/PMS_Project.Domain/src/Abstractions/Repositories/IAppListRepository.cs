using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;

namespace PMS_Project.Domain.src.Abstractions.Repositories
{
    public interface IAppListRepository : IBaseRepository<AppList>
    {
        Task<IEnumerable<AppList>> GetListsByProjectBoardAsync(Guid projectBoardId);
        Task UpdateListPositionsAsync(IEnumerable<AppList> lists);
    }
}