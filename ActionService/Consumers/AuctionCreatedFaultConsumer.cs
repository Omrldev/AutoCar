using Contracts;
using MassTransit;

namespace ActionService.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<ActionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<ActionCreated>> context)
        {
            Console.WriteLine("--> Consuming the fault error");

            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "Toyota";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("--> Not an argument");
            }
        }
    }
}
