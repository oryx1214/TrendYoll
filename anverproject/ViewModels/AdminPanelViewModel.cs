using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trendyol.Models;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;
using Trendyol.Views;

namespace Trendyol.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUserService;
        private ObservableCollection<Order> _order;
        private ObservableCollection<Products> _product;
        private Order _selectedOrder;
        private Products _selectedProducts;
        private RadioButtons _radioButtons;

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

        public ObservableCollection<Products> Product
        {
            get => _product;
            set => Set(ref _product, value);
        }

        public Products SelectedProduct
        {
            get => _selectedProducts;
            set => Set(ref _selectedProducts, value);
        }

        public RadioButtons RadioButton
        {
            get => _radioButtons;
            set => Set(ref _radioButtons, value);
        }

        public AdminPanelViewModel(INavigationService navigationService, ApplicationDbContext context, CurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _context = context;
            _currentUserService = currentUserService;
            RadioButton = new RadioButtons();
            Order = new ObservableCollection<Order>(_context.Orders);
            Product = new ObservableCollection<Products>(_context.Products);
        }

        public RelayCommand Exit
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<LoginViewModel>();
                });
        }

        public RelayCommand Add
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<AdminAddOrderViewModel>();
                });
        }

        public RelayCommand Radio
        {
            get => new(
                () =>
                {
                    try
                    {

                        if (RadioButton.OrderPlaced)
                        {
                            if (SelectedOrder.Status == "Заказ сделан")
                            {
                                SelectedOrder.Status = "Заказ сделан";
                            }
                            else
                            {
                                MessageBox.Show("Невозможно перенести заказ на этот этап");
                                return;
                            }
                        }
                        else if (RadioButton.ArrivedAtTheWarehouse)
                        {
                            if (SelectedOrder.Status == "Заказ сделан" || SelectedOrder.Status == "Поступил на склад")
                            {
                                SelectedOrder.Status = "Поступил на склад";
                            }
                            else
                            {
                                MessageBox.Show("Невозможно перенести заказ на этот этап");
                                return;
                            }
                        }
                        else if (RadioButton.Sent)
                        {
                            if (SelectedOrder.Status == "Поступил на склад" || SelectedOrder.Status == "Отправлен")
                            {
                                SelectedOrder.Status = "Отправлен";
                            }
                            else
                            {
                                MessageBox.Show("Невозможно перенести заказ на этот этап");
                                return;
                            }
                        }
                        else if (RadioButton.SmartCustomsCheck)
                        {
                            if (SelectedOrder.Status == "Отправлен" || SelectedOrder.Status == "На таможенной проверке")
                            {

                                SelectedOrder.Status = "На таможенной проверке";
                            }
                            else
                            {
                                MessageBox.Show("Невозможно перенести заказ на этот этап");
                                return;
                            }
                        }
                        else if (RadioButton.InFilial)
                        {
                            if (SelectedOrder.Status == "На таможенной проверке" || SelectedOrder.Status == "На почте")
                            {
                                SelectedOrder.Status = "На почте";
                            }
                            else
                            {
                                MessageBox.Show("Невозможно перенести заказ на этот этап");
                                return;
                            }
                        }
                        _context.SaveChanges();


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
                        if (SelectedProduct == null)
                        {
                            MessageBox.Show("Выберите заказ");
                            return;
                        }
                        var product = _context.Products.FirstOrDefault(p => p.Name == _selectedProducts.Name);
                        if (product != null)
                        {
                            _context.Products.Remove(product);
                            _context.SaveChanges();
                            Product.Remove(product);
                            MessageBox.Show("Продукт удалён");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
        }

    }
}
