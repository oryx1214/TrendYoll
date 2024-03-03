using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.Models;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class MyOrdersViewModel : ViewModelBase
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

        public MyOrdersViewModel(INavigationService navigationService, ApplicationDbContext context, CurrentUserService currentUserService)
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
    }
}
