using AutoMapper;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;
using PMS_Project.Domain.src.Abstractions.Repositories;

namespace PMS_Project.Application.Services
{
    public class AppListService : BaseService<AppList, CreateAppListDTO, AppListDTO, UpdateAppListDTO>, IAppListService
    {
        private readonly IAppListRepository _appListRepository;
        private readonly IMapper _mapper;

        public AppListService(IAppListRepository appListRepository, IMapper mapper)
            : base(appListRepository, mapper)
        {
            _appListRepository = appListRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppListDTO>> GetListsByProjectBoardAsync(Guid projectBoardId)
        {
            var lists = await _appListRepository.GetListsByProjectBoardAsync(projectBoardId);
            return _mapper.Map<IEnumerable<AppListDTO>>(lists)
                         .OrderBy(l => l.Position);
        }

        public async Task UpdateListPositionAsync(Guid listId, int newPosition)
        {
            var list = await _appListRepository.GetByIdAsync(listId);
            if (list == null)
                throw new KeyNotFoundException($"List with ID {listId} not found");

            list.Position = newPosition;
            await _appListRepository.UpdateAsync(list);
        }

        public async Task UpdateMultipleListPositionsAsync(UpdateListPositionsDTO positionsDTO)
        {
            var lists = new List<AppList>();
            foreach (var position in positionsDTO.ListPositions)
            {
                var list = await _appListRepository.GetByIdAsync(position.ListId);
                if (list != null)
                {
                    list.Position = position.NewPosition;
                    lists.Add(list);
                }
            }

            await _appListRepository.UpdateListPositionsAsync(lists);
        }

        public override async Task<AppListDTO> AddAsync(CreateAppListDTO createDTO)
        {
            var lists = await _appListRepository.GetListsByProjectBoardAsync(createDTO.ProjectBoardId);
            var maxPosition = lists.Any() ? lists.Max(l => l.Position) : -1;
            
            var appList = _mapper.Map<AppList>(createDTO);
            appList.Position = createDTO.Position != 0 ? createDTO.Position : maxPosition + 1;
            
            var addedList = await _appListRepository.AddAsync(appList);
            return _mapper.Map<AppListDTO>(addedList);
        }
    }
}