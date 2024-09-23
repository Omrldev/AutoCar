using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;

        public AuctionUpdatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine("--> Consuming the auction updated " + context.Message.Id);

            // var item = _mapper.Map<Item>(context.Message);

            var item = new Item()
            {
                ID = context.Message.Id.ToString(),
                Make = context.Message.Make,
                Model = context.Message.Model,
                Year = context.Message.Year,
                Color = context.Message.Color,
                Mileage = context.Message.Mileage
            };

            var result = await DB.Update<Item>()
                .Match(x => x.ID == context.Message.Id)
                .ModifyOnly(x => new
                {
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Color,
                    x.Mileage
                }, item)
                .ExecuteAsync();

            if (!result.IsAcknowledged) 
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongoDb");
        }
    }
}
