using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using DiplomaProject.Repositories;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;

namespace DiplomaProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly KraftWebAppContext _context;
        private readonly IOrderRepository _orderRepository;
        public const string SessionKey = "Cart";

        public OrderController(KraftWebAppContext context, IOrderRepository repository)
        {
            _context = context;
            _orderRepository = repository;
        }

        // GET: Order
        public async Task<IActionResult> Index(int? shopProfileId)
        {
            var orders =new List<Order>();
            if (shopProfileId != null)
            {
                var shop = await _context.ShopProfiles.FindAsync(shopProfileId);
                if (shop.SalesmanId == GetCurrentUserId())
                {
                    orders = await _context.Orders
                        .Where(o => o.ShopProfileId == shopProfileId)
                        .Include(o => o.Cart)
                        .ThenInclude(o => o.Customer)
                        .Include(o => o.ShopProfile)
                        .Include(o => o.DeliveryType)
                        .Include(o => o.ReadyStage)
                        .OrderByDescending(o=>o.DateOfFixation)
                        .ToListAsync();
                    ViewBag.ShopProfileName = shop.Name;
                }
            }
            else
            {
                orders = await _context.Orders
                    // .Include(o => o.Cart)
                    // .ThenInclude(o => o.Customer)
                    .Where(o=>o.Cart.CustomerId == GetCurrentUserId())
                    .Include(o=>o.ShopProfile)
                    //.Include(o => o.Cart)
                    //.ThenInclude(o => o.ShopProfile)
                    .Include(o => o.DeliveryType)
                    .Include(o => o.ReadyStage)
                    .AsSplitQuery()
                    .OrderByDescending(o=>o.DateOfFixation)
                    .ToListAsync();
                //.Include(o => o.ShopProfile);
            }

            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                //.Include(o => o.Customer)
                .Include(o => o.DeliveryType)
                .Include(o => o.ReadyStage)
                .Include(o => o.ShopProfile)
                .ThenInclude(o=>o.Salesman)
                .Include(o=>o.Cart)
                .ThenInclude(c=>c.ProductsInOrder)
                .ThenInclude(p=>p.Product)
                // .Include(o => o.Cart)
                // .ThenInclude(o => o.ShopProfile)
                .Include(o => o.Cart)
                .ThenInclude(o => o.Customer)
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult UserMakeOrder(int cartId)
        {
            ViewData["CartId"] = cartId;
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name");
            // var sum = _context.ProductsInOrder
            //     .Where(o => o.CartId == cartId)
            //     .Include(p => p.Product)
            //     .Sum(p => p.Product.Price);//.ToList();
            // var price = (from product in productInOrders
            //         ) 
            //ViewBag.Price = sum;
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserMakeOrder([Bind("Id,CartId,DeliveryTypeId,AddressToDelivery,DateBeReady,Comment")] UserMakeOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //order.CustomerId = user.Id;

                if (model.DateBeReady > DateTime.Now)
                {
                    //if(model.DeliveryTypeId!=1)
                    var currentCart = GetCurrentCart(); //CloseCart();
                    if (currentCart != null)
                    {
                        var order = new Order
                        {
                            CartId = model.CartId,
                            AddressToDelivery = model.AddressToDelivery,
                            UserComment = model.Comment,
                            DeliveryTypeId = model.DeliveryTypeId,
                            DateBeReady = model.DateBeReady,
                            //Price = ViewBag.Price,

                            ShopProfileId = currentCart.ShopProfileId,
                            ReadyStageId = 1,
                            DateOfFixation = DateTime.Now,
                            IsPaid = false,
                            IsDelivered = false
                        };

                        _context.Add(order);
                        await _context.SaveChangesAsync();

                        CloseCart();
                        return RedirectToAction(nameof(Details), new {id = order.Id});
                    }
                }
                else
                {
                    ViewBag.CartId = model.CartId;
                    ModelState.AddModelError("", "?????????????????????? ????????");
                }
            }
            
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name", model.DeliveryTypeId);
            return View(model);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> ShopGetsOrderFromUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o=>o.Cart)
                .ThenInclude(c=>c.Customer)
                .Include(o=>o.Cart)
                .ThenInclude(c=>c.ProductsInOrder)
                .ThenInclude(p=>p.Product)
                .Include(o=>o.ShopProfile)
                .Include(o=>o.DeliveryType)
                .Include(o=>o.ReadyStage)
                .FirstOrDefaultAsync(o=>o.Id==id);
            if (order == null)
            {
                return NotFound();
            }
            
            //ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Id", order.DeliveryTypeId);
            //ViewData["ReadyStageId"] = new SelectList(_context.ReadyStages, "Id", "Name", order.ReadyStageId);
            ViewBag.ShopProfileId = order.ShopProfileId;
            ViewData["CartId"] = order.CartId;
            ViewBag.TotalPrice = order.Cart.ProductsInOrder.Sum(p => p.FinalPrice);
            return View(order);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShopGetsOrderFromUser(int id, string[] productPrices, string[] productDescriptions, [Bind("Id,CustomerId,ShopProfileId,DeliveryTypeId,DateBeReady,DateOfFixation,IsDelivered,AddressToDelivery,SalesmanComment,Price,IsPaid,ReadyStageId")] Order model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Order order;
                try
                {
                    //var order = await _context.Orders.FindAsync(id);
                    order = await _context.Orders
                        // .Include(o=>o.Cart)
                        // .ThenInclude(c=>c.Customer)
                        // .Include(o=>o.Cart)
                        // .ThenInclude(c=>c.ProductsInOrder)
                        // .ThenInclude(p=>p.Product)
                        // .Include(o=>o.ShopProfile)
                        // .Include(o=>o.DeliveryType)
                        .Include(o => o.ReadyStage)
                        .FirstOrDefaultAsync(o => o.Id == id);
                    
                    order.DateBeReady = model.DateBeReady;
                    //order.ReadyStageId = model.ReadyStageId;
                    order.Price = model.Price;
                    //order.IsPaid = model.IsPaid;
                    order.SalesmanComment = model.SalesmanComment;
                    order.ReadyStageId = 2;

                    _context.Update(order);

                    var productsInOrder = _context.ProductsInOrder
                        .Where(p => p.CartId == order.CartId)
                        .ToList();
                    
                    for (var i = 0; i < productsInOrder.Count; i++)
                    {
                        productsInOrder[i].FinalPrice = Convert.ToDouble(productPrices[i]);
                        productsInOrder[i].FinalDescription = productDescriptions[i];
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                //return RedirectToAction(nameof(Index), new {shopProfileId = model.ShopProfileId});
                return RedirectToAction(nameof(Details), new {id = order.Id});
            }
            
            //ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Id", order.DeliveryTypeId);
            //ViewData["ReadyStageId"] = new SelectList(_context.ReadyStages, "Id", "Name", model.ReadyStageId);
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", order.ShopProfileId);
            return View(model);
        }
        
        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                // .Include(o => o.Cart)
                // .ThenInclude(o => o.ShopProfile)
                .Include(o => o.Cart)
                .ThenInclude(o => o.Customer)
                //.Include(o => o.Customer)
                .Include(o => o.DeliveryType)
                .Include(o => o.ReadyStage)
                .Include(o => o.ShopProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        
        public Cart CloseCart()
        {
            var sessionId  = HttpContext.Session.GetInt32(SessionKey);

            if (sessionId != null)
            {
                //var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

                var userId = GetCurrentUserId();

                // var cart = _context.Carts
                //     .Include(c => c.ProductsInOrder)
                //     .ThenInclude(p => p.Product)
                //     .FirstOrDefault(c => c.Id == sessionId);
                var cart = GetCurrentCart();
                
                if (cart.CustomerId == userId && cart.IsOpenForAddingProducts == true)
                {
                    cart.IsOpenForAddingProducts = false;
                    _context.Update(cart);
                    _context.SaveChanges();
                    
                    HttpContext.Session.Remove(SessionKey);
                    return cart;
                }
            }

            return null;
        }

        public Cart GetCurrentCart()
        {
            var sessionId = HttpContext.Session.GetInt32(SessionKey);
            if (sessionId == null) return null;
            
            var cart = _context.Carts
                .Include(c => c.ProductsInOrder)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.Id == sessionId);
            return cart;
        }
        
        public int GetCurrentUserId()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            return user.Id;
        }
        
        public User GetCurrentUser()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            return user;
        }

        public async Task<IActionResult> UserGetResponse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o=>o.Cart)
                .ThenInclude(c=>c.Customer)
                .Include(o=>o.Cart)
                .ThenInclude(c=>c.ProductsInOrder)
                .ThenInclude(p=>p.Product)
                .Include(o=>o.ShopProfile)
                .Include(o=>o.DeliveryType)
                .Include(o=>o.ReadyStage)
                .FirstOrDefaultAsync(o=>o.Id==id);
            if (order == null)
            {
                return NotFound();
            }
            
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name", order.DeliveryTypeId);
            //ViewData["ReadyStageId"] = new SelectList(_context.ReadyStages, "Id", "Name", order.ReadyStageId);
            ViewData["CartId"] = order.CartId;
            ViewBag.Price = order.Price;
            return View(order);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserGetResponse(int id, [Bind("Id,CartId,DeliveryTypeId,AddressToDelivery,UserComment,DateBeReady")] Order model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //order.CustomerId = user.Id;

                var order = await _orderRepository.GetOrderById(id);


                order.AddressToDelivery = model.AddressToDelivery;
                order.UserComment = model.UserComment;
                order.DeliveryTypeId = model.DeliveryTypeId;
                order.DateBeReady = order.DateBeReady;

                // ShopProfileId = currentCart.ShopProfileId,
                // ReadyStageId = 1,
                // DateOfFixation = DateTime.Today,
                // IsPaid = false,
                // IsDelivered = false


                // _context.Update(order);
                // await _context.SaveChangesAsync();
                await _orderRepository.UpdateOrder(order);
                return RedirectToAction(nameof(Index));

            }

            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name", model.DeliveryTypeId);
            return View(model);
        }

        public async Task<IActionResult> ReadyStageChanged(int id, string option)
        {
            var order = await _orderRepository.GetOrderById(id);

            switch (option)
            {
                case "AcceptOrder":
                    order.ReadyStageId = 3;
                    order.Price = _orderRepository.GetTotalOrderPrice(order.Id);
                    ViewBag.MessageText = "???????????????????? ????????????????????????";
                    break;
                case "PayForTheOrder":
                    order.IsPaid = true;
                    order.ReadyStageId = 4;
                    ViewBag.MessageText = "???????????????????? ????????????????";
                    break;
                case "PutOrderInWork":
                    order.ReadyStageId = 5;
                    ViewBag.MessageText = "???????????????????? ???????????????? ?? ??????????????";
                    break;
                case "OrderIsReady":
                    order.ReadyStageId = 6;
                    ViewBag.MessageText = "???????????????????? ????????????";
                    break;
                case "SendOrderToUser":
                    order.ReadyStageId = 7;
                    ViewBag.MessageText = "???????????????????? ?????????????????? ??????????????";
                    break;
                case "CancelOrder":
                    order.ReadyStageId = 8;
                    ViewBag.MessageText = "???????????????????? ??????????????????";
                    break;
                case "GetOrder":
                    order.IsDelivered = true;
                    order.ReadyStageId = 9;
                    ViewBag.MessageText = "???????????????????? ????????????????";
                    break;
            }

            ViewBag.OrderId = order.Id;

            await _orderRepository.UpdateOrder(order);
            return View();
        }
        
        //[AcceptVerbs("GET", "POST")]
        public IActionResult VerifyDateBeReady(DateTime DateBeReady)
        {
            return DateBeReady < DateTime.Now ? Json("Correct Data!") : Json(true);
        }
    }
}
