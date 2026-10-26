using System.ComponentModel.DataAnnotations;

namespace edzotermi_berlet_projekt.Models
{
    public class Belepes
    {
        public int Id { get; set; }

        [Required]
        public int BerletId { get; set; }

        public DateTime Idopont { get; set; } = DateTime.Now;

        public Berlet Berlet { get; set; } = null!;
    }
}
