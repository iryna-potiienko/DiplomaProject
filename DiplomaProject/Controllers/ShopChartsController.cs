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
            var shopProfile = await _context.ShopProfiles.FindAsync(shopProfileId);
            var ordersListAsync = await _context.Orders
                //.Include(o => o.OrderFeedback)
                .Where(o => o.ShopProfileId == shopProfileId)
                .OrderByDescending(o=>o.DateOfFixation)
                .ToListAsync();
            
            var resList = new List<object>();
            resList.Add(new[] {"Дата", "Кількість замовлень"});
            
            //var days=ordersListAsync.
            //var shopProfile = await _context.ShopProfiles.FindAsync(shopProfileId);
            var orders =  ordersListAsync;
            //var days = DateOnly.FromDateTime(DateTime.Now).AddDays(3);//shopProfile.DateAdded;

            for (var i = shopProfile.DateCreated; i < DateOnly.FromDateTime(DateTime.Now); i = i.AddDays(1))
            {
                var date = i;
                var count = orders.Count(o => DateOnly.FromDateTime(o.DateOfFixation.Date) == i);

                resList.Add(new object[] {date.ToShortDateString(), count});
            }

            return Ok(resList);
        }
    }
}
