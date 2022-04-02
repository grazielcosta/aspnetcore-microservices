using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CartModel(IBasketService basketService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public BasketModel Cart { get; set; } = new BasketModel();        

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "swn";
            Cart = await _basketService.GetBasket(userName);            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            string userName = "swn";
            var basket = await _basketService.GetBasket(userName);

            var item = basket?.Items?.SingleOrDefault(x => x.ProductId == productId);
            if (item == null)
                basket.Items.Remove(item);

            _ = await _basketService.UpdateBasket(basket);

            return RedirectToPage();
        }
    }
}