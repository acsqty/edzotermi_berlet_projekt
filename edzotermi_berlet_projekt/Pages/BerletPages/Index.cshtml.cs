
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

    public string Szures { get; set; } = "osszes";

    public async Task OnGetAsync(string? szures)
    {
        Szures = szures ?? "osszes";

        var berletek = await _context.Berletek
            .Include(b => b.Tag)
            .Include(b => b.BerletTipus)
            .Include(b => b.Belepesek)
            .OrderByDescending(b => b.Vasarlas)
            .ToListAsync();

        var maiNap = DateTime.Today;
        var hetMulva = maiNap.AddDays(7);

        switch (Szures)
        {
            case "ervenyes":
                berletek = berletek
                    .Where(b => b.Lejarat.Date >= maiNap &&
                        (!b.BerletTipus!.BelepesekSzama.HasValue ||
                         b.FelhasznaltBelepes <
                         b.BerletTipus.BelepesekSzama.Value))
                    .ToList();
                break;

            case "lejart":
                berletek = berletek
                    .Where(b => b.Lejarat.Date < maiNap)
                    .ToList();
                break;

            case "hamarosan":
                berletek = berletek
                    .Where(b => b.Lejarat.Date >= maiNap &&
                        b.Lejarat.Date <= hetMulva &&
                        (!b.BerletTipus!.BelepesekSzama.HasValue ||
                         b.FelhasznaltBelepes <
                         b.BerletTipus.BelepesekSzama.Value))
                    .ToList();
                break;

            case "elfogyott":
                berletek = berletek
                    .Where(b => b.BerletTipus!.BelepesekSzama.HasValue &&
                        b.FelhasznaltBelepes >=
                        b.BerletTipus.BelepesekSzama.Value &&
                        b.Lejarat.Date >= maiNap)
                    .ToList();
                break;
        }

        Berlet = berletek;
    }

    public async Task<IActionResult> OnPostBelepesAsync(int id, string? szures)
    {
        var berlet = await _context.Berletek
            .Include(b => b.BerletTipus)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlet == null)
        {
            TempData["Hiba"] = "A bérlet nem található.";
            return RedirectToPage(new { szures = szures ?? "osszes" });
        }

        if (berlet.Lejarat.Date < DateTime.Today)
        {
            TempData["Hiba"] = "A bérlet lejárt, belépés nem engedélyezett.";
            return RedirectToPage(new { szures = szures ?? "osszes" });
        }

        if (berlet.BerletTipus.BelepesekSzama.HasValue &&
            berlet.FelhasznaltBelepes >=
            berlet.BerletTipus.BelepesekSzama.Value)
        {
            TempData["Hiba"] = "A bérlet összes belépését felhasználták.";
            return RedirectToPage(new { szures = szures ?? "osszes" });
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

        return RedirectToPage(new { szures = szures ?? "osszes" });
    }
}
