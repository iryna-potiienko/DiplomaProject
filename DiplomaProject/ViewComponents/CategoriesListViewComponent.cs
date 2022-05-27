using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class CategoriesListViewComponent : ViewComponent
{
    private readonly KraftWebAppContext _context;

    public CategoriesListViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<ViewViewComponentResult> InvokeAsync()
    {
        var subcategories = await _context.Categories.ToListAsync();
        return View("CategoriesList", subcategories);
    }
}
