using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzurePOC.Services
{
    public class QueueManager
    {
        readonly string connectionString = "Endpoint=sb://hernanpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QmZkBfQi7Qa/aWV+EpR6hr0TJWOKyFHE03PcCQ6zxkQ=";

        public async Task PushMessageAsync(string queueName, string messageString)
        {
            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            var message = new BrokeredMessage(messageString);
            await client.SendAsync(message);
        }

        public async Task<Dictionary<string, string>> ReadAllMessagesAsync(string queueName)
        {
            var returnDict = new Dictionary<string, string>();
            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            BrokeredMessage message;
            while ((message = await client.ReceiveAsync()) != null)
            {
                returnDict.Add(message.MessageId, message.GetBody<string>());
            }

            return returnDict;
        }

        public async Task<Tuple<string, string>> ReadMessageAsync(string queueName)
        {
            var returnDict = new Dictionary<string, string>();
            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            var message = await client.ReceiveAsync();

            if (message != null)
            {
                await message.CompleteAsync();
                return Tuple.Create(message.MessageId, message.GetBody<string>());
            }
            else return null;
        }
    }
}
