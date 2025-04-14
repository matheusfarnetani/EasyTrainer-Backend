using Application.DTOs;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using Application.Exceptions;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class UserService : GenericService<User, CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IMapper mapper)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<UserOutputDTO> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email)
                ?? throw new EntityNotFoundException(nameof(User), 0);

            return _mapper.Map<UserOutputDTO>(user);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination)
        {
            var users = await _userRepository.GetUsersByInstructorIdAsync(instructorId);
            return Paginate(users, pagination);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination)
        {
            var users = await _userRepository.GetUsersByGoalIdAsync(goalId);
            return Paginate(users, pagination);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination)
        {
            var users = await _userRepository.GetUsersByLevelIdAsync(levelId);
            return Paginate(users, pagination);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination)
        {
            var users = await _userRepository.GetUsersByWorkoutIdAsync(workoutId);
            return Paginate(users, pagination);
        }

        private PaginationResponseDTO<UserOutputDTO> Paginate(IEnumerable<User> users, PaginationRequestDTO pagination)
        {
            var paginated = users
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<UserOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = users.Count(),
                Data = _mapper.Map<List<UserOutputDTO>>(paginated)
            };
        }
    }
}
