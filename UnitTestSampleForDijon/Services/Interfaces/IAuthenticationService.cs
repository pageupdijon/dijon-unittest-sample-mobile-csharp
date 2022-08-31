using System.Threading.Tasks;

namespace UnitTestSampleForDijon.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(string firstName, string lastName);
    }
}
