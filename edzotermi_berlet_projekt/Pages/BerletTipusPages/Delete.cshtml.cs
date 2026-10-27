
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

    public BerletTipus BerletTipus { get; set; } = default!;

    public bool VanKapcsolodoBerlet { get; set; }

    public string? HibaUzenet { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var berlettipus = await _context.BerletTipusok
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlettipus == null)
        {
            return NotFound();
        }

        BerletTipus = berlettipus;

        VanKapcsolodoBerlet = await _context.Berletek
            .AnyAsync(b => b.BerletTipusId == id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var berlettipus = await _context.BerletTipusok
            .FirstOrDefaultAsync(b => b.Id == id);

        if (berlettipus == null)
        {
            return NotFound();
        }

        bool vanKapcsolodoBerlet = await _context.Berletek
            .AnyAsync(b => b.BerletTipusId == id);

        if (vanKapcsolodoBerlet)
        {
            BerletTipus = berlettipus;
            VanKapcsolodoBerlet = true;
            HibaUzenet = "Ez a bérlettípus nem törölhető, mert már tartozik hozzá megvásárolt bérlet.";
            return Page();
        }

        _context.BerletTipusok.Remove(berlettipus);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
