using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace DiplomaProject.Controllers
{
    [Route("Charts")]
    //[ApiController]
    public class ShopChartsController : ControllerBase
    {
        private readonly KraftWebAppContext _context;

        public ShopChartsController(KraftWebAppContext context)
        {
            _context = context;
        }

        [HttpGet("OrdersEstimations/{shopProfileId:int}")]
        [Produces("application/json")]
        public async Task<ActionResult> OrdersEstimations(int shopProfileId)
        {
            var ordersListAsync = _context.Orders
                .Include(o => o.OrderFeedback)
                .Where(o => o.ShopProfileId == shopProfileId && o.OrderFeedback != null).ToListAsync();
            var resList = new List<object>();
            resList.Add(new[] {"Балів оцінено замовлення", "Кількість замовлень"});
            
            var orders = await ordersListAsync;
            resList.AddRange(
                from order in orders 
                select order.OrderFeedback.Estimation into estimationNumber 
                let count = orders.Count(o => o.OrderFeedback.Estimation == estimationNumber)
                select new object[] {estimationNumber.ToString(), count}
                );

            return Ok(resList);
        }
        
        [HttpGet("OrdersAmountPerDay/{shopProfileId:int}")]
        [Produces("application/json")]
        public async Task<ActionResult> OrdersAmountPerDay(int shopProfileId)
        {
            var ordersListAsync = _context.Orders
                //.Include(o => o.OrderFeedback)
                .Where(o => o.ShopProfileId == shopProfileId).ToListAsync();
            
            var resList = new List<object>();
            resList.Add(new[] {"Дата", "Кількість замовлень"});
            
            var orders = await ordersListAsync;
            foreach (var order in orders)
            {
                var date = order.DateOfFixation.Date;
                var count = orders.Count(o => o.DateOfFixation.Date == date);
                
                resList.Add(new object[] {date.ToShortDateString(),count});
            }

            return Ok(resList);
        }
    }
}
