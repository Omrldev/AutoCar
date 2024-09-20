using ActionService.DTOs;
using ActionService.Entities;
using AutoMapper;
using Contracts;

namespace ActionService.RequestHelpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
            CreateMap<CreateAuctionDto, Item>();
            CreateMap<AuctionDto, ActionCreated>();
        }
    }
}
