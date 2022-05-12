using System;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DiplomaProject.ViewComponents;

public class CancelOrderViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public CancelOrderViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<HtmlContentViewComponentResult> InvokeAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        order.ReadyStageId = 8;
        _context.Update(order);
        await _context.SaveChangesAsync();
        return new HtmlContentViewComponentResult(
            new HtmlString($"<p>Замовлення скасоване</p>")
        );
    }
}