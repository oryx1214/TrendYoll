using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trendyol.Models;
using Trendyol.Services.Classes;
using Trendyol.Services.Interfaces;

namespace Trendyol.ViewModels
{
    public class CountViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUserService;
        private Products _selectedProduct;
        private int _count;
        private string _product;

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public string Product
        {
            get => _product;
            set => Set(ref _product, value);
        }

        public Products SelectedProduct
        {
            get => _selectedProduct;
            set => Set(ref _selectedProduct, value);
        }

        public CountViewModel(INavigationService navigationService, ApplicationDbContext context, CurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _context = context;
            _currentUserService = currentUserService;
            _selectedProduct = new Products();
            Messenger.Default.Register<string>(this, "SelectedProductName", SetSelectedProductName);
            Messenger.Default.Register<int>(this, "SelectedProductCount", SetSelectedProductCount);
            Messenger.Default.Register<Products>(this, "SelectedProductForOrder", SetSelectedProduct);
        }

        private void SetSelectedProductCount(int count)
        {
            Count = count;
        }

        private void SetSelectedProductName(string productName)
        {
            Product = productName;
        }

        private void SetSelectedProduct(Products selectedProduct)
        {
            SelectedProduct = selectedProduct;
            _selectedProduct = selectedProduct;
        }

        public RelayCommand Back
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<AddOrderViewModel>();
                });
        }

        public RelayCommand AddOrder
        {
            get => new(
                () =>
                {
                    try
                    {
                        int currentId = _currentUserService.UserId;
                        if (_selectedProduct != null)
                        {
                            var wareHouseProduct = _context.WareHouse.FirstOrDefault(p => p.ProductId == _selectedProduct.Id);
                            if (wareHouseProduct != null)
                            {
                                if (wareHouseProduct.ProductCount < Count)
                                {
                                    MessageBox.Show("На складе отсуствует данное количество  товара.");
                                    _navigationService.NavigateTo<AddOrderViewModel>();
                                    return;
                                }
                                else if (Count == 0)
                                {
                                    MessageBox.Show("Введите количество товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }

                            if (_selectedProduct.Count == 0)
                            {
                                _context.Products.Remove(_selectedProduct);
                                _context.SaveChanges();
                            }


                            Order order = new Order
                            {
                                UserId = currentId,
                                Product = _selectedProduct.Name,
                                ProductsCount = Count,
                                ProductId = _selectedProduct.Id,
                                Status = "Заказ сделан",
                                Created = DateTime.Now,
                            };
                            _context.Orders.Add(order);
                            _context.SaveChanges();
                            var user = _context.Users.FirstOrDefault(u => u.UserId == _currentUserService.UserId);
                            _context.SaveChanges();
                            wareHouseProduct.ProductCount -= Count;
                            _context.SaveChanges();
                            _selectedProduct.Count -= Count;
                            _context.SaveChanges();
                            MessageBox.Show("Товар куплен.Чтобы отслеживать заказ перейдите в \"История заказов\"", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            Count = 0;
                            _navigationService.NavigateTo<MainPageViewModel>();

                        }
                        else
                        {
                            MessageBox.Show("Не удалось купить товар");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                });
        }
    }
}
