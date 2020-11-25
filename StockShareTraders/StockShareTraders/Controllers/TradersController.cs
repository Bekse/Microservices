using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockShareTraders.Data;
using StockShareTraders.Models;

namespace StockShareTraders.Controllers
{
    [Route("api/stockShareTraders")]
    [ApiController]
    public class TradersController : ControllerBase
    {
        private readonly StockShareTradersContext _context;

        public TradersController(StockShareTradersContext context)
        {
            _context = context;
        }

        // Get list of traders
        // GET: api/stockShareTraders/traders
        [HttpGet("traders")]
        public async Task<List<Trader>> GetTraders()
        {
            return await _context.Traders.ToListAsync();
        }

        // Get trader by id
        // GET: api/stockShareTraders/traders/id
        [HttpGet("traders/{id}")]
        public async Task<Trader> GetTraderById(int id)
        {
            return await _context.Traders.FindAsync(id);
        }

        // Post a trader
        // POST: api/stockShareTraders/traders
        [HttpPost("traders")]
        public async Task<IActionResult> PostTrader(Trader trader)
        {
            if (trader == null) throw new ArgumentNullException(nameof(trader));

            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Put a trader
        // PUT: api/stockShareTraders/traders/id
        [HttpPut("traders/{id}")]
        public async Task<IActionResult> PutTrader(Trader trader, int id)
        {
            if (trader == null) throw new ArgumentNullException(nameof(trader));

            var data = await _context.Traders.FindAsync(id);
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (!trader.IsTax) { data.Wallet = trader.Wallet; }
            if (trader.IsTax) { data.Wallet -= trader.Wallet; }

            data.IsTax = false;
            data.IsBuyer = false;
            _context.Traders.Update(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Delete a trader
        // Delete: api/stockShareTraders/traders/id
        [HttpDelete("traders/{id}")]
        public async Task<IActionResult> DeleteTrader(int id)
        {
            var trader = await _context.Traders.FindAsync(id);
            if (trader == null) throw new ArgumentNullException(nameof(trader));

            _context.Remove(trader);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
