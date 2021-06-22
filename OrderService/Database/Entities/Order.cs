using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Database.Entities
{
    public class Order : BaseEntity
    {
        public IEnumerable<OrderLine> OrderLines { get; set; }
        public OrderStatus Status { get; set; }

    }

    public enum OrderStatus
    {
        InProgress = 10,
        Approved = 20,
        Rejected = 30
    }
}
