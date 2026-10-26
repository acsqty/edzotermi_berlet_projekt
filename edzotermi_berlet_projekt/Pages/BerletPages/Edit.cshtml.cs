
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Data;
using edzotermi_berlet_projekt.Models;

namespace edzotermi_berlet_projekt.Pages.BerletPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Berlet Berlet { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var berlet = await _context.Berletek
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet == null)
        {
            return NotFound();
        }

        Berlet = berlet;

        await LoadListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        var existingBerlet = await _context.Berletek
            .FirstOrDefaultAsync(b => b.Id == Berlet.Id);

        if (existingBerlet == null)
        {
            return NotFound();
        }

        var tagExists = await _context.Tagok
            .AnyAsync(t => t.Id == Berlet.TagId);

        var berletTipus = await _context.BerletTipusok
            .FirstOrDefaultAsync(b => b.Id == Berlet.BerletTipusId);

        if (!tagExists || berletTipus == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Válassz ki egy létező tagot és bérlettípust.");

            await LoadListsAsync();
            return Page();
        }

        if (berletTipus.ErvenyessegNap < 1)
        {
            ModelState.AddModelError(
                string.Empty,
                "A bérlettípus érvényessége nem megfelelő.");

            await LoadListsAsync();
            return Page();
        }

        existingBerlet.TagId = Berlet.TagId;
        existingBerlet.BerletTipusId = Berlet.BerletTipusId;
        existingBerlet.Vasarlas = Berlet.Vasarlas.Date;
        existingBerlet.Lejarat = Berlet.Vasarlas.Date
            .AddDays(berletTipus.ErvenyessegNap);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadListsAsync()
    {
        ViewData["TagId"] = new SelectList(
            await _context.Tagok
                .OrderBy(t => t.Nev)
                .ToListAsync(),
            "Id",
            "Nev",
            Berlet.TagId);

        ViewData["BerletTipusId"] = new SelectList(
            await _context.BerletTipusok
                .OrderBy(b => b.Nev)
                .ToListAsync(),
            "Id",
            "Nev",
            Berlet.BerletTipusId);
    }
}
