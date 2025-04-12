using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class UserHasInstructor
    {
        public int UserId { get; set; }
        public int InstructorId { get; set; }

        public User User { get; set; } = null!;
        public Instructor Instructor { get; set; } = null!;
    }
}
