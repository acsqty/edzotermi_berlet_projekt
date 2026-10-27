
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.KimutatasPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int Ev { get; set; } = DateTime.Today.Year;
    public int Honap { get; set; } = DateTime.Today.Month;

    public decimal OsszesBevetel { get; set; }
    public int OsszesEladottBerlet { get; set; }

    public List<BerlettipusKimutatas> Kimutatas { get; set; } = new();

    public async Task OnGetAsync(int? ev, int? honap)
    {
        Ev = ev ?? DateTime.Today.Year;
        Honap = honap ?? DateTime.Today.Month;

        if (Ev < 2000 || Ev > 2100)
        {
            Ev = DateTime.Today.Year;
        }

        if (Honap < 1 || Honap > 12)
        {
            Honap = DateTime.Today.Month;
        }

        var kezdoDatum = new DateTime(Ev, Honap, 1);
        var kovetkezoHonap = kezdoDatum.AddMonths(1);

        var berletek = await _context.Berletek
            .Include(b => b.BerletTipus)
            .Where(b => b.Vasarlas >= kezdoDatum &&
                        b.Vasarlas < kovetkezoHonap)
            .ToListAsync();

        Kimutatas = berletek
            .GroupBy(b => b.BerletTipus.Nev)
            .Select(csoport => new BerlettipusKimutatas
            {
                BerlettipusNev = csoport.Key,
                EladottDarab = csoport.Count(),
                Bevetel = csoport.Sum(b => b.VasarlasiAr)
            })
            .OrderBy(x => x.BerlettipusNev)
            .ToList();

        OsszesEladottBerlet = berletek.Count;
        OsszesBevetel = berletek.Sum(b => b.VasarlasiAr);
    }

    public class BerlettipusKimutatas
    {
        public string BerlettipusNev { get; set; } = string.Empty;
        public int EladottDarab { get; set; }
        public decimal Bevetel { get; set; }
    }
}
