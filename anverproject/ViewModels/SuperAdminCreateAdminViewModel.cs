﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class SuperAdminCreateAdminViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private readonly AdminService _adminservice;

        private string _login;
        private string _password;
        private string _trypassword;

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string TryPassword
        {
            get => _trypassword;
            set => Set(ref _trypassword, value);
        }

        public SuperAdminCreateAdminViewModel(INavigationService navigationService, ApplicationDbContext context)
        {
            _navigationService = navigationService;
            _context = context;
            _adminservice = new AdminService(_context);
        }

        public RelayCommand Back
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<SuperAdminMenuViewModel>();
                });
        }

        public RelayCommand Add
        {
            get => new(
                () =>
                {
                    try
                    {
                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {
                            if (_context.Admin.Any(a => a.Name == Login))
                            {
                                MessageBox.Show("Админ с такими данными уже существует в базе данных", "Ошибка");
                                return;
                            }
                            else if (TryPassword != Password)
                            {
                                MessageBox.Show("Вы неправильно ввели повторный пароль", "Ошибка");
                                return;
                            }
                            else
                            {
                                var newadmin = _adminservice.AdminRegister(Login, Password);
                                _context.Admin.Add(newadmin);
                                _context.SaveChanges();
                                MessageBox.Show("Успешно создан", "Admin");
                                Password = "";
                                TryPassword = "";
                                _navigationService.NavigateTo<SuperAdminMenuViewModel>();
                            }
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
