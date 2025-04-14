using Application.DTOs.Instructor;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using Application.Exceptions;
using Application.DTOs;

namespace Application.Services.Implementations
{
    public class InstructorService : GenericService<Instructor, CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>, IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository, IMapper mapper)
            : base(instructorRepository, mapper)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorOutputDTO> GetByEmailAsync(string email)
        {
            var instructor = await _instructorRepository.GetInstructorByEmailAsync(email)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<InstructorOutputDTO>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            var instructors = await _instructorRepository.GetInstructorsByUserIdAsync(userId);

            var paginated = instructors
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<InstructorOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = instructors.Count(),
                Data = _mapper.Map<List<InstructorOutputDTO>>(paginated)
            };
        }

        public async Task<InstructorOutputDTO> GetByWorkoutIdAsync(int workoutId)
        {
            var instructor = await _instructorRepository.GetInstructorByWorkoutIdAsync(workoutId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<InstructorOutputDTO> GetByRoutineIdAsync(int routineId)
        {
            var instructor = await _instructorRepository.GetInstructorByRoutineIdAsync(routineId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<InstructorOutputDTO> GetByExerciseIdAsync(int exerciseId)
        {
            var instructor = await _instructorRepository.GetInstructorByExerciseIdAsync(exerciseId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }
    }
}
