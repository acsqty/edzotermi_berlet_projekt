using System.ComponentModel.DataAnnotations;

namespace edzotermi_berlet_projekt.Models
{
    public class BerletTipus
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A bérlettípus neve kötelező!")]
        [StringLength(100)]
        public string Nev { get; set; } = string.Empty;

        [Range(1, 3650)]
        public int ErvenyessegNap { get; set; }

        [Range(1, 10000)]
        public int? BelepesekSzama { get; set; }

        [Range(typeof(decimal), "0.01", "100000000")]
        public decimal Ar { get; set; }

        public ICollection<Berlet> Berletek { get; set; } = new List<Berlet>();
    }
}
