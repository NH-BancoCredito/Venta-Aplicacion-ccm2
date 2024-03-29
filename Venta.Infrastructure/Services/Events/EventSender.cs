﻿using Confluent.Kafka;
using Venta.Domain.Service.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Infrastructure.Services.Events
{
    public class EventSender : IEventSender
    {
        private readonly IPublisherFactory _publisherProducer;

        public EventSender(IPublisherFactory publisherProducer)
        {

            _publisherProducer = publisherProducer;
        }

        public async Task PublishAsync(string topic, string serializedMessage, CancellationToken cancellationToken)
        {
            var producer = _publisherProducer.GetProducer();
            
            await producer.ProduceAsync(
                topic.ToLower(),
                new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = serializedMessage },
                cancellationToken).ConfigureAwait(false);
        }
    }
}
