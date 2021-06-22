using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseService.Database.Entities;

namespace WarehouseService.Database
{
    public class WarehouseServiceDbContext : DbContext
    {
        public WarehouseServiceDbContext(DbContextOptions<WarehouseServiceDbContext> options) 
            : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemTransaction> ItemTransactions { get; set; }
    }
}
