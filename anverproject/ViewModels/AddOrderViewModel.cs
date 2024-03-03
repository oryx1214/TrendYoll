using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class AddOrderViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicationDbContext _context;
        private ObservableCollection<Products> _products;
        private Products _selectedProducts;

        public ObservableCollection<Products> Product
        {
            get => _products;
            set => Set(ref _products, value);  
        }

        public Products SelectedProduct
        {
            get => _selectedProducts;
            set
            {
                if (Set(ref _selectedProducts, value))
                {
                    Messenger.Default.Send(value.Name, "SelectedProductName");
                }
            }
        }


        public AddOrderViewModel(INavigationService navigationService, ApplicationDbContext context)
        {
            _navigationService = navigationService;
            _context = context;
            Product = new ObservableCollection<Products>(_context.Products.ToList());
            MessengerInstance.Register<Products>(this, "SelectedProductName", SetSelectedProductName);
        }

        private void SetSelectedProductName(Products selectedProduct)
        {
            SelectedProduct = selectedProduct;
        }

        public RelayCommand Exit
        {
            get => new(
                () =>
                {
                    _navigationService.NavigateTo<MainPageViewModel>();
                });
        }

        public RelayCommand Add
        {   
            get => new(
                () =>
                {
                    try
                    {
                        if (SelectedProduct != null)
                        {
                            Messenger.Default.Send(SelectedProduct, "SelectedProductForOrder");
                            _navigationService.NavigateTo<CountViewModel>();
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
