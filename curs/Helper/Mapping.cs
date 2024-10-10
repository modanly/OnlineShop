using curs.Models;
using OnlineShop.Db.Models;


namespace curs.Helper
{
    public static class Mapping
    {
        public static List<ProductViewModel> ToProductViewModels(List<Product> products)
        {
            var productsViewModels = new List<ProductViewModel>();
            foreach (var product in products)
            {

                productsViewModels.Add(ToProductViewModel(product));
            }
            return productsViewModels;
        }

        public static ProductViewModel ToProductViewModel(Product product)
        {

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                ImagePath = product.ImagePath
            };

        }

        public static CartViewModel ToCartViewModel(Cart cart)
        {
            if (cart == null)
            {
                return null;
            }
            return new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = ToCartItemViewModels(cart.Items)
            };
        }
        private static List<CartItemViewModel> ToCartItemViewModels(List<CartItem> cartDbItems)
        {
            var cartItems = new List<CartItemViewModel>();
            foreach (var cartDbItem in cartDbItems)
            {
                var cartItem = new CartItemViewModel
                {
                    Id = cartDbItem.Id,
                    Amount = cartDbItem.Amount,
                    Product = ToProductViewModel(cartDbItem.Product)

                };
                cartItems.Add(cartItem);
            }
            return cartItems;
        }

        public static OrderViewModel ToOrderViewModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                CreatedDateTime = order.CreatedDateTime,
                Status = (OrderStatusesViewModel)(int)order.Status,
                User = ToUserDeliveryInfoViewModel(order.User),
                Items = ToCartItemViewModels(order.Items)
            };
        }

        public static UserDeliveryInfoViewModel ToUserDeliveryInfoViewModel(UserDeliveryInfo userDeliveryInfo)
        {
            return new UserDeliveryInfoViewModel
            {
                Name = userDeliveryInfo.Name,
                Address = userDeliveryInfo.Address,
                Phone = userDeliveryInfo.Phone,
                Email = userDeliveryInfo.Email
            };
        }

        public static UserDeliveryInfo ToUser(UserDeliveryInfoViewModel user)
        {
            return new UserDeliveryInfo
            {
                Name = user.Name,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email
            };
        }
    }
}
