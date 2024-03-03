using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trendyol.Models;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class SuperAdminMenuViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private ObservableCollection<User> _user;
        private ObservableCollection<Order> _order;
        private ObservableCollection<Admin> _admin;
        private RadioButtons _radioButtons;
        private User _selectedUser;
        private Order _selectedOrder;
        private Admin _selectedAdmin;

        public ObservableCollection<User> User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }

        public ObservableCollection<Order> Order
        {
            get => _order;
            set => Set(ref _order, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        public ObservableCollection<Admin> Admin
        {
            get => _admin;
            set => Set(ref _admin, value);
        }

        public Admin SelectedAdmin
        {
            get => _selectedAdmin;
            set => Set(ref _selectedAdmin, value);
        }

        public RadioButtons RadioButton
        {
            get => _radioButtons;
            set => Set(ref _radioButtons, value);
        }

        public SuperAdminMenuViewModel(INavigationService navigationService, ApplicationDbContext context)
        {
            _navigationService = navigationService;
            _context = context;
            RadioButton = new RadioButtons();
            User = new ObservableCollection<User>(_context.Users.ToList());
            Order = new ObservableCollection<Order>(_context.Orders.Include(o => o.Users).ToList());
            Admin = new ObservableCollection<Admin>(_context.Admin.ToList());
        }

        public RelayCommand Back
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<LoginViewModel>();
                });
        }

        public RelayCommand CreateUser
        {
            get => new (
                () =>
                {
                    _navigationService.NavigateTo<SuperAdminCreateUserViewModel>();
                });
        }

        public RelayCommand CreateAdmin
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<SuperAdminCreateAdminViewModel>();
                });
        }

        public RelayCommand DeleteUser
        {
            get => new(
                () =>
                {
                    try
                    {
                        if (SelectedUser == null)
                        {
                            MessageBox.Show("Выберите пользователя");
                            return;
                        }
                        var user = _context.Users.FirstOrDefault(u => u.Name == _selectedUser.Name);
                        if (user != null)
                        {
                            _context.Users.Remove(user);
                            _context.SaveChanges();
                            User.Remove(user);
                            MessageBox.Show("Пользователь удалён");

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }

        public RelayCommand DeleteAdmin
        {
            get => new(
                () =>
                {
                    try
                    {
                        if (SelectedAdmin == null)
                        {
                            MessageBox.Show("Выберите админа");
                            return;
                        }
                        var admin = _context.Admin.FirstOrDefault(a => a.Name == _selectedAdmin.Name);
                        if (admin != null)
                        {
                            _context.Admin.Remove(admin);   
                            _context.SaveChanges();
                            Admin.Remove(admin);
                            MessageBox.Show("Админ удалён");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }

        public RelayCommand DeleteOrder
        {
            get => new(
                () =>
                {
                    try
                    {
                        if (SelectedOrder == null)
                        {
                            MessageBox.Show("Выберите заказ");
                            return;
                        }
                        var order = _context.Orders.FirstOrDefault(o => o.Product == _selectedOrder.Product);
                        if (order != null)
                        {
                            _context.Orders.Remove(order);
                            _context.SaveChanges();
                            Order.Remove(order);
                            MessageBox.Show("Заказ удалён");
                        }
                    }
                    catch ( Exception ex )
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }
        
    }
}
