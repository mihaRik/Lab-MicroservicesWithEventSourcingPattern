using Newtonsoft.Json;
using OrderService.Database;
using OrderService.Database.Entities;
using OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class OrderService
    {
        private readonly OrderServiceDbContext _db;
        private readonly RabbitMQService _rabbit;

        public OrderService(OrderServiceDbContext db, RabbitMQService rabbit)
        {
            _db = db;
            _rabbit = rabbit;
        }

        public IEnumerable<Item> GetItems()
        {
            return _db.Items.ToList();
        }

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            return await _db.Items.FindAsync(id);
        }

        public async Task PlaceOrderAsync(PlaceOrderDto dto)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderLines = new List<OrderLine>();

                    foreach (var orderLine in dto.OrderLines)
                    {
                        orderLines.Add(new OrderLine
                        {
                            Id = Guid.NewGuid(),
                            ItemId = orderLine.ItemId,
                            OrderedQuantity = orderLine.OrderedQuantity
                        });
                    }

                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        OrderLines = orderLines,
                        Status = OrderStatus.InProgress
                    };

                    //publish  event about new order
                    var json = JsonConvert.SerializeObject(order);

                    _rabbit.PublishEvent("order_placed", Encoding.UTF8.GetBytes(json));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
