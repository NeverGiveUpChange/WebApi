﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_RabbitMq.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class MqConfig
    {
        public string Host { get; set; }
        public ushort HeartBeat { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public TimeSpan NetworkRecoveryInterval { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 死信队列实体
    /// </summary>
    [RabbitMqQueue("dead-letter-{Queue}", ExchangeName = "dead-letter-{exchange}")]
    public class DeadLetterQueue
    {
        public string Body { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }

        public string RoutingKey { get; set; }

        public int RetryCount { get; set; }

        public string ExceptionMsg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    /// <summary>
    /// 交换器类型
    /// </summary>
    public static class ExchangeType
    {
        public static string Direct = "direct";
        public static string Fanout = "fanout";
        public static string Topic = "topic";
    }
    public enum ExchangeTypeEnum
    {
        Direct = 1,
        Fanout = 2,
        Topic = 3
    }


}
