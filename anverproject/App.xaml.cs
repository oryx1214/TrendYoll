using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using Trendyol.ViewModels;
using Trendyol.Views;
using Trendyol.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Trendyol.Services.Classes;

namespace Trendyol
{
    
    public partial class App : Application
    {
        public static SimpleInjector.Container Container { get; set; }
        void Register()
        {
            Container = new SimpleInjector.Container();

            Container.RegisterSingleton<IMessenger, Messenger>();
            Container.RegisterSingleton<INavigationService, Services.Classes.NavigationService>();
            Container.RegisterSingleton<CurrentUserService>();
            Container.RegisterSingleton<UserService>();
            Container.RegisterSingleton<ApplicationDbContext>();
            Container.RegisterSingleton<RegisterViewModel>();
            Container.RegisterSingleton<MainWindowViewModel>();
            Container.RegisterSingleton<LoginViewModel>();
            Container.RegisterSingleton<AccountInfoViewModel>();
            Container.RegisterSingleton<CountViewModel>();
            Container.RegisterSingleton<MainPageViewModel>();
            Container.RegisterSingleton<AdminAddOrderViewModel>();
            Container.RegisterSingleton<SuperAdminCreateAdminViewModel>();
            Container.RegisterSingleton<SuperAdminCreateUserViewModel>();

            Container.Register<AddOrderViewModel>(); 
            Container.Register<AdminPanelViewModel>();
            Container.Register<MyOrdersViewModel>();
            Container.Register<GancelOrderViewModel>();
            Container.Register<SuperAdminMenuViewModel>();
            Container.Verify();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Register();
            var window = new MainWindow();
            window.DataContext = Container.GetInstance<MainWindowViewModel>();
            window.Show();
        }
    }

}
