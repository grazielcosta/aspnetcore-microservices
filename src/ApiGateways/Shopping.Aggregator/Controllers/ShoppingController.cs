using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderingService _orderingService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderingService orderingService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            //Get basket with username
            // interate basket items and consume products with basket item productId member
            // map product related members into basketitem dto with extended columns 
            // return root ShoppingModel dto class witch including all repsonses

            var basket = await _basketService.GetBasket(userName);

            if (basket?.Items != null)
                foreach (var item in basket.Items)
                {
                    var product = await _catalogService.GetCatalog(item.ProductId);

                    item.ProductName = product.Name;
                    item.Category = product.Category;
                    item.Summary = product.Summary;
                    item.Description = product.Description;
                    item.ImageFile = product.ImageFile;

                }

            var orders = await _orderingService.GetOrdersByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
