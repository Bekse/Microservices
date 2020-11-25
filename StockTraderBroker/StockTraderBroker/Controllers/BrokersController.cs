using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StockTraderBroker.Models;

namespace StockTraderBroker.Controllers
{
    [Route("api/stockTraderBroker")]
    [ApiController]
    public class BrokersController : ControllerBase
    {
        private readonly HttpClient _stockShareControlClient;
        private readonly HttpClient _stockShareTradersClient;
        private readonly HttpClient _stockShareTransactionsClient;

        public BrokersController(IHttpClientFactory httpClientFactory)
        {
            _stockShareControlClient = httpClientFactory.CreateClient("stockShareControlClient");
            _stockShareTradersClient = httpClientFactory.CreateClient("stockShareTradersClient");
            _stockShareTransactionsClient = httpClientFactory.CreateClient("stockShareTransactionsClient");
        }

        // Buy Shares
        // POST: api/stockTraderBroker/trade/buy-share/{shareId}/{traderId}
        [HttpPost("trade/buy-share/{shareId}/{traderId}")]
        public async Task<IActionResult> BuyShare(int shareId, int traderId)
        {
            var share = await GetShare(shareId);
            var trader = await GetTrader(traderId);
            var transaction = CreateTransaction(share, trader);
            var cost = share.Price;

            trader.Wallet -= cost;
            await PutTrader(trader);
            await PutSeller(share);

            share.OwnerId = traderId;
            share.ForSale = false;
            await PutShare(share);

            //await DeleteShare(share);
            await PostTransactions(transaction);

            return Ok();
        }

        // Sell Shares
        // POST: api/stockTraderBroker/trade/sell-share/{shareId}
        [HttpPost("trade/sell-share/{shareId}")]
        public async Task<IActionResult> SellShare(int shareId)
        {
            var share = await GetShare(shareId);
            share.ForSale = true;
            await PutShare(share);
            return Ok();
        }

        // Private methods
        // Get a buyer
        private async Task<Trader> GetTrader(int traderId)
        {
            Trader trader;

            using (var response = await _stockShareTradersClient.GetAsync($"/api/stockShareTraders/traders/{traderId}"))
            {
                trader = JsonConvert.DeserializeObject<Trader>(await response.Content.ReadAsStringAsync());
            }

            return trader;
        }

        // Get a share
        private async Task<Share> GetShare(int shareId)
        {
            Share share;

            using (var response = await _stockShareControlClient.GetAsync($"/api/stockShareControl/shares/{shareId}"))
            {
                share = JsonConvert.DeserializeObject<Share>(await response.Content.ReadAsStringAsync());
            }

            return share;
        }

        // Add a transaction
        private async Task<IActionResult> PostTransactions(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");

            using (var response = await _stockShareTransactionsClient.PostAsync("/api/stockShareTransactions/transactions", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }

        // Update a buyer
        private async Task<IActionResult> PutTrader(Trader trader)
        {
            if (trader == null) throw new ArgumentNullException(nameof(trader));

            var content = new StringContent(JsonConvert.SerializeObject(trader), Encoding.UTF8, "application/json");

            using (var response = await _stockShareTradersClient.PutAsync($"/api/stockShareTraders/traders/{trader.Id}", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }

        // Update single share
        private async Task<IActionResult> PutShare(Share share)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));
            var content = new StringContent(JsonConvert.SerializeObject(share), Encoding.UTF8, "application/json");

            using (var response = await _stockShareControlClient.PutAsync($"/api/stockShareControl/shares/{share.Id}", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }

        // Update sellers
        private async Task<IActionResult> PutSeller(Share share)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));

            var trader = await GetTrader(share.OwnerId);
            trader.Wallet += share.Price;

            var content = new StringContent(JsonConvert.SerializeObject(trader), Encoding.UTF8, "application/json");

            using (var response = await _stockShareTradersClient.PutAsync($"/api/stockShareTraders/traders/{trader.Id}", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }

        // Delete a bought share
        private async Task<IActionResult> DeleteShare(Share share)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));

            using (var response = await _stockShareControlClient.DeleteAsync($"/api/stockShareControl/shares/{share.Id}"))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }

        // Create a transaction
        private Transaction CreateTransaction(Share share, Trader trader)
        {
            if (share == null) throw new ArgumentNullException(nameof(share));
            if (trader == null) throw new ArgumentNullException(nameof(trader));

            return new Transaction
            {
                BuyerId = trader.Id,
                ShareId = share.Id,
                SellerId = share.OwnerId,
                Amount = share.Price,
                DateTime = DateTime.Now
            };
        }
    }
}
