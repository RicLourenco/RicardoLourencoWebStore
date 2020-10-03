using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Helpers.Interfaces;
using RicardoLourencoWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Classes
{
    public class ConverterHelper : IConverterHelper
    {
        public Category ToCategory(CategoryViewModel model, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 :  model.Id,
                Name = model.Name
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category model)
        {
            return new CategoryViewModel
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public Order ToOrder(OrderViewModel model, bool isNew)
        {
            return new Order
            {
                Id = isNew ? 0 : model.Id,
                DeliveryDate = model.DeliveryDate,
                OrderDate = model.OrderDate,
                Items = model.Items,
                User = model.User
            };
        }

        public OrderViewModel ToOrderViewModel(Order model)
        {
            return new OrderViewModel
            {
                Id = model.Id,
                DeliveryDate = model.DeliveryDate,
                Items = model.Items,
                OrderDate = model.OrderDate,
                User = model.User
            };
        }

        public Product ToProduct(ProductViewModel model, string path, bool isNew)
        {
            return new Product
            {
                Id = model.Id,
                Category = model.Category,
                CategoryId = model.CategoryId,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            };
        }

        public ProductViewModel ToProductViewModel(Product model)
        {
            return new ProductViewModel
            {
                Id = model.Id,
                Category = model.Category,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            };
        }
    }
}
