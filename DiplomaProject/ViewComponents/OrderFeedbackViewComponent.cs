using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class OrderFeedbackViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public OrderFeedbackViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<ViewViewComponentResult> InvokeAsync(int orderId)
    {
        var orderFeedback =  _context.OrderFeedbacks.FirstOrDefault(f => f.OrderId==orderId);
        
        return View("OrderFeedback", orderFeedback);
    }
}