using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class GancelOrderViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUserService;
        private ObservableCollection<Order> _order;
        private Order _selectedOrder;

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


        public GancelOrderViewModel(INavigationService navigationService, ApplicationDbContext context, CurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _context = context;
            _currentUserService = currentUserService;
            Order = new ObservableCollection<Order>(_context.Orders.Where(
                o => o.UserId == _currentUserService.UserId));
        }

        public RelayCommand Exit
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<MainPageViewModel>();
                });
        }

        public RelayCommand Delete
        {
            get => new(
                () =>
                {
                    try
                    {
                        if (SelectedOrder == null)
                        {
                            MessageBox.Show("Выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            Order order = _context.Orders.FirstOrDefault(o => o.Product == _selectedOrder.Product);
                            if (order != null)
                            {
                                if (_selectedOrder.Status == "Заказ сделан")
                                {
                                    
                                    _context.Orders.Remove(order);
                                    _context.SaveChanges();
                                    Order.Remove(order);
                                    MessageBox.Show("Заказ успешно удален");
                                    SelectedOrder = null;
                                }
                                else
                                {
                                    MessageBox.Show("Ваш заказ уже впути, его нельзя отменить");
                                    return;
                                }
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
