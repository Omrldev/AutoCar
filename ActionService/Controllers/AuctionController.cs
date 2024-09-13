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
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
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
            var auction = _context.Auctions
                .Include (x => x.Item)
                .FirstOrDefault(x => x.Id == id);

            if (auction == null) return NotFound();

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("--> Problem updating auction");

            return Ok();
        }
    }
}
