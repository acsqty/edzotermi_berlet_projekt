using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.BerletTipusPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
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
        BerletTipus = berlettipus;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(BerletTipus).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BerletTipusExists(BerletTipus.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BerletTipusExists(int id)
    {
        return _context.BerletTipusok.Any(e => e.Id == id);
    }
}
