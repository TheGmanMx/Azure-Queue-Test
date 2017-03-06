using AzurePOC.Constants;
using AzurePOC.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QueuSender
{
    class Program
    {
        static readonly QueueManager _queueManager = new QueueManager();
        static int horriblyWritenCounter = 0;
        static void Main(string[] args)
        {
            bool doContinue = true;
            int messageCount = 0;
            Console.WriteLine("Welcome to the Queue Sender POC");
            do
            {
                if (horriblyWritenCounter > 0) continue;

                Console.Write("\r\nDo you want to send a message? (Y/N): ");
                doContinue = Console.ReadLine().ToLower() == "y";
                if (!doContinue) break;

                Console.Write("How many messages?: ");
                horriblyWritenCounter = messageCount = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine($"Start Sending {messageCount} messages to Queue");
                string guid = new Guid().ToString();
                foreach (var i in Enumerable.Range(0, messageCount))
                {
                    Task t = _queueManager.PushMessageAsync(Queues.POC, $"Test Message {guid} {i}");
                    t.ContinueWith(OnMessagePushed);
                }

                Console.WriteLine($"{messageCount} messages sent to Queue");

            } while (doContinue);
        }

        private static void OnMessagePushed(Task obj)
        {
            Console.Write(". ");
            horriblyWritenCounter--;
        }
    }
}
