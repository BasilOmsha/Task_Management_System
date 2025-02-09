namespace PMS_Project.Application.Abstractions.Services
{
    public interface IPaswdService
    {
        string HashPaswd(string paswd);
        bool VerifyPaswd(string paswd, string hashedPaswd);

    }
}