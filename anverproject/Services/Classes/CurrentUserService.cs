﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.Models;

namespace Trendyol.Services.Classes
{
    public class CurrentUserService : INotifyPropertyChanged
    {
        private int _userid;
        private string _name;
        private string _surname;
        private string _login;
        private string _email;

        public int UserId
        {
            get => _userid; 
            set
            {
                _userid = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value; 
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateUserData(User user)
        {
            UserId = user.UserId;
            Name = user.Name;
            Surname = user.Surname;
            Login = user.Login;
            Email = user.Email;
            
        }

    }


}
