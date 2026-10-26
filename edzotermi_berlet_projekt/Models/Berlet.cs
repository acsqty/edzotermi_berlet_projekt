using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace edzotermi_berlet_projekt.Models
{
    public class Berlet
    {
        public int Id { get; set; }

        [Required]
        public int TagId { get; set; }

        [Required]
        public int BerletTipusId { get; set; }

        public DateTime Vasarlas { get; set; } = DateTime.Today;

        public DateTime Lejarat { get; set; }

        [Range(0, int.MaxValue)]
        public int FelhasznaltBelepes { get; set; } = 0;

        [ValidateNever]
        public Tag Tag { get; set; } = null!;

        [ValidateNever]
        public BerletTipus BerletTipus { get; set; } = null!;

        [ValidateNever]
        public ICollection<Belepes> Belepesek { get; set; }
            = new List<Belepes>();
    }
}
