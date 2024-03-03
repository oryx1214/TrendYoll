using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.Models;

namespace Trendyol.Services.Classes
{
    public class AddOrderService
    {
        private readonly ApplicationDbContext _context;

        public AddOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Products AddProductOrder(string name, string description, int count)
        {
            Products product = new Products()
            {
                Name = name,
                Description = description,
                Count = count
            };
            return product;
        }
    }
}
