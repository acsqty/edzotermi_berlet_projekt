
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.TagPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Tag> Tag { get; set; } = new List<Tag>();

    public async Task OnGetAsync()
    {
        Tag = await _context.Tagok
            .Include(t => t.Berletek)
            .OrderBy(t => t.Nev)
            .ToListAsync();
    }
}
