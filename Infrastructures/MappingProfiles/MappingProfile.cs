using AutoMapper;
using Infrastructures.Dtos;
using Infrastructures.Models;

namespace Infrastructures.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User entity mappings
        CreateMap<User, UserDto>();
        CreateMap<User, UserShortDto>();

        // Project entity mappings
        CreateMap<Project, ProjectListDto>()
            .ForMember(dest => dest.TeamMembers, opt => opt.MapFrom(src => src.ProjectMembers));
        CreateMap<Project, ProjectDetailDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.TeamMembers, opt => opt.MapFrom(src => src.ProjectMembers));

        // ProjectMember entity mappings
        CreateMap<ProjectMember, ProjectMemberDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.AssignedTasksCount, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedTasksCount, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.IsDeleted))
            .ForMember(dest => dest.LastActive, opt => opt.Ignore());
        
        CreateMap<ProjectMember, ProjectMemberShortDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl));

        // Task entity mappings
        CreateMap<ProjectTask, ProjectTaskDto>()
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.Tags) ? new List<string>() : src.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()));
        
        CreateMap<ProjectTask, ProjectTaskDetailDto>()
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.TaskComments))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.TaskAttachments))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.Tags) ? new List<string>() : src.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()));
            
        // TaskComment entity mappings
        CreateMap<TaskComment, ProjectTaskCommentDto>()
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CommentText))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            
        // TaskAttachment entity mappings
        CreateMap<TaskAttachment, ProjectTaskAttachmentDto>()
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FilePath))
            .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom(src => src.UploadedBy));
    }
} 