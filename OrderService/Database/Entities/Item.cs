using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Database.Entities
{
    public class Item : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
