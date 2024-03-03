using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{

    public class LoginViewModel : ViewModelBase
    {
        private readonly UserService _usersService;
        private readonly AdminService _adminService;
        private readonly SuperAdminService _superAdminService;
        private readonly ApplicationDbContext _context;
        private readonly INavigationService _navigationService;
        private readonly CurrentUserService _currentUserService;
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }
        public LoginViewModel(INavigationService navigationService, CurrentUserService currentUserService)
        {
            _context = new ApplicationDbContext();
            _usersService = new UserService(_context);
            _adminService = new AdminService(_context);
            _superAdminService = new SuperAdminService(_context);
            _navigationService = navigationService;
            _currentUserService = currentUserService;
        }

        public RelayCommand Register
        {
            get => new(
            () =>
            {
                _navigationService.NavigateTo<RegisterViewModel>();
            }
        );
        }

        public RelayCommand Login
        {
            get => new(
                () =>
                {
                    try
                    {
                        if (_superAdminService.SuperAdminLogin(Email, Password))
                        {
                            _navigationService.NavigateTo<SuperAdminMenuViewModel>();
                            Email = null;
                            Password = null;
                        }
                        else if (_adminService.AdminLogin(Email, Password))
                        {
                            _navigationService.NavigateTo<AdminPanelViewModel>();
                            Email = null;
                            Password = null;
                        }
                        else if (_usersService.UserLogin(Email, Password))
                        {
                            var user = _usersService.GetUser(Email);
                            _currentUserService.UpdateUserData(user);
                            Email = null;
                            Password = null;
                            _navigationService.NavigateTo<MainPageViewModel>();
                        }
                        else
                        {
                            MessageBox.Show("Неправильный e-mail или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            Password = "";
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
        }



    }
}
