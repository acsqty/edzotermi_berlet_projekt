
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using edzotermi_berlet_projekt.Models;
using edzotermi_berlet_projekt.Data;

namespace edzotermi_berlet_projekt.Pages.TagPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Tag Tag { get; set; } = default!;

    public List<Berlet> ErvenyesBerletek { get; set; } = new();
    public List<Berlet> LejartBerletek { get; set; } = new();
    public List<Berlet> HamarosanLejaroBerletek { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tag = await _context.Tagok
            .Include(t => t.Berletek)
                .ThenInclude(b => b.BerletTipus)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null)
        {
            return NotFound();
        }

        Tag = tag;

        var maiNap = DateTime.Today;
        var hetMulva = maiNap.AddDays(7);

        ErvenyesBerletek = Tag.Berletek
            .Where(b => b.Lejarat.Date >= maiNap &&
                (!b.BerletTipus.BelepesekSzama.HasValue ||
                 b.FelhasznaltBelepes < b.BerletTipus.BelepesekSzama.Value))
            .OrderBy(b => b.Lejarat)
            .ToList();

        LejartBerletek = Tag.Berletek
            .Where(b => b.Lejarat.Date < maiNap)
            .OrderByDescending(b => b.Lejarat)
            .ToList();

        HamarosanLejaroBerletek = ErvenyesBerletek
            .Where(b => b.Lejarat.Date >= maiNap &&
                        b.Lejarat.Date <= hetMulva)
            .OrderBy(b => b.Lejarat)
            .ToList();

        return Page();
    }
}
