namespace Domain.API.Interfaces
{
    public interface IAdminCredentialProvider
    {
        string Email { get; }
        string PasswordHash { get; }
    }

}
