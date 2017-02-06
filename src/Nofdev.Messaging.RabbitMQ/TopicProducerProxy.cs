using System.Text;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Nofdev.Core.Message;
using RabbitMQ.Client;

namespace Nofdev.Messaging
{
    public class TopicProducerProxy : IInterceptor
    {
        private readonly TopicProducerConfig _topicProducerConfig;

        public TopicProducerProxy(TopicProducerConfig topicProducerConfig)
        {
            _topicProducerConfig = topicProducerConfig;
        }

        #region Implementation of IInterceptor

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var interfaceType = invocation.Method.DeclaringType;

            var topicMessage = new TopicMessage<dynamic>();
            topicMessage.Payload = invocation.Arguments;
            var message = JsonConvert.SerializeObject(topicMessage);

            var factory = new ConnectionFactory
            {
                HostName = _topicProducerConfig.BootstrapServers,
                UserName = _topicProducerConfig.UserName,
                Password = _topicProducerConfig.Password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var exchange = "topic";
                var rk = interfaceType.FullName.Replace(".", "-") + "-" + methodName;
                channel.ExchangeDeclare(exchange, ExchangeType.Topic);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                //properties.Headers = new Dictionary<string, object>();
                //todo:need headers?
                channel.BasicPublish(exchange, rk, properties, body);
            }
        }

        #endregion
    }
}