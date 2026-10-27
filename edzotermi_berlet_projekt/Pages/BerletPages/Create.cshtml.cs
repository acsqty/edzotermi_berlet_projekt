using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Data;
using edzotermi_berlet_projekt.Models;

namespace edzotermi_berlet_projekt.Pages.BerletPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Berlet Berlet { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Berlet.Vasarlas = DateTime.Today;
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

        var berletTipus = await _context.BerletTipusok
            .FirstOrDefaultAsync(b => b.Id == Berlet.BerletTipusId);

        var tagExists = await _context.Tagok
            .AnyAsync(t => t.Id == Berlet.TagId);

        if (berletTipus == null || !tagExists)
        {
            ModelState.AddModelError(string.Empty,
                "Válassz ki egy létező tagot és bérlettípust.");
            await LoadListsAsync();
            return Page();
        }

        if (berletTipus.ErvenyessegNap < 1)
        {
            ModelState.AddModelError(string.Empty,
                "A bérlettípus érvényessége nem megfelelő.");
            await LoadListsAsync();
            return Page();
        }

        Berlet.Vasarlas = Berlet.Vasarlas.Date;

        Berlet.Lejarat = Berlet.Vasarlas
            .AddDays(berletTipus.ErvenyessegNap);

        Berlet.VasarlasiAr = berletTipus.Ar;

        Berlet.FelhasznaltBelepes = 0;

        _context.Berletek.Add(Berlet);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadListsAsync()
    {
        ViewData["TagId"] = new SelectList(
            await _context.Tagok.OrderBy(t => t.Nev).ToListAsync(),
            "Id",
            "Nev",
            Berlet.TagId);

        ViewData["BerletTipusId"] = new SelectList(
            await _context.BerletTipusok.OrderBy(b => b.Nev).ToListAsync(),
            "Id",
            "Nev",
            Berlet.BerletTipusId);
    }
}