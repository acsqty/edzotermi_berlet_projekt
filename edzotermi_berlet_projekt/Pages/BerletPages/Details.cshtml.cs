
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Data;
using edzotermi_berlet_projekt.Models;

namespace edzotermi_berlet_projekt.Pages.BerletPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Berlet Berlet { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var berlet = await _context.Berletek
            .Include(b => b.Tag)
            .Include(b => b.BerletTipus)
            .Include(b => b.Belepesek)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet == null)
        {
            return NotFound();
        }

        Berlet = berlet;

        return Page();
    }
}
