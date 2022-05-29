using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ShopOrdersInfoViewComponent: ViewComponent
{
    private readonly IShopProfileRepository _shopProfileRepository;
    private readonly IOrderRepository _orderRepository;

    public ShopOrdersInfoViewComponent(IShopProfileRepository shopProfileRepository, IOrderRepository orderRepository)
    {
        _shopProfileRepository = shopProfileRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ViewViewComponentResult> InvokeAsync(int shopProfileId)
    {
        var shopProfile = await _shopProfileRepository.GetShopProfileDetailedById(shopProfileId);
        
        ViewBag.ProductsCount = shopProfile.Products.Count;
        ViewBag.OrdersCount = shopProfile.Orders.Count;
        ViewBag.OrdersUserGetCount = shopProfile.Orders
            .Count(o => o.ReadyStageId == 9);

        var shopOrders = _orderRepository.GetShopOrders(shopProfileId);
        ViewBag.OrdersInTimeCount = shopOrders.Count(o => o.OrderFeedback.IsInTime == true);
        ViewBag.OrdersSuccesfulCount = shopOrders.Count(o => o.OrderFeedback.IsEverythingOkay == true);

        return View("ShopOrdersInfo");
    }
}