using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_RabbitMq.Client
{
    /// <summary>
    /// 自定义的RabbitMq队列信息实体特性
    /// </summary>
    internal class RabbitMqQueueAttribute : Attribute
    {
        public RabbitMqQueueAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteKey { get; set; }
        /// <summary>
        /// 匹配类型
        /// </summary>
        public ExchangeTypeEnum TypeEnum { get; set; }
    }
}
