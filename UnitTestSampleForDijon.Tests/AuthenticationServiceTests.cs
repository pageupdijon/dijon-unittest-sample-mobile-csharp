using System;
using System.Threading.Tasks;
using UnitTestSampleForDijon.Commons;
using UnitTestSampleForDijon.Services;
using UnitTestSampleForDijon.Services.Interfaces;
using Xunit;

namespace UnitTestSampleForDijon.Tests
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task AuthenticateAsyncShouldReturnTrueIfJohnDoe()
        {
            var authenticationService = new AuthenticationService();

            Assert.True(await authenticationService.AuthenticateAsync("John", "Doe"));
        }

        [Fact]
        public async Task AuthenticateAsyncShouldReturnFalseForAnyoneNotNamedJohnDoe()
        {
            var authenticationService = new AuthenticationService();

            Assert.False(await authenticationService.AuthenticateAsync("John", "Smith"));
        }

        [Fact]
        public async Task AuthenticateAsyncShouldThrowNameTooShortExceptionIfFirstNameLessThan3Characters()
        {
            var authenticationService = new AuthenticationService();

            await Assert.ThrowsAsync<NameTooShortException>(() => authenticationService.AuthenticateAsync("Jo", "Doe"));
        }

        [Fact]
        public async Task AuthenticateAsyncShouldThrowNameTooShortExceptionIfLastNameLessThan3Characters()
        {
            var authenticationService = new AuthenticationService();

            await Assert.ThrowsAsync<NameTooShortException>(() => authenticationService.AuthenticateAsync("John", "Do"));
        }

        [Fact]
        public async Task AuthenticateAsyncShouldHandleNullParameters()
        {
            var authenticationService = new AuthenticationService();

            await Assert.ThrowsAsync<ArgumentNullException>(() => authenticationService.AuthenticateAsync(null, null));
        }
    }
}
