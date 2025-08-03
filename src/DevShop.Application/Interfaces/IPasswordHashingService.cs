namespace DevShop.Application.Interfaces
{
    public interface IPasswordHashingService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
