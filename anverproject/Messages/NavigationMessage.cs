using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Trendyol.Messages
{
    public class NavigationMessage
    {
        public ViewModelBase ViewModelType { get; set; }
    }
}
