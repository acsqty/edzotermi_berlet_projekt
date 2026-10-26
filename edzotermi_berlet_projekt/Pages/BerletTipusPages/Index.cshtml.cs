using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.BerletTipusPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<BerletTipus> BerletTipus { get; set; } = default!;

    public async Task OnGetAsync()
    {
        BerletTipus = await _context.BerletTipusok.ToListAsync();
    }
}
