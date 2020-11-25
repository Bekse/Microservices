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
using StockTaxingControl.Models;

namespace StockTaxingControl.Controllers
{
    [Route("api/stockTaxingControl")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private readonly HttpClient _stockShareTradersClient;

        public TaxesController(IHttpClientFactory httpClientFactory)
        {
            _stockShareTradersClient = httpClientFactory.CreateClient("stockShareTraders");
        }

        // Post a tax
        // POST: api/stockTaxingControl/taxing
        [HttpPost("taxing")]
        public async Task<IActionResult> PostTax(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var trader = new Trader { Wallet = transaction.Amount * (decimal)0.01, IsTax = true };

            var content = new StringContent(JsonConvert.SerializeObject(trader), Encoding.UTF8, "application/json");

            using (var response = await _stockShareTradersClient.PutAsync($"/api/stockShareTraders/traders/{transaction.SellerId}", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            return Ok();
        }
    }
}
