using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseService.Database.Entities
{
    public class ItemTransaction : BaseEntity
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
    }

    public enum TransactionType
    {
        Plus = 10,
        Minus = 20
    }
}
