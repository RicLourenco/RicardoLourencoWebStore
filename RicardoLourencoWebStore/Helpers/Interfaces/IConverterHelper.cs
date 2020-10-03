using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, string path, bool isNew);

        ProductViewModel ToProductViewModel(Product model);

        Category ToCategory(CategoryViewModel model, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category model);

        Order ToOrder(OrderViewModel model, bool isNew);

        OrderViewModel ToOrderViewModel(Order model);
    }
}
