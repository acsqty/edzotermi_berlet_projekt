
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.BerletPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Berlet> Berlet { get; set; } = new List<Berlet>();

    public async Task OnGetAsync()
    {
        Berlet = await _context.Berletek
            .Include(b => b.Tag)
            .Include(b => b.BerletTipus)
            .Include(b => b.Belepesek)
            .OrderByDescending(b => b.Vasarlas)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostBelepesAsync(int id)
    {
        var berlet = await _context.Berletek
            .Include(b => b.BerletTipus)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet == null)
        {
            TempData["Hiba"] = "A bérlet nem található.";
            return RedirectToPage();
        }

        if (berlet.Lejarat.Date < DateTime.Today)
        {
            TempData["Hiba"] = "A bérlet lejárt, belépés nem engedélyezett.";
            return RedirectToPage();
        }

        if (berlet.BerletTipus.BelepesekSzama.HasValue &&
            berlet.FelhasznaltBelepes >= berlet.BerletTipus.BelepesekSzama.Value)
        {
            TempData["Hiba"] = "A bérlet összes belépését felhasználták.";
            return RedirectToPage();
        }

        var belepes = new Belepes
        {
            BerletId = berlet.Id,
            Idopont = DateTime.Now
        };

        _context.Belepesek.Add(belepes);

        if (berlet.BerletTipus.BelepesekSzama.HasValue)
        {
            berlet.FelhasznaltBelepes++;
        }

        await _context.SaveChangesAsync();

        TempData["Siker"] = "A belépést sikeresen rögzítettük.";

        return RedirectToPage();
    }
}
