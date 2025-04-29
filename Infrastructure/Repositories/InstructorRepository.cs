using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InstructorRepository(AppDbContext context) : GenericRepository<Instructor>(context), IInstructorRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Instructor?> GetInstructorByEmailAsync(string email)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId)
        {
            return await _context.Instructors
                .Where(i => i.UserInstructors.Any(ui => ui.UserId == userId))
                .ToListAsync();
        }

        public async Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Workouts.Any(w => w.Id == workoutId));
        }

        public async Task<Instructor?> GetInstructorByRoutineIdAsync(int routineId)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Routines.Any(r => r.Id == routineId));
        }

        public async Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Exercises.Any(e => e.Id == exerciseId));
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Instructors.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Instructors
                .AnyAsync(i => i.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<bool> IsEmailTakenByOtherAsync(string email, int currentId)
        {
            return await _context.Instructors
                .AnyAsync(i => i.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && i.Id != currentId);
        }
    }
}
