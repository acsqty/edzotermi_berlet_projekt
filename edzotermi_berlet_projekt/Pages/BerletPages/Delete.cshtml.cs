using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.BerletPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Berlet Berlet { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var berlet = await _context.Berletek
            .Include(b => b.Tag)
            .Include(b => b.BerletTipus)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet is null)
        {
            return NotFound();
        }

        Berlet = berlet;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var berlet = await _context.Berletek
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet is not null)
        {
            _context.Berletek.Remove(berlet);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
