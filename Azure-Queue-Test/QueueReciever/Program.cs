using AzurePOC.Constants;
using AzurePOC.Services;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;

namespace QueueReciever
{
    class Program
    {
        static readonly QueueManager _queueManager = new QueueManager();
        //static readonly string connectionString = "Endpoint=sb://hernanpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QmZkBfQi7Qa/aWV+EpR6hr0TJWOKyFHE03PcCQ6zxkQ=";
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Queue Reciever POC");
            Console.WriteLine("Attempting to read messages");

            while (true)
            {
                Task<Tuple<string, string>> t = _queueManager.ReadMessageAsync(Queues.POC);
                t.ContinueWith(OnMessagePushed);
            }
            //var client = QueueClient.CreateFromConnectionString(connectionString, Queues.POC);            
            //client.OnMessage(message =>
            //{
            //    Console.WriteLine($"Message body: {message.MessageId}");
            //    Console.WriteLine($"Message id: {message.GetBody<string>()}");
            //});

            Console.ReadLine();
        }

        private static async Task OnMessagePushed(Task<Tuple<string, string>> obj)
        {
            Tuple<string, string> response = null;

            try
            {
                response = await obj;
            }
            catch { }

            if(response != null)
                Console.WriteLine($"Read Message {response.Item1} {response.Item2}");
        }
    }
}
