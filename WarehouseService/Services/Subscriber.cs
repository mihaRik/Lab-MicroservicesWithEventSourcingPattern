using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseService.Services
{
    public class Subscriber
    {
        private readonly RabbitMQService _rabbit;

        public Subscriber(RabbitMQService rabbit)
        {
            _rabbit = rabbit;
        }


    }
}
