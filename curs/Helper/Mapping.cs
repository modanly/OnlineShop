using OnlineShop.Models;
using OnlineShop.Db.Models;
using System.Data;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Helper
{
    public static class Mapping
    {
        public static List<ProductViewModel> ToProductViewModels(this List<Product> products)
        {
            var productsViewModels = new List<ProductViewModel>();
            foreach (var product in products)
            {

                productsViewModels.Add(ToProductViewModel(product));
            }
            return productsViewModels;
        }

        public static ProductViewModel ToProductViewModel(this Product product)
        {

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                ImagesPaths=product.Images.ToPaths()
            };

        }

        public static EditProductViewModel ToEditProductViewModel(this Product product)
        {
            return new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                ConcurrencyToken= product.ConcurrencyToken,
                ImagesPaths = product.Images.ToPaths()
            };
        }
        //public static Product ToProduct(this ProductViewModel productViewModel)
        //{
        //    return new Product
        //    {
        //        Id = productViewModel.Id,
        //        Name = productViewModel.Name,
        //        Cost = productViewModel.Cost,
        //        Description = productViewModel.Description
        //    };
        //}

        public static Product ToProduct(this AddProductViewModel addProductViewModel, List<string> imagesPaths)
        {
            return new Product
            {
                Name = addProductViewModel.Name,
                Cost = addProductViewModel.Cost,
                Description = addProductViewModel.Description,
                Images = ToImages(imagesPaths)
            };
        }
        public static Product ToProduct(this EditProductViewModel editProduct)
        {
            return new Product
            {
                Id = editProduct.Id,
                Name = editProduct.Name,
                Cost = editProduct.Cost,
                Description = editProduct.Description,
                ConcurrencyToken= editProduct.ConcurrencyToken,
                Images = editProduct.ImagesPaths.ToImages()
            };
        }

        public static List<Image> ToImages(this List<string> paths)
        {
            return paths.Select(x => new Image { Url = x }).ToList();
        }

        public static List<string> ToPaths(this List<Image> images)
        {
            return images.Select(x => x.Url).ToList();
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

        public static UserViewModel ToUserViewModel(this User user, UserManager<User> usersManager)
        {
            // Получаем роли пользователя
            var roles = usersManager.GetRolesAsync(user).Result;

            // Возвращаем UserViewModel с заполненными данными, включая роль
            return new UserViewModel
            {
                Id=user.Id,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Roles = usersManager.GetRolesAsync(user).Result.Select(role => new RolesViewModel
                {
                    Name = role
                }).ToList()
            };
        }
        
       

        

    }
}
