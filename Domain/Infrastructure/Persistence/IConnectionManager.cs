namespace Domain.Infrastructure.Persistence
{
    public interface IConnectionManager
    {
        string GetConnectionString(string role);
        string GetCurrentConnectionString();
    }
}
