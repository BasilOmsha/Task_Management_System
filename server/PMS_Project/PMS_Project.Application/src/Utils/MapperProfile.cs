using AutoMapper;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;

namespace PMS_Project.Application.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, GetUserInfoDTO>();
            CreateMap<UpdateUserInfoDTO, User>();
            CreateMap<User, UpdateUserInfoDTO>();

            CreateMap<CreateRoleDTO, Role>();
            CreateMap<Role, GetRoleDTO>();
            CreateMap<UpdateRoleDTO, Role>();
            CreateMap<Role, UpdateRoleDTO>();

            CreateMap<CreateWorkspaceDTO, Workspace>();
            CreateMap<Workspace, GetWorkspaceDTO>();
            CreateMap<UpdateWorkspaceDTO, Workspace>();
            CreateMap<Workspace, UpdateWorkspaceDTO>();

            CreateMap<CreateWorkspaceUserDTO, Workspace_User>();
            CreateMap<Workspace_User, GetWorkspaceUserDTO>();
            CreateMap<UpdateWorkspaceUserDTO, Workspace_User>();
            CreateMap<Workspace_User, UpdateWorkspaceUserDTO>();

            CreateMap<CreateProjectBoardDTO, ProjectBoard>();
            CreateMap<ProjectBoard, ProjectBoardDTO>();
            CreateMap<UpdateProjectBoardDTO, ProjectBoard>();
            CreateMap<ProjectBoard, UpdateProjectBoardDTO>();

            CreateMap<CreateProjectBoardUserDTO, ProjectBoard_User>(); //also for update
            CreateMap<ProjectBoard_User, GetProjectBoardUserDTO>();
            CreateMap<ProjectBoard, CreateProjectBoardUserDTO>();

            CreateMap<AppList, AppListDTO>()
                .ReverseMap();
            
            CreateMap<CreateAppListDTO, AppList>();
            CreateMap<UpdateAppListDTO, AppList>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TaskCard, TaskCardDTO>()
                .ReverseMap();

            CreateMap<CreateTaskCardDTO, TaskCard>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<TaskCard, TaskCardDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskCard_ProjectBoardLabels.Select(tl => tl.ProjectBoardLabel)));

            CreateMap<TaskCardDTO, TaskCard>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.TaskCard_ProjectBoardLabels, opt => opt.Ignore());

            CreateMap<TaskCardActivity, TaskCardActivityDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();

            
        }
    }
}