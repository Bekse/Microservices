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
using StockShareTransactions.Data;
using StockShareTransactions.Models;

namespace StockShareTransactions.Controllers
{
    [Route("api/stockShareTransactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly StockShareTransactionsContext _context;
        private readonly HttpClient _stockTaxingControlClient;

        public TransactionsController(StockShareTransactionsContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _stockTaxingControlClient = httpClientFactory.CreateClient("stockTaxingControl");
        }

        // Get list of Transactions
        // GET: api/stockShareTransactions/transactions
        [HttpGet("transactions")]
        public async Task<List<Transaction>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // Post a transaction
        // POST: api/stockShareTransactions/transactions
        [HttpPost("transactions")]
        public async Task<IActionResult> PostTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");

            using (var response = await _stockTaxingControlClient.PostAsync("api/stockTaxingControl/taxing", content))
            {
                await response.Content.ReadAsStringAsync();
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // For debugging purposes
        // Delete a transaction
        // DELETE: api/stockShareTransactions/transactions/id
        [HttpDelete("transactions/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
