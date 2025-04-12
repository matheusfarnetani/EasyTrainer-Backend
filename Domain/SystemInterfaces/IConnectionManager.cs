namespace Domain.SystemInterfaces
{
    public interface IConnectionManager
    {
        string GetConnectionString(string role);
    }
}
