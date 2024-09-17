using ActionService.Data;
using ActionService.DTOs;
using ActionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        public AuctionController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            var query = _context.Auctions
                .OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrWhiteSpace(date))
            {
                query = query.Where(x => x.UpdateAt
                .CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            var result = auctions.Select(x => new AuctionDto()
            {
                Id = x.Id,
                ReservePrice = x.ReservePrice,
                Seller = x.Seller,
                Winner = x.Winner,
                SoldAmount = x.SoldAmount.GetValueOrDefault(),
                CurrentHighBid = x.CurrentHighBid.GetValueOrDefault(),
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                AuctionEnd = x.AuctionEnd,
                Status = x.Status.ToString(),
                Make = x.Item.Make,
                Model = x.Item.Model,
                Year = x.Item.Year,
                Color = x.Item.Color,
                Mileage = x.Item.Mileage,
                ImageUrl = x.Item.ImageUrl
            }).ToList();

            return result;

            //return _mapper.Map<List<AuctionDto>>(auctions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null)
            {
                return NotFound();
            }

            return _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
        {
            var auction = _mapper.Map<Auction>(createAuctionDto);

            // TO DO: add user as a seller
            auction.Seller = "test";

            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("--> Problem to create a auction");

            return CreatedAtAction(nameof(GetAuctionById), 
                new {auction.Id}, _mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                .Include (x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null) return NotFound();

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("--> Problem updating auction");

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions
                .FindAsync(id);

            if (auction == null) return NotFound();

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("--> Problem deleting the auction");

            return Ok();
        }
    }
}
