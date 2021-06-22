using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseService.Database;
using WarehouseService.Database.Entities;
using WarehouseService.Dtos;

namespace WarehouseService.Services
{
    public class WarehouseService
    {
        private readonly WarehouseServiceDbContext _db;
        private readonly RabbitMQService _rabbit;

        public WarehouseService(WarehouseServiceDbContext db, RabbitMQService rabbit)
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

        public async Task ProcessOrder(OrderDto dto)
        {
            foreach (var orderLine in dto.OrderLines)
            {
                var quantityOnStock = await GetItemQuantityOnStockAsync(orderLine.ItemId);

                if (orderLine.OrderedQuantity > quantityOnStock)
                {
                    dto.Status = OrderStatus.Rejected;

                    var json = JsonConvert.SerializeObject(new OrderDto
                                                           {
                                                               Id = dto.Id,
                                                               Status = dto.Status
                                                           });

                    _rabbit.PublishEvent("order_processed", Encoding.UTF8.GetBytes(json));

                    return;
                }
                else
                {
                    //zabirayem so stoka
                    var transaction = new ItemTransaction
                    {
                        Id = Guid.NewGuid(),
                        ItemId = orderLine.ItemId,
                        Quantity = orderLine.OrderedQuantity,
                        Type = TransactionType.Minus
                    };

                    _db.ItemTransactions.Add(transaction);
                }
            }


            dto.Status = OrderStatus.Approved;

            var json2 = JsonConvert.SerializeObject(new OrderDto
            {
                Id = dto.Id,
                Status = dto.Status
            });

            _rabbit.PublishEvent("order_processed", Encoding.UTF8.GetBytes(json2));

            await _db.SaveChangesAsync();

            return;
        }

        public async Task<int> GetItemQuantityOnStockAsync(Guid id)
        {
            var plusTransactionsSum = await _db.ItemTransactions.Where(tr => tr.ItemId == id &&
                                                                      tr.Type == TransactionType.Plus)
                                                             .SumAsync(tr => tr.Quantity);

            var minusTransactionsSum = await _db.ItemTransactions.Where(tr => tr.ItemId == id &&
                                                                           tr.Type == TransactionType.Minus)
                                                              .SumAsync(tr => tr.Quantity);

            return plusTransactionsSum - minusTransactionsSum;
        }
    }
}
