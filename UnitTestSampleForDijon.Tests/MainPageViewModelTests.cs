using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Prism.Navigation;
using UnitTestSampleForDijon.Commons;
using UnitTestSampleForDijon.Services;
using UnitTestSampleForDijon.Services.Interfaces;
using UnitTestSampleForDijon.ViewModels;
using Xunit;

namespace UnitTestSampleForDijon.Tests
{
    public class MainPageViewModelTests
    {
        [Fact]
        public void TitleIsSetToMainPage()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);



            Assert.Equal("Main Page", MainVm.Title);
        }

        [Fact]
        public void NavigateToSecondPage()
        {
            var navigationService = new Mock<INavigationService>();
            var MainVm = new MainPageViewModel(navigationService.Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);


            MainVm.OnNavigateCommand.Execute();

            navigationService.Verify(x => x.NavigateAsync("NavigationPage/SecondPage"));
        }

        [Fact]
        public void UpdateFirstNameShouldUpdateFullName()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);


            MainVm.FirstName = "John";
            MainVm.LastName = "Doe";

            Assert.Equal("John Doe", MainVm.FullName);
        }

        [Fact]
        public void UpdateLastNameShouldUpdateFullName()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);


            MainVm.FirstName = "John";
            var handle = new AutoResetEvent(false);
            MainVm.PropertyChanged += (sender, e) =>
                                      {
                                          if (e.PropertyName == nameof(MainVm.FullName))
                                          {
                                              handle.Set();
                                          }
                                      };

            MainVm.LastName = "Doe";
            Assert.True(handle.WaitOne(TimeSpan.FromMilliseconds(100)));
            Assert.Equal("John Doe", MainVm.FullName);
        }

        [Fact]
        public void IfFirstNameIsEmptyOnValidateCommandShouldReturnFalse()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);


            MainVm.FirstName = "";
            MainVm.LastName = "Doe";

            Assert.False(MainVm.OnValidateCommand.CanExecute());
        }


        [Fact]
        public void IfLastNameIsEmptyOnValidateCommandShouldReturnFalse()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);


            MainVm.FirstName = "John";
            MainVm.LastName = "";

            Assert.False(MainVm.OnValidateCommand.CanExecute());
        }

        [Fact]
        public void IfFirstNameAndLastNameAreNotEmptyOnValidateCommandShouldReturnTrue()
        {
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object,
                                               new Mock<ILoggerService>().Object,
                                               new Mock<IAlertDialogService>().Object,
                                               new Mock<IAuthenticationService>().Object);

            MainVm.FirstName = "John";
            MainVm.LastName = "Doe";

            Assert.True(MainVm.OnValidateCommand.CanExecute());
        }

        [Fact]
        public async Task OnValidateCommandShouldLogInfoAboutUser()
        {
            var logger = new Mock<ILoggerService>();
            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object, logger.Object, new Mock<IAlertDialogService>().Object, new Mock<IAuthenticationService>().Object);

            MainVm.FirstName = "John";
            MainVm.LastName = "Doe";

            await MainVm.OnValidate();
            logger.Verify(x => x.Info($"User {MainVm.FullName} is asking to log in"));
        }

        [Fact]
        public async Task OnValidateShouldDisplayMessageIfNameTooShortExceptionThrown()
        {
            var alertDialog = new Mock<IAlertDialogService>();
            var authenticate = new Mock<IAuthenticationService>();

            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object, new Mock<ILoggerService>().Object, alertDialog.Object, authenticate.Object);
            authenticate.Setup(x => x.AuthenticateAsync(It.IsAny<string>(),It.IsAny<string>())).Throws(new NameTooShortException());

            await MainVm.OnValidate();
            alertDialog.Verify(x => x.ShowAlert("Name is too short"));


        }


        [Fact]
        public async Task AuthenticateAsyncShouldReturnTrueIfJohnDoe()
        {
            var authenticationService = new AuthenticationService();
            var alertDialog = new Mock<IAlertDialogService>();

            var MainVm = new MainPageViewModel(new Mock<INavigationService>().Object, new Mock<ILoggerService>().Object, alertDialog.Object, authenticationService);
            MainVm.FirstName = "Jo";
            MainVm.LastName = "Doe";

            await MainVm.OnValidate();
            alertDialog.Verify(x => x.ShowAlert("Name is too short"));

            //Assert.True(await authenticationService.AuthenticateAsync("John", "Doe"));
        }




        // [Fact]
        // public void NavigateToSecondPageWithParameter()
        // {
        //     var navigationService = new Mock<INavigationService>();
        //     var MainVm = new MainPageViewModel(navigationService.Object);
        //
        //     MainVm.NavigateToSecondPageWithParameterCommand.Execute(null);
        //
        //     navigationService.Verify(x => x.NavigateAsync("SecondPage", It.IsAny<NavigationParameters>()));
        // }
        //
        // [Fact]
    }
}
