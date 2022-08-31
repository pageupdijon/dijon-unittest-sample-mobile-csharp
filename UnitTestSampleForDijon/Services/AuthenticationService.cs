using System;
using System.Threading.Tasks;
using UnitTestSampleForDijon.Commons;
using UnitTestSampleForDijon.Services.Interfaces;

namespace UnitTestSampleForDijon.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<bool> AuthenticateAsync(string firstName, string lastName)
        {
            if (firstName is null || lastName is null)
            {
                throw new ArgumentNullException(nameof(firstName) + " or " + nameof(lastName) + " is null");
            }
            if (firstName.Length< 3 || lastName.Length < 3)
            {
                throw new NameTooShortException();
            }
            if (firstName == "John" && lastName == "Doe")
            {
                return true;
            }
            return false;
        }
    }
}
