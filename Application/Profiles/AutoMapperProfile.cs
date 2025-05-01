using Domain.Entities.Main;
using AutoMapper;
using Application.DTOs.User;
using Application.DTOs.Instructor;
using Application.DTOs.Goal;
using Application.DTOs.Level;
using Application.DTOs.TrainingType;
using Application.DTOs.Modality;
using Application.DTOs.Hashtag;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.DTOs.RoutineHasExercise;
using Application.DTOs.Auth;
using Domain.Entities.Relations;

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
            CreateMap<BaseRegisterDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            // Instructor mappings
            CreateMap<Instructor, InstructorOutputDTO>();
            CreateMap<CreateInstructorInputDTO, Instructor>();
            CreateMap<UpdateInstructorInputDTO, Instructor>();

            // Goal mappings
            CreateMap<Goal, GoalOutputDTO>();
            CreateMap<CreateGoalInputDTO, Goal>();
            CreateMap<UpdateGoalInputDTO, Goal>();

            // Level mappings
            CreateMap<Level, LevelOutputDTO>();
            CreateMap<CreateLevelInputDTO, Level>();
            CreateMap<UpdateLevelInputDTO, Level>();

            // Type mappings
            CreateMap<TrainingType, TypeOutputDTO>();
            CreateMap<CreateTypeInputDTO, TrainingType>();
            CreateMap<UpdateTypeInputDTO, TrainingType>();

            // Modality mappings
            CreateMap<Modality, ModalityOutputDTO>();
            CreateMap<CreateModalityInputDTO, Modality>();
            CreateMap<UpdateModalityInputDTO, Modality>();

            // Hashtag mappings
            CreateMap<Hashtag, HashtagOutputDTO>();
            CreateMap<CreateHashtagInputDTO, Hashtag>();
            CreateMap<UpdateHashtagInputDTO, Hashtag>();

            // Workout mappings
            CreateMap<Workout, WorkoutOutputDTO>()
                .ForMember(dest => dest.GoalIds, opt => opt.MapFrom(src => src.WorkoutGoals.Select(g => g.GoalId)))
                .ForMember(dest => dest.TypeIds, opt => opt.MapFrom(src => src.WorkoutTypes.Select(t => t.TypeId)))
                .ForMember(dest => dest.ModalityIds, opt => opt.MapFrom(src => src.WorkoutModalities.Select(m => m.ModalityId)))
                .ForMember(dest => dest.HashtagIds, opt => opt.MapFrom(src => src.WorkoutHashtags.Select(h => h.HashtagId)));

            CreateMap<CreateWorkoutInputDTO, Workout>();
            CreateMap<UpdateWorkoutInputDTO, Workout>();

            // Routine mappings
            CreateMap<Routine, RoutineOutputDTO>()
                .ForMember(dest => dest.GoalIds, opt => opt.MapFrom(src => src.RoutineGoals.Select(g => g.GoalId)))
                .ForMember(dest => dest.TypeIds, opt => opt.MapFrom(src => src.RoutineTypes.Select(t => t.TypeId)))
                .ForMember(dest => dest.ModalityIds, opt => opt.MapFrom(src => src.RoutineModalities.Select(m => m.ModalityId)))
                .ForMember(dest => dest.HashtagIds, opt => opt.MapFrom(src => src.RoutineHashtags.Select(h => h.HashtagId)));

            CreateMap<CreateRoutineInputDTO, Routine>();
            CreateMap<UpdateRoutineInputDTO, Routine>();

            // Exercise mappings
            CreateMap<Exercise, ExerciseOutputDTO>()
                .ForMember(dest => dest.GoalIds, opt => opt.MapFrom(src => src.ExerciseGoals.Select(g => g.GoalId)))
                .ForMember(dest => dest.TypeIds, opt => opt.MapFrom(src => src.ExerciseTypes.Select(t => t.TypeId)))
                .ForMember(dest => dest.ModalityIds, opt => opt.MapFrom(src => src.ExerciseModalities.Select(m => m.ModalityId)))
                .ForMember(dest => dest.HashtagIds, opt => opt.MapFrom(src => src.ExerciseHashtags.Select(h => h.HashtagId)))
                .ForMember(dest => dest.VariationIds, opt => opt.MapFrom(src => src.ExerciseVariations.Select(v => v.VariationId)));

            CreateMap<CreateExerciseInputDTO, Exercise>();
            CreateMap<UpdateExerciseInputDTO, Exercise>();

            CreateMap<RoutineHasExercise, RoutineHasExerciseOutputDTO>();
        }
    }
}
