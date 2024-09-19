using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<ActionCreated>
    {
        private readonly IMapper _mapper;

        public AuctionCreatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ActionCreated> context)
        {
            Console.WriteLine("--> Consuming the event from the auction service " + context.Message.Id);

            //var item = _mapper.Map<Item>(context);

            Item item = new ()
            {
                ID = context.Message.Id.ToString(),
                ReservePrice = context.Message.ReservePrice,
                Seller = context.Message.Seller,
                Winner = context.Message.Winner,
                SoldAmount = context.Message.SoldAmount,
                CurrentHighBid = context.Message.CurrentHighBid,
                CreateAt = context.Message.CreateAt,
                UpdateAt = context.Message.UpdateAt,
                AuctionEnd = context.Message.AuctionEnd,
                Status = context.Message.Status,
                Make = context.Message.Make,
                Model = context.Message.Model,
                Year = context.Message.Year,
                Color = context.Message.Color,
                Mileage = context.Message.Mileage,
                ImageUrl = context.Message.ImageUrl,
            };

            await item.SaveAsync();

        }
    }
}
