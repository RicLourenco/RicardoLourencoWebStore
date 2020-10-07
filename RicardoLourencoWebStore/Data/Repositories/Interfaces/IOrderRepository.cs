using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName, bool isReSeller);

        Task ModifyOrderDetailTempQuantityAsync(int id, int quantity);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrderAsync(DeliveryViewModel model);

        Task<Order> GetOrderAsync(int id);
    }
}
