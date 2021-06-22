using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class RabbitMQService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public RabbitMQService(IConfiguration config)
        {
            _host = config["RabbitMQ:Host"];
            _port = Convert.ToInt32(config["RabbitMQ:Port"]);
            _username = config["RabbitMQ:Username"];
            _password = config["RabbitMQ:Password"];
        }

        public void PublishEvent(string eventName, byte[] data)
        {
            var factory = new ConnectionFactory();

            factory.HostName = _host;
            factory.Port = _port;
            factory.UserName = _username;
            factory.Password = _password;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.BasicPublish(exchange: eventName,
                                         routingKey: "",
                                         basicProperties: null,
                                         body: data);
                }
            }
        }

        public string GetJsonFromEvent(string eventName)
        {
            var json = string.Empty;
            var factory = new ConnectionFactory();

            factory.HostName = _host;
            factory.Port = _port;
            factory.UserName = _username;
            factory.Password = _password;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(eventName, ExchangeType.Fanout);

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queueName, eventName, routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();

                        json = Encoding.UTF8.GetString(body);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                }
            }

            return json;
        }
    }
}
