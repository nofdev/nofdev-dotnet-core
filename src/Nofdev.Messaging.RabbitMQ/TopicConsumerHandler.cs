using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nofdev.Core.Message;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nofdev.Messaging
{
    public class TopicConsumerHandler
    {

        private readonly TopicConsumerConfig _topicConsumerConfig;
        private readonly ILogger<TopicConsumerHandler> _logger;


        public TopicConsumerHandler(TopicConsumerConfig topicConsumerConfig, ILogger<TopicConsumerHandler> logger)
        {
            _topicConsumerConfig = topicConsumerConfig;
            _logger = logger;
        }

        public void RegisterConsumer(object obj)
        {
            var pInterface = obj.GetType().GetInterfaces().FirstOrDefault();
            var pInterfacePath = pInterface.FullName;
            var methods = pInterface.GetMethods();
            foreach (var method in methods)
            {
                _logger.LogDebug($"-要监听的方法{method.Name}--------------");
                Task.Factory.StartNew(() => { Listen(pInterfacePath, method, obj); });
            }
        }

        private void Listen(string pInterfacePath, MethodInfo method, object obj)
        {
            var factory = new ConnectionFactory {HostName = _topicConsumerConfig.BootstrapServers};
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var exchange = "topic";
                    var rk = pInterfacePath.Replace(".", "-") + "-" + method.Name;
                
                    channel.ExchangeDeclare(exchange, ExchangeType.Topic);

                    var ok = channel.QueueDeclare();

                    channel.QueueBind(ok.QueueName, exchange, rk);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        _logger.LogDebug($"message received:{message}");

                        var topic = GetResult(method, message);

                        method.Invoke(obj, topic?.Payload);

                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicConsume(ok.QueueName, false, consumer);
              
           
        }


        private  TopicMessage<object[]> GetResult(MethodInfo method, string result)
        {
            _logger.LogDebug("The request return " + result);
            var realType = method.GetRealReturnType();
            _logger.LogDebug($"The method return type is {realType}");
            return JsonConvert.DeserializeObject<TopicMessage<object[]>>(result);
        }
    }

    public static class TypeUtil
    {
        public static Type GetRealReturnType(this MethodInfo method)
        {
            var returnType = method.ReturnType;
            var realType = returnType;
            if (returnType.GetTypeInfo().IsGenericType && returnType.GetTypeInfo().BaseType == typeof (Task))
            {
                realType = returnType.GenericTypeArguments[0];
            }
            else if (returnType == typeof (void))
            {
                realType = typeof (object);
            }
            return realType;
        }
    }
}