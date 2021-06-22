using OrderService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class PlaceOrderDto
    {
        public IEnumerable<OrderLineDto> OrderLines { get; set; }
    }

    public class OrderLineDto
    {
        public Guid ItemId { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
