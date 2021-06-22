using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseService.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }
        public OrderStatus Status { get; set; }

    }

    public class OrderLine
    {
        public Guid ItemId { get; set; }
        public int OrderedQuantity { get; set; }
    }

    public enum OrderStatus
    {
        InProgress = 10,
        Approved = 20,
        Rejected = 30
    }
}
