using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Database.Entities
{
    public class OrderLine : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
