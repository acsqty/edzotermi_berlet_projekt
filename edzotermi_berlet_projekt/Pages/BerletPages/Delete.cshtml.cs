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

        var berlet = await _context.Berletek.FirstOrDefaultAsync(m => m.Id == id);
        if (berlet is null)
        {
            return NotFound();
        }
        else
        {
            Berlet = berlet;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var berlet = await _context.Berletek.FindAsync(id);
        if (berlet != null)
        {
            Berlet = berlet;
            _context.Berletek.Remove(Berlet);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
