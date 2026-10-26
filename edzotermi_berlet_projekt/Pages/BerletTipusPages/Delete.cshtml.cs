using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.BerletTipusPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BerletTipus BerletTipus { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var berlettipus = await _context.BerletTipusok.FirstOrDefaultAsync(m => m.Id == id);
        if (berlettipus is null)
        {
            return NotFound();
        }
        else
        {
            BerletTipus = berlettipus;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var berlettipus = await _context.BerletTipusok.FindAsync(id);
        if (berlettipus != null)
        {
            BerletTipus = berlettipus;
            _context.BerletTipusok.Remove(BerletTipus);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
