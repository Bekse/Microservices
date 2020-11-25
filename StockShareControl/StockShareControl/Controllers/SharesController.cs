using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockShareControl.Data;
using StockShareControl.Models;

namespace StockShareControl.Controllers
{
    [Route("api/stockShareControl")]
    [ApiController]
    public class SharesController : ControllerBase
    {
        private readonly StockShareControlContext _context;

        public SharesController(StockShareControlContext context)
        {
            _context = context;
        }

        // Returns a list of all available shares
        // GET: api/stockShareControl/shares
        [HttpGet("shares")]
        public async Task<List<Share>> GetShares()
        {
            return await _context.Shares.ToListAsync();
        }

        // Returns a specific share by id
        // GET: api/stockShareControl/shares/id
        [HttpGet("shares/{id}")]
        public async Task<Share> GetShare(int id)
        {
            return await _context.Shares.FindAsync(id);
        }

        // Returns a list of shares with specific OwnerId
        // GET: api/stockShareControl/{traderId}/shares
        [HttpGet("shares/trader/{traderId}")]
        public async Task<List<Share>> GetSharesByTraderId(int traderId)
        {
            return await _context.Shares.Where(x => x.OwnerId == traderId).ToListAsync();
        }

        // Adds a share to the database
        // POST: api/stockShareControl/shares
        [HttpPost("shares")]
        public async Task<IActionResult> PostShare(Share share)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));

            _context.Shares.Add(share);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Updates a share in the database
        // PUT: api/stockShareControl/shares/id
        [HttpPut("shares/{id}")]
        public async Task<IActionResult> PutShare(int id, Share share)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));

            var data = await _context.Shares.FindAsync(id);
            if (data == null) throw new ArgumentNullException(nameof(data));

            data.OwnerId = share.OwnerId;
            data.Price = share.Price;
            data.ForSale = share.ForSale;

            _context.Shares.Update(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Removes a share from the database
        // DELETE: api/stockShareControl/shares/id
        [HttpDelete("shares/{id}")]
        public async Task<IActionResult> DeleteShare(int id)
        {
            var share = await _context.Shares.FindAsync(id);
            if (share == null) throw new ArgumentNullException(nameof(share));

            _context.Shares.Remove(share);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
