using Domain.Entities.Main;
using AutoMapper;
using Application.DTOs.User;
using Application.DTOs.Instructor;
using Application.DTOs.Goal;

namespace Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserOutputDTO>()
                .ForMember(dest => dest.GoalIds, opt => opt.MapFrom(src => src.UserGoals.Select(g => g.GoalId)))
                .ForMember(dest => dest.InstructorIds, opt => opt.MapFrom(src => src.UserInstructors.Select(i => i.InstructorId)));

            CreateMap<CreateUserInputDTO, User>();
            CreateMap<UpdateUserInputDTO, User>();

            // Instructor mappings
            CreateMap<Instructor, InstructorOutputDTO>();
            CreateMap<CreateInstructorInputDTO, Instructor>();
            CreateMap<UpdateInstructorInputDTO, Instructor>();

            // Goal mappings
            CreateMap<Goal, GoalOutputDTO>();
            CreateMap<CreateGoalInputDTO, Goal>();
            CreateMap<UpdateGoalInputDTO, Goal>();
        }
    }
}
