using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {

        }
        public ShoppingCart(string userName)
        {
            this.UserName = userName;
        }
        public decimal TotalPrice()
        {
            return Items?.Select(s => s.Price * s.Quantity)?.Sum() ?? 0;
        }
    }
}
