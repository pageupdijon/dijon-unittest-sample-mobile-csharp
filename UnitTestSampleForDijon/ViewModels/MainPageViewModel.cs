using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestSampleForDijon.Commons;
using UnitTestSampleForDijon.Services.Interfaces;

namespace UnitTestSampleForDijon.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IAlertDialogService _alertDialogService;
        private readonly IAuthenticationService _authenticationService;

        public MainPageViewModel(INavigationService navigationService,
                                 ILoggerService loggerService,
                                 IAlertDialogService alertDialogService,
                                 IAuthenticationService authenticationService)
            : base (navigationService)
        {
            _loggerService = loggerService;
            _alertDialogService = alertDialogService;
            _authenticationService = authenticationService;
            Title = "Main Page";
        }


        #region string => FirstName

        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                RaisePropertyChanged(nameof(FullName));
                OnValidateCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region string => LastName

        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                RaisePropertyChanged(nameof(FullName));
                OnValidateCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region string => FullName

        private string _fullName;

        public string FullName => FirstName + " " + LastName;
        #endregion

        #region DelegateCommand => OnValidateCommand
        public DelegateCommand OnValidateCommand => new DelegateCommand(async () => await OnValidate(), CanValidate);
        #endregion

        #region void => OnValidate()
        public async Task OnValidate()
        {
            _loggerService.Info($"User {FullName} is asking to log in");
            try
            {
                await _authenticationService.AuthenticateAsync(FirstName, LastName);

            }
            catch (NameTooShortException ntse)
            {
                _alertDialogService.ShowAlert("Name is too short");
            }

        }
        #endregion

        private bool CanValidate()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        }


        #region DelegateCommand => OnNavigateCommand
        public DelegateCommand OnNavigateCommand => new DelegateCommand(OnNavigate);
        #endregion

        #region void => OnNavigate()
        private void OnNavigate()
        {
             NavigationService.NavigateAsync("NavigationPage/SecondPage");
        }
        #endregion





    }
}
