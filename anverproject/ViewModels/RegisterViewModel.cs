using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Trendyol.Models;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private string _name;
        private string _surname;
        private string _login;
        private string _email;
        private string _password;
        private string _trypassword;

        public string Name
        {
            get => _name;
            set
            {
                if (Regex.IsMatch(value, "^[A-Z][a-z]+$") || string.IsNullOrEmpty(value))
                {
                    Set(ref _name, value);
                }
                else
                {
                    MessageBox.Show("Неправильный формат имени (попробуйте ввести имя с первой заглавной буквой и без дополнительных знаков или чисел)","Error",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                    return;
                }
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (Regex.IsMatch(value, "^[A-Z][a-z]+$") || string.IsNullOrEmpty(value))
                {
                    Set(ref _surname, value);
                }
                else
                {
                    MessageBox.Show("Неправильный формат фамилии (попробуйте ввести фамилию с первой заглавной буквой и без дополнительных знаков или чисел)", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                if (Regex.IsMatch(value, "^[a-zA-Z0-9_-]{3,16}$") || string.IsNullOrEmpty(value))
                {
                    Set(ref _login, value);
                }
                else
                {
                    MessageBox.Show("Неправильный формат логина (попробуйте ввести логин с использованием букв без дополнительных знаков)", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (Regex.IsMatch(value, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$") || string.IsNullOrEmpty(value))
                {
                    Set(ref _email, value);
                }
                else
                {
                    MessageBox.Show("Неправильный формат e-mail (попробуйте ввести e-mail с использованием букв,цифр и с препиской например @gmail.com)", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (Regex.IsMatch(value, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()-_+=])[A-Za-z\d!@#$%^&*()-_+=]{8,}$") || string.IsNullOrEmpty(value))
                {
                    Set(ref _password, value);
                }
                else
                {
                    MessageBox.Show("Неправильный формат пароля (попробуйте ввести пароль с использованием заглавной и прописной буквы и цифры(минимальный размер 8))", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }

        public string TryPassword
        {
            get => _trypassword;
            set => Set(ref _trypassword, value);
        }

        public RegisterViewModel(INavigationService navigationService, ApplicationDbContext context)
        {
            _navigationService = navigationService;
            _context = context;
        }

        public RelayCommand LoginUser
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<LoginViewModel>();
                });
        }

        public RelayCommand Register
        {
            get => new(
                () =>
                {
                    try
                    {
                        using (ApplicationDbContext context = new ApplicationDbContext())
                        {
                            if (context.Users.Any(u => u.Login == Login || u.Email == Email))
                            {
                                MessageBox.Show("Пользователь с такими данными уже существует в базе данных", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else if (TryPassword != Password)
                            {
                                MessageBox.Show("Пароли не совпадают", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                User user = new User
                                {
                                    Name = Name,
                                    Surname = Surname,
                                    Login = Login,
                                    Email = Email,
                                    Password = BCrypt.Net.BCrypt.HashPassword(Password),
                                };
                                context.Users.Add(user);
                                context.SaveChanges();
                                MessageBox.Show("Регистрация пройдена успешно");
                                Name = "";
                                Surname = "";
                                Login = "";
                                Email = "";
                                Password = "";
                                TryPassword = "";
                                _navigationService.NavigateTo<LoginViewModel>();
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
