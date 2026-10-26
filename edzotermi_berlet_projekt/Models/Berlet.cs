using System.ComponentModel.DataAnnotations;

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

        public Tag Tag { get; set; } = null!;

        public BerletTipus BerletTipus { get; set; } = null!;

        public ICollection<Belepes> Belepesek { get; set; } = new List<Belepes>();
    }
}
