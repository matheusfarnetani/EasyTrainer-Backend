namespace Domain.Infrastructure
{
    public interface IConnectionManager
    {
        string GetConnectionString(string role);
        string GetCurrentConnectionString();
    }
}
