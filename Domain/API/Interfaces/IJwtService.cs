namespace Domain.API.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string role, string email);
    }
}