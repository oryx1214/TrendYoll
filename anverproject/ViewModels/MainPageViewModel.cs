using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationSerivce;

        public MainPageViewModel(INavigationService navigationSerivce)
        {
            _navigationSerivce = navigationSerivce;
        }

        public RelayCommand LoginPage
        {
            get => new(
                () =>
                {
                    _navigationSerivce.NavigateTo<LoginViewModel>();
                });
        }

        public RelayCommand Account
        {
            get => new(
                () =>
                {
                    _navigationSerivce.NavigateTo<AccountInfoViewModel>();
                });
        }

        public RelayCommand AddOrder
        {
            get => new(
                () =>
                {
                    _navigationSerivce.NavigateTo<AddOrderViewModel>();
                });
        }

        public RelayCommand CancelOrder
        {
            get => new(
                () =>
                {
                    _navigationSerivce.NavigateTo<GancelOrderViewModel>();
                });
        }

        public RelayCommand MyOrders
        {
            get => new(
                () =>
                {
                    _navigationSerivce.NavigateTo<MyOrdersViewModel>();
                });
        }

    }
}
