namespace Domain.API.Interfaces
{
    public interface ICurrentUserContext
    {
        int Id { get; }
        string Role { get; }
        string Email { get; }
        bool IsExternalRequest { get; }
    }
}