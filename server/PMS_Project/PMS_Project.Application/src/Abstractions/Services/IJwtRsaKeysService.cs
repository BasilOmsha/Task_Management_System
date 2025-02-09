using System.Security.Cryptography;

namespace PMS_Project.Application.Abstractions.Services
{
    public interface IJwtRsaKeysService
    {
        RSA SigningKey { get; }
        RSA ValidationKey { get; }
    }
}